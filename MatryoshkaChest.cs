using UnityEngine;
using ItemAPI;
using Dungeonator;

namespace Blunderbeast
{
    public class MatryoshkaChest : PlayerItem
    {
        public static void Init()
        {
            //The name of the item
            string itemName = "Nesting Chest";

            //Refers to an embedded png in the project. Make sure to embed your resources!
            string resourceName = "Blunderbeast/Resources/matryoshkachest";

            //Create new GameObject
            GameObject obj = new GameObject();

            //Add a ActiveItem component to the object
            var item = obj.AddComponent<MatryoshkaChest>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Recursive";
            string longDesc = "Requires a key. This peculiar box grants a random chest when unlocked.\n\n" +
                "A seemingly endless amount of chests is contained within it. Nobody has ever managed to reach the bottom.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //"example_pool" here is the item pool. In the console you'd type "give example_pool:sweating_bullets"
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "rtr");

            //Set the cooldown type and duration of the cooldown
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Damage, 900f);

            //Adds a passive modifier, like curse, coolness, damage, etc. to the item. Works for passives and actives.

            //Set some other fields
            item.consumable = false;
            item.quality = PickupObject.ItemQuality.A;
        }

        public override void Pickup(PlayerController player)
        {
            if (!this.m_pickedUpThisRun)
            {
                LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(67).gameObject, player);
            }
            base.Pickup(player);
        }

        //Removes one heart from the player, gives them 1 armor
        protected override void DoEffect(PlayerController user)
        {
            float curKeys = user.carriedConsumables.KeyBullets;
            if (curKeys > 0)
            {
                AkSoundEngine.PostEvent("Play_OBJ_goldenlock_open_01", base.gameObject);
                user.carriedConsumables.KeyBullets--;

                if (user && user.CurrentRoom != null)
                {
                    IntVector2 bestRewardLocation = user.CurrentRoom.GetBestRewardLocation(IntVector2.One * 3, RoomHandler.RewardLocationStyle.PlayerCenter, true);
                    Chest chest = GameManager.Instance.RewardManager.SpawnTotallyRandomChest(bestRewardLocation);
                    if (chest)
                    {
                        chest.IsLocked = false;
                    }

                    if (user.HasPickupID(487))
                    {
                        float value = UnityEngine.Random.Range(0.0f, 1.0f);
                        bool flag = value < 0.2;
                        if (flag)
                        {
                            IntVector2 bestRewardLocation2 = user.CurrentRoom.GetBestRewardLocation(IntVector2.One * 3, RoomHandler.RewardLocationStyle.Original, true);
                            Chest chest2 = GameManager.Instance.RewardManager.SpawnTotallyRandomChest(bestRewardLocation2);
                            if (chest2)
                            {
                                chest2.IsLocked = false;
                            }
                        }
                    }

                }
            }
        }

        //Disables the item if the player's health is less than or equal to 1 heart
        public override bool CanBeUsed(PlayerController user)
        {
            return user.carriedConsumables.KeyBullets > 0;
        }
    }
}