using MelonLoader;
using BTD_Mod_Helper;
using MonkeyBuccFourthPath;
using PathsPlusPlus;
using Il2CppAssets.Scripts.Models.Towers;
using BTD_Mod_Helper.Api.Enums;
using Il2Cpp;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using JetBrains.Annotations;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack;
using Il2CppSystem.IO;
using Il2CppAssets.Scripts.Simulation.Towers.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Utils;
using System.Collections.Generic;
using System.Linq;
using Il2CppAssets.Scripts.Models.TowerSets;
using BTD_Mod_Helper.Api.Towers;
using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Unity.Display;
using BTD_Mod_Helper.Api.Display;
using UnityEngine;
using Il2CppAssets.Scripts.Models.GenericBehaviors;
using Il2CppAssets.Scripts.Simulation.SMath;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities;
using Il2CppAssets.Scripts.Models.Towers.TowerFilters;
using Il2CppAssets.Scripts.Models.Map;
using Il2CppAssets.Scripts.Models.Towers.Weapons.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions;
using Il2CppAssets.Scripts.Simulation.Towers;
using Il2CppAssets.Scripts.Models.Towers.Filters;
using System.Runtime.CompilerServices;
using Il2CppAssets.Scripts.Models.Bloons.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack.Behaviors;
using Il2CppInterop.Runtime.InteropTypes.Arrays;

[assembly: MelonInfo(typeof(MonkeyBuccFourthPath.MonkeyBuccFourthPath), ModHelperData.Name, ModHelperData.Version, ModHelperData.RepoOwner)]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]

namespace MonkeyBuccFourthPath;

public class MonkeyBuccFourthPath : BloonsTD6Mod
{
    public override void OnApplicationStart()
    {
        ModHelper.Msg<MonkeyBuccFourthPath>("MonkeyBuccFourthPath loaded!");
    }
    public class FourthPath2 : PathPlusPlus
    {
        public override string Tower => TowerType.MonkeyBuccaneer;
        public override int UpgradeCount => 5;

    }
    public class WaterSolubeDarts : UpgradePlusPlus<FourthPath2>
    {
        public override int Cost => 150;
        public override int Tier => 1;
        public override string? Portrait => "tier1Portrait";
        public override string Icon => VanillaSprites.VeryQuickShotsUpgradeIcon;
        public override string Description => "Darts have more pierce and travel faster.";

        public override void ApplyUpgrade(TowerModel towerModel)
        {
            var attackModel = towerModel.GetAttackModel();
            towerModel.ApplyDisplay<tier1Display>();
            attackModel.weapons[0].projectile.pierce += 2;
            attackModel.weapons[0].projectile.GetBehavior<TravelStraitModel>().speed *= 3f;
        }
    }
    public class tier1Display : ModDisplay
    {
        public override string BaseDisplay => GetDisplay(TowerType.MonkeyBuccaneer);
        public override void ModifyDisplayNode(UnityDisplayNode node)
        {
            SetMeshTexture(node, "tier1Display", 1);
        }
    }
    public class BetterEngine : UpgradePlusPlus<FourthPath2>
    {
        public override int Cost => 550;
        public override int Tier => 2;
        public override string Icon => VanillaSprites.BiggerJetsUpgradeIcon;

        public override string? Portrait => "tier2Portrait";
        public override string Description => "Better engine shoots darts faster, and they fly even faster.";

        public override void ApplyUpgrade(TowerModel towerModel)
        {
            var attackModel = towerModel.GetAttackModel();
            towerModel.ApplyDisplay<tier2Display>();
            attackModel.weapons[0].rate *= 0.78f;
            attackModel.weapons[0].projectile.GetBehavior<TravelStraitModel>().speed *= 3f;
        }
    }
    public class tier2Display : ModDisplay
    {
        public override string BaseDisplay => GetDisplay(TowerType.MonkeyBuccaneer, 0, 0, 2);
        public override void ModifyDisplayNode(UnityDisplayNode node)
        {
            SetMeshTexture(node, "tier2Display", 2);
        }
    }
    public class WaterJet : UpgradePlusPlus<FourthPath2>
    {
        public override int Cost => 2670;
        public override int Tier => 3;
        public override string Icon => VanillaSprites.WaterBloonPopIcon;
        public override string? Portrait => "tier3Portrait";
        public override string Description => "Propels razor sharp water bursts instead of darts, which push bloons back and damages them.";

