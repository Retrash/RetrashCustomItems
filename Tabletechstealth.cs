using System;
using UnityEngine;
using ItemAPI;
using System.Collections;

namespace Blunderbeast
{
    public class Tabletechstealth : PassiveItem
    {

        //Call this method from the Start() method of your ETGModule extension
        public static void Init()
        {
            //The name of the item
            string itemName = "Table Tech Stealth";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "Blunderbeast/Resources/tabletechstealth";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<Tabletechstealth>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Invisible Flips";
            string longDesc = "This long lost table technique will turn its user invisible when a table is flipped. \n\n" +
                "This chapter of the \"Tabla Sutra\" was banished to the depths of the Gungeon by the original founders of Flipjutsu. \n\n" +
                "The most wondrous flip is always the one you never notice. Embrace the silent tables and flip until only silence remains.";

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
            player.OnTableFlipped = (Action<FlippableCover>)Delegate.Combine(player.OnTableFlipped, new Action<FlippableCover>(this.StealthEffect));
        }

        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            player.OnTableFlipped = (Action<FlippableCover>)Delegate.Remove(player.OnTableFlipped, new Action<FlippableCover>(this.StealthEffect));
            return debrisObject;
        }

        private void StealthEffect(FlippableCover obj)
        {
            PlayerController player = this.Owner;
            this.BreakStealth(player);
            player.OnItemStolen += this.BreakStealthOnSteal;
            player.ChangeSpecialShaderFlag(1, 1f);
            player.healthHaver.OnDamaged += this.OnDamaged;
            player.SetIsStealthed(true, "table");
            player.SetCapableOfStealing(true, "table", null);
            GameManager.Instance.StartCoroutine(this.Unstealthy());
        }

        private IEnumerator Unstealthy()
        {
            PlayerController player = this.Owner;
            yield return new WaitForSeconds(0.15f);
            player.OnDidUnstealthyAction += this.BreakStealth;
            yield break;
        }

        private void OnDamaged(float resultValue, float maxValue, CoreDamageTypes damageTypes, DamageCategory damageCategory, Vector2 damageDirection)
        {
            PlayerController player = this.Owner;
            this.BreakStealth(player);
        }

        private void BreakStealthOnSteal(PlayerController arg1, ShopItemController arg2)
        {
            this.BreakStealth(arg1);
        }

        private void BreakStealth(PlayerController player)
        {
            player.ChangeSpecialShaderFlag(1, 0f);
            player.OnItemStolen -= this.BreakStealthOnSteal;
            player.SetIsStealthed(false, "table");
            player.healthHaver.OnDamaged -= this.OnDamaged;
            player.SetCapableOfStealing(false, "table", null);
            player.OnDidUnstealthyAction -= this.BreakStealth;
            AkSoundEngine.PostEvent("Play_ENM_wizardred_appear_01", base.gameObject);
        }
    }
}

