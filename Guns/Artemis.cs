using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using Gungeon;
using ItemAPI;
using MonoMod;
using UnityEngine;

namespace Blunderbeast
{
    public class Artemis : GunBehaviour
    {

        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Exalted Armbow", "artemis");
            Game.Items.Rename("outdated_gun_mods:exalted_armbow", "rtr:exalted_armbow");
            gun.gameObject.AddComponent<Artemis>();
            gun.SetShortDescription("Kaliber May Cry");
            gun.SetLongDescription("Armbow blessed by the Goddess Artemissile, deals more damage against jammed enemies.\n\n" +
                "It is said that this weapon was originally crafted by a demonic gunsmith on behalf of Kaliber herself. How it become favored by the Goddess Artemissile remains a mystery.");
            gun.SetupSprite(null, "artemis_idle_001", 8);
            gun.SetAnimationFPS(gun.shootAnimation, 10);
            gun.SetAnimationFPS(gun.reloadAnimation, 8);
            gun.gunHandedness = GunHandedness.HiddenOneHanded;

            Gun targetGun = PickupObjectDatabase.GetById(806) as Gun;
            gun.AddProjectileModuleFrom(targetGun, true, false);
            gun.SetBaseMaxAmmo(300);

            StatModifier statModifier = new StatModifier();
            statModifier.statToBoost = PlayerStats.StatType.Curse;
            statModifier.amount = 1f;
            statModifier.modifyType = StatModifier.ModifyMethod.ADDITIVE;

            Array.Resize<StatModifier>(ref gun.passiveStatModifiers, gun.passiveStatModifiers.Length + 1);
            gun.passiveStatModifiers[gun.passiveStatModifiers.Length - 1] = statModifier;

            gun.DefaultModule.customAmmoType = targetGun.DefaultModule.customAmmoType;
            gun.DefaultModule.ammoType = targetGun.DefaultModule.ammoType;
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Burst;
            gun.damageModifier = 1;
            gun.reloadTime = 1.1f;
            gun.DefaultModule.cooldownTime = 0.55f;
            gun.DefaultModule.burstShotCount = 4;
            gun.DefaultModule.burstCooldownTime = 0.05f;
            gun.DefaultModule.numberOfShotsInClip = 12;
            gun.DefaultModule.angleVariance = 20f;
            gun.barrelOffset.transform.localPosition += new Vector3(0.1f, 0f, 0);
            gun.quality = PickupObject.ItemQuality.A;
            gun.encounterTrackable.EncounterGuid = "artemis";
            gun.CanBeDropped = true;
            Gun component = PickupObjectDatabase.GetById(577) as Gun;
            gun.muzzleFlashEffects = component.muzzleFlashEffects;
            ETGMod.Databases.Items.Add(gun, null, "ANY");

            Gun hiteffects = PickupObjectDatabase.GetById(609) as Gun;

            //CREATES NEW PROJECTILE
            Projectile NewProjectileArty = Instantiate<Projectile>(targetGun.DefaultModule.projectiles[0]);
            NewProjectileArty.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(NewProjectileArty.gameObject);
            DontDestroyOnLoad(NewProjectileArty);
            gun.DefaultModule.projectiles[0] = NewProjectileArty;
            NewProjectileArty.transform.parent = gun.barrelOffset;

            //SETS PROJECTILE STATS
            NewProjectileArty.DefaultTintColor = Color.magenta;
            NewProjectileArty.HasDefaultTint = true;
            NewProjectileArty.hitEffects = hiteffects.DefaultModule.projectiles[0].hitEffects;
            NewProjectileArty.baseData.damage = 10f;
            NewProjectileArty.baseData.speed *= 0.75f;
            NewProjectileArty.AdditionalScaleMultiplier = 1.1f;
            NewProjectileArty.pierceMinorBreakables = true;
            PierceProjModifier piercing = NewProjectileArty.gameObject.GetComponent<PierceProjModifier>();
            if (piercing != null)
            {
                GameManager.Destroy(piercing);
            }
            HomingModifier homingModifier = NewProjectileArty.gameObject.AddComponent<HomingModifier>();
            homingModifier.HomingRadius = 7f;
            homingModifier.AngularVelocity = 500f;
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

            if (!playerController.HasPickupID(538))
            {
                projectile.BlackPhantomDamageMultiplier = 2f;
            }
            if (playerController.HasPickupID(538))
            {
                projectile.BlackPhantomDamageMultiplier = 3f;
            }

            if ((playerController.HasPickupID(538) && playerController.HasPickupID(407)) || playerController.HasPickupID(815))
            {
                projectile.OnHitEnemy += JamEnemy;
            }
        }

        private void JamEnemy(Projectile arg1, SpeculativeRigidbody arg2, bool arg3)
        {
            AIActor component = arg2.GetComponent<AIActor>();
            bool flag2 = component == null;
            if (!flag2)
            {
                bool flag3 = component.healthHaver == null;
                if (!flag3)
                {
                    bool isBlackPhantom = component.IsBlackPhantom;
                    if (!isBlackPhantom)
                    {
                        component.BecomeBlackPhantom();
                    }
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
                AkSoundEngine.PostEvent("Play_WPN_crossbow_reload_01", base.gameObject);
            }
        }

        public override void OnPostFired(PlayerController player, Gun gun)
        {
            AkSoundEngine.PostEvent("Play_WPN_charmedbow_shot_01", base.gameObject);
        }

    }
}
