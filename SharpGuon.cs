using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Dungeonator;
using ItemAPI;
using MonoMod.RuntimeDetour;
using UnityEngine;

namespace Blunderbeast
{
    public class SharpGuon : IounStoneOrbitalItem
    {

        //Call this method from the Start() method of your ETGModule extension
        public static void Init()
        {
            //The name of the item
            string itemName = "Colorless Guon Stone";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "Blunderbeast/Resources/sharpguon";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<SharpGuon>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Razor-Sharp";
            string longDesc = "Damages enemies it touches, slightly increases damage.\n\n" +
                "Stone drained of its color by the Jammed. All of its defensive abilities have been stripped away in favor of raw strength.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "rtr");

            //Adds the actual passive effect to the item

            //Set the rarity of the item
            item.quality = PickupObject.ItemQuality.A;
            SharpGuon.BuildPrefab();
            item.OrbitalPrefab = SharpGuon.orbitalPrefab;
            item.Identifier = IounStoneOrbitalItem.IounStoneIdentifier.GENERIC;
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Damage, 1.1f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Curse, 1f, StatModifier.ModifyMethod.ADDITIVE);
        }

        public static Hook guonHook;
        public static PlayerOrbital orbitalPrefab;

        public static void BuildPrefab()
        {
            bool flag = SharpGuon.orbitalPrefab != null;
            bool flag2 = !flag;
            if (flag2)
            {
                GameObject gameObject = SpriteBuilder.SpriteFromResource("Blunderbeast/Resources/sharpguon-orbital", null, true);
                gameObject.name = "Sharp Guon";
                
                SpeculativeRigidbody speculativeRigidbody = gameObject.GetComponent<tk2dSprite>().SetUpSpeculativeRigidbody(IntVector2.Zero, new IntVector2(17, 18));
                SharpGuon.orbitalPrefab = gameObject.AddComponent<PlayerOrbital>();
                speculativeRigidbody.CollideWithTileMap = false;
                speculativeRigidbody.CollideWithOthers = true;
                speculativeRigidbody.PrimaryPixelCollider.CollisionLayer = CollisionLayer.Projectile;
                SharpGuon.orbitalPrefab.shouldRotate = false;
                SharpGuon.orbitalPrefab.orbitRadius = 6f;
                SharpGuon.orbitalPrefab.orbitDegreesPerSecond = 120f;
                SharpGuon.orbitalPrefab.SetOrbitalTier(0);
                UnityEngine.Object.DontDestroyOnLoad(gameObject);
                FakePrefab.MarkAsFakePrefab(gameObject);
                gameObject.SetActive(false);
            }
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            SharpGuon.guonHook = new Hook(typeof(PlayerOrbital).GetMethod("Initialize"), typeof(SharpGuon).GetMethod("GuonInit"));
            bool flag = player.gameObject.GetComponent<SharpGuon.SharpGuonBehavior>() != null;
            if (flag)
            {
                player.gameObject.GetComponent<SharpGuon.SharpGuonBehavior>().Destroy();
            }
            player.gameObject.AddComponent<SharpGuon.SharpGuonBehavior>();
            GameManager.Instance.OnNewLevelFullyLoaded += this.FixGuon;
            if (this.m_extantOrbital != null)
            {
                SpeculativeRigidbody specRigidbody = this.m_extantOrbital.GetComponent<PlayerOrbital>().specRigidbody;
                specRigidbody.OnPreRigidbodyCollision += this.OnPreCollision;
            }
        }

