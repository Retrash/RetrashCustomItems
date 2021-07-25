using System;
using System.Collections;
using System.Collections.Generic;
using Gungeon;
using ItemAPI;
using MonoMod;
using UnityEngine;

namespace Blunderbeast
{
    public class Alchemiser : GunBehaviour
    {

        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Alchemical Gun", "alchemiser");
            Game.Items.Rename("outdated_gun_mods:alchemical_gun", "rtr:alchemical_gun");
            gun.gameObject.AddComponent<Alchemiser>();
            gun.SetShortDescription("Toxic Transmutation");
            gun.SetLongDescription("Transmutes poisoned enemies. Reload to create dangerous poisonous liquid.\n\n" +
                "The Alchemical Gun was created by an ancient denizen of the Oubliette. Some say the toxic environment was used by its owner to test the gun's capabilities. However, many suggest he had merely lost his mind being trapped in the sewers for so long.");
            gun.SetupSprite(null, "alchemiser_idle_001", 8);
            gun.SetAnimationFPS(gun.shootAnimation, 16);
            gun.SetAnimationFPS(gun.reloadAnimation, 8);
            Gun targetGun = PickupObjectDatabase.GetById(577) as Gun;
            Gun targetGun2 = PickupObjectDatabase.GetById(577) as Gun;
            gun.AddProjectileModuleFrom(targetGun2, true, false);
            gun.SetBaseMaxAmmo(300);
            gun.DefaultModule.customAmmoType = targetGun.DefaultModule.customAmmoType;
            gun.DefaultModule.ammoType = targetGun.DefaultModule.ammoType;
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.damageModifier = 1;
            gun.reloadTime = 1.3f;
            gun.barrelOffset.transform.localPosition += new Vector3(-0.1f, -0f, 0);
            gun.DefaultModule.cooldownTime = 0.2f;
            gun.DefaultModule.numberOfShotsInClip = 10;
            gun.DefaultModule.angleVariance = 6f;
            gun.quality = PickupObject.ItemQuality.C;
            gun.encounterTrackable.EncounterGuid = "alchemiser";
            gun.gunClass = GunClass.RIFLE;
            gun.CanBeDropped = true;
            Gun component = PickupObjectDatabase.GetById(61) as Gun;
            gun.muzzleFlashEffects = component.muzzleFlashEffects;

            //CREATES NEW PROJECTILE
            Projectile NewProjectileAlch = Instantiate<Projectile>(targetGun.DefaultModule.projectiles[0]);
            NewProjectileAlch.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(NewProjectileAlch.gameObject);
            DontDestroyOnLoad(NewProjectileAlch);
            gun.DefaultModule.projectiles[0] = NewProjectileAlch;
            NewProjectileAlch.transform.parent = gun.barrelOffset;

            NewProjectileAlch.AdditionalScaleMultiplier *= 1.1f;

            AssetBundle assetBundle = ResourceManager.LoadAssetBundle("shared_auto_001");
            Alchemiser.goopDefs = new List<GoopDefinition>();
            foreach (string text in Alchemiser.goops)
            {
                GoopDefinition goopDefinition;
                try
                {
                    GameObject gameObject = assetBundle.LoadAsset(text) as GameObject;
                    goopDefinition = gameObject.GetComponent<GoopDefinition>();
                }
                catch
                {
                    goopDefinition = (assetBundle.LoadAsset(text) as GoopDefinition);
                }
                goopDefinition.name = text.Replace("assets/data/goops/", "").Replace(".asset", "");
                Alchemiser.goopDefs.Add(goopDefinition);
            }
            List<GoopDefinition> goopDefs = Alchemiser.goopDefs;
            ETGMod.Databases.Items.Add(gun, null, "ANY");
        }

        private static List<GoopDefinition> goopDefs;

        private bool HasReloaded, HasImmunity;

        private DamageTypeModifier m_poisonImmunity;


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

