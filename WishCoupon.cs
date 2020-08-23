using UnityEngine;
using ItemAPI;
using System.Collections.Generic;
using System.Linq;
using Dungeonator;
using System;
using System.Collections;

namespace Blunderbeast
{
    public class WishCoupon : PassiveItem
    {

        //Call this method from the Start() method of your ETGModule extension
        public static void Init()
        {
            //The name of the item
            string itemName = "Wish Charm";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "Blunderbeast/Resources/wishcharm";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<WishCoupon>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Djinn's Mercy";
            string longDesc = "Temporarily summons a Gunie as an ally whenever injured. \n\n" +
                "This charm is a symbol of cooperation made with the mystical Gunies. They are still very fickle beings and will only lend their aid under the right circumstances.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "rtr");

            //Adds the actual passive effect to the item

            //Set the rarity of the item
            item.quality = PickupObject.ItemQuality.C;
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.healthHaver.OnDamaged += this.HandleGenieSpawn;
            player.OnRoomClearEvent += this.RoomCleared;
            player.OnEnteredCombat += this.OnEnteredCombat;
        }

        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            ActiveGenie = false;
            player.healthHaver.OnDamaged -= this.HandleGenieSpawn;
            player.OnRoomClearEvent -= this.RoomCleared;
            player.OnEnteredCombat -= this.OnEnteredCombat;
            return debrisObject;
        }

        private void OnEnteredCombat()
        {
            if (Owner.HasPickupID(0))
            roomCount += 1;
            if (roomCount >= 3)
            {
                if (Owner.IsInCombat)
                {
                    ActiveGenie = true;
                    GameManager.Instance.StartCoroutine(this.GenieTimer());
                    roomCount = 0;
                }
            }
        }

        private int roomCount;

        private bool ActiveGenie;

        private void RoomCleared(PlayerController player)
        {
            ActiveGenie = false;
        }

        private void HandleGenieSpawn(float resultValue, float maxValue, CoreDamageTypes damageTypes, DamageCategory damageCategory, Vector2 damageDirection)
        {
            PlayerController player = this.Owner;
            if (ActiveGenie != true && Owner.IsInCombat)
            {
                ActiveGenie = true;
                GameManager.Instance.StartCoroutine(this.GenieTimer());
            }

            if (player.HasPickupID(369))
            {
                Projectile projectile = ((Gun)ETGMod.Databases.Items[369]).DefaultModule.projectiles[0];
                GameObject gameObject = SpawnManager.SpawnProjectile(projectile.gameObject, player.sprite.WorldCenter, Quaternion.Euler(0f, 0f, 0f), true);
                Projectile component = gameObject.GetComponent<Projectile>();
                if (component != null)
                {
                    component.Owner = player;
                    component.baseData.range = 0f;
                    component.baseData.damage = 0f;
                    component.collidesWithPlayer = true;
                }
            }
        }

        private IEnumerator GenieTimer()
        {

            yield return new WaitForSeconds(0.1f);
            if (Owner.IsInCombat && ActiveGenie == true)
            {
                RoomHandler absoluteRoom = Owner.transform.position.GetAbsoluteRoom();
                AIActor randomActiveEnemy = absoluteRoom.GetRandomActiveEnemy(false);
                if (randomActiveEnemy != null && randomActiveEnemy.healthHaver && randomActiveEnemy.healthHaver.IsAlive && randomActiveEnemy.healthHaver.IsVulnerable)
                {
                    this.SpawnGenie(randomActiveEnemy);
                    GameManager.Instance.StartCoroutine(this.GenieTimer2());
                    //if (Owner.HasPickupID(0))
                    //{
                    //    GameManager.Instance.StartCoroutine(this.GenieTimerSynergy());
                    //}

                }
                else
                {
                    GameManager.Instance.StartCoroutine(this.GenieTimer());
                }
                yield break;
            }
        }

        //private IEnumerator GenieTimerSynergy()
        //{

