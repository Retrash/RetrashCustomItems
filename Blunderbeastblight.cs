using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ItemAPI;

namespace Blunderbeast
{
    public class Blunderbeastblight : PassiveItem
    {
        //Call this method from the Start() method of your ETGModule extension
        public static void Init()
        {
            //The name of the item
            string itemName = "Blunderbeast Blight";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "Blunderbeast/Resources/blunderbeast";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<Blunderbeastblight>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Unleash It";
            string longDesc = "When unarmored, gain great power at a cost.\n\n" +
                "A curse that slowly turns its bearer into a terrible beast. Many tried to contain it but, misled by the whispers promising them strength, all of them eventually succumbed to the temptation.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "rtr");

            //Adds the actual passive effect to the item

            //Set the rarity of the item
            item.quality = PickupObject.ItemQuality.A;
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            EvaluateStats(player, true);
        }

        public override DebrisObject Drop(PlayerController player)
        {
            EvaluateStats(player, true);
            return base.Drop(player);
        }

        protected override void Update()
        {
            if (Owner)
            {
                EvaluateStats(Owner);
            }

            else { return; }
        }

        private bool hasArmor, hadArmor, shouldRestat;
        private void EvaluateStats(PlayerController player, bool force = false)
        {
            hasArmor = (player.healthHaver.Armor > 0 || player.healthHaver.HasCrest);
            shouldRestat = (hadArmor != hasArmor);
            if (!(shouldRestat || force)) return; //don't restat if player already has correct stats

            if (hasArmor)
            {
                AddStat(PlayerStats.StatType.Coolness, 3f);

                RemoveStat(PlayerStats.StatType.Damage);
                RemoveStat(PlayerStats.StatType.RateOfFire);
                RemoveStat(PlayerStats.StatType.Accuracy);
                RemoveStat(PlayerStats.StatType.Curse);
                RemoveStat(PlayerStats.StatType.MovementSpeed);
                hadArmor = true;
                player.stats.RecalculateStats(player, true, false);

            }
            else
            {
                AddStat(PlayerStats.StatType.Damage, 1.2f, StatModifier.ModifyMethod.MULTIPLICATIVE);
                AddStat(PlayerStats.StatType.RateOfFire, 1.2f, StatModifier.ModifyMethod.MULTIPLICATIVE);
                AddStat(PlayerStats.StatType.Accuracy, 1.25f, StatModifier.ModifyMethod.MULTIPLICATIVE);
                AddStat(PlayerStats.StatType.Curse, 3f);
                AddStat(PlayerStats.StatType.MovementSpeed, 1.5f, StatModifier.ModifyMethod.ADDITIVE);

                RemoveStat(PlayerStats.StatType.Coolness);
                hadArmor = false;
                player.stats.RecalculateStats(player, true, false);
            }

            
        }

        private void AddStat(PlayerStats.StatType statType, float amount, StatModifier.ModifyMethod method = StatModifier.ModifyMethod.ADDITIVE)
        {
            foreach (var m in passiveStatModifiers)
            {
                if (m.statToBoost == statType) return; //don't add duplicates
            }

            StatModifier modifier = new StatModifier
            {
                amount = amount,
                statToBoost = statType,
                modifyType = method
            };

            if (this.passiveStatModifiers == null)
                this.passiveStatModifiers = new StatModifier[] { modifier };
            else
                this.passiveStatModifiers = this.passiveStatModifiers.Concat(new StatModifier[] { modifier }).ToArray();
        }

        private void RemoveStat(PlayerStats.StatType statType)
        {
            var newModifiers = new List<StatModifier>();
            for (int i = 0; i < passiveStatModifiers.Length; i++)
            {
                if (passiveStatModifiers[i].statToBoost != statType)
                    newModifiers.Add(passiveStatModifiers[i]);
            }
            this.passiveStatModifiers = newModifiers.ToArray();
        }
    }
}
