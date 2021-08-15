using UnityEngine;
using ItemAPI;
using Dungeonator;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Blunderbeast
{
    public class WarVase : PassiveItem
    {

        //Call this method from the Start() method of your ETGModule extension
        public static void Init()
        {
            //The name of the item
            string itemName = "War Vase";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "Blunderbeast/Resources/warvase";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<WarVase>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Weaponized Pottery";
            string longDesc = "Destroyed breakable objects fire at enemies.\n\n" +
                "Military commander of the Ceramic division. Inspires all of its inanimate troops to take up arms against the Gundead invaders.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "rtr");

            //Adds the actual passive effect to the item

            //Set the rarity of the item
            item.quality = PickupObject.ItemQuality.B;
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.OnEnteredCombat += EnterRoom;
        }

        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            player.OnEnteredCombat -= EnterRoom;
            return debrisObject;
        }

        private void EnterRoom()
        {
            RoomHandler currentRoom = GameManager.Instance.PrimaryPlayer.CurrentRoom;
            for (int i = 0; i < StaticReferenceManager.AllMinorBreakables.Count; i++)
            {
                MinorBreakable minorBreakable = StaticReferenceManager.AllMinorBreakables[i];
                if (minorBreakable && !minorBreakable.IsBroken && minorBreakable.CenterPoint.GetAbsoluteRoom() == currentRoom && !minorBreakable.IgnoredForPotShotsModifier)
                {
                    MinorBreakable minorBreakable2 = minorBreakable;
                    minorBreakable2.OnBreakContext = (Action<MinorBreakable>)Delegate.Combine(minorBreakable2.OnBreakContext, new Action<MinorBreakable>(this.HandleBroken));
                }
            }
        }

        public AIActor GetNearestEnemy(List<AIActor> activeEnemies, Vector2 position, out float nearestDistance, string[] filter)
        {
            AIActor aiactor = null;
            nearestDistance = float.MaxValue;
            bool flag = activeEnemies == null;
            bool flag2 = flag;
            bool flag3 = flag2;
            bool flag4 = flag3;
            AIActor result;
            if (flag4)
            {
                result = null;
            }
            else
            {
                for (int i = 0; i < activeEnemies.Count; i++)
                {
                    AIActor aiactor2 = activeEnemies[i];
                    bool flag12 = !aiactor2.healthHaver.IsDead && aiactor2.healthHaver.IsVulnerable;
                    if (flag12)
                    {
                        bool flag13 = filter == null || !filter.Contains(aiactor2.EnemyGuid);
                        if (flag13)
                        {
                            float num = Vector2.Distance(position, aiactor2.CenterPosition);
                            bool flag17 = num < nearestDistance;
                            if (flag17)
                            {
                                nearestDistance = num;
                                aiactor = aiactor2;
                            }
                        }
                    }
                }
                result = aiactor;
            }
            return result;
        }

        private void HandleBroken(MinorBreakable mb)
        {
            PlayerController player = this.Owner;
            if (player.HasPickupID(532) || player.HasPickupID(214))
            {
                float value = UnityEngine.Random.Range(0.0f, 1.0f);
                if (value < 0.1)
                {
                    LootEngine.SpawnItem(PickupObjectDatabase.GetById(68).gameObject, mb.sprite.WorldCenter, Vector2.zero, 1f, false, false, false);
                }
            }

            if (player.CurrentRoom.HasActiveEnemies(RoomHandler.ActiveEnemyType.All))
            {
                if (mb && mb != null)
                {
                    float num = 10f;
                    List<AIActor> activeEnemies = this.Owner.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
                    bool flag3 = activeEnemies == null | activeEnemies.Count <= 0;
                    bool flag4 = !flag3;
                    if (flag4)
                    {
                        AIActor nearestEnemy = this.GetNearestEnemy(activeEnemies, mb.sprite.WorldCenter, out num, null);
                        if (nearestEnemy && nearestEnemy != null)
                        {

                            if (player.HasPickupID(152) && player.CurrentGun.PickupObjectId == 152)
                            {
                                Vector2 unitCenter = mb.sprite.WorldCenter;
                                Vector2 unitCenter2 = nearestEnemy.specRigidbody.HitboxPixelCollider.UnitCenter;
                                float z = BraveMathCollege.Atan2Degrees((unitCenter2 - unitCenter).normalized);
                                Projectile projectile = ((Gun)ETGMod.Databases.Items[152]).DefaultModule.projectiles[0];
                                GameObject gameObject = SpawnManager.SpawnProjectile(projectile.gameObject, mb.sprite.WorldCenter, Quaternion.Euler(0f, 0f, z), true);
                                Projectile component = gameObject.GetComponent<Projectile>();
                                bool flag11 = component != null;
                                if (flag11)
                                {
                                    component.Owner = player;
                                    component.baseData.range *= 5f;
                                    component.pierceMinorBreakables = true;
                                    component.collidesWithPlayer = false;
                                }
                            }

                            if (player.HasPickupID(7))
                            {
                                Vector2 unitCenter = mb.sprite.WorldCenter;
                                Vector2 unitCenter2 = nearestEnemy.specRigidbody.HitboxPixelCollider.UnitCenter;
                                float z = BraveMathCollege.Atan2Degrees((unitCenter2 - unitCenter).normalized);
                                Projectile projectile = ((Gun)ETGMod.Databases.Items[7]).DefaultModule.projectiles[0];
                                GameObject gameObject = SpawnManager.SpawnProjectile(projectile.gameObject, mb.sprite.WorldCenter, Quaternion.Euler(0f, 0f, z), true);
                                Projectile component = gameObject.GetComponent<Projectile>();
                                bool flag11 = component != null;
                                if (flag11)
                                {
                                    component.Owner = player;
                                    component.baseData.range *= 5f;
                                    component.pierceMinorBreakables = true;
                                    component.collidesWithPlayer = false;
                                }
                            }

                            else if (!player.HasPickupID(7) && player.CurrentGun.PickupObjectId != 152)
                            {
                                Vector2 unitCenter = mb.sprite.WorldCenter;
                                Vector2 unitCenter2 = nearestEnemy.specRigidbody.HitboxPixelCollider.UnitCenter;
                                float z = BraveMathCollege.Atan2Degrees((unitCenter2 - unitCenter).normalized);
                                Projectile projectile = ((Gun)ETGMod.Databases.Items[15]).DefaultModule.projectiles[0];
                                GameObject gameObject = SpawnManager.SpawnProjectile(projectile.gameObject, mb.sprite.WorldCenter, Quaternion.Euler(0f, 0f, z), true);
                                Projectile component = gameObject.GetComponent<Projectile>();
                                bool flag11 = component != null;
                                if (flag11)
                                {
                                    component.Owner = player;
                                    component.baseData.range *= 5f;
                                    component.pierceMinorBreakables = true;
                                    component.collidesWithPlayer = false;
                                }
                            }
                        }



                        else { return; }

                    }
                }
            }
        }
    }
}
