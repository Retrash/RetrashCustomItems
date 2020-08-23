using System;
using System.Collections;
using Gungeon;
using ItemAPI;
using MonoMod;
using UnityEngine;

namespace Blunderbeast
{
    public class GrenadeOnAStick : GunBehaviour
    {



        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Pinhead On A Stick", "grenadeonastick");
            Game.Items.Rename("outdated_gun_mods:pinhead_on_a_stick", "rtr:pinhead_on_a_stick");
            gun.gameObject.AddComponent<GrenadeOnAStick>();
            gun.SetShortDescription("How'd You Find This?");
            gun.SetLongDescription("Thank you for downloading my mod, enjoy this totally stupid gun I hid in here.");
            gun.SetupSprite(null, "grenadeonastick_idle_001", 8);
            gun.SetAnimationFPS(gun.idleAnimation, 1);
            gun.SetAnimationFPS(gun.shootAnimation, 14);
            gun.SetAnimationFPS(gun.reloadAnimation, 10);
            gun.AddProjectileModuleFrom("klobb", true, true);
            gun.SetBaseMaxAmmo(200);
            gun.OverrideAngleSnap = 1;
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.damageModifier = 1;
            gun.DefaultModule.cooldownTime = 0.09f;
            gun.DefaultModule.numberOfShotsInClip = 1;
            gun.DefaultModule.angleVariance = 0f;
            gun.barrelOffset.transform.localPosition += new Vector3(0, 0, 0);
            gun.quality = PickupObject.ItemQuality.EXCLUDED;
   
            gun.encounterTrackable.EncounterGuid = "grenadeonastick";
            gun.gunClass = GunClass.SILLY;
            gun.CanBeDropped = true;
            gun.muzzleFlashEffects = null;
            ETGMod.Databases.Items.Add(gun, null, "ANY");

            //CREATES NEW PROJECTILE
            //Projectile NewProjectileOrnate = Instantiate<Projectile>(duelingpistol.DefaultModule.projectiles[0]);
            //NewProjectileOrnate.gameObject.SetActive(false);
            //FakePrefab.MarkAsFakePrefab(NewProjectileOrnate.gameObject);
            //DontDestroyOnLoad(NewProjectileOrnate);
            //gun.DefaultModule.projectiles[0] = NewProjectileOrnate;
            //NewProjectileOrnate.transform.parent = gun.barrelOffset;

            //SETS PROJECTILE STATS
            //NewProjectileOrnate.baseData.damage = 5.2f;
            //NewProjectileOrnate.baseData.speed *= 0.9f;
            //NewProjectileOrnate.baseData.force *= 0.5f;
            //NewProjectileOrnate.AdditionalScaleMultiplier = 1f;
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

        //public override void OnReloadPressed(PlayerController player, Gun gun, bool bSOMETHING)
        //{
        //    AkSoundEngine.PostEvent("Stop_WPN_All", base.gameObject);          
        //    base.OnReloadPressed(player, gun, bSOMETHING);
        //    AkSoundEngine.PostEvent("Play_WPN_duelingpistol_reload_01", base.gameObject);
        //}

        public override void OnPostFired(PlayerController player, Gun gun)
        {
            Exploder.DoDefaultExplosion(gun.sprite.WorldBottomRight, Vector2.zero, null, false, CoreDamageTypes.None, false);
            AkSoundEngine.PostEvent("Play_BOSS_DragunGold_Grenade_01", base.gameObject);
        }
    }
}
