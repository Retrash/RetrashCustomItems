using System;
using System.Collections;
using Gungeon;
using ItemAPI;
using MonoMod;
using UnityEngine;

namespace Blunderbeast
{
    public class Craftsman : GunBehaviour
    {



        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("The Constructor", "buildersbest");
            Game.Items.Rename("outdated_gun_mods:the_constructor", "rtr:the_constructor");
            gun.gameObject.AddComponent<Craftsman>();
            gun.SetShortDescription("From The Ground Up");
            gun.SetLongDescription("Killing enemies has a chance of creating useful contraptions.\n\n" +
                "Famous all over the galaxy for being the number one tool for DIY projects.");
            gun.SetupSprite(null, "buildersbest_idle_001", 8);
            gun.SetAnimationFPS(gun.shootAnimation, 12);
            gun.SetAnimationFPS(gun.reloadAnimation, 6);
            Gun targetGun = PickupObjectDatabase.GetById(26) as Gun;
            Gun defaultmoduleGun = PickupObjectDatabase.GetById(26) as Gun;
            gun.AddProjectileModuleFrom(defaultmoduleGun, true, false);
            gun.SetBaseMaxAmmo(200);

            gun.DefaultModule.customAmmoType = targetGun.DefaultModule.customAmmoType;
            gun.DefaultModule.ammoType = targetGun.DefaultModule.ammoType;
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.damageModifier = 1;
            gun.reloadTime = 1.4f;
            gun.barrelOffset.transform.localPosition += new Vector3(0.2f, 0.2f, 0);
            gun.DefaultModule.cooldownTime = 0.45f;
            gun.DefaultModule.numberOfShotsInClip = 15;
            gun.DefaultModule.angleVariance = 4f;
            gun.quality = PickupObject.ItemQuality.A;
            gun.encounterTrackable.EncounterGuid = "buildersbest";
            gun.gunClass = GunClass.PISTOL;
            gun.CanBeDropped = true;
            Gun component = PickupObjectDatabase.GetById(47) as Gun;
            gun.muzzleFlashEffects = component.muzzleFlashEffects;
            ETGMod.Databases.Items.Add(gun, null, "ANY");

            Gun hiteffects = PickupObjectDatabase.GetById(444) as Gun;

            //CREATES NEW PROJECTILE
            Projectile NewProjectileCraft = Instantiate<Projectile>(targetGun.DefaultModule.projectiles[0]);
            NewProjectileCraft.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(NewProjectileCraft.gameObject);
            DontDestroyOnLoad(NewProjectileCraft);
            gun.DefaultModule.projectiles[0] = NewProjectileCraft;
            NewProjectileCraft.transform.parent = gun.barrelOffset;

            //SETS PROJECTILE STATS
            NewProjectileCraft.DefaultTintColor = new Color(99f, 30f, 0f).WithAlpha(0.95f);
            NewProjectileCraft.HasDefaultTint = true;
            NewProjectileCraft.baseData.damage = 20f;
            NewProjectileCraft.baseData.range *= 5f;
            NewProjectileCraft.baseData.speed *= 2f;
            NewProjectileCraft.hitEffects = hiteffects.DefaultModule.projectiles[0].hitEffects;
            NewProjectileCraft.AdditionalScaleMultiplier = 3f;
            NewProjectileCraft.baseData.force *= 5f;
            NewProjectileCraft.pierceMinorBreakables = true;

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
            if (playerController == null)
            {
                this.gun.ammo = this.gun.GetBaseMaxAmmo();
            }
            this.gun.DefaultModule.ammoCost = 1;

            //projectile.OnWillKillEnemy += OnKill;
            projectile.OnHitEnemy = (Action<Projectile, SpeculativeRigidbody, bool>)Delegate.Combine(projectile.OnHitEnemy, new Action<Projectile, SpeculativeRigidbody, bool>(this.HandleHit));
        }

        private IEnumerator Clang()
        {
            yield return new WaitForSeconds(0);
            AkSoundEngine.PostEvent("Play_OBJ_spears_clank_01", base.gameObject);
            yield return new WaitForSeconds(0.225f);
            AkSoundEngine.PostEvent("Play_OBJ_spears_clank_01", base.gameObject);
            yield return new WaitForSeconds(0.225f);
            AkSoundEngine.PostEvent("Play_OBJ_spears_clank_01", base.gameObject);
            yield break;
        }

        private int armorCount;


