using System;
using System.Linq;

namespace Blunderbeast
{
    public static class SynergySetup
    {
        public static bool PlayerHasActiveSynergy(this PlayerController player, string synergyNameToCheck)
        {
            foreach (int num in player.ActiveExtraSynergies)
            {
                AdvancedSynergyEntry advancedSynergyEntry = GameManager.Instance.SynergyManager.synergies[num];
                bool flag = advancedSynergyEntry.NameKey == synergyNameToCheck;
                if (flag)
                {
                    return true;
                }
            }
            return false;
        }

        public static void AddPassiveStatModifier(this Gun gun, PlayerStats.StatType statType, float amount, StatModifier.ModifyMethod modifyMethod)
        {
            gun.passiveStatModifiers = gun.passiveStatModifiers.Concat(new StatModifier[]
            {
                new StatModifier
                {
                    statToBoost = statType,
                    amount = amount,
                    modifyType = modifyMethod
                }
            }).ToArray<StatModifier>();
        }

        public static void AddCurrentGunStatModifier(this Gun gun, PlayerStats.StatType statType, float amount, StatModifier.ModifyMethod modifyMethod)
        {
            gun.currentGunStatModifiers = gun.currentGunStatModifiers.Concat(new StatModifier[]
            {
                new StatModifier
                {
                    statToBoost = statType,
                    amount = amount,
                    modifyType = modifyMethod
                }
            }).ToArray<StatModifier>();
        }

        public static void AddCurrentGunDamageTypeModifier(this Gun gun, CoreDamageTypes damageTypes, float damageMultiplier)
        {
            gun.currentGunDamageTypeModifiers = gun.currentGunDamageTypeModifiers.Concat(new DamageTypeModifier[]
            {
                new DamageTypeModifier
                {
                    damageType = damageTypes,
                    damageMultiplier = damageMultiplier
                }
            }).ToArray<DamageTypeModifier>();
        }
    }
}
