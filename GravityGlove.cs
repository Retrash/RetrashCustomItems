using UnityEngine;
using ItemAPI;
using System;
using System.Collections;
using InControl;
using Dungeonator;
using Gungeon;

namespace Blunderbeast
{
    public class GravityGlove : PassiveItem
    {

        //Call this method from the Start() method of your ETGModule extension
        public static void Init()
        {
            //The name of the item
            string itemName = "Gravity Glove";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "Blunderbeast/Resources/gravityglove";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<GravityGlove>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Virtual Pull";
            string longDesc = "Significantly improves the effectiveness of thrown guns.\n\n" +
                "Special glove created on Planet 17 before its forceful occupation by the Hegemony of Man. Its magnetizing force makes it intrinsically bound to the Hyper Disc.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "rtr");

            //Adds the actual passive effect to the item

            //Set the rarity of the item
            item.quality = PickupObject.ItemQuality.B;
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.ThrownGunDamage, 2.5f, StatModifier.ModifyMethod.MULTIPLICATIVE);
        }

        public override void Pickup(PlayerController player)
        {
            if (!this.m_pickedUpThisRun)
            {
                player.inventory.AddGunToInventory(PickupObjectDatabase.GetByEncounterName("Hyper Disc") as Gun, true);
            }
            base.Pickup(player);
            player.PostProcessProjectile += this.PostProcessProjectile;
            player.PostProcessThrownGun += this.PostProcessThrownGun;
            GameManager.Instance.OnNewLevelFullyLoaded += this.RecoverDisc;
        }


        protected override void Update()
        {
            if (Owner)
            {
                RoomHandler currentRoom = GameManager.Instance.PrimaryPlayer.CurrentRoom;
                for (int i = 0; i < StaticReferenceManager.AllDebris.Count; i++)
                {
                    DebrisObject alldebris = StaticReferenceManager.AllDebris[i];
                    if (alldebris && alldebris.sprite.WorldCenter.GetAbsoluteRoom() == currentRoom && alldebris.IsPickupObject && alldebris.IsCorpse && alldebris.canRotate)
                    {
                        if (alldebris.GetComponent<PickupMover>() == null)
                        {
                            PickupMover pickupMover = alldebris.gameObject.AddComponent<PickupMover>();
                            if (pickupMover.specRigidbody)
                            {
                                pickupMover.specRigidbody.CollideWithTileMap = false;
                            }
                            pickupMover.acceleration = 100f;
                            pickupMover.maxSpeed = 12f;
                            pickupMover.minRadius = 0.85f;
                            pickupMover.moveIfRoomUnclear = true;
                            pickupMover.stopPathingOnContact = true;
                        }
                    }
                }
            }

            else { return; }
        }

        private IEnumerator RecoverDisc2()
        {
            PlayerController player = this.Owner;
            yield return new WaitForSeconds(0.15f);
            if (!player.inventory.AllGuns.Contains(PickupObjectDatabase.GetByName("Hyper Disc") as Gun))
            {
                player.inventory.AddGunToInventory(PickupObjectDatabase.GetByEncounterName("Hyper Disc") as Gun, true);
            }
            yield break;
        }


