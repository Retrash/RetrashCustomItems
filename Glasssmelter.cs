using UnityEngine;
using ItemAPI;
using Dungeonator;
using System.Collections.Generic;
using System.Linq;

namespace Blunderbeast
{
    public class Glasssmelter : PassiveItem
    {
        //Call this method from the Start() method of your ETGModule extension
        public static void Init()
        {
            //The name of the item
            string itemName = "Glass Smelter";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "Blunderbeast/Resources/glasssmelter";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<Glasssmelter>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Questionable Craftsmanship";
            string longDesc = "Creates a random glass item at the start of every floor.\n\n" +
                "This portable furnace was a favorite of the Lady of Pane. This smelter normally only creates fragile glass baubles, but on rare occasions is able to produce objects of great value.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "rtr");

            //Adds the actual passive effect to the item

            //Set the rarity of the item
            item.quality = PickupObject.ItemQuality.B;
        }

        public override void Pickup(PlayerController player)
        {

            if (!this.m_pickedUpThisRun)
            {
                AkSoundEngine.PostEvent("Play_OBJ_crystal_shatter_01", base.gameObject);
                PickupObject byId = PickupObjectDatabase.GetById(565);
                player.AcquirePassiveItemPrefabDirectly(byId as PassiveItem);
                player.AcquirePassiveItemPrefabDirectly(byId as PassiveItem);
                player.AcquirePassiveItemPrefabDirectly(byId as PassiveItem);
            }
            base.Pickup(player);
            GameManager.Instance.OnNewLevelFullyLoaded += this.RandomEffect;
        }

        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            GameManager.Instance.OnNewLevelFullyLoaded -= this.RandomEffect;
            return debrisObject;
        }

        protected override void OnDestroy()
        {
            if (Owner)
            {
                GameManager.Instance.OnNewLevelFullyLoaded -= this.RandomEffect;
            }
            base.OnDestroy();
        }


