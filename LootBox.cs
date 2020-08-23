using UnityEngine;
using ItemAPI;
using System.Collections;

namespace Blunderbeast
{
    public class LootBox : PlayerItem
    {
        public static void Init()
        {
            //The name of the item
            string itemName = "Loot Capsule";

            //Refers to an embedded png in the project. Make sure to embed your resources!
            string resourceName = "Blunderbeast/Resources/lootbox";

            //Create new GameObject
            GameObject obj = new GameObject();

            //Add a ActiveItem component to the object
            var item = obj.AddComponent<LootBox>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Surprise Mechanics";
            string longDesc = "25 casings for a mystery item, no refunds!";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //"example_pool" here is the item pool. In the console you'd type "give example_pool:sweating_bullets"
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "rtr");

            //Set the cooldown type and duration of the cooldown
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Damage, 400f);

            //Adds a passive modifier, like curse, coolness, damage, etc. to the item. Works for passives and actives.
            //Set some other fields
            item.consumable = true;
            item.quality = PickupObject.ItemQuality.D;
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
        }

        public override bool CanBeUsed(PlayerController user)
        {
            return user.carriedConsumables.Currency >= 25;
        }

        private IEnumerator GamblingTime()
        {
            yield return new WaitForSeconds(1.5f);
            GetItem();
            yield break;
        }

        private void GetItem()
        {
            PlayerController user = this.LastOwner;
            float value = UnityEngine.Random.Range(0.0f, 1f);
            bool flag = value < 0.7;
            if (flag)
            {
                PickupObject.ItemQuality itemQuality = PickupObject.ItemQuality.D;
                PickupObject itemOfTypeAndQuality = LootEngine.GetItemOfTypeAndQuality<PickupObject>(itemQuality, (UnityEngine.Random.value >= 0.5f) ? GameManager.Instance.RewardManager.GunsLootTable : GameManager.Instance.RewardManager.ItemsLootTable, false);
                if (itemOfTypeAndQuality)
                {
                    LootEngine.SpawnItem(itemOfTypeAndQuality.gameObject, user.CenterPosition, Vector2.up, 1f, true, true, false);
                }

            }
            else
            {
                bool flag2 = value < 0.85;
                if (flag2)
                {
                    PickupObject.ItemQuality itemQuality5 = PickupObject.ItemQuality.C;
                    PickupObject itemOfTypeAndQuality5 = LootEngine.GetItemOfTypeAndQuality<PickupObject>(itemQuality5, (UnityEngine.Random.value >= 0.5f) ? GameManager.Instance.RewardManager.GunsLootTable : GameManager.Instance.RewardManager.ItemsLootTable, false);
                    if (itemOfTypeAndQuality5)
                    {
                        LootEngine.SpawnItem(itemOfTypeAndQuality5.gameObject, user.CenterPosition, Vector2.up, 1f, true, true, false);
                    }
                }
                else
                {
                    bool flag3 = value < 0.95;
                    if (flag3)
                    {
                        PickupObject.ItemQuality itemQuality4 = PickupObject.ItemQuality.B;
                        PickupObject itemOfTypeAndQuality4 = LootEngine.GetItemOfTypeAndQuality<PickupObject>(itemQuality4, (UnityEngine.Random.value >= 0.5f) ? GameManager.Instance.RewardManager.GunsLootTable : GameManager.Instance.RewardManager.ItemsLootTable, false);
                        if (itemOfTypeAndQuality4)
                        {
                            LootEngine.SpawnItem(itemOfTypeAndQuality4.gameObject, user.CenterPosition, Vector2.up, 1f, true, true, false);
                        }
                    }
                    else
                    {
                        bool flag4 = value < 0.98;
                        if (flag4)
                        {
                            PickupObject.ItemQuality itemQuality3 = PickupObject.ItemQuality.A;
                            PickupObject itemOfTypeAndQuality3 = LootEngine.GetItemOfTypeAndQuality<PickupObject>(itemQuality3, (UnityEngine.Random.value >= 0.5f) ? GameManager.Instance.RewardManager.GunsLootTable : GameManager.Instance.RewardManager.ItemsLootTable, false);
                            if (itemOfTypeAndQuality3)
                            {
                                LootEngine.SpawnItem(itemOfTypeAndQuality3.gameObject, user.CenterPosition, Vector2.up, 1f, true, true, false);
                            }
                        }
                        else
                        {
                            PickupObject.ItemQuality itemQuality2 = PickupObject.ItemQuality.S;
                            PickupObject itemOfTypeAndQuality2 = LootEngine.GetItemOfTypeAndQuality<PickupObject>(itemQuality2, (UnityEngine.Random.value >= 0.5f) ? GameManager.Instance.RewardManager.GunsLootTable : GameManager.Instance.RewardManager.ItemsLootTable, false);
                            if (itemOfTypeAndQuality2)
                            {
                                LootEngine.SpawnItem(itemOfTypeAndQuality2.gameObject, user.CenterPosition, Vector2.up, 1f, true, true, false);
                            }
                        }
                    }
                }
            }
        }


        protected override void DoEffect(PlayerController user)
        {
            user.carriedConsumables.Currency -= 25;
            AkSoundEngine.PostEvent("Play_OBJ_Chest_Synergy_Slots_01", base.gameObject);
            GameManager.Instance.StartCoroutine(this.GamblingTime());
            if (user.HasPickupID(476))
            {
                LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetByName("Loot Capsule").gameObject, user);
            }
        }
    }
}