        private void HandleHit(Projectile arg1, SpeculativeRigidbody arg2, bool arg3)
        {
            if (arg2.aiActor != null && arg2.aiActor.behaviorSpeculator && !arg2.aiActor.IsHarmlessEnemy)
            {

                PlayerController player = gun.CurrentOwner as PlayerController;
                if ((player.HasPickupID(131) || player.HasPickupID(239) || player.HasPickupID(815)) || (gun.DuctTapeMergedGunIDs != null))
                {
                    float value2 = UnityEngine.Random.Range(0.0f, 1.0f);
                    if (value2 <= 0.05 && armorCount < 10)
                    {
                        GameManager.Instance.StartCoroutine(this.Clang());
                        LootEngine.SpawnItem(PickupObjectDatabase.GetById(120).gameObject, arg2.sprite.WorldBottomCenter, Vector2.up, 1f, true, false, true);
                        armorCount += 1;
                    }
                }

                float value = UnityEngine.Random.Range(0.0f, 1.0f);
                if (value <= 0.05)
                {
                    GameManager.Instance.StartCoroutine(this.Clang());

                    // TURRET
                    SpawnObjectPlayerItem portableturret = PickupObjectDatabase.GetById(201).GetComponent<SpawnObjectPlayerItem>();
                    String turretGuid = portableturret.enemyGuidToSpawn;
                    GameObject objectToSpawn = EnemyDatabase.GetOrLoadByGuid(turretGuid).gameObject;

                    GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(objectToSpawn, arg2.sprite.WorldBottomCenter, Quaternion.identity);
                    tk2dBaseSprite component4 = gameObject2.GetComponent<tk2dBaseSprite>();
                    if (component4)
                    {
                        component4.sprite.IsPerpendicular = true;
                        component4.PlaceAtPositionByAnchor(arg2.sprite.WorldBottomCenter.ToVector3ZUp(component4.transform.position.z), tk2dBaseSprite.Anchor.MiddleCenter);
                    }
                    this.spawnedPlayerObject = gameObject2;
                    PortableTurretController component5 = this.spawnedPlayerObject.GetComponent<PortableTurretController>();
                    if (component5)
                    {
                        component5.sourcePlayer = player;
                    }
                }

                else
                {
                    if (value <= 0.1)
                    {
                        GameManager.Instance.StartCoroutine(this.Clang());

                        //MINE
                        SpawnObjectPlayerItem proximitymine = PickupObjectDatabase.GetById(66).GetComponent<SpawnObjectPlayerItem>();
                        GameObject mineObject = proximitymine.objectToSpawn.gameObject;

                        GameObject mineObject2 = UnityEngine.Object.Instantiate<GameObject>(mineObject, arg2.sprite.WorldBottomCenter, Quaternion.identity);
                        tk2dBaseSprite mineSprite = mineObject2.GetComponent<tk2dBaseSprite>();
                        if (mineSprite)
                        {
                            mineSprite.IsPerpendicular = true;
                            mineSprite.PlaceAtPositionByAnchor(arg2.sprite.WorldBottomCenter, tk2dBaseSprite.Anchor.MiddleCenter);
                        }
                    }

                    else
                    {
                        if (value <= 0.15)
                        {
                            GameManager.Instance.StartCoroutine(this.Clang());

                            //TABLE
                            FoldingTableItem foldingtable = PickupObjectDatabase.GetById(644).GetComponent<FoldingTableItem>();
                            GameObject tableObject = foldingtable.TableToSpawn.gameObject;
                            GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(tableObject.gameObject, arg2.sprite.WorldBottomCenter, Quaternion.identity);
                            SpeculativeRigidbody componentInChildren = gameObject.GetComponentInChildren<SpeculativeRigidbody>();
                            FlippableCover component = gameObject.GetComponent<FlippableCover>();
                            component.transform.position.XY().GetAbsoluteRoom().RegisterInteractable(component);
                            component.ConfigureOnPlacement(component.transform.position.XY().GetAbsoluteRoom());
                            componentInChildren.Initialize();
                            PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(componentInChildren, null, false);
                        }
                    }
                }
            }
        }

        //private void OnKill(Projectile arg1, SpeculativeRigidbody arg2)
        //{
        //    if (!arg2.aiActor.healthHaver.IsDead)
        //    {
        //        PlayerController player = gun.CurrentOwner as PlayerController;
        //        if ((player.HasPickupID(131) || player.HasPickupID(239) || player.HasPickupID(815)) || (gun.DuctTapeMergedGunIDs != null))
        //        {
        //            float value2 = UnityEngine.Random.Range(0.0f, 1.0f);
        //            if (value2 <= 0.06 && armorCount < 10)
        //            {
        //                GameManager.Instance.StartCoroutine(this.Clang());
        //                LootEngine.SpawnItem(PickupObjectDatabase.GetById(120).gameObject, arg2.sprite.WorldBottomCenter, Vector2.up, 1f, true, false, true);
        //                armorCount += 1;
        //            }                   
        //        }

