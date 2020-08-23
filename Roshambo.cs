using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ItemAPI;
using System;
using Dungeonator;

namespace Blunderbeast
{
    public class Roshambo : PassiveItem
    {

        //Call this method from the Start() method of your ETGModule extension
        public static void Init()
        {

            //The name of the item
            string itemName = "Roshambo";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "Blunderbeast/Resources/roshambo";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<Roshambo>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Triple Threat";
            string longDesc = "Imbues its wielder's guns with elemental properties that swap on every reload. Become one with the elements.\n\n" +
                "Cylinder forged by the combined efforts of three legendary gunsmiths. It was later gifted to a cunning Gungeoneer who sought to become a renowned treasure hunter.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "rtr");

            //Adds the actual passive effect to the item

            //Set the rarity of the item
            item.quality = PickupObject.ItemQuality.S;

            AssetBundle assetBundle = ResourceManager.LoadAssetBundle("shared_auto_001");
            Roshambo.goopDefs = new List<GoopDefinition>();
            foreach (string text in Roshambo.goops)
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
                Roshambo.goopDefs.Add(goopDefinition);
            }
            List<GoopDefinition> goopDefs = Roshambo.goopDefs;
        }

        private int goopIndex;

        private void SetGoopIndex(int index)
        {
            this.goopIndex = index;
            this.currentGoop = Roshambo.goopDefs[index];
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            this.SetGoopIndex(1);
            fireMode = true;

            this.m_electricityImmunity = new DamageTypeModifier();
            this.m_electricityImmunity.damageMultiplier = 0f;
            this.m_electricityImmunity.damageType = CoreDamageTypes.Electric;
            player.healthHaver.damageTypeModifiers.Add(this.m_electricityImmunity);

            this.m_poisonImmunity = new DamageTypeModifier();
            this.m_poisonImmunity.damageMultiplier = 0f;
            this.m_poisonImmunity.damageType = CoreDamageTypes.Poison;
            player.healthHaver.damageTypeModifiers.Add(this.m_poisonImmunity);

            this.m_fireImmunity = new DamageTypeModifier();
            this.m_fireImmunity.damageMultiplier = 0f;
            this.m_fireImmunity.damageType = CoreDamageTypes.Fire;
            player.healthHaver.damageTypeModifiers.Add(this.m_fireImmunity);

            player.PostProcessProjectile += this.PostProcessProjectile;
            player.PostProcessBeam += this.PostProcessBeam;
            player.PostProcessBeamTick += this.PostProcessBeamTick;
            player.OnReloadedGun += SwapElement;
            
        }

        private DamageTypeModifier m_electricityImmunity;
        private DamageTypeModifier m_fireImmunity;
        private DamageTypeModifier m_poisonImmunity;

