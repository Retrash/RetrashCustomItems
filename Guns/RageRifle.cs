using System;
using System.Collections;
using Gungeon;
using ItemAPI;
using MonoMod;
using UnityEngine;

namespace Blunderbeast
{
    public class RageRifle : GunBehaviour
    {

        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Berserker Rifle", "ragerifle");
            Game.Items.Rename("outdated_gun_mods:berserker_rifle", "rtr:berserker_rifle");
            gun.gameObject.AddComponent<RageRifle>();
            gun.SetShortDescription("Unstoppable Frenzy");
            gun.SetLongDescription("Contains pure and unfettered rage.\n\n" +
                "Simply holding this gun can turn even the most peaceful beings into bloodthirsty killing machines.");
            gun.SetupSprite(null, "ragerifle_idle_001", 8);
            gun.SetAnimationFPS(gun.shootAnimation, 24);

            gun.SetAnimationFPS(gun.reloadAnimation, 4);
            Gun targetGun = PickupObjectDatabase.GetById(762) as Gun;
            gun.AddProjectileModuleFrom("klobb", true, false);
            gun.SetBaseMaxAmmo(666);
            RandomProjectileReplacementItem berserkPrefab = PickupObjectDatabase.GetById(524).GetComponent<RandomProjectileReplacementItem>();
            Projectile berserkProj = berserkPrefab.ReplacementProjectile;

            Gun projGun = PickupObjectDatabase.GetById(329) as Gun;

            gun.DefaultModule.usesOptionalFinalProjectile = true;
            gun.DefaultModule.numberOfFinalProjectiles = 2;
            gun.DefaultModule.finalProjectile = berserkProj;
            gun.DefaultModule.finalCustomAmmoType = targetGun.DefaultModule.customAmmoType;
            gun.DefaultModule.finalAmmoType = targetGun.DefaultModule.ammoType;

            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Automatic;
            gun.damageModifier = 1;
            gun.reloadTime = 1.5f;
            gun.DefaultModule.cooldownTime = 0.05f;
            gun.DefaultModule.numberOfShotsInClip = 22;
            gun.DefaultModule.angleVariance = 40f;
            gun.barrelOffset.transform.localPosition += new Vector3(1f, 0f, 0);
            gun.quality = PickupObject.ItemQuality.S;
            gun.encounterTrackable.EncounterGuid = "ragerifle";
            gun.gunClass = GunClass.RIFLE;
            gun.CanBeDropped = true;

            Gun muzzleGun = PickupObjectDatabase.GetById(519) as Gun;
            gun.muzzleFlashEffects = muzzleGun.muzzleFlashEffects;

            Gun component = PickupObjectDatabase.GetById(37) as Gun;
            gun.finalMuzzleFlashEffects = component.muzzleFlashEffects;
            ETGMod.Databases.Items.Add(gun, null, "ANY");

            //CREATES NEW PROJECTILE
            Projectile NewProjectileBerserk = Instantiate<Projectile>(projGun.DefaultModule.chargeProjectiles[1].Projectile);
            NewProjectileBerserk.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(NewProjectileBerserk.gameObject);
            DontDestroyOnLoad(NewProjectileBerserk);
            gun.DefaultModule.projectiles[0] = NewProjectileBerserk;
            NewProjectileBerserk.transform.parent = gun.barrelOffset;

            //SETS PROJECTILE STATS
            NewProjectileBerserk.AdditionalScaleMultiplier *= 0.6f;
            NewProjectileBerserk.baseData.damage = 4f;
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

            if (playerController.HasPickupID(524) || playerController.HasPickupID(815))
            {
                gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Burst;
                gun.DefaultModule.burstShotCount = 50;
                gun.DefaultModule.burstCooldownTime = 0.01f;
                projectile.OnHitEnemy += HandleKilledEnemy;
            }
        }

        private void HandleKilledEnemy(Projectile arg1, SpeculativeRigidbody arg2, bool fatal)
        {
            if (fatal)
            {
                gun.ammo += 1;
            }
        }

        public override void OnReloadPressed(PlayerController player, Gun gun, bool bSOMETHING)
        {
            if (gun.IsReloading && HasReloaded == true)
            {
                HasReloaded = false;
                AkSoundEngine.PostEvent("Stop_WPN_All", base.gameObject);
                base.OnReloadPressed(player, gun, bSOMETHING);
                AkSoundEngine.PostEvent("Play_WPN_dl45heavylaser_reload", base.gameObject);
            }
        }

