using System;
using System.Collections;
using Gungeon;
using ItemAPI;
using MonoMod;
using UnityEngine;

namespace Blunderbeast
{
    public class OrnatePistol : GunBehaviour
    {



        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Ornate Pistol", "ornatepistol");
            Game.Items.Rename("outdated_gun_mods:ornate_pistol", "rtr:ornate_pistol");
            gun.gameObject.AddComponent<OrnatePistol>();
            gun.SetShortDescription("Cherished Memories");
            gun.SetLongDescription("An elegant firearm better suited on a collector's shelf than on the battlefield.\n\n" +
                "The phrase \"Do not forget who you are\" is engraved on the barrel.");
            gun.SetupSprite(null, "ornatepistol_idle_001", 8);
            gun.SetAnimationFPS(gun.shootAnimation, 14);
            gun.SetAnimationFPS(gun.reloadAnimation, 10);
            Gun duelingpistol = PickupObjectDatabase.GetById(9) as Gun;
            gun.AddProjectileModuleFrom("dueling_pistol", true, false);
            gun.SetBaseMaxAmmo(200);
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.damageModifier = 1;
            gun.reloadTime = 1.2f;
            gun.DefaultModule.cooldownTime = 0.09f;
            gun.DefaultModule.numberOfShotsInClip = 3;
            gun.DefaultModule.angleVariance = 9f;
            gun.quality = PickupObject.ItemQuality.D;
            gun.encounterTrackable.EncounterGuid = "ornatepistol";
            gun.gunClass = GunClass.PISTOL;
            gun.CanBeDropped = true;
            Gun component = Game.Items["derringer"].GetComponent<Gun>();
            gun.muzzleFlashEffects = component.muzzleFlashEffects;
            ETGMod.Databases.Items.Add(gun, null, "ANY");
            OrnatePistolID = gun.PickupObjectId;

            //CREATES NEW PROJECTILE
            Projectile NewProjectileOrnate = Instantiate<Projectile>(duelingpistol.DefaultModule.projectiles[0]);
            NewProjectileOrnate.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(NewProjectileOrnate.gameObject);
            DontDestroyOnLoad(NewProjectileOrnate);
            gun.DefaultModule.projectiles[0] = NewProjectileOrnate;
            NewProjectileOrnate.transform.parent = gun.barrelOffset;

            //SETS PROJECTILE STATS
            NewProjectileOrnate.baseData.damage = 5.5f;
            NewProjectileOrnate.baseData.speed *= 0.9f;
            NewProjectileOrnate.baseData.force *= 0.5f;
            NewProjectileOrnate.AdditionalScaleMultiplier = 1.1f;
        }

        private bool HasReloaded;
        public static int OrnatePistolID;


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
            AkSoundEngine.PostEvent("Play_WPN_duelingpistol_shot_01", base.gameObject);
        }
    }
}