        private static string[] goops = new string[]
        {
            "assets/data/goops/water goop.asset",
            "assets/data/goops/napalmgoopquickignite.asset",
            "assets/data/goops/poison goop.asset"
        };

        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            fireMode = false;
            poisonMode = false;
            electricMode = false;
            player.OnReloadedGun -= SwapElement;
            player.PostProcessProjectile -= this.PostProcessProjectile;
            player.PostProcessBeam -= this.PostProcessBeam;
            player.PostProcessBeamTick -= this.PostProcessBeamTick;
            player.healthHaver.damageTypeModifiers.Remove(this.m_electricityImmunity);
            player.healthHaver.damageTypeModifiers.Remove(this.m_poisonImmunity);
            player.healthHaver.damageTypeModifiers.Remove(this.m_fireImmunity);
            return debrisObject;
        }

        private bool hasSynergy;

        protected override void Update()
        {
            if (Owner)
            {
                if (Owner.IsInCombat)
                {
                    HandleElectrifyEffect();
                }

                if (Owner.CurrentGun.PickupObjectId == 381 && hasSynergy == false)
                {
                    hasSynergy = true;
                    Owner.CurrentGun.OnPostFired += OnPostFired;
                    AddStat(PlayerStats.StatType.ReloadSpeed, 0.5f, StatModifier.ModifyMethod.MULTIPLICATIVE);
                }

                if (Owner.CurrentGun.PickupObjectId != 381 && hasSynergy == true)
                {
                    hasSynergy = false;
                    Owner.CurrentGun.OnPostFired -= OnPostFired;
                    RemoveStat(PlayerStats.StatType.ReloadSpeed);
                }

                else { return; }
            }
        }

        private void AddStat(PlayerStats.StatType statType, float amount, StatModifier.ModifyMethod method = StatModifier.ModifyMethod.ADDITIVE)
        {

            StatModifier modifier = new StatModifier
            {
                amount = amount,
                statToBoost = statType,
                modifyType = method
            };

            if (this.passiveStatModifiers == null)
                this.passiveStatModifiers = new StatModifier[] { modifier };
            else
                this.passiveStatModifiers = this.passiveStatModifiers.Concat(new StatModifier[] { modifier }).ToArray();
        }

        private void RemoveStat(PlayerStats.StatType statType)
        {
            var newModifiers = new List<StatModifier>();
            for (int i = 0; i < passiveStatModifiers.Length; i++)
            {
                if (passiveStatModifiers[i].statToBoost != statType)
                    newModifiers.Add(passiveStatModifiers[i]);
            }
            this.passiveStatModifiers = newModifiers.ToArray();
        }


        private void OnPostFired(PlayerController player, Gun gun)
        {
            if (gun.PickupObjectId == 381)
            {
                SwapElement(player, gun);
            }
        }

        private void HandleElectrifyEffect()
        {
            RoomHandler absoluteRoom = Owner.CurrentRoom;
            List<AIActor> activeEnemies = absoluteRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
            if (activeEnemies != null)
            {
                for (int i = 0; i < activeEnemies.Count; i++)
                {
                    this.ElectrifyEnemies(activeEnemies[i]);
                }
            }
        }

        private void ElectrifyEnemies(AIActor target)
        {
            if (target != null && target.behaviorSpeculator)
            {
                for (int i = 0; i < StaticReferenceManager.AllGoops.Count; i++)
                {
                    if (StaticReferenceManager.AllGoops[i] != null && StaticReferenceManager.AllGoops[i].goopDefinition == Roshambo.goopDefs[0])
                    {
                        StaticReferenceManager.AllGoops[i].ElectrifyGoopCircle(target.sprite.WorldBottomCenter, 1f);
                    }
                }
            }
        }

        private void HandleDestructionFireGoop(Projectile sourceProjectile)
        {
            DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(Roshambo.goopDefs[1]).TimedAddGoopCircle(sourceProjectile.LastPosition, 1f, 0.25f, false);
        }

        private void HandleDestructionPoisonGoop(Projectile sourceProjectile)
        {
            DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(Roshambo.goopDefs[2]).TimedAddGoopCircle(sourceProjectile.LastPosition, 1f, 0.25f, false);
        }

        private void HandleDestructionElectricGoop(Projectile sourceProjectile)
        {
            DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(Roshambo.goopDefs[0]).TimedAddGoopCircle(sourceProjectile.LastPosition, 1.25f, 0.25f, false);
        }

        private void PostProcessBeamTick(BeamController beam, SpeculativeRigidbody hitRigidbody, float tickRate)
        {
            if (hitRigidbody != null)
            {
                if (electricMode)
                {
                    beam.projectile.damageTypes = CoreDamageTypes.Water & CoreDamageTypes.Electric;
                    DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(this.currentGoop).TimedAddGoopCircle(hitRigidbody.sprite.WorldBottomCenter, 1.25f, 0.25f, false);
                }

                if (poisonMode)
                {
                    beam.projectile.damageTypes = CoreDamageTypes.Poison;
                    DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(this.currentGoop).TimedAddGoopCircle(hitRigidbody.sprite.WorldBottomCenter, 1f, 0.25f, false);
                }

                if (fireMode)
                {
                    beam.projectile.damageTypes = CoreDamageTypes.Fire;
                    DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(this.currentGoop).TimedAddGoopCircle(hitRigidbody.sprite.WorldBottomCenter, 1f, 0.25f, false);
                }
            }
        }


        private void HandleHitEnemyFire(Projectile arg1, SpeculativeRigidbody arg2, bool arg3)
        {
            DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(Roshambo.goopDefs[1]).TimedAddGoopCircle(arg2.sprite.WorldBottomCenter, 1f, 0.25f, false);
        }

        private void HandleHitEnemyPoison(Projectile arg1, SpeculativeRigidbody arg2, bool arg3)
        {
            DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(Roshambo.goopDefs[2]).TimedAddGoopCircle(arg2.sprite.WorldBottomCenter, 1f, 0.25f, false);
        }

        private void HandleHitEnemyElectric(Projectile arg1, SpeculativeRigidbody arg2, bool arg3)
        {
            DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(Roshambo.goopDefs[0]).TimedAddGoopCircle(arg2.sprite.WorldBottomCenter, 1.25f, 0.25f, false);
        }


        private void PostProcessProjectile(Projectile sourceProjectile, float effectChanceScalar)
        {

            if (electricMode)
            {
                sourceProjectile.OnDestruction += HandleDestructionElectricGoop;
                sourceProjectile.damageTypes = CoreDamageTypes.Water & CoreDamageTypes.Electric;
                sourceProjectile.AdjustPlayerProjectileTint(Color.blue.WithAlpha(Color.blue.a / 2f), 5, 0f);
                sourceProjectile.OnHitEnemy += HandleHitEnemyElectric;
            }

            if (poisonMode)
            {
                sourceProjectile.OnDestruction += HandleDestructionPoisonGoop;
                sourceProjectile.damageTypes = CoreDamageTypes.Poison;
                sourceProjectile.AdjustPlayerProjectileTint(Color.green.WithAlpha(Color.green.a / 2f), 5, 0f);
                sourceProjectile.OnHitEnemy += HandleHitEnemyPoison;
            }

            if (fireMode)
            {
                sourceProjectile.OnDestruction += HandleDestructionFireGoop;
                sourceProjectile.damageTypes = CoreDamageTypes.Fire;
                sourceProjectile.AdjustPlayerProjectileTint(Color.red.WithAlpha(Color.red.a / 2f), 5, 0f);
                sourceProjectile.OnHitEnemy += HandleHitEnemyFire;
            }
        }

        private void PostProcessBeam(BeamController beam)
        {
            if (electricMode)
            {
                beam.AdjustPlayerBeamTint(Color.blue.WithAlpha(Color.blue.a / 2f), 5, 0f);
            }

            if (poisonMode)
            {
                beam.AdjustPlayerBeamTint(Color.green.WithAlpha(Color.green.a / 2f), 5, 0f);
            }

            if (fireMode)
            {
                beam.AdjustPlayerBeamTint(Color.red.WithAlpha(Color.red.a / 2f), 5, 0f);
            }
        }


        private void SwapElement(PlayerController player, Gun usedGun)
        {

            if (poisonMode == true)
            {
                this.SetGoopIndex(1);
                fireMode = true;
                electricMode = false;
                poisonMode = false;
            }

            else if (fireMode == true)
            {
                this.SetGoopIndex(0);

                electricMode = true;
                poisonMode = false;
                fireMode = false;
            }

            else if (electricMode == true)
            {
                this.SetGoopIndex(2);

                poisonMode = true;
                fireMode = false;
                electricMode = false;
            }
        }

        private GoopDefinition currentGoop;

        private static List<GoopDefinition> goopDefs;

        public bool fireMode, poisonMode, electricMode;



    }
}

