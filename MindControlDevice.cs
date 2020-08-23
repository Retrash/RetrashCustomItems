using UnityEngine;
using ItemAPI;
using Dungeonator;
using System.Collections.Generic;

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

        protected void AffectEnemy(AIActor target)
        {
            if (target != null)
            {
                if (target.IsNormalEnemy && !target.healthHaver.IsBoss && !target.IsHarmlessEnemy && !target.gameObject.GetComponent<MindControlEffect>())
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