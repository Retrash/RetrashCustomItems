using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ItemAPI;

namespace Blunderbeast
{
    public class LockBullets : PassiveItem
    {

        //Call this method from the Start() method of your ETGModule extension
        public static void Init()
        {
            //The name of the item
            string itemName = "Locked Bullets";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "Blunderbeast/Resources/lockedbullets";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<LockBullets>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Unlockable Potential";
            string longDesc = "Increased damage for each key carried, up to a maximum of three.\n\n" +
                "Deemed too powerful by the Gungeon's Bullet Safety Department, an intricate lock system has been placed on these bullets to make them less lethal.\n\n" + "Very few locksmiths would be able to completely neutralize its mechanism.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "rtr");

            //Adds the actual passive effect to the item

            //Set the rarity of the item
            item.quality = PickupObject.ItemQuality.B;
            LockBulletsPickupID = item.PickupObjectId;
        }

        public static int LockBulletsPickupID;

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

        private bool noKey, oneKey, twoKeys, threeKeys, infiniKey, hasInfiniKey, hadInfiniKey, shouldRestat;
        private int currentKeys, lastKeys;



        private void EvaluateStats(PlayerController player, bool force = false)
        {
            this.currentKeys = player.carriedConsumables.KeyBullets;
            this.hasInfiniKey = (base.Owner.HasPickupID(166));
            noKey = (player.carriedConsumables.KeyBullets == 0 && !base.Owner.HasPickupID(166));
            oneKey = (player.carriedConsumables.KeyBullets == 1 && !base.Owner.HasPickupID(166));
            twoKeys = (player.carriedConsumables.KeyBullets == 2 && !base.Owner.HasPickupID(166));
            threeKeys = (player.carriedConsumables.KeyBullets >= 3 && !base.Owner.HasPickupID(166));
            infiniKey = (base.Owner.HasPickupID(166));
            this.shouldRestat = (this.currentKeys != this.lastKeys) | (this.hadInfiniKey != hasInfiniKey);
            bool flag = !this.shouldRestat && !force;
            if (!flag)
            {
                if (oneKey)
                {
                    RemoveStat(PlayerStats.StatType.Damage);
                    AddStat(PlayerStats.StatType.Damage, 1.1f, StatModifier.ModifyMethod.MULTIPLICATIVE);
                    this.lastKeys = this.currentKeys;
                    hadInfiniKey = false;
                }

                if (twoKeys)
                {
                    RemoveStat(PlayerStats.StatType.Damage);
                    AddStat(PlayerStats.StatType.Damage, 1.2f, StatModifier.ModifyMethod.MULTIPLICATIVE);
                    this.lastKeys = this.currentKeys;
                    hadInfiniKey = false;
                }

                if (threeKeys)
                {
                    RemoveStat(PlayerStats.StatType.Damage);
                    AddStat(PlayerStats.StatType.Damage, 1.3f, StatModifier.ModifyMethod.MULTIPLICATIVE);
                    this.lastKeys = this.currentKeys;
                    hadInfiniKey = false;
                }

                if (infiniKey)
                {
                    AkSoundEngine.PostEvent("Play_OBJ_goldenlock_open_01", this.gameObject);
                    LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetByName("Deadly Bullets").gameObject, player);
                    player.RemovePassiveItem(LockBulletsPickupID);
                    hadInfiniKey = true;
                }

                if (noKey)
                {
                    RemoveStat(PlayerStats.StatType.Damage);
                    this.lastKeys = this.currentKeys;
                    hadInfiniKey = false;
                }

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

