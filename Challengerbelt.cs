using UnityEngine;
using ItemAPI;
using Dungeonator;

namespace Blunderbeast
{
    public class Challengerbelt : PassiveItem
    {

        //Call this method from the Start() method of your ETGModule extension
        public static void Init()
        {
            //The name of the item
            string itemName = "Challenger's Belt";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "Blunderbeast/Resources/challengerbelt";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<Challengerbelt>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Trial of the Gun";
            string longDesc = "Grants powerful benefits but activates Challenge Mode. Breaks when removed. \n\n" +
                "This belt was awarded to the first Gungeoneers who dared brave the trials of the Icosahedrax.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "rtr");

            //Adds the actual passive effect to the item

            //Set the rarity of the item
            item.quality = PickupObject.ItemQuality.S;
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Damage, 2f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Health, 1f, StatModifier.ModifyMethod.ADDITIVE);
        }

        public ChallengeModeType ChallengeMode;

        private static int challengeLevel = 0;


        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            GameManager.Instance.OnNewLevelFullyLoaded += this.ChestReward;
            ChallengeManager instance = ChallengeManager.Instance;
            if (ChallengeManager.Instance == false)
            {
                ChallengeManager.ChallengeModeType = ChallengeModeType.ChallengeMode;
            }
            else
            {
                challengeLevel += 1;
            }
        }

        public void Break()
        {
            this.m_pickedUp = true;
            UnityEngine.Object.Destroy(base.gameObject, 1f);
            challengeLevel -= 1;
            if (challengeLevel < 0)
            {
                challengeLevel = 0;
                ChallengeManager.ChallengeModeType = ChallengeModeType.None;
            }

        }

        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            Challengerbelt component = debrisObject.GetComponent<Challengerbelt>();
            component.Break();
            return debrisObject;
        }

        protected override void OnDestroy()
        {
            GameManager.Instance.OnNewLevelFullyLoaded -= this.ChestReward;
            base.OnDestroy();
        }

        private void ChestReward()
        {
            string text;
            string header;
            header = "Bravery is rewarded";
            text = "Congratulations";
            this.Notify(header, text);
            RoomHandler absoluteRoom = base.transform.position.GetAbsoluteRoom();
            IntVector2? randomAvailableCell = absoluteRoom.GetRandomAvailableCell(new IntVector2?(IntVector2.One * 4), new CellTypes?(CellTypes.FLOOR), false, null);
            IntVector2? intVector = (randomAvailableCell == null) ? null : new IntVector2?(randomAvailableCell.GetValueOrDefault() + IntVector2.One);
            if (intVector != null)
            {
                Chest chest = GameManager.Instance.RewardManager.SpawnRewardChestAt(intVector.Value);
                if (chest)
                {
                    chest.ForceUnlock();
                }
            }
            else
            {
                Chest chest = GameManager.Instance.RewardManager.SpawnRewardChestAt(absoluteRoom.GetBestRewardLocation(new IntVector2(3, 3), RoomHandler.RewardLocationStyle.Original, true) + IntVector2.Up);
                if (chest)
                {
                    chest.ForceUnlock();
                }
            }
        }

        private void Notify(string header, string text)
        {
            tk2dBaseSprite notificationObjectSprite = GameUIRoot.Instance.notificationController.notificationObjectSprite;
            GameUIRoot.Instance.notificationController.DoCustomNotification(header, text, notificationObjectSprite.Collection, notificationObjectSprite.spriteId, UINotificationController.NotificationColor.GOLD, false, true);
        }

    }
}

