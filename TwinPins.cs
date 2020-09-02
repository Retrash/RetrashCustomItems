using UnityEngine;
using ItemAPI;
using System.Linq;

namespace Blunderbeast
{
    public class TwinPins : PassiveItem
    {

        //Call this method from the Start() method of your ETGModule extension
        public static void Init()
        {
            //The name of the item
            string itemName = "Twin Pins";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "Blunderbeast/Resources/twinbadges-small";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<TwinPins>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "The Bro Code";
            string longDesc = "A small label is attached : \n\n" +
                "\"Support your local economy and one day you'll be as cool as us. -Shades & Smiley.\"";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "rtr");

            //Adds the actual passive effect to the item

            //Set the rarity of the item
            item.quality = PickupObject.ItemQuality.D;
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.OnItemPurchased += this.OnItemPurchased;
        }


        private void OnItemPurchased(PlayerController player, ShopItemController obj)
        {
            AkSoundEngine.PostEvent("Play_WPN_radgun_cool_01", base.gameObject);
           

            AddStat(PlayerStats.StatType.Coolness, 0.5f);

            if (base.Owner.HasPickupID(22))
            {
                AddStat(PlayerStats.StatType.Coolness, 0.5f);
            }

            if (base.Owner.HasPickupID(35))
            {
                LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(70).gameObject, player);
            }

            if (base.Owner.HasPickupID(148))
            {
                float value = UnityEngine.Random.Range(0.0f, 1.0f);
                if (value < 0.5)
                {
                    LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(127).gameObject, player);
                }
            }

            player.stats.RecalculateStats(player, true, false);
        }

        private void AddStat(PlayerStats.StatType statType, float amount, StatModifier.ModifyMethod method = StatModifier.ModifyMethod.ADDITIVE)
        {
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

        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            player.OnItemPurchased -= this.OnItemPurchased;
            return debrisObject;
        }
    }
}