        private void OnPreCollision(SpeculativeRigidbody myRigidbody, PixelCollider myCollider, SpeculativeRigidbody otherRigidbody, PixelCollider otherCollider)
        {
            PlayerController player = this.Owner;
            PhysicsEngine.SkipCollision = true;
            RoomHandler currentRoom = GameManager.Instance.PrimaryPlayer.CurrentRoom;
            AIActor component = otherRigidbody.GetComponent<AIActor>();
            bool flag = component != null;
            if (flag)
            {
                if (component.healthHaver && component.CenterPosition.GetAbsoluteRoom() == currentRoom)
                {
                    if (didDamage == false && !player.IsStealthed)
                    {
                        didDamage = true;
                        GameManager.Instance.StartCoroutine(this.DamageTimer());

                        if (player.HasPickupID(822) || player.HasPickupID(457))
                        {
                            Gun gun = PickupObjectDatabase.GetById(616) as Gun;
                            GameActorHealthEffect bleedEffect = new GameActorHealthEffect
                            {
                                TintColor = Color.red,
                                DeathTintColor = Color.red,
                                AppliesTint = true,
                                AppliesDeathTint = true,
                                AffectsEnemies = true,
                                AffectsPlayers = false,
                                effectIdentifier = "sharpguonbleed",
                                resistanceType = EffectResistanceType.None,
                                duration = 3f,
                                DamagePerSecondToEnemies = 3f,
                                stackMode = GameActorEffect.EffectStackingMode.Refresh
                            };
                            component.ApplyEffect(bleedEffect, 1, null);


                            Vector3 vector = component.sprite.WorldBottomLeft.ToVector3ZisY(0);
                            Vector3 vector2 = component.sprite.WorldTopRight.ToVector3ZisY(0);
                            float num = (vector2.y - vector.y) * (vector2.x - vector.x);
                            float num2 = 50f * num;
                            int num3 = Mathf.CeilToInt(Mathf.Max(1f, num2 * BraveTime.DeltaTime));
                            int num4 = num3;
                            Vector3 minPosition = vector;
                            Vector3 maxPosition = vector2;
                            Vector3 direction = Vector3.up / 2f;
                            float angleVariance = 120f;
                            float magnitudeVariance = 3f;
                            float? startLifetime = new float?(UnityEngine.Random.Range(12, 20f));
                            GlobalSparksDoer.DoRandomParticleBurst(num4, minPosition, maxPosition, direction, angleVariance, magnitudeVariance, null, startLifetime, null, GlobalSparksDoer.SparksType.BLOODY_BLOOD);
                        }

                        component.healthHaver.ApplyDamage(4f, Vector2.zero, "Sharp Guon", CoreDamageTypes.None, DamageCategory.Normal, false, null, false);

                    }
                }
            }
        }

        private void FixGuon()
        {
            PlayerController player = this.Owner;
            bool flag = player.gameObject.GetComponent<SharpGuon.SharpGuonBehavior>() != null;
            if (flag)
            {
                player.gameObject.GetComponent<SharpGuon.SharpGuonBehavior>().Destroy();
            }
            player.gameObject.AddComponent<SharpGuon.SharpGuonBehavior>();

            if (this.m_extantOrbital != null)
            {
                SpeculativeRigidbody specRigidbody = this.m_extantOrbital.GetComponent<PlayerOrbital>().specRigidbody;
                specRigidbody.OnPreRigidbodyCollision += this.OnPreCollision;
            }
        }


        public override DebrisObject Drop(PlayerController player)
        {
            SharpGuon.guonHook.Dispose();
            bool flag = player.gameObject.GetComponent<SharpGuon.SharpGuonBehavior>() != null;
            if (flag)
            {
                player.gameObject.GetComponent<SharpGuon.SharpGuonBehavior>().Destroy();
            }
            GameManager.Instance.OnNewLevelFullyLoaded -= this.FixGuon;
            return base.Drop(player);
        }


        protected override void OnDestroy()
        {
            if (Owner)
            {
                SharpGuon.guonHook.Dispose();
                PlayerController player = this.Owner;
                bool flag = player.gameObject.GetComponent<SharpGuon.SharpGuonBehavior>() != null;
                if (flag)
                {
                    player.gameObject.GetComponent<SharpGuon.SharpGuonBehavior>().Destroy();
                }
                GameManager.Instance.OnNewLevelFullyLoaded -= this.FixGuon;
            }
            base.OnDestroy();
        }

        public static void GuonInit(Action<PlayerOrbital, PlayerController> orig, PlayerOrbital self, PlayerController player)
        {
            orig(self, player);
        }

        public bool didDamage;


        private IEnumerator DamageTimer()
        {
            yield return new WaitForSeconds(0.011f);
            didDamage = false;
            yield break;
        }

        private class SharpGuonBehavior : BraveBehaviour
        {
            private void Start()
            {
                this.owner = base.GetComponent<PlayerController>();
            }

            public void Destroy()
            {
                UnityEngine.Object.Destroy(this);
            }

            private PlayerController owner;
        }
    }
}