        public override void OnAutoReload(PlayerController player, Gun gun)
        {
            if (player.HasPickupID(323) || player.HasPickupID(815))
            {
                float value = UnityEngine.Random.Range(0.0f, 1.0f);
                if (value <= 0.25f)
                {
                    this.RageDamageMultiplier = 2f;
                    this.RageDuration = 5f;
                    this.RageFlatColor = new Color(0.5f, 0f, 0f, 0.75f);

                    TableFlipItem rageitem = PickupObjectDatabase.GetById(399).GetComponent<TableFlipItem>();

                    if (this.m_rageElapsed > 0f)
                    {
                        this.m_rageElapsed = this.RageDuration;
                        if (player.HasActiveBonusSynergy(CustomSynergyType.ANGRIER_BULLETS, false))
                        {
                            this.m_rageElapsed *= 3f;
                        }
                        if (this.rageInstanceVFX == null)
                        {
                            this.rageInstanceVFX = player.PlayEffectOnActor(rageitem.RageOverheadVFX, new Vector3(0f, 1.375f, 0f), true, true, false);
                        }
                    }
                    else
                    {
                        player.StartCoroutine(this.HandleRageCooldown());
                    }
                }
            }
        }

        public float RageDamageMultiplier;

        public float RageDuration;

        public Color RageFlatColor;

        private float m_rageElapsed;

        private GameObject rageInstanceVFX;

        private IEnumerator HandleRageCooldown()
        {
            PlayerController player = this.gun.CurrentOwner as PlayerController;
            TableFlipItem rageitem = PickupObjectDatabase.GetById(399).GetComponent<TableFlipItem>();

            this.rageInstanceVFX = null;
            this.rageInstanceVFX = player.PlayEffectOnActor(rageitem.RageOverheadVFX, new Vector3(0f, 1.375f, 0f), true, true, false);
            this.m_rageElapsed = this.RageDuration;
            if (player.HasActiveBonusSynergy(CustomSynergyType.ANGRIER_BULLETS, false))
            {
                this.m_rageElapsed *= 3f;
            }
            StatModifier damageStat = new StatModifier();
            damageStat.amount = this.RageDamageMultiplier;
            damageStat.modifyType = StatModifier.ModifyMethod.MULTIPLICATIVE;
            damageStat.statToBoost = PlayerStats.StatType.Damage;
            PlayerController cachedOwner = player;
            cachedOwner.ownerlessStatModifiers.Add(damageStat);
            cachedOwner.stats.RecalculateStats(cachedOwner, false, false);
            Color rageColor = this.RageFlatColor;
            while (this.m_rageElapsed > 0f)
            {
                cachedOwner.baseFlatColorOverride = rageColor.WithAlpha(Mathf.Lerp(rageColor.a, 0f, 1f - Mathf.Clamp01(this.m_rageElapsed)));
                if (this.rageInstanceVFX && this.m_rageElapsed < this.RageDuration - 1f)
                {
                    this.rageInstanceVFX.GetComponent<tk2dSpriteAnimator>().PlayAndDestroyObject("rage_face_vfx_out", null);
                    this.rageInstanceVFX = null;
                }
                yield return null;
                this.m_rageElapsed -= BraveTime.DeltaTime;
            }
            if (this.rageInstanceVFX)
            {
                this.rageInstanceVFX.GetComponent<tk2dSpriteAnimator>().PlayAndDestroyObject("rage_face_vfx_out", null);
            }
            cachedOwner.ownerlessStatModifiers.Remove(damageStat);
            cachedOwner.stats.RecalculateStats(cachedOwner, false, false);
            yield break;
        }


        public override void OnPostFired(PlayerController player, Gun gun)
        {
            if (gun.ClipShotsRemaining <= 2)
            {
                RandomProjectileReplacementItem berserkPrefab = PickupObjectDatabase.GetById(524).GetComponent<RandomProjectileReplacementItem>();
                String berserkAudio = berserkPrefab.ReplacementAudioEvent;

                AkSoundEngine.PostEvent(berserkAudio, base.gameObject);
            }

            else
            {
                AkSoundEngine.PostEvent("Play_WPN_h4mmer_shot_01", base.gameObject);
            }
        }

    }
}
