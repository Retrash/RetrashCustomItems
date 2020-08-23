using UnityEngine;
using ItemAPI;
using Dungeonator;
using System.Collections;

namespace Blunderbeast
{
    public class CounterfeitCrown : PassiveItem
    {

        //Call this method from the Start() method of your ETGModule extension
        public static void Init()
        {
            //The name of the item
            string itemName = "Charlatan's Crown";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "Blunderbeast/Resources/counterfeitcrown";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<CounterfeitCrown>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Fake It Till You Make It";
            string longDesc = "Provides chamber masteries to the undeserving, lowers bravery.\n\n" +
                "This wooden crown was adorned by an infamous king who often told tall tales about his adventures.\n\n" +
                "He was sent to the Gungeon after someone revealed the truth about his so-called feats. Even now, he still searches for someone gullible enough to believe in his stories.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "rtr");

            //Adds the actual passive effect to the item

            //Set the rarity of the item
            item.quality = PickupObject.ItemQuality.C;
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.DamageToBosses, 0.7f, StatModifier.ModifyMethod.MULTIPLICATIVE);
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.healthHaver.OnDamaged += this.OnDamaged;
        }

        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            player.healthHaver.OnDamaged -= this.OnDamaged;
            return debrisObject;
        }

        protected override void Update()
        {
            if (Owner)
            {
                if (Owner.IsInCombat && this.CanBeDropped == true)
                {
                    this.CanBeDropped = false;
                }

                else if (!Owner.IsInCombat && this.CanBeDropped == false)
                {
                    this.CanBeDropped = true;
                }
            }
        }

        private void OnDamaged(float resultValue, float maxValue, CoreDamageTypes damageTypes, DamageCategory damageCategory, Vector2 damageDirection)
        {
            GameManager.Instance.StartCoroutine(this.SaveFlawless());
        }

        private IEnumerator SaveFlawless()
        {
            yield return new WaitForSeconds(0.1f);
            PlayerController player = this.Owner;
            if (player.CurrentRoom != null)
            {
                player.CurrentRoom.PlayerHasTakenDamageInThisRoom = false;
            }
            yield break;
        }
    }
}