        //    yield return new WaitForSeconds(2f);
        //    if (Owner.IsInCombat && ActiveGenie == true)
        //    {
        //        RoomHandler absoluteRoom = Owner.transform.position.GetAbsoluteRoom();
        //        AIActor randomActiveEnemy = absoluteRoom.GetRandomActiveEnemy(false);
        //        if (randomActiveEnemy != null && randomActiveEnemy.healthHaver && randomActiveEnemy.healthHaver.IsAlive && randomActiveEnemy.healthHaver.IsVulnerable && randomActiveEnemy.healthHaver.CanCurrentlyBeKilled)
        //        {
        //            this.SpawnGenie(randomActiveEnemy);
        //            GameManager.Instance.StartCoroutine(this.GenieTimer2());
        //        }
        //        else
        //        {
        //            GameManager.Instance.StartCoroutine(this.GenieTimer());
        //        }
        //        yield break;
        //    }
        //}


        private IEnumerator GenieTimer2()
        {
            if (Owner.IsInCombat && ActiveGenie == true)
            {
                yield return new WaitForSeconds(1f);
            }
            if (Owner.IsInCombat && ActiveGenie == true)
            {
                yield return new WaitForSeconds(1f);
            }
            if (Owner.IsInCombat && ActiveGenie == true)
            {
                yield return new WaitForSeconds(1f);
            }
            if (Owner.IsInCombat && ActiveGenie == true)
            {
                yield return new WaitForSeconds(1f);
            }
            if (Owner.IsInCombat && ActiveGenie == true)
            {
                yield return new WaitForSeconds(1f);
            }
            if (Owner.IsInCombat && ActiveGenie == true)
            {
                yield return new WaitForSeconds(1f);
            }
            yield return new WaitForSeconds(1f);
            if (Owner.IsInCombat && ActiveGenie == true)
            {
                RoomHandler absoluteRoom = Owner.transform.position.GetAbsoluteRoom();
                AIActor randomActiveEnemy = absoluteRoom.GetRandomActiveEnemy(false);
                if (randomActiveEnemy != null && randomActiveEnemy.healthHaver && randomActiveEnemy.healthHaver.IsAlive && randomActiveEnemy.healthHaver.IsVulnerable)
                {
                    this.SpawnGenie(randomActiveEnemy);
                    GameManager.Instance.StartCoroutine(this.GenieTimer2());
                }
                else
                {
                    GameManager.Instance.StartCoroutine(this.GenieTimer());
                }
                yield break;
            }
        }


        protected void SpawnGenie(AIActor randomActiveEnemy)
        {
            PlayerController player = this.Owner;
            if (randomActiveEnemy && randomActiveEnemy.behaviorSpeculator && ActiveGenie == true)
            {
                randomActiveEnemy.behaviorSpeculator.Stun(1f, false);
                Projectile projectile = ((Gun)ETGMod.Databases.Items[0]).DefaultModule.projectiles[0];
                GameObject gameObject = SpawnManager.SpawnProjectile(projectile.gameObject, randomActiveEnemy.sprite.WorldCenter, Quaternion.Euler(0f, 0f, 0f), true);
                GameObject gameObject2 = SpawnManager.SpawnProjectile(projectile.gameObject, randomActiveEnemy.sprite.WorldCenter, Quaternion.Euler(0f, 0f, 0f), true);
                GameObject gameObject3 = SpawnManager.SpawnProjectile(projectile.gameObject, randomActiveEnemy.sprite.WorldCenter, Quaternion.Euler(0f, 0f, 0f), true);
                Projectile component = gameObject.GetComponent<Projectile>();
                Projectile component2 = gameObject2.GetComponent<Projectile>();
                Projectile component3 = gameObject3.GetComponent<Projectile>();
                bool flag11 = component != null && component2 != null && component3 != null;
                bool flag12 = flag11;
                bool flag13 = flag12;
                if (flag13)
                {
                    component.IgnoreTileCollisionsFor(10);
                    component.Owner = player;
                    component.baseData.damage = 0f;
                    component.baseData.force = 0f;
                    component.baseData.range = 0f;
                    component.pierceMinorBreakables = true;
                    component.collidesWithPlayer = false;
                    component2.IgnoreTileCollisionsFor(10);
                    component2.Owner = player;
                    component2.baseData.damage = 0f;
                    component2.baseData.force = 0f;
                    component2.baseData.range = 0f;
                    component2.pierceMinorBreakables = true;
                    component2.collidesWithPlayer = false;
                    component3.IgnoreTileCollisionsFor(10);
                    component3.Owner = player;
                    component3.baseData.damage = 0f;
                    component3.baseData.force = 0f;
                    component3.baseData.range = 0f;
                    component3.pierceMinorBreakables = true;
                    component3.collidesWithPlayer = false;
                }
            }
        }
    }
}

