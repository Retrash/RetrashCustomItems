using UnityEngine;
using ItemAPI;
using System.Collections;
using Gungeon;
using Dungeonator;
using System.Collections.Generic;

namespace Blunderbeast
{
    public class BigRedButtonGag : PlayerItem
    {
        public static void Init()
        {
            //The name of the item
            string itemName = "Big Red Button";

            //Refers to an embedded png in the project. Make sure to embed your resources!
            string resourceName = "Blunderbeast/Resources/bigredbutton";

            //Create new GameObject
            GameObject obj = new GameObject();

            //Add a ActiveItem component to the object
            var item = obj.AddComponent<BigRedButtonGag>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "I'm Warning You, Don't Press It";
            string longDesc = "A voice in your head is telling you not to press it.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //"example_pool" here is the item pool. In the console you'd type "give example_pool:sweating_bullets"
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "rtr");

            //Set the cooldown type and duration of the cooldown
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Timed, 0f);

            //Adds a passive modifier, like curse, coolness, damage, etc. to the item. Works for passives and actives.

            //Set some other fields
            item.consumable = true;
            item.quality = PickupObject.ItemQuality.EXCLUDED;
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
        }

        protected override void DoEffect(PlayerController user)
        {
            AkSoundEngine.PostEvent("Play_OBJ_computer_boop_01", this.gameObject);
            GameManager.Instance.StartCoroutine(this.Misdirect());       
        }

        private IEnumerator Misdirect()
        {

            yield return new WaitForSeconds(1f);
            PlayerController player = this.LastOwner;
            PickupObject.ItemQuality itemQuality = PickupObject.ItemQuality.S;
            PickupObject.ItemQuality itemQuality2 = PickupObject.ItemQuality.A;
            PickupObject itemOfTypeAndQuality = LootEngine.GetItemOfTypeAndQuality<PickupObject>(itemQuality, GameManager.Instance.RewardManager.ItemsLootTable, false);
            PickupObject itemOfTypeAndQuality2 = LootEngine.GetItemOfTypeAndQuality<PickupObject>(itemQuality2, GameManager.Instance.RewardManager.ItemsLootTable, false);
            LootEngine.SpawnItem(itemOfTypeAndQuality.gameObject, player.specRigidbody.UnitCenter, Vector2.left, 1f, false, true, false);
            LootEngine.SpawnItem(itemOfTypeAndQuality2.gameObject, player.specRigidbody.UnitCenter, Vector2.right, 1f, false, true, false);

            yield return new WaitForSeconds(15f);
            string text;
            string header;
            header = "I told you not to press it";
            text = "Prepare to feel the pain";
            this.Notify(header, text);

            yield return new WaitForSeconds(2f);
            Material glitchPass = new Material(Shader.Find("Brave/Internal/GlitchUnlit"));
            Pixelator.Instance.RegisterAdditionalRenderPass(glitchPass);

            yield return new WaitForSeconds(2f);
            GameManager.Instance.InjectedFlowPath = "Core Game Flows/Secret_DoubleBeholster_Flow";
            Pixelator.Instance.FadeToBlack(0.5f, false, 0f);
            GameManager.Instance.DelayedLoadNextLevel(0.5f);
            yield return new WaitForSeconds(1f);
            PlayerController user = this.LastOwner;
            user.healthHaver.NextShotKills = true;
            yield break;         
        }


        private void Notify(string header, string text)
        {
            tk2dBaseSprite notificationObjectSprite = GameUIRoot.Instance.notificationController.notificationObjectSprite;
            GameUIRoot.Instance.notificationController.DoCustomNotification(header, text, notificationObjectSprite.Collection, notificationObjectSprite.spriteId, UINotificationController.NotificationColor.PURPLE, false, true);
        }
    }
}

