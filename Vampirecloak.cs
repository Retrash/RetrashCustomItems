using UnityEngine;
using ItemAPI;
using System;

namespace Blunderbeast
{
    public class Vampirecloak : PassiveItem
    {
        public static int VampirecloakPickupID;

        //Call this method from the Start() method of your ETGModule extension
        public static void Init()
        {
            //The name of the item
            string itemName = "Vampire Cloak";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "Blunderbeast/Resources/vampirecloak";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<Vampirecloak>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Twilight Mantle";
            string longDesc = "Bestows vampiric powers.\n\n" +
                "Living cloak lost by a powerful vampire trapped in the Gungeon. Contrary to popular beliefs, vampires really are just regular undeads wearing enchanted cloaks.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "rtr");

            //Adds the actual passive effect to the item
            //Set the rarity of the item
            item.quality = PickupObject.ItemQuality.B;
            item.CanBeDropped = false;

            VampirecloakPickupID = item.PickupObjectId;
        }

        public override void Pickup(PlayerController player)
        {
            Effect(player);
            base.Pickup(player);
        }

        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            return debrisObject;
        }


        private void Effect(PlayerController player)
        {
            player.gameObject.AddComponent<Smallbloodthirst>();
        }
    }
}