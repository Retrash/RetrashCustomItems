using UnityEngine;
using ItemAPI;
using Dungeonator;
using System.Linq;
using System.Collections.Generic;
using MonoMod.RuntimeDetour;
using System.Reflection;
using System;
using System.Collections;

namespace Blunderbeast
{
    public class CrystalBall : PlayerItem
    {
        public static void Init()
        {
            //The name of the item
            string itemName = "Crystal Ball";

            //Refers to an embedded png in the project. Make sure to embed your resources!
            string resourceName = "Blunderbeast/Resources/crystalball";

            //Create new GameObject
            GameObject obj = new GameObject();

            //Add a ActiveItem component to the object
            var item = obj.AddComponent<CrystalBall>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Mesmerizing Sphere";
            string longDesc = "Reveals and opens all hidden rooms.\n\n" +
                "Gazing into it can uncover even the deepest held secrets. Is said to have been used by the Gungeon's Master to keep an eye on the lower chambers.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //"example_pool" here is the item pool. In the console you'd type "give example_pool:sweating_bullets"
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "rtr");

            //Set the cooldown type and duration of the cooldown
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.PerRoom, 99999f);
            //Adds a passive modifier, like curse, coolness, damage, etc. to the item. Works for passives and actives.

            //Set some other fields
            item.consumable = false;
            item.quality = PickupObject.ItemQuality.C;
            CrystalBallID = item.PickupObjectId;
        }

        private static int CrystalBallID;

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            GameManager.Instance.OnNewLevelFullyLoaded += this.ResetCooldown;
        }

        public DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            GameManager.Instance.OnNewLevelFullyLoaded -= this.ResetCooldown;
            return debrisObject;
        }

        private static Hook LifeOrbHook2 = new Hook(
      typeof(LifeOrbGunModifier).GetMethod("ClearSoul", BindingFlags.Instance | BindingFlags.NonPublic),
      typeof(CrystalBall).GetMethod("SoulHook")
  );

        private static GameObject vfx, vfx2;

        public static void SoulHook(Action<LifeOrbGunModifier, bool> orig, LifeOrbGunModifier lifeorb, bool disabling)
        {
            orig(lifeorb, disabling);

            vfx = lifeorb.OnBurstDamageVFX;
            vfx2 = lifeorb.OnBurstGunVFX;
            PlayerController player = GameManager.Instance.PrimaryPlayer;
            PlayerController player2 = GameManager.Instance.SecondaryPlayer;
            if ((player.HasPickupID(CrystalBallID) && player.CurrentGun.PickupObjectId == 595) || (player2.HasPickupID(CrystalBallID) && player2.CurrentGun.PickupObjectId == 595))
            {
                GameManager.Instance.StartCoroutine(DamageDelay());

            }
        }

        private static IEnumerator DamageDelay()
        {
            yield return new WaitForSeconds(4f);
            PlayerController player = GameManager.Instance.PrimaryPlayer;
            RoomHandler currentRoom = player.CurrentRoom;
            List<AIActor> activeEnemies = currentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
            if (activeEnemies != null)
            {
                for (int i = 0; i < activeEnemies.Count; i++)
                {
                    SpawnManager.SpawnVFX(vfx2, player.CurrentGun.barrelOffset.position, Quaternion.identity);
                    DamageEnemies(activeEnemies[i]);
                }
            }
            yield break;
        }

        private static void DamageEnemies(AIActor a)
        {
            if (a && a.healthHaver && !a.IsGone)
            {              
                a.PlayEffectOnActor(vfx, Vector3.zero, false, true, false);
                a.healthHaver.ApplyDamage(20f, Vector2.zero, "projectile", CoreDamageTypes.None, DamageCategory.Normal, false, null, false);
            }
        }

        private void ResetCooldown()
        {
            base.ClearCooldowns();
        }

        protected override void DoEffect(PlayerController user)
        {
            AkSoundEngine.PostEvent("Play_WPN_Life_Orb_Capture_01", base.gameObject);
            RevealSecretRooms();
        }

        private void RevealSecretRooms()
        {
            PlayerController player = this.LastOwner;
            for (int i = 0; i < GameManager.Instance.Dungeon.data.rooms.Count; i++)
            {
                RoomHandler roomHandler = GameManager.Instance.Dungeon.data.rooms[i];
                if (roomHandler.connectedRooms.Count != 0)
                {
                    if (roomHandler.area.PrototypeRoomCategory == PrototypeDungeonRoom.RoomCategory.SECRET)
                    {
                        roomHandler.RevealedOnMap = true;
                        Minimap.Instance.RevealMinimapRoom(roomHandler, true, true, roomHandler == GameManager.Instance.PrimaryPlayer.CurrentRoom);
                    }
                }

                for (int j = 0; j < StaticReferenceManager.AllMajorBreakables.Count; j++)
                {
                    if (StaticReferenceManager.AllMajorBreakables[j].IsSecretDoor)
                    {
                        StaticReferenceManager.AllMajorBreakables[j].ApplyDamage(1E+10f, Vector2.zero, false, true, true);
                        StaticReferenceManager.AllMajorBreakables[j].ApplyDamage(1E+10f, Vector2.zero, false, true, true);
                        StaticReferenceManager.AllMajorBreakables[j].ApplyDamage(1E+10f, Vector2.zero, false, true, true);
                    }
                }

                if (player.HasPickupID(595))
                {
                    Minimap.Instance.RevealAllRooms(true);
                }

            }
        }
    }
}