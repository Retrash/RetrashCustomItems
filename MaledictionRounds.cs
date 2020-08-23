using UnityEngine;
using ItemAPI;
using System.Linq;
using Dungeonator;
using System.Collections.Generic;
using System.Collections;
using System;

namespace Blunderbeast
{
    public class MaledictionRounds : PassiveItem
    {

        //Call this method from the Start() method of your ETGModule extension
        public static void Init()
        {
            //The name of the item
            string itemName = "Malediction Rounds";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "Blunderbeast/Resources/maledictionrounds";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<MaledictionRounds>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Severing The Threads";
            string longDesc = "Bullets enchanted with ancient voodoo magic. Has a chance to spread a deadly curse after an enemy is killed.";


            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "rtr");

            //Adds the actual passive effect to the item

            //Set the rarity of the item
            item.quality = PickupObject.ItemQuality.S;
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Curse, 1f, StatModifier.ModifyMethod.ADDITIVE);
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.OnKilledEnemy += this.HandleDeathEffect;
        }

        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            player.OnKilledEnemy -= this.HandleDeathEffect;
            return debrisObject;
        }

        private void HandleDeathEffect(PlayerController player)
        {
            GameManager.Instance.StartCoroutine(this.DeathTimer());
        }

        private IEnumerator DeathTimer()
        {
            PlayerController player = this.Owner;
            yield return new WaitForSeconds(0.1f);
            RoomHandler absoluteRoom = base.transform.position.GetAbsoluteRoom();
            AIActor randomActiveEnemy = absoluteRoom.GetRandomActiveEnemy(false);
            if (randomActiveEnemy != null && randomActiveEnemy.healthHaver && !randomActiveEnemy.healthHaver.IsBoss && randomActiveEnemy.healthHaver.IsAlive && randomActiveEnemy.healthHaver.IsVulnerable)
            {
                float num = 0.35f;
                float curseValue = player.stats.GetStatValue(PlayerStats.StatType.Curse);
                float value = UnityEngine.Random.Range(0.0f, 1.0f);

                if (player.HasPickupID(571))
                {
                    bool flag2 = value < ((curseValue * 0.03f) + num);
                    if (flag2)
                    {
                        this.ActivateDeath(randomActiveEnemy);
                    }
                }

                else
                {
                    bool flag = value < num;
                    if (flag)
                    {
                        this.ActivateDeath(randomActiveEnemy);
                    }
                }

                yield break;

            }
            else if (randomActiveEnemy != null && randomActiveEnemy.healthHaver && !randomActiveEnemy.healthHaver.IsBoss && !randomActiveEnemy.healthHaver.IsAlive && !randomActiveEnemy.healthHaver.IsVulnerable)
            {
                GameManager.Instance.StartCoroutine(this.DeathTimer());
            }
            else if (randomActiveEnemy != null && randomActiveEnemy.healthHaver && randomActiveEnemy.healthHaver.IsBoss && randomActiveEnemy.healthHaver.IsAlive)
            {
                this.BossDamage(randomActiveEnemy);
                yield break;
            }
            yield break;
        }

        protected void BossDamage(AIActor randomActiveEnemy)
        {
            if (randomActiveEnemy && randomActiveEnemy.behaviorSpeculator)
            {
                randomActiveEnemy.PlayEffectOnActor(ResourceCache.Acquire("Global VFX/VFX_Curse") as GameObject, Vector3.zero, false, false, false);
                randomActiveEnemy.healthHaver.ApplyDamage(100f, Vector2.zero, "Cursed", CoreDamageTypes.None, DamageCategory.Normal, true, null, true);
            }
        }

        protected void ActivateDeath(AIActor randomActiveEnemy)
        {
            if (randomActiveEnemy && randomActiveEnemy.behaviorSpeculator)
            {
                randomActiveEnemy.PlayEffectOnActor(ResourceCache.Acquire("Global VFX/VFX_Curse") as GameObject, Vector3.zero, false, false, false);
                SpawnEnemyOnDeath component = randomActiveEnemy.GetComponent<SpawnEnemyOnDeath>();
                if (component)
                {
                    UnityEngine.Object.Destroy(component);
                }
                randomActiveEnemy.healthHaver.minimumHealth = 0f;
                randomActiveEnemy.healthHaver.ApplyDamage(10000f, Vector2.zero, "Cursed", CoreDamageTypes.None, DamageCategory.Unstoppable, true, null, false);
            }
        }
    }
}