        private void RandomEffect()
        {
            PlayerController player = this.Owner;

            AkSoundEngine.PostEvent("Play_WPN_burninghand_shot_01", base.gameObject);

            if (ETGMod.Databases.Items["Glass Ammolet"] != null && ETGMod.Databases.Items["Glass Chamber"] != null && ETGMod.Databases.Items["Glass Rounds"] != null)
            {
                float value = UnityEngine.Random.Range(0.0f, 1.0f);

                bool SingleStone = value <= 0.3;
                if (SingleStone && !player.HasPickupID(152))
                {
                    //SINGLE STONE
                    AkSoundEngine.PostEvent("Play_OBJ_crystal_shatter_01", base.gameObject);
                    LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(565).gameObject, player);
                }
                else
                {
                    bool DoubleStone = value <= 0.7;
                    if (DoubleStone && !player.HasPickupID(152))
                    {
                        //DOUBLE STONE
                        AkSoundEngine.PostEvent("Play_OBJ_crystal_shatter_01", base.gameObject);
                        PickupObject byId = PickupObjectDatabase.GetById(565);
                        player.AcquirePassiveItemPrefabDirectly(byId as PassiveItem);
                        LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(565).gameObject, player);
                    }
                    else
                    {
                        bool TripleStone = value <= 0.9;
                        if (TripleStone)
                        {
                            //TRIPLE STONE
                            AkSoundEngine.PostEvent("Play_OBJ_ironcoin_wish_01", base.gameObject);
                            PickupObject byId = PickupObjectDatabase.GetById(565);
                            player.AcquirePassiveItemPrefabDirectly(byId as PassiveItem);
                            player.AcquirePassiveItemPrefabDirectly(byId as PassiveItem);
                            LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(565).gameObject, player);
                        }
                        else
                        {
                            bool Sunglasses = value <= 0.9125;
                            if (Sunglasses)
                            {
                                //SUNGLASSES
                                AkSoundEngine.PostEvent("Play_OBJ_ironcoin_wish_01", base.gameObject);
                                LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(290).gameObject, player);
                            }
                            else
                            {
                                bool Bottle = value <= 0.925;
                                if (Bottle && !player.HasPickupID(558))
                                {
                                    //BOTTLE
                                    AkSoundEngine.PostEvent("Play_OBJ_ironcoin_wish_01", base.gameObject);
                                    float baseStatValue = player.stats.GetBaseStatValue(PlayerStats.StatType.AdditionalItemCapacity);
                                    float num = 1f + baseStatValue;
                                    player.stats.SetBaseStatValue(PlayerStats.StatType.AdditionalItemCapacity, num, player);
                                    LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(558).gameObject, player);
                                }
                                else
                                {
                                    bool Flask = value <= 0.9375;
                                    if (Flask && !player.HasPickupID(267))
                                    {
                                        //KNIGHT FLASK
                                        AkSoundEngine.PostEvent("Play_OBJ_ironcoin_wish_01", base.gameObject);
                                        float baseStatValue = player.stats.GetBaseStatValue(PlayerStats.StatType.AdditionalItemCapacity);
                                        float num = 1f + baseStatValue;
                                        player.stats.SetBaseStatValue(PlayerStats.StatType.AdditionalItemCapacity, num, player);
                                        LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(267).gameObject, player);
                                    }
                                    else
                                    {
                                        bool Cannon = value <= 0.95;
                                        if (Cannon && !player.HasPickupID(540))
                                        {
                                            //GLASS CANNON
                                            AkSoundEngine.PostEvent("Play_OBJ_ironcoin_wish_01", base.gameObject);
                                            LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(540).gameObject, player);
                                        }
                                        else
                                        {
                                            bool HeartBottle = value <= 0.9625;
                                            if (HeartBottle)
                                            {
                                                //HEART BOTTLE
                                                AkSoundEngine.PostEvent("Play_OBJ_ironcoin_wish_01", base.gameObject);
                                                LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(424).gameObject, player);
                                            }
                                            else
                                            {
                                                bool GlassRounds = value <= 0.975;
                                                if (GlassRounds)
                                                {
                                                    //GLASS ROUNDS
                                                    AkSoundEngine.PostEvent("Play_OBJ_ironcoin_wish_01", base.gameObject);
                                                    LootEngine.GivePrefabToPlayer(ETGMod.Databases.Items["Glass Rounds"].gameObject, player);
                                                }
                                                else
                                                {
                                                    bool GlassAmmolet = value <= 0.9875;
                                                    if (GlassAmmolet)
                                                    {
                                                        //GLASS AMMOLET
                                                        AkSoundEngine.PostEvent("Play_OBJ_ironcoin_wish_01", base.gameObject);
                                                        LootEngine.GivePrefabToPlayer(ETGMod.Databases.Items["Glass Ammolet"].gameObject, player);
                                                    }
                                                    else
                                                    {
                                                        bool GlassChamber = value <= 1;
                                                        if (GlassChamber)
                                                        {

                                                            //GLASS CHAMBER
                                                            AkSoundEngine.PostEvent("Play_OBJ_ironcoin_wish_01", base.gameObject);
                                                            LootEngine.GivePrefabToPlayer(ETGMod.Databases.Items["Glass Chamber"].gameObject, player);
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
                }
            }

            else if (ETGMod.Databases.Items["Glass Ammolet"] == null || ETGMod.Databases.Items["Glass Chamber"] == null || ETGMod.Databases.Items["Glass Rounds"] == null)
            {
                float value = UnityEngine.Random.Range(0.0f, 1.0f);

                bool SingleStone = value <= 0.3;
                if (SingleStone && !player.HasPickupID(152))
                {
                    //SINGLE STONE
                    AkSoundEngine.PostEvent("Play_OBJ_crystal_shatter_01", base.gameObject);
                    LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(565).gameObject, player);
                }
                else
                {
                    bool DoubleStone = value <= 0.7;
                    if (DoubleStone && !player.HasPickupID(152))
                    {
                        //DOUBLE STONE
                        AkSoundEngine.PostEvent("Play_OBJ_crystal_shatter_01", base.gameObject);
                        PickupObject byId = PickupObjectDatabase.GetById(565);
                        player.AcquirePassiveItemPrefabDirectly(byId as PassiveItem);
                        LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(565).gameObject, player);
                    }
                    else
                    {
                        bool TripleStone = value <= 0.9;
                        if (TripleStone)
                        {
                            //TRIPLE STONE
                            AkSoundEngine.PostEvent("Play_OBJ_ironcoin_wish_01", base.gameObject);
                            PickupObject byId = PickupObjectDatabase.GetById(565);
                            player.AcquirePassiveItemPrefabDirectly(byId as PassiveItem);
                            player.AcquirePassiveItemPrefabDirectly(byId as PassiveItem);
                            LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(565).gameObject, player);
                        }
                        else
                        {
                            bool Sunglasses = value <= 0.92;
                            if (Sunglasses)
                            {
                                //SUNGLASSES
                                AkSoundEngine.PostEvent("Play_OBJ_ironcoin_wish_01", base.gameObject);
                                LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(290).gameObject, player);
                            }
                            else
                            {
                                bool Bottle = value <= 0.94;
                                if (Bottle && !player.HasPickupID(558))
                                {
                                    //BOTTLE
                                    AkSoundEngine.PostEvent("Play_OBJ_ironcoin_wish_01", base.gameObject);
                                    float baseStatValue = player.stats.GetBaseStatValue(PlayerStats.StatType.AdditionalItemCapacity);
                                    float num = 1f + baseStatValue;
                                    player.stats.SetBaseStatValue(PlayerStats.StatType.AdditionalItemCapacity, num, player);
                                    LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(558).gameObject, player);
                                }
                                else
                                {
                                    bool Flask = value <= 0.96;
                                    if (Flask && !player.HasPickupID(267))
                                    {
                                        //KNIGHT FLASK
                                        AkSoundEngine.PostEvent("Play_OBJ_ironcoin_wish_01", base.gameObject);
                                        float baseStatValue = player.stats.GetBaseStatValue(PlayerStats.StatType.AdditionalItemCapacity);
                                        float num = 1f + baseStatValue;
                                        player.stats.SetBaseStatValue(PlayerStats.StatType.AdditionalItemCapacity, num, player);
                                        LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(267).gameObject, player);
                                    }
                                    else
                                    {
                                        bool Cannon = value <= 0.98;
                                        if (Cannon && !player.HasPickupID(540))
                                        {
                                            //GLASS CANNON
                                            AkSoundEngine.PostEvent("Play_OBJ_ironcoin_wish_01", base.gameObject);
                                            LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(540).gameObject, player);
                                        }
                                        else
                                        {
                                            bool HeartBottle = value <= 1;
                                            if (HeartBottle)
                                            {
                                                //HEART BOTTLE
                                                AkSoundEngine.PostEvent("Play_OBJ_ironcoin_wish_01", base.gameObject);
                                                LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(424).gameObject, player);
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
    }
}