        public override void ApplyUpgrade(TowerModel towerModel)
        {
            var attackModel = towerModel.GetAttackModel();
            towerModel.ApplyDisplay<tier3Display>();
            attackModel.weapons[0].rate *= 0.65f;
            attackModel.weapons[0].projectile.ApplyDisplay<waterDisplay>();
            attackModel.weapons[0].projectile.GetDamageModel().damage += 6f;
            attackModel.weapons[0].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
            attackModel.weapons[0].projectile.AddBehavior(new WindModel("windModel_", 5f, 10f, 100f, false, null, 0, null, 3));
        }
    }
    public class tier3Display : ModDisplay
    {
        public override string BaseDisplay => GetDisplay(TowerType.MonkeyBuccaneer, 0, 0, 2);
        public override void ModifyDisplayNode(UnityDisplayNode node)
        {
            SetMeshTexture(node, "tier3Display", 2);
        }
    }
    public class waterDisplay : ModDisplay
    {
        public override string BaseDisplay => Generic2dDisplay;
        public override void ModifyDisplayNode(UnityDisplayNode node)
        {
            Set2DTexture(node, "waterJet");
        }
    }
    public class HydroBlaster : UpgradePlusPlus<FourthPath2>
    {
        public override int Cost => 9000;
        public override int Tier => 4;

        public override string Icon => "hose";
        public override string? Portrait => "tier4Portrait";
        public override string Description => "More pierce and damage. Squelcher ability - Fires a constant stream of razor water, cutting up bloons like nothing.";

        public override void ApplyUpgrade(TowerModel towerModel)
        {
            var attackModel = towerModel.GetAttackModel();
            towerModel.ApplyDisplay<tier4Display>();
            attackModel.weapons[0].rate *= 0.65f;
            attackModel.weapons[0].projectile.pierce *= 2;
            attackModel.weapons[0].projectile.GetDamageModel().damage += 5f;
            var abilityModel = Game.instance.model.GetTowerFromId("BoomerangMonkey-040").GetAbility().Duplicate();
            abilityModel.RemoveBehavior<CreateSoundOnAbilityModel>();
            abilityModel.icon = towerModel.icon;
            abilityModel.GetBehavior<TurboModel>().projectileDisplay.assetPath = CreatePrefabReference<waterDisplay>();
            abilityModel.GetBehavior<TurboModel>().extraDamage = 1;
            abilityModel.GetBehavior<TurboModel>().multiplier *= 0.1f;
            towerModel.AddBehavior(abilityModel);
        }
    }
    public class tier4Display : ModDisplay
    {
        public override string BaseDisplay => GetDisplay(TowerType.MonkeyBuccaneer, 0, 0, 2);
        public override void ModifyDisplayNode(UnityDisplayNode node)
        {
            SetMeshTexture(node, "tier4Display", 2);
        }
    }
    public class FloodSprayer: UpgradePlusPlus<FourthPath2>
    {
        public override int Cost => 68000;
        public override int Tier => 5;

        public override string Icon => "hydroGun";
        public override string? Portrait => "tier5Portrait";
        public override string Description => "Can push back MOAB class, and deal alot of damage to them. Also has a constant stream.";

        public override void ApplyUpgrade(TowerModel towerModel)
        {
            var attackModel = towerModel.GetAttackModel();
            towerModel.ApplyDisplay<tier5Display>();
            attackModel.weapons[0].rate *= 0.1f;
            attackModel.weapons[0].projectile.AddBehavior(new DamageModifierForTagModel("moabs","Moabs",2f,10f,false,false));
            attackModel.weapons[0].projectile.GetDamageModel().damage *= 3;
            attackModel.weapons[0].projectile.GetBehavior<WindModel>().affectMoab = true;
            towerModel.GetAbility().GetBehavior<TurboModel>().extraDamage += 45;
            towerModel.GetAbility().cooldown *= 0.6f;
            towerModel.GetAbility().GetBehavior<TurboModel>().multiplier = 0.8f;
        }
    }
    public class tier5Display : ModDisplay
    {
        public override string BaseDisplay => GetDisplay(TowerType.MonkeyBuccaneer, 0, 4, 0);
        public override void ModifyDisplayNode(UnityDisplayNode node)
        {
            SetMeshTexture(node, "tier5Display", 1);
        }
    }
}