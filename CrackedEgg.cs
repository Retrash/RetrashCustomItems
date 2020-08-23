using UnityEngine;
using ItemAPI;
using Dungeonator;
using System.Collections.Generic;
using System.Collections;
using System;
using MonoMod.RuntimeDetour;
using System.Reflection;
using Gungeon;

namespace Blunderbeast
{
    public class CrackedEgg : PassiveItem
    {

        //Call this method from the Start() method of your ETGModule extension
        public static void Init()
        {
            //The name of the item
            string itemName = "Crackling Egg";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "Blunderbeast/Resources/crackedegg";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<CrackedEgg>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Yolked!";
            string longDesc = "Has a certain appeal, don't break it!\n\n" +
                "This mysterious egg was worshipped for thousands of years by a long lineage of cats. Some say that if you listen closely, strange phrases can be heard coming from inside its shell.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "rtr");

            //Adds the actual passive effect to the item

            //Set the rarity of the item
            item.quality = PickupObject.ItemQuality.C;
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.GlobalPriceMultiplier, 0.8f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            CrackedEggID = item.PickupObjectId;
        }

        private static int CrackedEggID;

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            AkSoundEngine.PostEvent("Play_AGUNIM_VO_FIGHT_LAUGH_01", base.gameObject);
            player.healthHaver.OnDamaged += this.BreakEgg;
        }

        private static Hook ThroneHook = new Hook(
               typeof(WingsItem).GetMethod("HandleRollStarted", BindingFlags.Instance | BindingFlags.NonPublic),
               typeof(CrackedEgg).GetMethod("RollThroneHook")
           );

        private static bool onCooldown;

        public static void RollThroneHook(Action<WingsItem, PlayerController, Vector2> orig, WingsItem throne, PlayerController p, Vector2 rollDirection)
        {
            if (p.HasPickupID(CrackedEggID))
            {
                if (onCooldown == false)
                {
                    onCooldown = true;
                    GameManager.Instance.StartCoroutine(StartCooldown());
                    Gun scrambler = PickupObjectDatabase.GetById(445) as Gun;
                    Projectile projectile = scrambler.DefaultModule.projectiles[0];
                    float z = UnityEngine.Random.Range(0, 360);
                    GameObject gameObject = SpawnManager.SpawnProjectile(projectile.gameObject, p.specRigidbody.UnitCenter, Quaternion.Euler(0f, 0f, z), true);
                    Projectile component = gameObject.GetComponent<Projectile>();
                    if (component != null)
                    {
                        component.Owner = p;
                        component.Shooter = p.specRigidbody;
                    }

                    float z2 = UnityEngine.Random.Range(0, 360);
                    GameObject gameObject2 = SpawnManager.SpawnProjectile(projectile.gameObject, p.specRigidbody.UnitCenter, Quaternion.Euler(0f, 0f, z2), true);
                    Projectile component2 = gameObject2.GetComponent<Projectile>();
                    if (component2 != null)
                    {
                        component2.Owner = p;
                        component2.Shooter = p.specRigidbody;
                    }

                }
            }

            else
            {
                orig(throne, p, rollDirection);
            }
        }

        private static IEnumerator StartCooldown()
        {
            yield return new WaitForSeconds(2f);
            onCooldown = false;
            yield break;
        }


        private void BreakEgg(float resultValue, float maxValue, CoreDamageTypes damageTypes, DamageCategory damageCategory, Vector2 damageDirection)
        {
            float value = UnityEngine.Random.Range(0.0f, 1.0f);
            bool flag = value < 0.15;
            if (flag && !Owner.HasPickupID(817))
            {
                PlayerController player = this.Owner;
                float curHealth = player.healthHaver.GetCurrentHealth();
                AkSoundEngine.PostEvent("Play_WPN_egg_impact_01", base.gameObject);

                player.PlayEffectOnActor(ResourceCache.Acquire("Global VFX/VFX_Healing_Sparkles_001") as GameObject, Vector3.zero, true, false, false);
                player.healthHaver.ForceSetCurrentHealth(curHealth + 0.5f);

                UnityEngine.Object.Destroy(base.gameObject, 1f);
                player.DropPassiveItem(this);

                PickupObject.ItemQuality itemQuality = PickupObject.ItemQuality.D;
                PickupObject itemOfTypeAndQuality = LootEngine.GetItemOfTypeAndQuality<PickupObject>(itemQuality, (UnityEngine.Random.value >= 0.5f) ? GameManager.Instance.RewardManager.GunsLootTable : GameManager.Instance.RewardManager.ItemsLootTable, false);
                if (itemOfTypeAndQuality)
                {
                    LootEngine.SpawnItem(itemOfTypeAndQuality.gameObject, base.transform.position, Vector2.up, 0.1f, true, false, false);
                }
            }

            else { return; }
        }

        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            player.healthHaver.OnDamaged -= this.BreakEgg;
            return debrisObject;
        }
    }
}

