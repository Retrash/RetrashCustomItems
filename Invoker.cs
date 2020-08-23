using UnityEngine;
using ItemAPI;
using Dungeonator;
using System;
using System.Collections;

namespace Blunderbeast
{
    public class Invoker : PlayerItem
    {
        public static void Init()
        {
            //The name of the item
            string itemName = "Invoker";

            //Refers to an embedded png in the project. Make sure to embed your resources!
            string resourceName = "Blunderbeast/Resources/invoker";

            //Create new GameObject
            GameObject obj = new GameObject();

            //Add a ActiveItem component to the object
            var item = obj.AddComponent<Invoker>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Safe Harm";
            string longDesc = "Activates damage-triggered effects without injuring its user.\n\n" +
                "This gun of unknown origin is unable to cause injuries but has a peculiar ability to channel innate powers when shot at oneself. Perhaps an attempt to recreate the Gun That Can Kill The Past.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //"example_pool" here is the item pool. In the console you'd type "give example_pool:sweating_bullets"
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "rtr");

            //Set the cooldown type and duration of the cooldown
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Damage, 400f);

            //Adds a passive modifier, like curse, coolness, damage, etc. to the item. Works for passives and actives.

            //Set some other fields
            item.consumable = false;
            item.quality = PickupObject.ItemQuality.D;
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
        }

        protected override void DoEffect(PlayerController user)
        {

            AkSoundEngine.PostEvent("Play_WPN_deck4rd_shot_01", base.gameObject);
            float curHealth = user.healthHaver.GetCurrentHealth();

            if (user.healthHaver.NextShotKills == true)
            {
                user.healthHaver.NextShotKills = false;
                if (user.HasPickupID(564))
                {
                    for (int i = 0; i < user.activeItems.Count; i++)
                    {
                        if (user.activeItems[i].PickupObjectId == (499) && !user.activeItems[i].IsOnCooldown)
                        {
                            user.healthHaver.ApplyDamage(0.5f, Vector2.zero, "Invoker... how?", CoreDamageTypes.None, DamageCategory.Normal, true, null, false);
                        }

                        else if (user.Blanks > 0)
                        {
                            user.healthHaver.ApplyDamage(0.5f, Vector2.zero, "Invoker... how?", CoreDamageTypes.None, DamageCategory.Normal, true, null, false);
                        }

                        else if (user.healthHaver.Armor >= 1f || user.healthHaver.HasCrest)
                        {
                            if (user.CurrentRoom != null && user.CurrentRoom.PlayerHasTakenDamageInThisRoom == false)
                            {
                                user.healthHaver.Armor += 1f;
                                user.healthHaver.ApplyDamage(0.5f, Vector2.zero, "Invoker... how?", CoreDamageTypes.None, DamageCategory.Normal, true, null, false);
                                GameManager.Instance.StartCoroutine(this.SaveFlawless());
                            }

                            else
                            {
                                user.healthHaver.Armor += 1f;
                                user.healthHaver.ApplyDamage(0.5f, Vector2.zero, "Invoker... how?", CoreDamageTypes.None, DamageCategory.Normal, true, null, false);
                            }
                        }

                        else if (curHealth == 0.5f)
                        {
                            if (user.CurrentRoom != null && user.CurrentRoom.PlayerHasTakenDamageInThisRoom == false)
                            {
                                user.healthHaver.ForceSetCurrentHealth(curHealth + 0.5f);
                                user.healthHaver.ApplyDamage(0.5f, Vector2.zero, "Invoker... how?", CoreDamageTypes.None, DamageCategory.Normal, true, null, false);

                                GameManager.Instance.StartCoroutine(this.SaveFlawless());
                            }

                            else
                            {
                                user.healthHaver.ForceSetCurrentHealth(curHealth + 0.5f);
                                user.healthHaver.ApplyDamage(0.5f, Vector2.zero, "Invoker... how?", CoreDamageTypes.None, DamageCategory.Normal, true, null, false);
                            }
                        }

                        else
                        {
                            if (user.CurrentRoom != null && user.CurrentRoom.PlayerHasTakenDamageInThisRoom == false)
                            {
                                user.healthHaver.ApplyDamage(0.5f, Vector2.zero, "Invoker... how?", CoreDamageTypes.None, DamageCategory.Normal, true, null, false);
                                float curHealth2 = user.healthHaver.GetCurrentHealth();
                                user.healthHaver.ForceSetCurrentHealth(curHealth2 + 0.5f);
                                GameManager.Instance.StartCoroutine(this.SaveFlawless());
                            }

                            else
                            {
                                user.healthHaver.ApplyDamage(0.5f, Vector2.zero, "Invoker... how?", CoreDamageTypes.None, DamageCategory.Normal, true, null, false);
                                float curHealth2 = user.healthHaver.GetCurrentHealth();
                                user.healthHaver.ForceSetCurrentHealth(curHealth2 + 0.5f);
                            }
                        }
                    }
                }

                else if (user.healthHaver.Armor >= 1f || user.healthHaver.HasCrest)
                {
                    if (user.CurrentRoom != null && user.CurrentRoom.PlayerHasTakenDamageInThisRoom == false)
                    {
                        user.healthHaver.Armor += 1f;
                        user.healthHaver.ApplyDamage(0.5f, Vector2.zero, "Invoker... how?", CoreDamageTypes.None, DamageCategory.Normal, true, null, false);
                        GameManager.Instance.StartCoroutine(this.SaveFlawless());
                    }

                    else
                    {
                        user.healthHaver.Armor += 1f;
                        user.healthHaver.ApplyDamage(0.5f, Vector2.zero, "Invoker... how?", CoreDamageTypes.None, DamageCategory.Normal, true, null, false);
                    }
                }

                else if (curHealth == 0.5f)
                {
                    if (user.CurrentRoom != null && user.CurrentRoom.PlayerHasTakenDamageInThisRoom == false)
                    {
                        user.healthHaver.ForceSetCurrentHealth(curHealth + 0.5f);
                        user.healthHaver.ApplyDamage(0.5f, Vector2.zero, "Invoker... how?", CoreDamageTypes.None, DamageCategory.Normal, true, null, false);

                        GameManager.Instance.StartCoroutine(this.SaveFlawless());
                    }

                    else
                    {
                        user.healthHaver.ForceSetCurrentHealth(curHealth + 0.5f);
                        user.healthHaver.ApplyDamage(0.5f, Vector2.zero, "Invoker... how?", CoreDamageTypes.None, DamageCategory.Normal, true, null, false);
                    }
                }

                else
                {
                    if (user.CurrentRoom != null && user.CurrentRoom.PlayerHasTakenDamageInThisRoom == false)
                    {
                        user.healthHaver.ApplyDamage(0.5f, Vector2.zero, "Invoker... how?", CoreDamageTypes.None, DamageCategory.Normal, true, null, false);
                        float curHealth2 = user.healthHaver.GetCurrentHealth();
                        user.healthHaver.ForceSetCurrentHealth(curHealth2 + 0.5f);
                        GameManager.Instance.StartCoroutine(this.SaveFlawless());
                    }

                    else
                    {
                        user.healthHaver.ApplyDamage(0.5f, Vector2.zero, "Invoker... how?", CoreDamageTypes.None, DamageCategory.Normal, true, null, false);
                        float curHealth2 = user.healthHaver.GetCurrentHealth();
                        user.healthHaver.ForceSetCurrentHealth(curHealth2 + 0.5f);
                    }
                }
            }

            else
            {
                if (user.HasPickupID(564))
                {
                    for (int i = 0; i < user.activeItems.Count; i++)
                    {
                        if (user.activeItems[i].PickupObjectId == (499) && !user.activeItems[i].IsOnCooldown)
                        {
                            user.healthHaver.ApplyDamage(0.5f, Vector2.zero, "Invoker... how?", CoreDamageTypes.None, DamageCategory.Normal, true, null, false);
                        }

                        else if (user.Blanks > 0)
                        {
                            user.healthHaver.ApplyDamage(0.5f, Vector2.zero, "Invoker... how?", CoreDamageTypes.None, DamageCategory.Normal, true, null, false);
                        }

                        else if (user.healthHaver.Armor >= 1f || user.healthHaver.HasCrest)
                        {
                            if (user.CurrentRoom != null && user.CurrentRoom.PlayerHasTakenDamageInThisRoom == false)
                            {
                                user.healthHaver.Armor += 1f;
                                user.healthHaver.ApplyDamage(0.5f, Vector2.zero, "Invoker... how?", CoreDamageTypes.None, DamageCategory.Normal, true, null, false);
                                GameManager.Instance.StartCoroutine(this.SaveFlawless());
                            }

                            else
                            {
                                user.healthHaver.Armor += 1f;
                                user.healthHaver.ApplyDamage(0.5f, Vector2.zero, "Invoker... how?", CoreDamageTypes.None, DamageCategory.Normal, true, null, false);
                            }
                        }

                        else if (curHealth == 0.5f)
                        {
                            if (user.CurrentRoom != null && user.CurrentRoom.PlayerHasTakenDamageInThisRoom == false)
                            {
                                user.healthHaver.ForceSetCurrentHealth(curHealth + 0.5f);
                                user.healthHaver.ApplyDamage(0.5f, Vector2.zero, "Invoker... how?", CoreDamageTypes.None, DamageCategory.Normal, true, null, false);

                                GameManager.Instance.StartCoroutine(this.SaveFlawless());
                            }

                            else
                            {
                                user.healthHaver.ForceSetCurrentHealth(curHealth + 0.5f);
                                user.healthHaver.ApplyDamage(0.5f, Vector2.zero, "Invoker... how?", CoreDamageTypes.None, DamageCategory.Normal, true, null, false);
                            }
                        }

                        else
                        {
                            if (user.CurrentRoom != null && user.CurrentRoom.PlayerHasTakenDamageInThisRoom == false)
                            {
                                user.healthHaver.ApplyDamage(0.5f, Vector2.zero, "Invoker... how?", CoreDamageTypes.None, DamageCategory.Normal, true, null, false);
                                float curHealth2 = user.healthHaver.GetCurrentHealth();
                                user.healthHaver.ForceSetCurrentHealth(curHealth2 + 0.5f);
                                GameManager.Instance.StartCoroutine(this.SaveFlawless());
                            }

                            else
                            {
                                user.healthHaver.ApplyDamage(0.5f, Vector2.zero, "Invoker... how?", CoreDamageTypes.None, DamageCategory.Normal, true, null, false);
                                float curHealth2 = user.healthHaver.GetCurrentHealth();
                                user.healthHaver.ForceSetCurrentHealth(curHealth2 + 0.5f);
                            }
                        }
                    }
                }

                else if (user.healthHaver.Armor >= 1f || user.healthHaver.HasCrest)
                {
                    if (user.CurrentRoom != null && user.CurrentRoom.PlayerHasTakenDamageInThisRoom == false)
                    {
                        user.healthHaver.Armor += 1f;
                        user.healthHaver.ApplyDamage(0.5f, Vector2.zero, "Invoker... how?", CoreDamageTypes.None, DamageCategory.Normal, true, null, false);
                        GameManager.Instance.StartCoroutine(this.SaveFlawless());
                    }

                    else
                    {
                        user.healthHaver.Armor += 1f;
                        user.healthHaver.ApplyDamage(0.5f, Vector2.zero, "Invoker... how?", CoreDamageTypes.None, DamageCategory.Normal, true, null, false);
                    }
                }

                else if (curHealth == 0.5f)
                {
                    if (user.CurrentRoom != null && user.CurrentRoom.PlayerHasTakenDamageInThisRoom == false)
                    {
                        user.healthHaver.ForceSetCurrentHealth(curHealth + 0.5f);
                        user.healthHaver.ApplyDamage(0.5f, Vector2.zero, "Invoker... how?", CoreDamageTypes.None, DamageCategory.Normal, true, null, false);

                        GameManager.Instance.StartCoroutine(this.SaveFlawless());
                    }

                    else
                    {
                        user.healthHaver.ForceSetCurrentHealth(curHealth + 0.5f);
                        user.healthHaver.ApplyDamage(0.5f, Vector2.zero, "Invoker... how?", CoreDamageTypes.None, DamageCategory.Normal, true, null, false);
                    }
                }

                else
                {
                    if (user.CurrentRoom != null && user.CurrentRoom.PlayerHasTakenDamageInThisRoom == false)
                    {
                        user.healthHaver.ApplyDamage(0.5f, Vector2.zero, "Invoker... how?", CoreDamageTypes.None, DamageCategory.Normal, true, null, false);
                        float curHealth2 = user.healthHaver.GetCurrentHealth();
                        user.healthHaver.ForceSetCurrentHealth(curHealth2 + 0.5f);
                        GameManager.Instance.StartCoroutine(this.SaveFlawless());
                    }

                    else
                    {
                        user.healthHaver.ApplyDamage(0.5f, Vector2.zero, "Invoker... how?", CoreDamageTypes.None, DamageCategory.Normal, true, null, false);
                        float curHealth2 = user.healthHaver.GetCurrentHealth();
                        user.healthHaver.ForceSetCurrentHealth(curHealth2 + 0.5f);
                    }
                }
            }


            if (user.HasPickupID(489))
            {
                Projectile projectile = ((Gun)ETGMod.Databases.Items[198]).DefaultModule.projectiles[0];
                GameObject gameObject = SpawnManager.SpawnProjectile(projectile.gameObject, user.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (user.CurrentGun == null) ? 0f : user.CurrentGun.CurrentAngle), true);
                Projectile component = gameObject.GetComponent<Projectile>();
                if (component != null)
                {
                    component.Owner = user;
                    component.baseData.range *= 20f;
                    component.baseData.damage *= 2f;
                }
            }
        }

        private IEnumerator SaveFlawless()
        {
            yield return new WaitForSeconds(0.1f);
            PlayerController player = this.LastOwner;
            if (player.CurrentRoom != null)
            {
                player.CurrentRoom.PlayerHasTakenDamageInThisRoom = false;
            }
            yield break;
        }

        public override bool CanBeUsed(PlayerController user)
        {
            return user.healthHaver.IsVulnerable;
        }

    }
}