                if (player.HasPickupID(205) || player.HasPickupID(313) || player.HasPickupID(815))
                {
                    DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(Alchemiser.goopDefs[0]).TimedAddGoopCircle(player.sprite.WorldBottomCenter, 1f, 0.25f, false);

                    if (HasImmunity == false)
                    {
                        HasImmunity = true;
                        this.m_poisonImmunity = new DamageTypeModifier();
                        this.m_poisonImmunity.damageMultiplier = 0f;
                        this.m_poisonImmunity.damageType = CoreDamageTypes.Poison;
                        player.healthHaver.damageTypeModifiers.Add(this.m_poisonImmunity);
                    }
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

            projectile.OnHitEnemy = (Action<Projectile, SpeculativeRigidbody, bool>)Delegate.Combine(projectile.OnHitEnemy, new Action<Projectile, SpeculativeRigidbody, bool>(this.HandleHit));
        }

        public GameActorHealthEffect HealthModifierEffect;

        private void HandleHit(Projectile arg1, SpeculativeRigidbody arg2, bool arg3)
        {
            if (arg2.aiActor != null && !arg2.healthHaver.IsBoss && !arg2.healthHaver.IsDead && arg2.aiActor.behaviorSpeculator && !arg2.aiActor.IsHarmlessEnemy && arg2.aiActor != null)
            {
                GameActorEffect poison = arg2.aiActor.GetEffect(DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(goopDefs[0]).goopDefinition.HealthModifierEffect.effectIdentifier);
                if (poison != null)
                {
                    AIActor orLoadByGuid = EnemyDatabase.GetOrLoadByGuid("8b43a5c59b854eb780f9ab669ec26b7a");
                    arg2.aiActor.EraseFromExistenceWithRewards(true);
                    AIActor aiactor = AIActor.Spawn(orLoadByGuid, arg2.aiActor.CenterPosition, arg2.transform.position.GetAbsoluteRoom(), true, AIActor.AwakenAnimationType.Awaken, true);
                    LootEngine.DoDefaultItemPoof(aiactor.CenterPosition, false, false);
                    aiactor.DiesOnCollison = true;
                    aiactor.ImmuneToAllEffects = true;

                    //arg2.aiActor.Transmogrify(orLoadByGuid, (GameObject)ResourceCache.Acquire("Global VFX/VFX_Item_Spawn_Poof"));
                }
            }
        }

        public override void OnReloadPressed(PlayerController player, Gun gun, bool bSOMETHING)
        {
            if (gun.IsReloading && HasReloaded == true)
            {

                float num = 0.75f;

                if (gun.ClipShotsRemaining == 0)
                {
                    DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(Alchemiser.goopDefs[0]).TimedAddGoopCircle(gun.sprite.WorldBottomCenter, 3.75f, num, false);
                }

                else if (gun.ClipShotsRemaining == 1)
                {
                    DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(Alchemiser.goopDefs[0]).TimedAddGoopCircle(gun.sprite.WorldBottomCenter, 3.5f, num, false);
                }

                else if (gun.ClipShotsRemaining == 2)
                {
                    DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(Alchemiser.goopDefs[0]).TimedAddGoopCircle(gun.sprite.WorldBottomCenter, 3.25f, num, false);
                }

                else if (gun.ClipShotsRemaining == 3)
                {
                    DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(Alchemiser.goopDefs[0]).TimedAddGoopCircle(gun.sprite.WorldBottomCenter, 3f, num, false);
                }

                else if (gun.ClipShotsRemaining == 4)
                {
                    DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(Alchemiser.goopDefs[0]).TimedAddGoopCircle(gun.sprite.WorldBottomCenter, 2.5f, num, false);
                }

                else if (gun.ClipShotsRemaining == 5)
                {
                    DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(Alchemiser.goopDefs[0]).TimedAddGoopCircle(gun.sprite.WorldBottomCenter, 2.25f, num, false);
                }

                else if (gun.ClipShotsRemaining >= 6)
                {
                    DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(Alchemiser.goopDefs[0]).TimedAddGoopCircle(gun.sprite.WorldBottomCenter, 2f, num, false);
                }

                HasReloaded = false;
                AkSoundEngine.PostEvent("Stop_WPN_All", base.gameObject);
                base.OnReloadPressed(player, gun, bSOMETHING);
                AkSoundEngine.PostEvent("Play_OBJ_coolant_leak_01", base.gameObject);
            }
        }

        private static string[] goops = new string[]
      {
            "assets/data/goops/poison goop.asset"
      };


        public override void OnPostFired(PlayerController player, Gun gun)
        {
            AkSoundEngine.PostEvent("Play_WPN_bountyhunterarm_shot_02", base.gameObject);
        }



    }
}