        private void RecoverDisc()
        {
            GameManager.Instance.StartCoroutine(this.RecoverDisc2());
        }



        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            player.PostProcessProjectile -= this.PostProcessProjectile;
            player.PostProcessThrownGun -= this.PostProcessThrownGun;
            GameManager.Instance.OnNewLevelFullyLoaded -= this.RecoverDisc;
            return debrisObject;
        }

        protected override void OnDestroy()
        {
            if (Owner)
            {
                GameManager.Instance.OnNewLevelFullyLoaded -= this.RecoverDisc;
            }
            base.OnDestroy();
        }

        private void HandleBlankOnHit(DebrisObject obj)
        {
            PlayerController player = this.Owner;
            this.DoMicroBlank((!obj.specRigidbody) ? obj.transform.position.XY() : obj.specRigidbody.UnitCenter);
            obj.PreventFallingInPits = true;

            if (player.HasPickupID(519))
            {
                Gun combinedRifle = Game.Items["combined_rifle"].GetComponent<Gun>();
                Projectile projectile = combinedRifle.alternateVolley.projectiles[0].projectiles[0];
                int num = UnityEngine.Random.Range(1, 1 + 1);
                float num2 = 360f / (float)num;
                float num3 = UnityEngine.Random.Range(0f, num2);
                float z = ((float)UnityEngine.Random.Range(0, 360));
                GameObject gameObject = SpawnManager.SpawnProjectile(projectile.gameObject, obj.specRigidbody.UnitCenter, Quaternion.Euler(0f, 0f, z), true);
                Projectile component = gameObject.GetComponent<Projectile>();
                if (component != null)
                {
                    component.Owner = Owner;
                    component.Shooter = Owner.specRigidbody;
                    component.baseData.damage *= 0.5f;
                }
            }


            obj.OnGrounded = (Action<DebrisObject>)Delegate.Remove(obj.OnGrounded, new Action<DebrisObject>(this.HandleBlankOnHit));
        }


        private void HandleReturnLikeBoomerang(DebrisObject obj)
        {
            PlayerController player = this.Owner;
            obj.sprite.IsPerpendicular = true;
            obj.sprite.automaticallyManagesDepth = true;
            obj.sprite.ignoresTiltworldDepth = true;
            obj.FlagAsPickup();
            obj.IsCorpse = true;
            obj.canRotate = true;
            obj.OnGrounded = (Action<DebrisObject>)Delegate.Remove(obj.OnGrounded, new Action<DebrisObject>(this.HandleReturnLikeBoomerang));
        }

        private void PostProcessProjectile(Projectile proj, float effectChanceScalar)
        {
            PlayerController player = this.Owner;
            if (player.CurrentGun.PickupObjectId == 519)
            {
                HomingModifier projhomingModifier = proj.gameObject.AddComponent<HomingModifier>();
                projhomingModifier.HomingRadius = 10f;
                projhomingModifier.AngularVelocity = 750f;
            }
        }

        private void PostProcessThrownGun(Projectile obj)
        {
            PlayerController player = this.Owner;
            AkSoundEngine.PostEvent("Play_WPN_unicornhorn_shot_01", base.gameObject);
            HomingModifier homingModifier = obj.gameObject.GetComponent<HomingModifier>();
            homingModifier = obj.gameObject.AddComponent<HomingModifier>();
            homingModifier.HomingRadius = 10f;
            homingModifier.AngularVelocity = 1000f;
            obj.pierceMinorBreakables = true;
            obj.IgnoreTileCollisionsFor(0.01f);
            obj.OnBecameDebris = (Action<DebrisObject>)Delegate.Combine(obj.OnBecameDebrisGrounded, new Action<DebrisObject>(this.HandleReturnLikeBoomerang));
            obj.OnBecameDebris = (Action<DebrisObject>)Delegate.Combine(obj.OnBecameDebris, new Action<DebrisObject>(this.HandleBlankOnHit));
        }

        private void DoMicroBlank(Vector2 center)
        {
            PlayerController player = this.Owner;
            GameObject silencerVFX = (GameObject)ResourceCache.Acquire("Global VFX/BlankVFX_Ghost");
            AkSoundEngine.PostEvent("Play_OBJ_silenceblank_small_01", base.gameObject);
            GameObject gameObject = new GameObject("silencer");
            SilencerInstance silencerInstance = gameObject.AddComponent<SilencerInstance>();
            float additionalTimeAtMaxRadius = 0.25f;
            silencerInstance.TriggerSilencer(center, 25f, 5f, silencerVFX, 0f, 3f, 3f, 3f, 250f, 5f, additionalTimeAtMaxRadius, player, true, false);
        }
    }
}

