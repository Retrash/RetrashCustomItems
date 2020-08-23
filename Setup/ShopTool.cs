using System;
using System.Collections.Generic;
using Dungeonator;
using Gungeon;
using UnityEngine;

namespace Blunderbeast
{
    public class ShopTool : MonoBehaviour
    {
        public static void Init()
        {
            ShopTool.sharedAssets = ResourceManager.LoadAssetBundle("shared_auto_001");
            ShopTool.Shop_Key_Items_01 = ShopTool.sharedAssets.LoadAsset<GenericLootTable>("Shop_Key_Items_01");
            ShopTool.Shop_Truck_Items_01 = ShopTool.sharedAssets.LoadAsset<GenericLootTable>("Shop_Truck_Items_01");
            ShopTool.Shop_Curse_Items_01 = ShopTool.sharedAssets.LoadAsset<GenericLootTable>("Shop_Curse_Items_01");
            ShopTool.Shop_Goop_Items_01 = ShopTool.sharedAssets.LoadAsset<GenericLootTable>("Shop_Goop_Items_01");
            ShopTool.Shop_Blank_Items_01 = ShopTool.sharedAssets.LoadAsset<GenericLootTable>("Shop_Blank_Items_01");

            ShopTool.ForgeDungeonPrefab = DungeonDatabase.GetOrLoadByName("Base_Forge");
            ShopTool.BlacksmithShop = ShopTool.ForgeDungeonPrefab.PatternSettings.flows[0].AllNodes[10].overrideExactRoom;
            ShopTool.BlackSmith_Items_01 = (ShopTool.BlacksmithShop.placedObjects[8].nonenemyBehaviour as BaseShopController).shopItemsGroup2;

            //KEY SHOP

            ShopTool.Shop_Key_Items_01.defaultItemDrops.Add(new WeightedGameObject
            {
                rawGameObject = null,
                pickupId = ETGMod.Databases.Items["Locked Bullets"].PickupObjectId,
                weight = 1f,
                forceDuplicatesPossible = false,
                additionalPrerequisites = new DungeonPrerequisite[0]
            });

            ShopTool.Shop_Key_Items_01.defaultItemDrops.Add(new WeightedGameObject
            {
                rawGameObject = null,
                pickupId = ETGMod.Databases.Items["Nesting Chest"].PickupObjectId,
                weight = 1f,
                forceDuplicatesPossible = false,
                additionalPrerequisites = new DungeonPrerequisite[0]
            });

            ShopTool.Shop_Key_Items_01.defaultItemDrops.Add(new WeightedGameObject
            {
                rawGameObject = null,
                pickupId = ETGMod.Databases.Items["Loot Capsule"].PickupObjectId,
                weight = 1f,
                forceDuplicatesPossible = false,
                additionalPrerequisites = new DungeonPrerequisite[0]
            });

            //TRORC SHOP

            ShopTool.Shop_Truck_Items_01.defaultItemDrops.Add(new WeightedGameObject
            {
                rawGameObject = null,
                pickupId = ETGMod.Databases.Items["Recycloader"].PickupObjectId,
                weight = 1f,
                forceDuplicatesPossible = false,
                additionalPrerequisites = new DungeonPrerequisite[0]
            });

            ShopTool.Shop_Truck_Items_01.defaultItemDrops.Add(new WeightedGameObject
            {
                rawGameObject = null,
                pickupId = ETGMod.Databases.Items["Frag grenade"].PickupObjectId,
                weight = 1f,
                forceDuplicatesPossible = false,
                additionalPrerequisites = new DungeonPrerequisite[0]
            });

            ShopTool.Shop_Truck_Items_01.defaultItemDrops.Add(new WeightedGameObject
            {
                rawGameObject = null,
                pickupId = ETGMod.Databases.Items["War Vase"].PickupObjectId,
                weight = 1f,
                forceDuplicatesPossible = false,
                additionalPrerequisites = new DungeonPrerequisite[0]
            });

            ShopTool.Shop_Truck_Items_01.defaultItemDrops.Add(new WeightedGameObject
            {
                rawGameObject = null,
                pickupId = ETGMod.Databases.Items["Mind Control Device"].PickupObjectId,
                weight = 1f,
                forceDuplicatesPossible = false,
                additionalPrerequisites = new DungeonPrerequisite[0]
            });

            ShopTool.Shop_Truck_Items_01.defaultItemDrops.Add(new WeightedGameObject
            {
                rawGameObject = null,
                pickupId = ETGMod.Databases.Items["Boombox"].PickupObjectId,
                weight = 1f,
                forceDuplicatesPossible = false,
                additionalPrerequisites = new DungeonPrerequisite[0]
            });          

            //CURSE SHOP

            ShopTool.Shop_Curse_Items_01.defaultItemDrops.Add(new WeightedGameObject
            {
                rawGameObject = null,
                pickupId = ETGMod.Databases.Items["Malediction Rounds"].PickupObjectId,
                weight = 1f,
                forceDuplicatesPossible = false,
                additionalPrerequisites = new DungeonPrerequisite[0]
            });

            ShopTool.Shop_Curse_Items_01.defaultItemDrops.Add(new WeightedGameObject
            {
                rawGameObject = null,
                pickupId = ETGMod.Databases.Items["Vampire Cloak"].PickupObjectId,
                weight = 1f,
                forceDuplicatesPossible = false,
                additionalPrerequisites = new DungeonPrerequisite[0]
            });

            ShopTool.Shop_Curse_Items_01.defaultItemDrops.Add(new WeightedGameObject
            {
                rawGameObject = null,
                pickupId = ETGMod.Databases.Items["Beastblood Injection"].PickupObjectId,
                weight = 1f,
                forceDuplicatesPossible = false,
                additionalPrerequisites = new DungeonPrerequisite[0]
            });


            ShopTool.Shop_Curse_Items_01.defaultItemDrops.Add(new WeightedGameObject
            {
                rawGameObject = null,
                pickupId = ETGMod.Databases.Items["Blunderbeast Blight"].PickupObjectId,
                weight = 1f,
                forceDuplicatesPossible = false,
                additionalPrerequisites = new DungeonPrerequisite[0]
            });

            ShopTool.Shop_Curse_Items_01.defaultItemDrops.Add(new WeightedGameObject
            {
                rawGameObject = null,
                pickupId = ETGMod.Databases.Items["Repository Of The Damned"].PickupObjectId,
                weight = 1f,
                forceDuplicatesPossible = false,
                additionalPrerequisites = new DungeonPrerequisite[0]
            });


            //GOOP SHOP

            ShopTool.Shop_Goop_Items_01.defaultItemDrops.Add(new WeightedGameObject
            {
                rawGameObject = null,
                pickupId = PickupObjectDatabase.GetByEncounterName("Alchemical Gun").PickupObjectId,
                weight = 1f,
                forceDuplicatesPossible = false,
                additionalPrerequisites = new DungeonPrerequisite[0]
            });

            ShopTool.Shop_Goop_Items_01.defaultItemDrops.Add(new WeightedGameObject
            {
                rawGameObject = null,
                pickupId = ETGMod.Databases.Items["Panacea"].PickupObjectId,
                weight = 1f,
                forceDuplicatesPossible = false,
                additionalPrerequisites = new DungeonPrerequisite[0]
            });

            ShopTool.Shop_Goop_Items_01.defaultItemDrops.Add(new WeightedGameObject
            {
                rawGameObject = null,
                pickupId = ETGMod.Databases.Items["Beastblood Injection"].PickupObjectId,
                weight = 1f,
                forceDuplicatesPossible = false,
                additionalPrerequisites = new DungeonPrerequisite[0]
            });

            ShopTool.Shop_Goop_Items_01.defaultItemDrops.Add(new WeightedGameObject
            {
                rawGameObject = null,
                pickupId = ETGMod.Databases.Items["Crackling Egg"].PickupObjectId,
                weight = 1f,
                forceDuplicatesPossible = false,
                additionalPrerequisites = new DungeonPrerequisite[0]
            });

            //BLANK SHOP

            ShopTool.Shop_Blank_Items_01.defaultItemDrops.Add(new WeightedGameObject
            {
                rawGameObject = null,
                pickupId = ETGMod.Databases.Items["Barrier Ammolet"].PickupObjectId,
                weight = 1f,
                forceDuplicatesPossible = false,
                additionalPrerequisites = new DungeonPrerequisite[0]
            });

            ShopTool.Shop_Blank_Items_01.defaultItemDrops.Add(new WeightedGameObject
            {
                rawGameObject = null,
                pickupId = ETGMod.Databases.Items["Blank Spellbook"].PickupObjectId,
                weight = 1f,
                forceDuplicatesPossible = false,
                additionalPrerequisites = new DungeonPrerequisite[0]
            });

            ShopTool.Shop_Blank_Items_01.defaultItemDrops.Add(new WeightedGameObject
            {
                rawGameObject = null,
                pickupId = ETGMod.Databases.Items["Rusted Armor"].PickupObjectId,
                weight = 1f,
                forceDuplicatesPossible = false,
                additionalPrerequisites = new DungeonPrerequisite[0]
            });

            //BLACKSMITH SHOP

            ShopTool.BlackSmith_Items_01.defaultItemDrops.Add(new WeightedGameObject
            {
                rawGameObject = null,
                pickupId = ETGMod.Databases.Items["Glass Smelter"].PickupObjectId,
                weight = 1f,
                forceDuplicatesPossible = false,
                additionalPrerequisites = new DungeonPrerequisite[0]
            });

            ShopTool.BlackSmith_Items_01.defaultItemDrops.Add(new WeightedGameObject
            {
                rawGameObject = null,
                pickupId = ETGMod.Databases.Items["Storm Charger"].PickupObjectId,
                weight = 1f,
                forceDuplicatesPossible = false,
                additionalPrerequisites = new DungeonPrerequisite[0]
            });

            ShopTool.BlackSmith_Items_01.defaultItemDrops.Add(new WeightedGameObject
            {
                rawGameObject = null,
                pickupId = ETGMod.Databases.Items["Roshambo"].PickupObjectId,
                weight = 1f,
                forceDuplicatesPossible = false,
                additionalPrerequisites = new DungeonPrerequisite[0]
            });

            ShopTool.BlackSmith_Items_01.defaultItemDrops.Add(new WeightedGameObject
            {
                rawGameObject = null,
                pickupId = ETGMod.Databases.Items["Locked Bullets"].PickupObjectId,
                weight = 1f,
                forceDuplicatesPossible = false,
                additionalPrerequisites = new DungeonPrerequisite[0]
            });

            ShopTool.BlackSmith_Items_01.defaultItemDrops.Add(new WeightedGameObject
            {
                rawGameObject = null,
                pickupId = PickupObjectDatabase.GetByEncounterName("The Constructor").PickupObjectId,
                weight = 1f,
                forceDuplicatesPossible = false,
                additionalPrerequisites = new DungeonPrerequisite[0]
            });

            ForgeDungeonPrefab = null;

            return;
        }

        public static AssetBundle sharedAssets;

        private static Dungeon ForgeDungeonPrefab;
        public static PrototypeDungeonRoom BlacksmithShop;

        public static GenericLootTable Shop_Key_Items_01;
        public static GenericLootTable Shop_Truck_Items_01;
        public static GenericLootTable Shop_Curse_Items_01;
        public static GenericLootTable Shop_Goop_Items_01;
        public static GenericLootTable Shop_Blank_Items_01;
        public static GenericLootTable BlackSmith_Items_01;
    }
}
