using System;
using System.Collections;
using Gungeon;
using ItemAPI;
using MonoMod;
using UnityEngine;

namespace Blunderbeast
{
    public class Infinigun : GunBehaviour
    {

        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Infinigun", "infinigun");
            Game.Items.Rename("outdated_gun_mods:infinigun", "rtr:infinigun");            
            Gun gun2 = PickupObjectDatabase.GetById(651) as Gun;
            gun.gameObject.AddComponent<Infinigun>();
            gun.SetShortDescription("Not Even Close");
            gun.SetLongDescription("\n\n" + "This prototype blaster is able to generate unlimited energy using the perpetual motion of its chamber, theoretically granting it infinite ammo.");
            gun.SetupSprite(null, "infinigun_idle_001", 8);
            gun.SetAnimationFPS(gun.shootAnimation, 14);
            gun.SetAnimationFPS(gun.reloadAnimation, 0);
            Gun defaultmoduleGun = PickupObjectDatabase.GetById(651) as Gun;
            gun.AddProjectileModuleFrom(defaultmoduleGun, true, false);
            gun.SetBaseMaxAmmo(1);
            gun.DefaultModule.ammoCost = 0;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Automatic;
            gun.damageModifier = 1;
            gun.InfiniteAmmo = true;
            gun.reloadTime = -1f;
            gun.DefaultModule.cooldownTime = 0.1f;
            gun.barrelOffset.transform.localPosition += new Vector3(0.25f, 0f, 0);
            gun.DefaultModule.numberOfShotsInClip = 1;
            gun.DefaultModule.angleVariance = gun2.DefaultModule.angleVariance;
            gun.quality = PickupObject.ItemQuality.B;
            gun.encounterTrackable.EncounterGuid = "infinigun";
            gun.gunClass = GunClass.FULLAUTO;
            gun.CanBeDropped = true;
            gun.DefaultModule.customAmmoType = gun2.DefaultModule.customAmmoType;
            gun.DefaultModule.ammoType = gun2.DefaultModule.ammoType;
            gun.muzzleFlashEffects = gun2.muzzleFlashEffects;
            ETGMod.Databases.Items.Add(gun, null, "ANY");

            //CREATES NEW PROJECTILE
            Projectile NewProjectileInf = Instantiate<Projectile>(defaultmoduleGun.DefaultModule.projectiles[0]);
            NewProjectileInf.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(NewProjectileInf.gameObject);
            DontDestroyOnLoad(NewProjectileInf);
            gun.DefaultModule.projectiles[0] = NewProjectileInf;
            NewProjectileInf.transform.parent = gun.barrelOffset;

            //SETS PROJECTILE STATS
            NewProjectileInf.baseData.damage = 3f;
            NewProjectileInf.AdditionalScaleMultiplier = 1.2f;
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
            gun.ClipShotsRemaining = 2;
            gun.GainAmmo(2);          
        }

        public override void OnPostFired(PlayerController player, Gun gun)
        {
            AkSoundEngine.PostEvent("Play_WPN_knav3_shot_01", base.gameObject);
            gun.ClearReloadData();
        }
    }
}