        //        float value = UnityEngine.Random.Range(0.0f, 1.0f);
        //        if (value <= 0.06)
        //        {
        //            GameManager.Instance.StartCoroutine(this.Clang());

        //            // TURRET
        //            SpawnObjectPlayerItem portableturret = PickupObjectDatabase.GetById(201).GetComponent<SpawnObjectPlayerItem>();
        //            String turretGuid = portableturret.enemyGuidToSpawn;
        //            GameObject objectToSpawn = EnemyDatabase.GetOrLoadByGuid(turretGuid).gameObject;

        //            GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(objectToSpawn, arg2.sprite.WorldBottomCenter, Quaternion.identity);
        //            tk2dBaseSprite component4 = gameObject2.GetComponent<tk2dBaseSprite>();
        //            if (component4)
        //            {
        //                component4.sprite.IsPerpendicular = true;
        //                component4.PlaceAtPositionByAnchor(arg2.sprite.WorldBottomCenter.ToVector3ZUp(component4.transform.position.z), tk2dBaseSprite.Anchor.MiddleCenter);
        //            }
        //            this.spawnedPlayerObject = gameObject2;
        //            PortableTurretController component5 = this.spawnedPlayerObject.GetComponent<PortableTurretController>();
        //            if (component5)
        //            {
        //                component5.sourcePlayer = player;
        //            }
        //        }

        //        else
        //        {
        //            if (value <= 0.12)
        //            {
        //                GameManager.Instance.StartCoroutine(this.Clang());

        //                //MINE
        //                SpawnObjectPlayerItem proximitymine = PickupObjectDatabase.GetById(66).GetComponent<SpawnObjectPlayerItem>();
        //                GameObject mineObject = proximitymine.objectToSpawn.gameObject;

        //                GameObject mineObject2 = UnityEngine.Object.Instantiate<GameObject>(mineObject, arg2.sprite.WorldBottomCenter, Quaternion.identity);
        //                tk2dBaseSprite mineSprite = mineObject2.GetComponent<tk2dBaseSprite>();
        //                if (mineSprite)
        //                {
        //                    mineSprite.IsPerpendicular = true;
        //                    mineSprite.PlaceAtPositionByAnchor(arg2.sprite.WorldBottomCenter, tk2dBaseSprite.Anchor.MiddleCenter);
        //                }
        //            }

        //            else
        //            {
        //                if (value <= 0.18)
        //                {
        //                    GameManager.Instance.StartCoroutine(this.Clang());

        //                    //TABLE
        //                    FoldingTableItem foldingtable = PickupObjectDatabase.GetById(644).GetComponent<FoldingTableItem>();
        //                    GameObject tableObject = foldingtable.TableToSpawn.gameObject;
        //                    GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(tableObject.gameObject, arg2.sprite.WorldBottomCenter, Quaternion.identity);
        //                    SpeculativeRigidbody componentInChildren = gameObject.GetComponentInChildren<SpeculativeRigidbody>();
        //                    FlippableCover component = gameObject.GetComponent<FlippableCover>();
        //                    component.transform.position.XY().GetAbsoluteRoom().RegisterInteractable(component);
        //                    component.ConfigureOnPlacement(component.transform.position.XY().GetAbsoluteRoom());
        //                    componentInChildren.Initialize();
        //                    PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(componentInChildren, null, false);
        //                }
        //            }
        //        }               
        //    }
        //}

        public GameObject spawnedPlayerObject;

        public override void OnReloadPressed(PlayerController player, Gun gun, bool bSOMETHING)
        {
            if (gun.IsReloading && HasReloaded == true)
            {
                HasReloaded = false;
                AkSoundEngine.PostEvent("Stop_WPN_All", base.gameObject);
                base.OnReloadPressed(player, gun, bSOMETHING);
                AkSoundEngine.PostEvent("Play_WPN_SNS_reload_01", base.gameObject);
            }
        }

        public override void OnPostFired(PlayerController player, Gun gun)
        {
            AkSoundEngine.PostEvent("Play_WPN_rg_shot_01", base.gameObject);
        }

    }
}
