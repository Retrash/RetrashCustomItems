using UnityEngine;
using ItemAPI;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Blunderbeast
{
    public class RepositoryDamned : PassiveItem
    {

        //Call this method from the Start() method of your ETGModule extension
        public static void Init()
        {
            //The name of the item
            string itemName = "Repository Of The Damned";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "Blunderbeast/Resources/damnedrepository";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<RepositoryDamned>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Forbidden Knowledge";
            string longDesc = "Chests contain more.\n\n" +
                "Long kept hidden by the Esoteric Order, this alien object grants insight into the eternal abyss deep below Bullet Hell.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "rtr");

            //Adds the actual passive effect to the item

            //Set the rarity of the item
            item.quality = PickupObject.ItemQuality.S;
        }

        public override void Pickup(PlayerController player)
        {
            ETGMod.Chest.OnPostOpen += OnOpenedChest;
            base.Pickup(player);
        }

        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            ETGMod.Chest.OnPostOpen -= OnOpenedChest;
            return debrisObject;
        }

        private void OnOpenedChest(Chest chest, PlayerController player)
        {

            AkSoundEngine.PostEvent("Play_ENM_creecher_peel_01", base.gameObject);

            if (chest.breakAnimName.Contains("wood"))
            {
                PickupObject pickupObject = this.OpenD(player);

                StatModifier statModifier = new StatModifier();
                statModifier.statToBoost = PlayerStats.StatType.Curse;
                statModifier.amount = 2f;
                statModifier.modifyType = StatModifier.ModifyMethod.ADDITIVE;

                if (pickupObject is Gun)
                {
                    Gun gun = pickupObject as Gun;
                    Array.Resize<StatModifier>(ref gun.passiveStatModifiers, gun.passiveStatModifiers.Length + 1);
                    gun.passiveStatModifiers[gun.passiveStatModifiers.Length - 1] = statModifier;
                }
                else if (pickupObject is PassiveItem)
                {
                    PassiveItem passiveItem = pickupObject as PassiveItem;
                    Array.Resize<StatModifier>(ref passiveItem.passiveStatModifiers, passiveItem.passiveStatModifiers.Length + 1);
                    passiveItem.passiveStatModifiers[passiveItem.passiveStatModifiers.Length - 1] = statModifier;
                }
                else if (pickupObject is PlayerItem)
                {
                    PlayerItem playerItem = pickupObject as PlayerItem;
                    Array.Resize<StatModifier>(ref playerItem.passiveStatModifiers, playerItem.passiveStatModifiers.Length + 1);
                    playerItem.passiveStatModifiers[playerItem.passiveStatModifiers.Length - 1] = statModifier;
                }
            }

            if (chest.breakAnimName.Contains("silver"))
            {
                PickupObject pickupObject = this.OpenC(player);

                StatModifier statModifier = new StatModifier();
                statModifier.statToBoost = PlayerStats.StatType.Curse;
                statModifier.amount = 2f;
                statModifier.modifyType = StatModifier.ModifyMethod.ADDITIVE;

                if (pickupObject is Gun)
                {
                    Gun gun = pickupObject as Gun;
                    Array.Resize<StatModifier>(ref gun.passiveStatModifiers, gun.passiveStatModifiers.Length + 1);
                    gun.passiveStatModifiers[gun.passiveStatModifiers.Length - 1] = statModifier;
                }
                else if (pickupObject is PassiveItem)
                {
                    PassiveItem passiveItem = pickupObject as PassiveItem;
                    Array.Resize<StatModifier>(ref passiveItem.passiveStatModifiers, passiveItem.passiveStatModifiers.Length + 1);
                    passiveItem.passiveStatModifiers[passiveItem.passiveStatModifiers.Length - 1] = statModifier;
                }
                else if (pickupObject is PlayerItem)
                {
                    PlayerItem playerItem = pickupObject as PlayerItem;
                    Array.Resize<StatModifier>(ref playerItem.passiveStatModifiers, playerItem.passiveStatModifiers.Length + 1);
                    playerItem.passiveStatModifiers[playerItem.passiveStatModifiers.Length - 1] = statModifier;
                }
            }

            if (chest.breakAnimName.Contains("green"))
            {
                PickupObject pickupObject = this.OpenB(player);

                StatModifier statModifier = new StatModifier();
                statModifier.statToBoost = PlayerStats.StatType.Curse;
                statModifier.amount = 2f;
                statModifier.modifyType = StatModifier.ModifyMethod.ADDITIVE;

                if (pickupObject is Gun)
                {
                    Gun gun = pickupObject as Gun;
                    Array.Resize<StatModifier>(ref gun.passiveStatModifiers, gun.passiveStatModifiers.Length + 1);
                    gun.passiveStatModifiers[gun.passiveStatModifiers.Length - 1] = statModifier;
                }
                else if (pickupObject is PassiveItem)
                {
                    PassiveItem passiveItem = pickupObject as PassiveItem;
                    Array.Resize<StatModifier>(ref passiveItem.passiveStatModifiers, passiveItem.passiveStatModifiers.Length + 1);
                    passiveItem.passiveStatModifiers[passiveItem.passiveStatModifiers.Length - 1] = statModifier;
                }
                else if (pickupObject is PlayerItem)
                {
                    PlayerItem playerItem = pickupObject as PlayerItem;
                    Array.Resize<StatModifier>(ref playerItem.passiveStatModifiers, playerItem.passiveStatModifiers.Length + 1);
                    playerItem.passiveStatModifiers[playerItem.passiveStatModifiers.Length - 1] = statModifier;
                }
            }

            if (chest.breakAnimName.Contains("redgold"))
            {
                PickupObject pickupObject = this.OpenA(player);

                StatModifier statModifier = new StatModifier();
                statModifier.statToBoost = PlayerStats.StatType.Curse;
                statModifier.amount = 2f;
                statModifier.modifyType = StatModifier.ModifyMethod.ADDITIVE;

                if (pickupObject is Gun)
                {
                    Gun gun = pickupObject as Gun;
                    Array.Resize<StatModifier>(ref gun.passiveStatModifiers, gun.passiveStatModifiers.Length + 1);
                    gun.passiveStatModifiers[gun.passiveStatModifiers.Length - 1] = statModifier;
                }
                else if (pickupObject is PassiveItem)
                {
                    PassiveItem passiveItem = pickupObject as PassiveItem;
                    Array.Resize<StatModifier>(ref passiveItem.passiveStatModifiers, passiveItem.passiveStatModifiers.Length + 1);
                    passiveItem.passiveStatModifiers[passiveItem.passiveStatModifiers.Length - 1] = statModifier;
                }
                else if (pickupObject is PlayerItem)
                {
                    PlayerItem playerItem = pickupObject as PlayerItem;
                    Array.Resize<StatModifier>(ref playerItem.passiveStatModifiers, playerItem.passiveStatModifiers.Length + 1);
                    playerItem.passiveStatModifiers[playerItem.passiveStatModifiers.Length - 1] = statModifier;
                }
            }


            if (chest.breakAnimName.Contains("black"))
            {
                PickupObject pickupObject = this.OpenS(player);

                StatModifier statModifier = new StatModifier();
                statModifier.statToBoost = PlayerStats.StatType.Curse;
                statModifier.amount = 2f;
                statModifier.modifyType = StatModifier.ModifyMethod.ADDITIVE;

                if (pickupObject is Gun)
                {
                    Gun gun = pickupObject as Gun;
                    Array.Resize<StatModifier>(ref gun.passiveStatModifiers, gun.passiveStatModifiers.Length + 1);
                    gun.passiveStatModifiers[gun.passiveStatModifiers.Length - 1] = statModifier;
                }
                else if (pickupObject is PassiveItem)
                {
                    PassiveItem passiveItem = pickupObject as PassiveItem;
                    Array.Resize<StatModifier>(ref passiveItem.passiveStatModifiers, passiveItem.passiveStatModifiers.Length + 1);
                    passiveItem.passiveStatModifiers[passiveItem.passiveStatModifiers.Length - 1] = statModifier;
                }
                else if (pickupObject is PlayerItem)
                {
                    PlayerItem playerItem = pickupObject as PlayerItem;
                    Array.Resize<StatModifier>(ref playerItem.passiveStatModifiers, playerItem.passiveStatModifiers.Length + 1);
                    playerItem.passiveStatModifiers[playerItem.passiveStatModifiers.Length - 1] = statModifier;
                }
            }

            else if (!chest.breakAnimName.Contains("black") && !chest.breakAnimName.Contains("redgold") && !chest.breakAnimName.Contains("green") && !chest.breakAnimName.Contains("silver") && !chest.breakAnimName.Contains("wood"))
            {
                PickupObject pickupObject = this.OpenRandom(player);

                StatModifier statModifier = new StatModifier();
                statModifier.statToBoost = PlayerStats.StatType.Curse;
                statModifier.amount = 2f;
                statModifier.modifyType = StatModifier.ModifyMethod.ADDITIVE;

                if (pickupObject is Gun)
                {
                    Gun gun = pickupObject as Gun;
                    Array.Resize<StatModifier>(ref gun.passiveStatModifiers, gun.passiveStatModifiers.Length + 1);
                    gun.passiveStatModifiers[gun.passiveStatModifiers.Length - 1] = statModifier;
                }
                else if (pickupObject is PassiveItem)
                {
                    PassiveItem passiveItem = pickupObject as PassiveItem;
                    Array.Resize<StatModifier>(ref passiveItem.passiveStatModifiers, passiveItem.passiveStatModifiers.Length + 1);
                    passiveItem.passiveStatModifiers[passiveItem.passiveStatModifiers.Length - 1] = statModifier;
                }
                else if (pickupObject is PlayerItem)
                {
                    PlayerItem playerItem = pickupObject as PlayerItem;
                    Array.Resize<StatModifier>(ref playerItem.passiveStatModifiers, playerItem.passiveStatModifiers.Length + 1);
                    playerItem.passiveStatModifiers[playerItem.passiveStatModifiers.Length - 1] = statModifier;
                }
            }


        }

        private PickupObject OpenD(PlayerController user)
        {
            PickupObject.ItemQuality itemQuality = PickupObject.ItemQuality.D;
            PickupObject itemOfTypeAndQuality = (UnityEngine.Random.value >= 0.5f) ? LootEngine.GetItemOfTypeAndQuality<PickupObject>(itemQuality, GameManager.Instance.RewardManager.GunsLootTable, false) : LootEngine.GetItemOfTypeAndQuality<PickupObject>(itemQuality, GameManager.Instance.RewardManager.ItemsLootTable, false);
            DebrisObject debrisObject = LootEngine.SpawnItem(itemOfTypeAndQuality.gameObject, user.CenterPosition, Vector2.up, 1f, true, true, false);
            if (debrisObject)
            {
                Vector2 v = (!debrisObject.sprite) ? (debrisObject.transform.position.XY() + new Vector2(0.5f, 0.5f)) : debrisObject.sprite.WorldCenter;
                GameObject gameObject = SpawnManager.SpawnVFX((GameObject)BraveResources.Load("Global VFX/VFX_BlackPhantomDeath", ".prefab"), v, Quaternion.identity, false);
                if (gameObject && gameObject.GetComponent<tk2dSprite>())
                {
                    tk2dSprite component = gameObject.GetComponent<tk2dSprite>();
                    component.HeightOffGround = 5f;
                    component.UpdateZDepth();
                }
                return debrisObject.GetComponentInChildren<PickupObject>();
            }
            return null;
        }

        private PickupObject OpenC(PlayerController user)
        {
            PickupObject.ItemQuality itemQuality = PickupObject.ItemQuality.C;
            PickupObject itemOfTypeAndQuality = (UnityEngine.Random.value >= 0.5f) ? LootEngine.GetItemOfTypeAndQuality<PickupObject>(itemQuality, GameManager.Instance.RewardManager.GunsLootTable, false) : LootEngine.GetItemOfTypeAndQuality<PickupObject>(itemQuality, GameManager.Instance.RewardManager.ItemsLootTable, false);
            DebrisObject debrisObject = LootEngine.SpawnItem(itemOfTypeAndQuality.gameObject, user.CenterPosition, Vector2.up, 1f, true, true, false);
            if (debrisObject)
            {
                Vector2 v = (!debrisObject.sprite) ? (debrisObject.transform.position.XY() + new Vector2(0.5f, 0.5f)) : debrisObject.sprite.WorldCenter;
                GameObject gameObject = SpawnManager.SpawnVFX((GameObject)BraveResources.Load("Global VFX/VFX_BlackPhantomDeath", ".prefab"), v, Quaternion.identity, false);
                if (gameObject && gameObject.GetComponent<tk2dSprite>())
                {
                    tk2dSprite component = gameObject.GetComponent<tk2dSprite>();
                    component.HeightOffGround = 5f;
                    component.UpdateZDepth();
                }
                return debrisObject.GetComponentInChildren<PickupObject>();
            }
            return null;
        }

        private PickupObject OpenB(PlayerController user)
        {
            PickupObject.ItemQuality itemQuality = PickupObject.ItemQuality.B;
            PickupObject itemOfTypeAndQuality = (UnityEngine.Random.value >= 0.5f) ? LootEngine.GetItemOfTypeAndQuality<PickupObject>(itemQuality, GameManager.Instance.RewardManager.GunsLootTable, false) : LootEngine.GetItemOfTypeAndQuality<PickupObject>(itemQuality, GameManager.Instance.RewardManager.ItemsLootTable, false);
            DebrisObject debrisObject = LootEngine.SpawnItem(itemOfTypeAndQuality.gameObject, user.CenterPosition, Vector2.up, 1f, true, true, false);
            if (debrisObject)
            {
                Vector2 v = (!debrisObject.sprite) ? (debrisObject.transform.position.XY() + new Vector2(0.5f, 0.5f)) : debrisObject.sprite.WorldCenter;
                GameObject gameObject = SpawnManager.SpawnVFX((GameObject)BraveResources.Load("Global VFX/VFX_BlackPhantomDeath", ".prefab"), v, Quaternion.identity, false);
                if (gameObject && gameObject.GetComponent<tk2dSprite>())
                {
                    tk2dSprite component = gameObject.GetComponent<tk2dSprite>();
                    component.HeightOffGround = 5f;
                    component.UpdateZDepth();
                }
                return debrisObject.GetComponentInChildren<PickupObject>();
            }
            return null;
        }

        private PickupObject OpenA(PlayerController user)
        {
            PickupObject.ItemQuality itemQuality = PickupObject.ItemQuality.A;
            PickupObject itemOfTypeAndQuality = (UnityEngine.Random.value >= 0.5f) ? LootEngine.GetItemOfTypeAndQuality<PickupObject>(itemQuality, GameManager.Instance.RewardManager.GunsLootTable, false) : LootEngine.GetItemOfTypeAndQuality<PickupObject>(itemQuality, GameManager.Instance.RewardManager.ItemsLootTable, false);
            DebrisObject debrisObject = LootEngine.SpawnItem(itemOfTypeAndQuality.gameObject, user.CenterPosition, Vector2.up, 1f, true, true, false);
            if (debrisObject)
            {
                Vector2 v = (!debrisObject.sprite) ? (debrisObject.transform.position.XY() + new Vector2(0.5f, 0.5f)) : debrisObject.sprite.WorldCenter;
                GameObject gameObject = SpawnManager.SpawnVFX((GameObject)BraveResources.Load("Global VFX/VFX_BlackPhantomDeath", ".prefab"), v, Quaternion.identity, false);
                if (gameObject && gameObject.GetComponent<tk2dSprite>())
                {
                    tk2dSprite component = gameObject.GetComponent<tk2dSprite>();
                    component.HeightOffGround = 5f;
                    component.UpdateZDepth();
                }
                return debrisObject.GetComponentInChildren<PickupObject>();
            }
            return null;
        }

        private PickupObject OpenS(PlayerController user)
        {
            PickupObject.ItemQuality itemQuality = PickupObject.ItemQuality.S;
            PickupObject itemOfTypeAndQuality = (UnityEngine.Random.value >= 0.5f) ? LootEngine.GetItemOfTypeAndQuality<PickupObject>(itemQuality, GameManager.Instance.RewardManager.GunsLootTable, false) : LootEngine.GetItemOfTypeAndQuality<PickupObject>(itemQuality, GameManager.Instance.RewardManager.ItemsLootTable, false);
            DebrisObject debrisObject = LootEngine.SpawnItem(itemOfTypeAndQuality.gameObject, user.CenterPosition, Vector2.up, 1f, true, true, false);
            if (debrisObject)
            {
                Vector2 v = (!debrisObject.sprite) ? (debrisObject.transform.position.XY() + new Vector2(0.5f, 0.5f)) : debrisObject.sprite.WorldCenter;
                GameObject gameObject = SpawnManager.SpawnVFX((GameObject)BraveResources.Load("Global VFX/VFX_BlackPhantomDeath", ".prefab"), v, Quaternion.identity, false);
                if (gameObject && gameObject.GetComponent<tk2dSprite>())
                {
                    tk2dSprite component = gameObject.GetComponent<tk2dSprite>();
                    component.HeightOffGround = 5f;
                    component.UpdateZDepth();
                }
                return debrisObject.GetComponentInChildren<PickupObject>();
            }
            return null;
        }

        private PickupObject OpenRandom(PlayerController user)
        {
            PickupObject.ItemQuality startQuality = PickupObject.ItemQuality.D;
            PickupObject.ItemQuality endQuality = PickupObject.ItemQuality.S;
            PickupObject.ItemQuality itemQuality = (PickupObject.ItemQuality)UnityEngine.Random.Range((int)startQuality, (int)(endQuality + 1));

            PickupObject itemOfTypeAndQuality = (UnityEngine.Random.value >= 0.5f) ? LootEngine.GetItemOfTypeAndQuality<PickupObject>(itemQuality, GameManager.Instance.RewardManager.GunsLootTable, false) : LootEngine.GetItemOfTypeAndQuality<PickupObject>(itemQuality, GameManager.Instance.RewardManager.ItemsLootTable, false);
            DebrisObject debrisObject = LootEngine.SpawnItem(itemOfTypeAndQuality.gameObject, user.CenterPosition, Vector2.up, 1f, true, true, false);
            if (debrisObject)
            {
                Vector2 v = (!debrisObject.sprite) ? (debrisObject.transform.position.XY() + new Vector2(0.5f, 0.5f)) : debrisObject.sprite.WorldCenter;
                GameObject gameObject = SpawnManager.SpawnVFX((GameObject)BraveResources.Load("Global VFX/VFX_BlackPhantomDeath", ".prefab"), v, Quaternion.identity, false);
                if (gameObject && gameObject.GetComponent<tk2dSprite>())
                {
                    tk2dSprite component = gameObject.GetComponent<tk2dSprite>();
                    component.HeightOffGround = 5f;
                    component.UpdateZDepth();
                }
                return debrisObject.GetComponentInChildren<PickupObject>();
            }
            return null;
        }




        private Gun lastGun;

        protected override void Update()
        {
            if (Owner)
            {
                if (Owner.CurrentGun != lastGun)
                {
                    lastGun = Owner.CurrentGun;
                    if (Owner.CurrentGun.passiveStatModifiers.Length == 0)
                    {
                        RemoveStat(PlayerStats.StatType.Damage);
                        Owner.stats.RecalculateStats(Owner, true, false);
                    }

                    else
                    {
                        foreach (StatModifier statmodifier in Owner.CurrentGun.passiveStatModifiers)
                        {
                            if (statmodifier.statToBoost == PlayerStats.StatType.Curse)
                            {
                                AddStat(PlayerStats.StatType.Damage, 1.1f, StatModifier.ModifyMethod.MULTIPLICATIVE);
                                Owner.stats.RecalculateStats(Owner, true, false);
                            }
                        }
                    }
                }
            }
        }




        private void AddStat(PlayerStats.StatType statType, float amount, StatModifier.ModifyMethod method = StatModifier.ModifyMethod.ADDITIVE)
        {
            foreach (var m in passiveStatModifiers)
            {
                if (m.statToBoost == statType) return; //don't add duplicates
            }

            StatModifier modifier = new StatModifier
            {
                amount = amount,
                statToBoost = statType,
                modifyType = method
            };

            if (this.passiveStatModifiers == null)
                this.passiveStatModifiers = new StatModifier[] { modifier };
            else
                this.passiveStatModifiers = this.passiveStatModifiers.Concat(new StatModifier[] { modifier }).ToArray();
        }

        private void RemoveStat(PlayerStats.StatType statType)
        {
            var newModifiers = new List<StatModifier>();
            for (int i = 0; i < passiveStatModifiers.Length; i++)
            {
                if (passiveStatModifiers[i].statToBoost != statType)
                    newModifiers.Add(passiveStatModifiers[i]);
            }
            this.passiveStatModifiers = newModifiers.ToArray();
        }
    }
}

