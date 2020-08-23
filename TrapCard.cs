using UnityEngine;
using ItemAPI;
using Dungeonator;
using System.Collections;
using System;
using MonoMod.RuntimeDetour;
using System.Reflection;

namespace Blunderbeast
{
    public class TrapCard : PassiveItem
    {

        //Call this method from the Start() method of your ETGModule extension
        public static void Init()
        {
            //The name of the item
            string itemName = "Trap Card";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "Blunderbeast/Resources/trapcard";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<TrapCard>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Counterplay";
            string longDesc = "Chance to counter with a bomb when dodge rolling over enemy bullets.\n\n" +
                "This type of card is sometimes used by denizens of the Gungeon as a mean to win arguments without having to resort to gunfire.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "rtr");

            //Adds the actual passive effect to the item

            //Set the rarity of the item
            item.quality = PickupObject.ItemQuality.B;
            TrapCardId = item.PickupObjectId;
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.EnemyProjectileSpeedMultiplier, 0.9f, StatModifier.ModifyMethod.MULTIPLICATIVE);
        }

        private static int TrapCardId;

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.OnDodgedProjectile += SpawnBomb;
            player.OnDodgedBeam += SpawnBombBeam;
        }

        private static Hook RatHook = new Hook(
               typeof(RatPackItem).GetMethod("EatBullet", BindingFlags.Instance | BindingFlags.NonPublic),
               typeof(TrapCard).GetMethod("RollHook")
           );

        public static void RollHook(Action<RatPackItem, Projectile> orig, RatPackItem item, Projectile proj)
        {
            PlayerController player = item.LastOwner;
            if (player.HasPickupID(TrapCardId))
            {             
                if (cooldown == false)
                {
                    float value = UnityEngine.Random.Range(0.0f, 1.0f);
                    bool flag = value < 0.3;
                    if (flag || player.HasPickupID(423))
                    {
                        AIActor enemy = proj.Owner as AIActor;
                        if (enemy != null)
                        {
                            cooldown = true;
                            GameManager.Instance.StartCoroutine(StartCooldown());
                            LootEngine.DoDefaultItemPoof(enemy.sprite.WorldBottomCenter, false, true);
                            AkSoundEngine.PostEvent("Play_OBJ_bomb_fuse_01", enemy.gameObject);

                            if (player.HasPickupID(109) || player.HasPickupID(364) || player.HasPickupID(170))
                            {
                                SpawnObjectPlayerItem icebombPrefab = PickupObjectDatabase.GetById(109).GetComponent<SpawnObjectPlayerItem>();
                                GameObject icebombObject = icebombPrefab.objectToSpawn.gameObject;

                                GameObject icebombObject2 = UnityEngine.Object.Instantiate<GameObject>(icebombObject, enemy.sprite.WorldBottomCenter, Quaternion.identity);
                                tk2dBaseSprite icebombsprite = icebombObject2.GetComponent<tk2dBaseSprite>();
                                if (icebombsprite)
                                {
                                    icebombsprite.PlaceAtPositionByAnchor(enemy.sprite.WorldBottomCenter, tk2dBaseSprite.Anchor.MiddleCenter);
                                }
                            }

                            else if (!player.HasPickupID(109) || !player.HasPickupID(364) || !player.HasPickupID(170))
                            {
                                SpawnObjectPlayerItem bigbombPrefab = PickupObjectDatabase.GetById(108).GetComponent<SpawnObjectPlayerItem>();
                                GameObject bigbombObject = bigbombPrefab.objectToSpawn.gameObject;

                                GameObject bigbombObject2 = UnityEngine.Object.Instantiate<GameObject>(bigbombObject, enemy.sprite.WorldBottomCenter, Quaternion.identity);
                                tk2dBaseSprite bombsprite = bigbombObject2.GetComponent<tk2dBaseSprite>();
                                if (bombsprite)
                                {
                                    bombsprite.PlaceAtPositionByAnchor(enemy.sprite.WorldBottomCenter, tk2dBaseSprite.Anchor.MiddleCenter);
                                }
                            }
                        }

                        else
                        {
                            RoomHandler absoluteRoom = player.CurrentRoom;
                            if (absoluteRoom.HasActiveEnemies(RoomHandler.ActiveEnemyType.All))
                            {
                                AIActor randomActiveEnemy = absoluteRoom.GetNearestEnemy(Vector2.zero, out float nearestDistance, true, true);
                                nearestDistance = float.MaxValue;
                                if (randomActiveEnemy != null)
                                {
                                    if (player.HasPickupID(109) || player.HasPickupID(364) || player.HasPickupID(170))
                                    {
                                        SpawnObjectPlayerItem icebombPrefab = PickupObjectDatabase.GetById(109).GetComponent<SpawnObjectPlayerItem>();
                                        GameObject icebombObject = icebombPrefab.objectToSpawn.gameObject;

                                        GameObject icebombObject2 = UnityEngine.Object.Instantiate<GameObject>(icebombObject, randomActiveEnemy.sprite.WorldBottomCenter, Quaternion.identity);
                                        tk2dBaseSprite icebombsprite = icebombObject2.GetComponent<tk2dBaseSprite>();
                                        if (icebombsprite)
                                        {
                                            icebombsprite.PlaceAtPositionByAnchor(randomActiveEnemy.sprite.WorldBottomCenter, tk2dBaseSprite.Anchor.MiddleCenter);
                                        }
                                    }

                                    else if (!player.HasPickupID(109) || !player.HasPickupID(364) || !player.HasPickupID(170))
                                    {
                                        SpawnObjectPlayerItem bigbombPrefab = PickupObjectDatabase.GetById(108).GetComponent<SpawnObjectPlayerItem>();
                                        GameObject bigbombObject = bigbombPrefab.objectToSpawn.gameObject;

                                        GameObject bigbombObject2 = UnityEngine.Object.Instantiate<GameObject>(bigbombObject, randomActiveEnemy.sprite.WorldBottomCenter, Quaternion.identity);
                                        tk2dBaseSprite bombsprite = bigbombObject2.GetComponent<tk2dBaseSprite>();
                                        if (bombsprite)
                                        {
                                            bombsprite.PlaceAtPositionByAnchor(randomActiveEnemy.sprite.WorldBottomCenter, tk2dBaseSprite.Anchor.MiddleCenter);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            orig(item, proj);
        }           
        

        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            player.OnDodgedProjectile -= SpawnBomb;
            player.OnDodgedBeam -= SpawnBombBeam;
            return debrisObject;
        }

        private static bool cooldown;

        private void SpawnBomb(Projectile proj)
        {
            PlayerController player = this.Owner;
            AIActor enemy = proj.Owner as AIActor;
            if (cooldown == false)
            {
                float value = UnityEngine.Random.Range(0.0f, 1.0f);
                bool flag = value < 0.3;
                if (flag || player.HasPickupID(423))
                {
                    if (enemy != null)
                    {
                        cooldown = true;
                        GameManager.Instance.StartCoroutine(StartCooldown());
                        LootEngine.DoDefaultItemPoof(enemy.sprite.WorldBottomCenter, false, true);
                        AkSoundEngine.PostEvent("Play_OBJ_bomb_fuse_01", gameObject);

                        if (player.HasPickupID(109) || player.HasPickupID(364) || player.HasPickupID(170))
                        {
                            SpawnObjectPlayerItem icebombPrefab = PickupObjectDatabase.GetById(109).GetComponent<SpawnObjectPlayerItem>();
                            GameObject icebombObject = icebombPrefab.objectToSpawn.gameObject;

                            GameObject icebombObject2 = UnityEngine.Object.Instantiate<GameObject>(icebombObject, enemy.sprite.WorldBottomCenter, Quaternion.identity);
                            tk2dBaseSprite icebombsprite = icebombObject2.GetComponent<tk2dBaseSprite>();
                            if (icebombsprite)
                            {
                                icebombsprite.PlaceAtPositionByAnchor(enemy.sprite.WorldBottomCenter, tk2dBaseSprite.Anchor.MiddleCenter);
                            }
                        }

                        else if (!player.HasPickupID(109) || !player.HasPickupID(364) || !player.HasPickupID(170))
                        {
                            SpawnObjectPlayerItem bigbombPrefab = PickupObjectDatabase.GetById(108).GetComponent<SpawnObjectPlayerItem>();
                            GameObject bigbombObject = bigbombPrefab.objectToSpawn.gameObject;

                            GameObject bigbombObject2 = UnityEngine.Object.Instantiate<GameObject>(bigbombObject, enemy.sprite.WorldBottomCenter, Quaternion.identity);
                            tk2dBaseSprite bombsprite = bigbombObject2.GetComponent<tk2dBaseSprite>();
                            if (bombsprite)
                            {
                                bombsprite.PlaceAtPositionByAnchor(enemy.sprite.WorldBottomCenter, tk2dBaseSprite.Anchor.MiddleCenter);
                            }
                        }
                    }

                    else
                    {
                        RoomHandler absoluteRoom = player.CurrentRoom;
                        if (absoluteRoom.HasActiveEnemies(RoomHandler.ActiveEnemyType.All))
                        {
                            AIActor randomActiveEnemy = absoluteRoom.GetNearestEnemy(Vector2.zero, out float nearestDistance, true, true);
                            nearestDistance = float.MaxValue;
                            if (randomActiveEnemy != null)
                            {
                                if (player.HasPickupID(109) || player.HasPickupID(364) || player.HasPickupID(170))
                                {
                                    SpawnObjectPlayerItem icebombPrefab = PickupObjectDatabase.GetById(109).GetComponent<SpawnObjectPlayerItem>();
                                    GameObject icebombObject = icebombPrefab.objectToSpawn.gameObject;

                                    GameObject icebombObject2 = UnityEngine.Object.Instantiate<GameObject>(icebombObject, randomActiveEnemy.sprite.WorldBottomCenter, Quaternion.identity);
                                    tk2dBaseSprite icebombsprite = icebombObject2.GetComponent<tk2dBaseSprite>();
                                    if (icebombsprite)
                                    {
                                        icebombsprite.PlaceAtPositionByAnchor(randomActiveEnemy.sprite.WorldBottomCenter, tk2dBaseSprite.Anchor.MiddleCenter);
                                    }
                                }

                                else if (!player.HasPickupID(109) || !player.HasPickupID(364) || !player.HasPickupID(170))
                                {
                                    SpawnObjectPlayerItem bigbombPrefab = PickupObjectDatabase.GetById(108).GetComponent<SpawnObjectPlayerItem>();
                                    GameObject bigbombObject = bigbombPrefab.objectToSpawn.gameObject;

                                    GameObject bigbombObject2 = UnityEngine.Object.Instantiate<GameObject>(bigbombObject, randomActiveEnemy.sprite.WorldBottomCenter, Quaternion.identity);
                                    tk2dBaseSprite bombsprite = bigbombObject2.GetComponent<tk2dBaseSprite>();
                                    if (bombsprite)
                                    {
                                        bombsprite.PlaceAtPositionByAnchor(randomActiveEnemy.sprite.WorldBottomCenter, tk2dBaseSprite.Anchor.MiddleCenter);
                                    }
                                }
                            }
                        }
                    }

                }
            }
        }

        private static IEnumerator StartCooldown()
        {
            yield return new WaitForSeconds(2f);
            cooldown = false;
            yield break;
        }


        private void SpawnBombBeam(BeamController beam, PlayerController player)
        {
            AIActor enemy = beam.Owner as AIActor;
            if (cooldown == false)
            {
                float value = UnityEngine.Random.Range(0.0f, 1.0f);
                bool flag = value < 0.125;
                if (flag || player.HasPickupID(423))
                {
                    if (enemy != null)
                    {
                        cooldown = true;
                        GameManager.Instance.StartCoroutine(StartCooldown());
                        LootEngine.DoDefaultItemPoof(enemy.sprite.WorldBottomCenter, false, true);
                        AkSoundEngine.PostEvent("Play_OBJ_bomb_fuse_01", base.gameObject);

                        if (player.HasPickupID(109) || player.HasPickupID(364) || player.HasPickupID(170))
                        {
                            SpawnObjectPlayerItem icebombPrefab = PickupObjectDatabase.GetById(109).GetComponent<SpawnObjectPlayerItem>();
                            GameObject icebombObject = icebombPrefab.objectToSpawn.gameObject;

                            GameObject icebombObject2 = UnityEngine.Object.Instantiate<GameObject>(icebombObject, enemy.sprite.WorldBottomCenter, Quaternion.identity);
                            tk2dBaseSprite icebombsprite = icebombObject2.GetComponent<tk2dBaseSprite>();
                            if (icebombsprite)
                            {
                                icebombsprite.PlaceAtPositionByAnchor(enemy.sprite.WorldBottomCenter, tk2dBaseSprite.Anchor.MiddleCenter);
                            }
                        }

                        else if (!player.HasPickupID(109) || !player.HasPickupID(364) || !player.HasPickupID(170))
                        {
                            SpawnObjectPlayerItem bigbombPrefab = PickupObjectDatabase.GetById(108).GetComponent<SpawnObjectPlayerItem>();
                            GameObject bigbombObject = bigbombPrefab.objectToSpawn.gameObject;

                            GameObject bigbombObject2 = UnityEngine.Object.Instantiate<GameObject>(bigbombObject, enemy.sprite.WorldBottomCenter, Quaternion.identity);
                            tk2dBaseSprite bombsprite = bigbombObject2.GetComponent<tk2dBaseSprite>();
                            if (bombsprite)
                            {
                                bombsprite.PlaceAtPositionByAnchor(enemy.sprite.WorldBottomCenter, tk2dBaseSprite.Anchor.MiddleCenter);
                            }
                        }
                    }

                    else
                    {
                        RoomHandler absoluteRoom = player.CurrentRoom;
                        if (absoluteRoom.HasActiveEnemies(RoomHandler.ActiveEnemyType.All))
                        {
                            AIActor randomActiveEnemy = absoluteRoom.GetNearestEnemy(Vector2.zero, out float nearestDistance, true, true);
                            nearestDistance = float.MaxValue;
                            if (randomActiveEnemy != null)
                            {
                                if (player.HasPickupID(109) || player.HasPickupID(364) || player.HasPickupID(170))
                                {
                                    SpawnObjectPlayerItem icebombPrefab = PickupObjectDatabase.GetById(109).GetComponent<SpawnObjectPlayerItem>();
                                    GameObject icebombObject = icebombPrefab.objectToSpawn.gameObject;

                                    GameObject icebombObject2 = UnityEngine.Object.Instantiate<GameObject>(icebombObject, randomActiveEnemy.sprite.WorldBottomCenter, Quaternion.identity);
                                    tk2dBaseSprite icebombsprite = icebombObject2.GetComponent<tk2dBaseSprite>();
                                    if (icebombsprite)
                                    {
                                        icebombsprite.PlaceAtPositionByAnchor(randomActiveEnemy.sprite.WorldBottomCenter, tk2dBaseSprite.Anchor.MiddleCenter);
                                    }
                                }

                                else if (!player.HasPickupID(109) || !player.HasPickupID(364) || !player.HasPickupID(170))
                                {
                                    SpawnObjectPlayerItem bigbombPrefab = PickupObjectDatabase.GetById(108).GetComponent<SpawnObjectPlayerItem>();
                                    GameObject bigbombObject = bigbombPrefab.objectToSpawn.gameObject;

                                    GameObject bigbombObject2 = UnityEngine.Object.Instantiate<GameObject>(bigbombObject, randomActiveEnemy.sprite.WorldBottomCenter, Quaternion.identity);
                                    tk2dBaseSprite bombsprite = bigbombObject2.GetComponent<tk2dBaseSprite>();
                                    if (bombsprite)
                                    {
                                        bombsprite.PlaceAtPositionByAnchor(randomActiveEnemy.sprite.WorldBottomCenter, tk2dBaseSprite.Anchor.MiddleCenter);
                                    }
                                }
                            }
                        }
                    }
                }                
            }
        }
    }
}

