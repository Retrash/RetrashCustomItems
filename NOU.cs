using UnityEngine;
using ItemAPI;
using Dungeonator;
using System.Collections;
using System.Text;
using System;

namespace Blunderbeast
{
    public class NOU : PassiveItem
    {

        //Call this method from the Start() method of your ETGModule extension
        public static void Init()
        {
            //The name of the item
            string itemName = "No-U";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "Blunderbeast/Resources/NOU";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<NOU>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Blacklisted";
            string longDesc = "Abstract concept made literal.\n\n" +
                "Increases the damage of every gun that does not contain the letter U in its name.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "rtr");

            //Adds the actual passive effect to the item

            //Set the rarity of the item
            item.quality = PickupObject.ItemQuality.C;
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.PostProcessProjectile += this.PostProcessProjectile;
            player.PostProcessBeam += this.PostProcessBeam;
        }

        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            player.PostProcessProjectile -= this.PostProcessProjectile;
            player.PostProcessBeam -= this.PostProcessBeam;
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

        private void PostProcessProjectile(Projectile sourceProjectile, float effectChanceScalar)
        {
            PlayerController player = this.Owner;
            string gunName = player.CurrentGun.EncounterNameOrDisplayName;
            if (gunName.Contains("u") || gunName.Contains("U"))
            {
                sourceProjectile.baseData.speed *= 0.5f;
            }

            else
            {
                if (player.HasPickupID(340))
                {
                    foreach (char character in gunName.ToCharArray())
                    {
                        if (character == 'r')
                        {
                            sourceProjectile.baseData.damage *= 1.1f;
                        }
                    }
                }

                sourceProjectile.baseData.damage *= 1.25f;

            }
        }

        private void PostProcessBeam(BeamController beam)
        {
            if (beam)
            {
                Projectile projectile = beam.projectile;
                if (projectile)
                {
                    this.PostProcessProjectile(projectile, 1f);
                }
            }
        }
    }
}

