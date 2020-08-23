using System;
using System.Collections;
using Gungeon;
using ItemAPI;
using MonoMod;
using UnityEngine;

namespace Blunderbeast
{
    public class Leafblower : GunBehaviour
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Verdant Blaster", "leafblower");
            Game.Items.Rename("outdated_gun_mods:verdant_blaster", "rtr:verdant_blaster");
            gun.gameObject.AddComponent<Leafblower>();
            gun.SetShortDescription("Leaf Storm");
            gun.SetLongDescription("Gun carved from the tree branch of a Great Oak.\n\n" +
                "The deadliness of its leaves is not to be underestimated inside the Gungeon.");
            gun.SetupSprite(null, "leafblower_idle_001", 8);
            gun.SetAnimationFPS(gun.shootAnimation, 12);
            gun.SetAnimationFPS(gun.reloadAnimation, 1);
            Gun targetGun = PickupObjectDatabase.GetById(339) as Gun;
            Gun targetGun2 = PickupObjectDatabase.GetById(620) as Gun;
            gun.AddProjectileModuleFrom("klobb", true, false);
            gun.SetBaseMaxAmmo(500);

            gun.DefaultModule.customAmmoType = targetGun.DefaultModule.customAmmoType;
            gun.DefaultModule.ammoType = targetGun.DefaultModule.ammoType;
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Automatic;
            gun.damageModifier = 1;
            gun.reloadTime = 2f;
            gun.barrelOffset.transform.localPosition += new Vector3(0.2f, -0.1f, 0);
            gun.DefaultModule.cooldownTime = 0.07f;
            gun.DefaultModule.numberOfShotsInClip = 100;
            gun.DefaultModule.angleVariance = 10f;
            gun.quality = PickupObject.ItemQuality.B;
            gun.encounterTrackable.EncounterGuid = "leafblower";
            gun.gunClass = GunClass.FULLAUTO;
            gun.CanBeDropped = true;
            Gun component = PickupObjectDatabase.GetById(339) as Gun;
            gun.muzzleFlashEffects = component.muzzleFlashEffects;
            ETGMod.Databases.Items.Add(gun, null, "ANY");

            //CREATES NEW PROJECTILE
            Projectile NewProjectileLeaf = Instantiate<Projectile>(targetGun2.DefaultModule.chargeProjectiles[0].Projectile);
            NewProjectileLeaf.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(NewProjectileLeaf.gameObject);
            DontDestroyOnLoad(NewProjectileLeaf);
            gun.DefaultModule.projectiles[0] = NewProjectileLeaf;
            NewProjectileLeaf.transform.parent = gun.barrelOffset;

            //SETS PROJECTILE STATS
            NewProjectileLeaf.AdditionalScaleMultiplier *= 1.1f;
        }

        private bool HasReloaded;

        protected void Update()
        {
            PlayerController player = gun.CurrentOwner as PlayerController;

            if (player && player != null)
            {
                if (gun.PreventNormalFireAudio != true)
                {
                    gun.PreventNormalFireAudio = true;
                }

                if (!gun.IsReloading && HasReloaded == false)
                {
                    HasReloaded = true;
                }
            }
        }

        public override void PostProcessProjectile(Projectile projectile)
        {
            PlayerController playerController = this.gun.CurrentOwner as PlayerController;
            bool flag = playerController == null;
            if (flag)
            {
                this.gun.ammo = this.gun.GetBaseMaxAmmo();
            }
            this.gun.DefaultModule.ammoCost = 1;
        }

        public override void OnReloadPressed(PlayerController player, Gun gun, bool bSOMETHING)
        {
            if (gun.IsReloading && HasReloaded == true)
            {
                HasReloaded = false;
                AkSoundEngine.PostEvent("Stop_WPN_All", base.gameObject);
                base.OnReloadPressed(player, gun, bSOMETHING);
                AkSoundEngine.PostEvent("Play_WPN_dartgun_reload_01", base.gameObject);
            }
        }

        public override void OnPostFired(PlayerController player, Gun gun)
        {
            AkSoundEngine.PostEvent("Play_WPN_zorgun_shot_01", base.gameObject);
        }
    }
}
