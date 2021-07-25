using UnityEngine;
using ItemAPI;
using Dungeonator;
using System.Collections;

namespace Blunderbeast
{
    public class FragGrenade : PlayerItem
    {
        public static void Init()
        {
            //The name of the item
            string itemName = "Frag Grenade";

            //Refers to an embedded png in the project. Make sure to embed your resources!
            string resourceName = "Blunderbeast/Resources/sparegrenade2";

            //Create new GameObject
            GameObject obj = new GameObject();

            //Add a ActiveItem component to the object
            var item = obj.AddComponent<FragGrenade>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Catch!";
            string longDesc = "Pull the pin and throw, it's not exactly rocket science.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //"example_pool" here is the item pool. In the console you'd type "give example_pool:sweating_bullets"
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "rtr");

            //Set the cooldown type and duration of the cooldown
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Damage, 170);

            //Adds a passive modifier, like curse, coolness, damage, etc. to the item. Works for passives and actives.
            //Set some other fields
            item.consumable = false;
            item.UsesNumberOfUsesBeforeCooldown = true;
            item.numberOfUses = 3;
            item.quality = PickupObject.ItemQuality.C;
        }

        private bool hasSynergy, OnCooldown, CanShootNade;

        public override void Pickup(PlayerController player)
        {
            CanShootNade = true;
            OnCooldown = false;
            base.Pickup(player);
        }

        public override void Update()
        {
            if (LastOwner)
            {
                if (!LastOwner.HasPickupID(541))
                {
                    hasSynergy = false;
                }

                if (LastOwner.CurrentGun.PickupObjectId == 541)
                {
                    if (LastOwner.CurrentGun.ClipShotsRemaining > 0)
                    {
                        CanShootNade = true;
                    }
                }

                foreach (Gun gun in LastOwner.inventory.AllGuns)
                {
                    if (gun.PickupObjectId == 541 && hasSynergy == false)
                    {
                        hasSynergy = true;
                        LastOwner.CurrentGun.OnFinishAttack += OnPostFired;
                    }

                    if (gun.PickupObjectId == 19)
                    {
                        if (gun.CurrentAmmo != 0)
                        {
                            this.numberOfUses = (gun.CurrentAmmo + 3);
                        }
                        else if (this.m_cachedNumberOfUses != 3)
                        {
                            this.m_cachedNumberOfUses = 3;
                        }
                    }
                }
            }
        }

        protected override void OnPreDrop(PlayerController user)
        {
            hasSynergy = false;

            foreach (Gun gun in LastOwner.inventory.AllGuns)
            {
                if (gun.PickupObjectId == 541)
                {
                    LastOwner.CurrentGun.OnFinishAttack -= OnPostFired;
                }
            }
        }

        public DebrisObject Drop(PlayerController user)
        {
            DebrisObject debrisObject = base.Drop(user);
            hasSynergy = false;

            foreach (Gun gun in LastOwner.inventory.AllGuns)
            {
                if (gun.PickupObjectId == 541)
                {
                    LastOwner.CurrentGun.OnFinishAttack -= OnPostFired;
                }
            }
            return debrisObject;
        }


        private void OnPostFired(PlayerController player, Gun gun)
        {
            if (player && gun.PickupObjectId == 541 && gun.ClipShotsRemaining == 0 && CanShootNade == true)
            {
                Projectile projectile2 = ((Gun)ETGMod.Databases.Items[19]).DefaultModule.projectiles[0];
                Vector3 vector = player.unadjustedAimPoint - player.LockedApproximateSpriteCenter;
                Vector3 vector2 = player.specRigidbody.UnitCenter;
                GameObject gameObject = SpawnManager.SpawnProjectile(projectile2.gameObject, player.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (player.CurrentGun == null) ? 0f : player.CurrentGun.CurrentAngle), true);
                Projectile component = gameObject.GetComponent<Projectile>();
                bool flag = component != null;
                if (flag)
                {
                    ExplosiveModifier explosive = component.GetComponent<ExplosiveModifier>();
                    explosive.explosionData.damage *= 0.8f;
                    component.baseData.damage *= 0.8f;
                    component.Owner = player;
                    component.Shooter = player.specRigidbody;
                }
                CanShootNade = false;
            }
        }


        protected override void DoEffect(PlayerController user)
        {
            OnCooldown = true;
            GameManager.Instance.StartCoroutine(NadeCooldown());

            if (user.HasPickupID(19))
            {
                foreach (Gun gun in LastOwner.inventory.AllGuns)
                {
                    if (gun.PickupObjectId == 19 && gun.CurrentAmmo > 0)
                    {
                        gun.ammo -= 1;
                    }
                }
            }

            AkSoundEngine.PostEvent("Play_BOSS_Punchout_Swing_Right_01", base.gameObject);

            if (user.HasPickupID(39) || user.HasPickupID(398))
            {
                Projectile projectile = ((Gun)ETGMod.Databases.Items[39]).DefaultModule.projectiles[0];
                Vector3 vector = user.unadjustedAimPoint - user.LockedApproximateSpriteCenter;
                Vector3 vector2 = user.specRigidbody.UnitCenter;
                GameObject gameObject = SpawnManager.SpawnProjectile(projectile.gameObject, user.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (user.CurrentGun == null) ? 0f : user.CurrentGun.CurrentAngle), true);
                Projectile component = gameObject.GetComponent<Projectile>();
                bool flag = component != null;
                if (flag)
                {
                    component.Owner = user;
                    component.Shooter = user.specRigidbody;
                }
            }

            else
            {
                Projectile projectile = ((Gun)ETGMod.Databases.Items[19]).DefaultModule.projectiles[0];
                Vector3 vector = user.unadjustedAimPoint - user.LockedApproximateSpriteCenter;
                Vector3 vector2 = user.specRigidbody.UnitCenter;
                GameObject gameObject = SpawnManager.SpawnProjectile(projectile.gameObject, user.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (user.CurrentGun == null) ? 0f : user.CurrentGun.CurrentAngle), true);
                Projectile component = gameObject.GetComponent<Projectile>();
                bool flag = component != null;
                if (flag)
                {
                    component.Owner = user;
                    component.Shooter = user.specRigidbody;
                }
            }

            
        }

        private IEnumerator NadeCooldown()
        {
            yield return new WaitForSeconds(0.2f);
            OnCooldown = false;
            yield break;
        }



        public override bool CanBeUsed(PlayerController user)
        {
            return OnCooldown == false;
        }
    }
}