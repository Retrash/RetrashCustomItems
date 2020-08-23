using UnityEngine;
using ItemAPI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Blunderbeast
{
    public class IcySkull : PassiveItem
    {

        //Call this method from the Start() method of your ETGModule extension
        public static void Init()
        {
            //The name of the item
            string itemName = "Icy Skull";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "Blunderbeast/Resources/iceskull3";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<IcySkull>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Chilled To The Bone";
            string longDesc = "Terrifying cold, this skull will freeze solid any enemy that comes close.\n\n" +
                "Once belonged to a Gunjurer who chose to dedicate his life to the art of cryomancy. Even after his death, the remnants of his magic still lingers.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "rtr");

            //Adds the actual passive effect to the item

            //Set the rarity of the item
            item.quality = PickupObject.ItemQuality.B;
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Coolness, 1f);
        }

        public override void Pickup(PlayerController player)
        {
            LiveAmmoItem liveammo = PickupObjectDatabase.GetById(414).GetComponent<LiveAmmoItem>();

            if (!PassiveItem.ActiveFlagItems.ContainsKey(player))
            {
                PassiveItem.ActiveFlagItems.Add(player, new Dictionary<Type, int>());
            }
            if (!PassiveItem.ActiveFlagItems[player].ContainsKey(liveammo.GetType()))
            {
                PassiveItem.ActiveFlagItems[player].Add(liveammo.GetType(), 1);
            }
            else
            {
                PassiveItem.ActiveFlagItems[player][liveammo.GetType()] = PassiveItem.ActiveFlagItems[player][liveammo.GetType()] + 1;
            }

            player.PostProcessBeamTick += PostProcessBeamTick;
            SpeculativeRigidbody specRigidbody = player.specRigidbody;
            specRigidbody.OnRigidbodyCollision += HandleRigidbodyCollision;
            base.Pickup(player);
        }

        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);

            LiveAmmoItem liveammo = PickupObjectDatabase.GetById(414).GetComponent<LiveAmmoItem>();

            if (PassiveItem.ActiveFlagItems[player].ContainsKey(liveammo.GetType()))
            {
                PassiveItem.ActiveFlagItems[player][liveammo.GetType()] = Mathf.Max(0, PassiveItem.ActiveFlagItems[player][liveammo.GetType()] - 1);
                if (PassiveItem.ActiveFlagItems[player][liveammo.GetType()] == 0)
                {
                    PassiveItem.ActiveFlagItems[player].Remove(liveammo.GetType());
                }
            }
            player.PostProcessBeamTick -= PostProcessBeamTick;
            SpeculativeRigidbody specRigidbody = player.specRigidbody;
            specRigidbody.OnRigidbodyCollision -= HandleRigidbodyCollision;
            return debrisObject;
        }

        private void PostProcessBeamTick(BeamController beam, SpeculativeRigidbody spec, float tickRate)
        {
            GameActor gameActor = spec.gameActor;
            if (!gameActor)
            {
                return;
            }

            PlayerController player = this.Owner;
            if (player.CurrentGun.PickupObjectId == 40)
            {
                beam.projectile.baseData.damage *= 1.003f;
            }

            BulletStatusEffectItem frostbullets = PickupObjectDatabase.GetById(278).GetComponent<BulletStatusEffectItem>();
            GameActorFreezeEffect frostfreeze = frostbullets.FreezeModifierEffect;

            if (player.CurrentGun.PickupObjectId == 464)
            {
                if (UnityEngine.Random.value < BraveMathCollege.SliceProbability(1f, tickRate))
                {
                    gameActor.ApplyEffect(frostfreeze, 0.015f, null);
                }
            }
        }

        private void HandleRigidbodyCollision(CollisionData rigidbodyCollision)
        {
            BulletStatusEffectItem frostbullets = PickupObjectDatabase.GetById(278).GetComponent<BulletStatusEffectItem>();
            GameActorFreezeEffect frostfreeze = frostbullets.FreezeModifierEffect;

            if (Owner && rigidbodyCollision.OtherRigidbody)
            {
                AIActor aiActor = rigidbodyCollision.OtherRigidbody.aiActor;
                if (aiActor != null && aiActor.healthHaver.CanCurrentlyBeKilled)
                {
                    aiActor.ApplyEffect(frostfreeze, 10f, null);
                }
            }
        }       

        protected override void Update()
        {
            if (Owner)
            {              
                if (Owner.HasPickupID(364))
                {
                    Owner.CurrentRoom.ApplyActionToNearbyEnemies(Owner.CenterPosition, 4f, new Action<AIActor, float>(this.AuraAction));
                    if (this.radialIndicator == null)
                    {
                        this.HandleRadialIndicator();
                        bool flag5 = this.radialIndicator != null;
                        if (flag5)
                        {
                            this.radialIndicator.CurrentRadius = 4f;
                            radialIndicator.IsFire = false;
                        }
                    }
                }

                else if (this.radialIndicator != null)
                {
                    this.UnhandleRadialIndicator();
                }

                if (Owner.CurrentGun == lastGun)
                {
                    return;
                }

                else
                {
                    if ((Owner.CurrentGun.PickupObjectId == 464 || Owner.CurrentGun.PickupObjectId == 45))
                    {
                        AddStat(PlayerStats.StatType.Damage, 1.2f, StatModifier.ModifyMethod.MULTIPLICATIVE);
                        AddStat(PlayerStats.StatType.ReloadSpeed, 0.8f, StatModifier.ModifyMethod.MULTIPLICATIVE);
                        Owner.stats.RecalculateStats(Owner, true, false);
                        lastGun = Owner.CurrentGun;
                    }

                    else if ((Owner.CurrentGun.PickupObjectId != 464 && Owner.CurrentGun.PickupObjectId != 45))
                    {
                        RemoveStat(PlayerStats.StatType.Damage);
                        RemoveStat(PlayerStats.StatType.ReloadSpeed);
                        Owner.stats.RecalculateStats(Owner, true, false);
                        lastGun = Owner.CurrentGun;
                    }
                }

            }

            else if (this.radialIndicator != null)
            {
                this.UnhandleRadialIndicator();
            }
        }

        private Gun lastGun;

        private void AuraAction(AIActor aiactor, float d)
        {
            BulletStatusEffectItem frostbullets = PickupObjectDatabase.GetById(278).GetComponent<BulletStatusEffectItem>();
            GameActorFreezeEffect frostfreeze = frostbullets.FreezeModifierEffect;
            if (aiactor != null && aiactor.healthHaver.CanCurrentlyBeKilled)
            {
                aiactor.ApplyEffect(frostfreeze, 0.04f, null);
            }
        }


        private void HandleRadialIndicator()
        {
            bool flag = !this.radialIndicatorActive;
            if (flag)
            {
                this.radialIndicatorActive = true;
                this.radialIndicator = ((GameObject)UnityEngine.Object.Instantiate(ResourceCache.Acquire("Global VFX/HeatIndicator"), Owner.CenterPosition.ToVector3ZisY(0f), Quaternion.identity, Owner.transform)).GetComponent<HeatIndicatorController>();
                this.radialIndicator.CurrentColor = new Color(0.5f, 5f, 20f).WithAlpha(0.25f);
                radialIndicator.IsFire = false;
            }
        }

        private void UnhandleRadialIndicator()
        {
            bool flag = this.radialIndicatorActive;
            if (flag)
            {
                this.radialIndicatorActive = false;
                bool flag2 = this.radialIndicator;
                if (flag2)
                {
                    this.radialIndicator.EndEffect();
                }
                this.radialIndicator = null;
            }
        }

        private void AddStat(PlayerStats.StatType statType, float amount, StatModifier.ModifyMethod method = StatModifier.ModifyMethod.ADDITIVE)
        {
            foreach (var m in passiveStatModifiers)
            {
                if (m.statToBoost == statType) return; //don't add duplicates
            }

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

        private bool radialIndicatorActive;

        private HeatIndicatorController radialIndicator;
    }
}

