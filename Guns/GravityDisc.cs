using System;
using System.Collections;
using Gungeon;
using MonoMod;
using UnityEngine;

namespace Blunderbeast
{
    public class GravityDisc : GunBehaviour
    {

        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Hyper Disc", "hyperdisc");
            Game.Items.Rename("outdated_gun_mods:hyper_disc", "rtr:hyper_disc");
            gun.gameObject.AddComponent<GravityDisc>();
            gun.SetShortDescription("Two Steps Back");
            gun.SetLongDescription("Magnetized disc linked to the Gravity Glove. Can only be thrown.");
            gun.SetupSprite(null, "gravitydisc_idle_001", 8);
            gun.SetAnimationFPS(gun.shootAnimation, 14);
            gun.SetAnimationFPS(gun.reloadAnimation, 10);
            gun.AddProjectileModuleFrom("black_hole_gun", true, false);
            gun.SetBaseMaxAmmo(0);
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Charged;
            gun.damageModifier = 1;
            gun.reloadTime = 0f;
            gun.DefaultModule.cooldownTime = 0f;
            gun.DefaultModule.numberOfShotsInClip = 0;
            gun.DefaultModule.angleVariance = 0f;
            gun.quality = PickupObject.ItemQuality.SPECIAL;
            gun.encounterTrackable.EncounterGuid = "hyperdisc";
            gun.CanBeSold = false;
            gun.CanGainAmmo = false;
            gun.CanBeDropped = true;
            gun.IgnoredByRat = true;
            Gun component = PickupObjectDatabase.GetById(169) as Gun;
            gun.muzzleFlashEffects = component.muzzleFlashEffects;
            if (gun.ammo > 0)
            {
                gun.ammo = 0;
            }
            gun.encounterTrackable.m_doNotificationOnEncounter = false;
            ETGMod.Databases.Items.Add(gun, null, "ANY");
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

        public override void OnReloadPressed(PlayerController player, Gun gun, bool bSOMETHING)
        {
            if (gun.IsReloading && HasReloaded == true)
            {
                HasReloaded = false;
                AkSoundEngine.PostEvent("Stop_WPN_All", base.gameObject);
                base.OnReloadPressed(player, gun, bSOMETHING);
            }
        }


        public override void OnPostFired(PlayerController player, Gun gun)
        {
            AkSoundEngine.PostEvent("Play_WPN_unicornhorn_shot_01", base.gameObject);
        }

    }
}
