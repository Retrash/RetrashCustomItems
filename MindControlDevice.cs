using UnityEngine;
using ItemAPI;
using Dungeonator;
using System.Collections.Generic;
using System.Collections;

namespace Blunderbeast
{
    public class MindControlDevice : PlayerItem
    {
        public static void Init()
        {
            //The name of the item
            string itemName = "Mind Control Device";

            //Refers to an embedded png in the project. Make sure to embed your resources!
            string resourceName = "Blunderbeast/Resources/mindcontroldevice";

            //Create new GameObject
            GameObject obj = new GameObject();

            //Add a ActiveItem component to the object
            var item = obj.AddComponent<MindControlDevice>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Batteries Not Included";
            string longDesc = "Takes control of enemies.\n\n" +
                "Custom controller made specifically to rig gundead boxing matches, a highly lucrative business in the Gungeon.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //"example_pool" here is the item pool. In the console you'd type "give example_pool:sweating_bullets"
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "rtr");

            //Set the cooldown type and duration of the cooldown
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Damage, 700f);

            //Adds a passive modifier, like curse, coolness, damage, etc. to the item. Works for passives and actives.

            //Set some other fields
            item.consumable = false;
            item.quality = PickupObject.ItemQuality.B;
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.PostProcessProjectile += this.PostProcessProjectile;
        }

        public DebrisObject Drop(PlayerController user)
        {
            DebrisObject debrisObject = base.Drop(user);
            user.PostProcessProjectile -= this.PostProcessProjectile;
            return debrisObject;
        }


        private void PostProcessProjectile(Projectile sourceProjectile, float effectChanceScalar)
        {
            if (LastOwner.CurrentGun.PickupObjectId == 511)
            {
                sourceProjectile.baseData.damage *= 5f;
            }

            else
            {
                return;
            }
        }


        protected override void DoEffect(PlayerController user)
        {
            AkSoundEngine.PostEvent("Play_obj_computer_break_01", base.gameObject);
            HandleMindControlEffect();
        }

        private void HandleMindControlEffect()
        {

            RoomHandler absoluteRoom = base.transform.position.GetAbsoluteRoom();
            List<AIActor> activeEnemies = absoluteRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);

            if (activeEnemies != null)
            {
                for (int i = 0; i < activeEnemies.Count; i++)
                {
                    this.AffectEnemy(activeEnemies[i]);
                }
            }
        }

        //private void BossHelp()
        //{

        //        AIActor orLoadByGuid = EnemyDatabase.GetOrLoadByGuid("01972dee89fc4404a5c408d50007dad5");
        //        IntVector2? intVector = new IntVector2?(this.LastOwner.CurrentRoom.GetRandomVisibleClearSpot(1, 1));
        //        bool flag = intVector != null;
        //        if (flag)
        //        {
        //            AIActor aiactor = AIActor.Spawn(orLoadByGuid.aiActor, intVector.Value, GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(intVector.Value), true, AIActor.AwakenAnimationType.Default, true);
        //            aiactor.CanTargetEnemies = true;
        //            aiactor.CanTargetPlayers = false;
        //            PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(aiactor.specRigidbody, null, false);
        //            aiactor.gameObject.AddComponent<KillOnRoomClear>();
        //            aiactor.IgnoreForRoomClear = true;
        //            aiactor.HandleReinforcementFallIntoRoom(0f);

        //            if (aiactor.IsNormalEnemy && !aiactor.healthHaver.IsBoss && !aiactor.gameObject.GetComponent<MindControlEffect>())
        //            {
        //                MindControlEffect orAddComponent = aiactor.gameObject.GetOrAddComponent<MindControlEffect>();
        //                orAddComponent.owner = this.LastOwner;
        //            }
        //        }
        //}

        protected void AffectEnemy(AIActor target)
        {
            if (target != null)
            {
                //if (target.healthHaver.IsBoss)
                //{
                //    this.BossHelp();
                //}

                if (target.IsNormalEnemy && !target.healthHaver.IsBoss && !target.gameObject.GetComponent<MindControlEffect>())
                {
                    target.behaviorSpeculator.Stun(0.2f, true);
                    MindControlEffect orAddComponent = target.gameObject.GetOrAddComponent<MindControlEffect>();
                    orAddComponent.owner = this.LastOwner;
                }
            }
        }

        public override bool CanBeUsed(PlayerController user)
        {
            return user.CurrentRoom.HasActiveEnemies(RoomHandler.ActiveEnemyType.All);
        }

    }
}