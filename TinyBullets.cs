using UnityEngine;
using ItemAPI;

namespace Blunderbeast
{
    public class Tinybullets : PassiveItem
    {

        //Call this method from the Start() method of your ETGModule extension
        public static void Init()
        {
            //The name of the item
            string itemName = "Tiny Bullets";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "Blunderbeast/Resources/tinybullets";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<Tinybullets>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "The Exception";
            string longDesc = "Makes bullets tiny and much faster.\n\n" +
                "These ant-sized bullets have a bad habit of creating tiny holes in most of the Gungeon's furniture.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "rtr");

            //Adds the actual passive effect to the item

            //Set the rarity of the item
            item.quality = PickupObject.ItemQuality.C;
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.PlayerBulletScale, 0.5f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.ProjectileSpeed, 1.5f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.RateOfFire, 1.15f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.KnockbackMultiplier, 0.35f, StatModifier.ModifyMethod.MULTIPLICATIVE);
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);

            player.PostProcessProjectile += this.PostProcessProjectile;

            foreach (Gun gun in Owner.inventory.AllGuns)
            {
                if (Owner.inventory.ContainsGun(180))
                {
                    if (gun.PickupObjectId.Equals(180))
                    {
                        gun.DefaultModule.numberOfShotsInClip = 2;
                        gun.SetBaseMaxAmmo(60);
                    }
                }
            }
        }

        public override DebrisObject Drop(PlayerController player)
        {


            foreach (Gun gun in Owner.inventory.AllGuns)
            {
                if (Owner.inventory.ContainsGun(180))
                {
                    if (gun.PickupObjectId.Equals(180))
                    {
                        Gun cricketGun = PickupObjectDatabase.GetById(180) as Gun;
                        int maxAmmo = cricketGun.AdjustedMaxAmmo;
                        gun.DefaultModule.numberOfShotsInClip = cricketGun.DefaultModule.numberOfShotsInClip;
                        gun.SetBaseMaxAmmo(maxAmmo);
                    }
                }
            }
            DebrisObject debrisObject = base.Drop(player);
            player.PostProcessProjectile -= this.PostProcessProjectile;
            return debrisObject;
        }

        private void PostProcessProjectile(Projectile sourceProjectile, float effectChanceScalar)
        {
            PlayerController player = this.Owner;
            if (player.CurrentGun.PickupObjectId.Equals(251))
            {
                sourceProjectile.AdditionalScaleMultiplier *= 2f;
            }

            if (player.HasPickupID(277))
            {
                float value = UnityEngine.Random.Range(0.0f, 1.0f);
                bool flag = value < 0.2;
                if (flag)
                {
                    sourceProjectile.AdditionalScaleMultiplier *= 0.5f;
                    sourceProjectile.baseData.damage *= 0.8f;
                    sourceProjectile.baseData.speed *= 1.1f;
                    sourceProjectile.baseData.force *= 1f;
                }
                else
                {

                    bool flag2 = value < 0.4;
                    if (flag2)
                    {
                        sourceProjectile.AdditionalScaleMultiplier *= 0.75f;
                        sourceProjectile.baseData.damage *= 0.9f;
                        sourceProjectile.baseData.speed *= 1.05f;
                        sourceProjectile.baseData.force *= 1.2f;
                    }
                    else
                    {

                        bool flag3 = value < 0.6;
                        if (flag3)
                        {
                            sourceProjectile.AdditionalScaleMultiplier *= 1f;
                            sourceProjectile.baseData.damage *= 1f;
                            sourceProjectile.baseData.speed *= 1f;
                            sourceProjectile.baseData.force *= 1.4f;
                        }
                        else
                        {

                            bool flag4 = value < 0.8;
                            if (flag4)
                            {
                                sourceProjectile.AdditionalScaleMultiplier *= 1.5f;
                                sourceProjectile.baseData.damage *= 1.1f;
                                sourceProjectile.baseData.speed *= 0.8f;
                                sourceProjectile.baseData.force *= 1.6f;
                            }
                            else
                            {
                                sourceProjectile.AdditionalScaleMultiplier *= 2.1f;
                                sourceProjectile.baseData.damage *= 1.2f;
                                sourceProjectile.baseData.speed *= 0.6f;
                                sourceProjectile.baseData.force *= 1.8f;
                            }
                        }
                    }
                }
            }

            else { return; }
        }


        protected override void Update()
        {
            if (Owner)
            {

                foreach (Gun gun in Owner.inventory.AllGuns)
                {
                    if (Owner.inventory.ContainsGun(180))
                    {
                        if (gun.PickupObjectId.Equals(180))
                        {
                            if (gun.DefaultModule.numberOfShotsInClip != 2)
                            {
                                gun.DefaultModule.numberOfShotsInClip = 2;
                            }
                            if (gun.AdjustedMaxAmmo != 60)
                            {
                                gun.SetBaseMaxAmmo(60);
                            }
                        }
                    }
                }
            }
        }
    }
}


