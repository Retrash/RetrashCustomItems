using UnityEngine;
using ItemAPI;

namespace Blunderbeast
{
    public class BarrelCharger : PassiveItem
    {

        //Call this method from the Start() method of your ETGModule extension
        public static void Init()
        {
            //The name of the item
            string itemName = "Storm Charger";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "Blunderbeast/Resources/barrelcharger";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<BarrelCharger>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Instant Transmission";
            string longDesc = "Charged guns no longer need to charge.\n\n" +
                "This was once the magazine of a gun that unleashed a heavy storm upon the Gungeon. All of its other components were eventually dismantled.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "rtr");

            //Adds the actual passive effect to the item

            //Set the rarity of the item
            item.quality = PickupObject.ItemQuality.A;
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.ChargeAmountMultiplier, 100000f, StatModifier.ModifyMethod.MULTIPLICATIVE);
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);

            foreach (Gun gun in Owner.inventory.AllGuns)
            {
                if (gun.DefaultModule.shootStyle == ProjectileModule.ShootStyle.Beam)
                {
                    BeamController beamController = gun.DefaultModule.projectiles[0].GetComponent<BeamController>();
                    if (beamController != null && beamController.usesChargeDelay == true)
                    {
                        beamController.usesChargeDelay = false;
                    }
                }

                if (Owner.inventory.ContainsGun(153) || Owner.inventory.ContainsGun(13))
                {
                    if ((gun.PickupObjectId.Equals(153) || gun.PickupObjectId.Equals(13)))
                    {
                        if (gun.DefaultModule.cooldownTime != 0.1f)
                        {
                            gun.DefaultModule.cooldownTime = 0.1f;
                        }
                        if (gun.DefaultModule.numberOfShotsInClip != 200)
                        {
                            gun.DefaultModule.numberOfShotsInClip = 200;
                        }
                        if (gun.reloadTime != 0)
                        {
                            gun.reloadTime = 0;
                        }
                        if (gun.DefaultModule.shootStyle != ProjectileModule.ShootStyle.Automatic)
                        {
                            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Automatic;
                        }
                    }
                }
            }

            this.m_electricityImmunity = new DamageTypeModifier();
            this.m_electricityImmunity.damageMultiplier = 0f;
            this.m_electricityImmunity.damageType = CoreDamageTypes.Electric;
            player.healthHaver.damageTypeModifiers.Add(this.m_electricityImmunity);
        }

        protected override void Update()
        {
            if (Owner)
            {

                foreach (Gun gun in Owner.inventory.AllGuns)
                {
                    if (gun.DefaultModule.shootStyle == ProjectileModule.ShootStyle.Beam)
                    {
                        BeamController beamController = gun.DefaultModule.projectiles[0].GetComponent<BeamController>();
                        if (beamController != null && beamController.usesChargeDelay == true)
                        {
                            beamController.usesChargeDelay = false;
                        }
                    }

                    if (Owner.inventory.ContainsGun(153) || Owner.inventory.ContainsGun(13))
                    {
                        if ((gun.PickupObjectId.Equals(153) || gun.PickupObjectId.Equals(13)))
                        {
                            if (gun.DefaultModule.cooldownTime != 0.1f)
                            {
                                gun.DefaultModule.cooldownTime = 0.1f;
                            }
                            if (gun.DefaultModule.numberOfShotsInClip != 200)
                            {
                                gun.DefaultModule.numberOfShotsInClip = 200;
                            }
                            if (gun.reloadTime != 0)
                            {
                                gun.reloadTime = 0;
                            }
                            if (gun.DefaultModule.shootStyle != ProjectileModule.ShootStyle.Automatic)
                            {
                                gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Automatic;
                            }

                        }
                    }
                }
            }
        }

        public override DebrisObject Drop(PlayerController player)
        {

            foreach (Gun gun in Owner.inventory.AllGuns)
            {
                if (gun.DefaultModule.shootStyle == ProjectileModule.ShootStyle.Beam)
                {
                    BeamController beamController = gun.DefaultModule.projectiles[0].GetComponent<BeamController>();
                    if (beamController != null)
                    {
                        beamController.usesChargeDelay = true;
                    }
                }

                if (Owner.inventory.ContainsGun(153) || Owner.inventory.ContainsGun(13))
                {
                    if (gun.PickupObjectId.Equals(153))
                    {
                        Gun shockGun1 = PickupObjectDatabase.GetById(153) as Gun;
                        gun.DefaultModule.cooldownTime = shockGun1.DefaultModule.cooldownTime;
                        gun.DefaultModule.numberOfShotsInClip = shockGun1.DefaultModule.numberOfShotsInClip;
                        gun.reloadTime = shockGun1.reloadTime;
                        gun.DefaultModule.shootStyle = shockGun1.DefaultModule.shootStyle;
                    }

                    if (gun.PickupObjectId.Equals(13))
                    {
                        Gun shockGun2 = PickupObjectDatabase.GetById(13) as Gun;
                        gun.DefaultModule.cooldownTime = shockGun2.DefaultModule.cooldownTime;
                        gun.DefaultModule.numberOfShotsInClip = shockGun2.DefaultModule.numberOfShotsInClip;
                        gun.reloadTime = shockGun2.reloadTime;
                        gun.DefaultModule.shootStyle = shockGun2.DefaultModule.shootStyle;
                    }
                }
            }
            DebrisObject debrisObject = base.Drop(player);
            player.healthHaver.damageTypeModifiers.Remove(this.m_electricityImmunity);
            return debrisObject;
        }

        public bool ConfersElectricityImmunity;
        private DamageTypeModifier m_electricityImmunity;

    }
}

