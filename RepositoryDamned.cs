using UnityEngine;
using ItemAPI;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Blunderbeast
{
    public class RepositoryDamned : PassiveItem
    {

        //Call this method from the Start() method of your ETGModule extension
        public static void Init()
        {
            //The name of the item
            string itemName = "Repository Of The Damned";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "Blunderbeast/Resources/damnedrepository";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<RepositoryDamned>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Forbidden Knowledge";
            string longDesc = "Contains a gun shrouded in darkness, improves the damage of all cursed weaponry.\n\n" +
                "Long kept hidden by the Esoteric Order, this alien object grants insight into the eternal abyss deep below Bullet Hell.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "rtr");

            //Adds the actual passive effect to the item

            //Set the rarity of the item
            item.quality = PickupObject.ItemQuality.S;
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Curse, 1f);
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Health, 1f);
        }

        public override void Pickup(PlayerController player)
        {
            if (!this.m_pickedUpThisRun)
            {
                base.Pickup(player);
                AkSoundEngine.PostEvent("Play_ENM_creecher_peel_01", base.gameObject);
                float curHealth = player.healthHaver.GetCurrentHealth();
                player.healthHaver.ForceSetCurrentHealth(curHealth - 1);   
                
                PickupObject pickupObject = this.Open(player);
                Gun gun = pickupObject as Gun;
                StatModifier statModifier = new StatModifier();
                statModifier.statToBoost = PlayerStats.StatType.Curse;
                statModifier.amount = 1f;
                statModifier.modifyType = StatModifier.ModifyMethod.ADDITIVE;
                Array.Resize<StatModifier>(ref gun.passiveStatModifiers, gun.passiveStatModifiers.Length + 1);
                gun.passiveStatModifiers[gun.passiveStatModifiers.Length - 1] = statModifier;
            }
            else
            {
                base.Pickup(player);
            }           
        }

        private Gun lastGun;

        protected override void Update()
        {
            if (Owner)
            {
                if (Owner.CurrentGun != lastGun)
                {
                    lastGun = Owner.CurrentGun;
                    if (Owner.CurrentGun.passiveStatModifiers.Length == 0)
                    {
                        RemoveStat(PlayerStats.StatType.Damage);
                        Owner.stats.RecalculateStats(Owner, true, false);
                    }

                    else
                    {
                        foreach (StatModifier statmodifier in Owner.CurrentGun.passiveStatModifiers)
                        {
                            if (statmodifier.statToBoost == PlayerStats.StatType.Curse)
                            {
                                AddStat(PlayerStats.StatType.Damage, 1.66f, StatModifier.ModifyMethod.MULTIPLICATIVE);
                                Owner.stats.RecalculateStats(Owner, true, false);
                            }
                        }
                    }
                }
            }
        }


        private PickupObject Open(PlayerController user)
        {
            PickupObject.ItemQuality itemQuality = (UnityEngine.Random.value >= 0.5f) ? PickupObject.ItemQuality.B : PickupObject.ItemQuality.A;
            PickupObject itemOfTypeAndQuality = LootEngine.GetItemOfTypeAndQuality<PickupObject>(itemQuality, GameManager.Instance.RewardManager.GunsLootTable, false);
            DebrisObject debrisObject = LootEngine.SpawnItem(itemOfTypeAndQuality.gameObject, user.CenterPosition, Vector2.up, 1f, true, true, false);
            if (debrisObject)
            {
                Vector2 v = (!debrisObject.sprite) ? (debrisObject.transform.position.XY() + new Vector2(0.5f, 0.5f)) : debrisObject.sprite.WorldCenter;
                GameObject gameObject = SpawnManager.SpawnVFX((GameObject)BraveResources.Load("Global VFX/VFX_BlackPhantomDeath", ".prefab"), v, Quaternion.identity, false);
                if (gameObject && gameObject.GetComponent<tk2dSprite>())
                {
                    tk2dSprite component = gameObject.GetComponent<tk2dSprite>();
                    component.HeightOffGround = 5f;
                    component.UpdateZDepth();
                }
                return debrisObject.GetComponentInChildren<PickupObject>();
            }
            return null;
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
    }
}

