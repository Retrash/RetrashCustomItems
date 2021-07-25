using UnityEngine;
using ItemAPI;
using Dungeonator;
using System.Collections.Generic;
using System.Collections;
using Gungeon;
using System;
using System.Linq;

namespace Blunderbeast
{
    public class InfinityGuontletPassive : PassiveItem
    {
        public static void Init()
        {
            //The name of the item
            string itemName = "Infinity Guontlet";

            //Refers to an embedded png in the project. Make sure to embed your resources!
            string resourceName = "Blunderbeast/Resources/infinityguontlet";

            //Create new GameObject
            GameObject obj = new GameObject();

            //Add a ActiveItem component to the object
            var item = obj.AddComponent<InfinityGuontletPassive>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Perfectly Balanced";
            string longDesc = "Once per floor, can transmute Glass Guon Stones using its wielder's lifeforce. Will unleash its true power once all 7 magical stones are collected.\n\n" +
                "Gauntlet belonging to an interdimensional tyrant, specially made to synchronize with the energy of Guon Stones.\n\n" + "Obsessed by the magical Guon Stones, he scoured numerous galaxies to find a blacksmith capable of creating an instrument that harnesses their potential.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //"example_pool" here is the item pool. In the console you'd type "give example_pool:sweating_bullets"
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "rtr");

            //Set some other fields
            item.quality = PickupObject.ItemQuality.A;
            InfinityGuontletPassive.effect = InfinityGuontletPassive.GetCombineEffect();
        }

        private bool newFloor, newRoom, HasBeenPickedUp;

        public override void Pickup(PlayerController player)
        {
            if (HasBeenPickedUp == false)
            {
                newFloor = true;
                HasBeenPickedUp = true;
            }
            base.Pickup(player);
            GameManager.Instance.OnNewLevelFullyLoaded += this.FloorReset;
            player.OnEnteredCombat += EnterRoom;
        }

        public override DebrisObject Drop(PlayerController player)
        {
            GameManager.Instance.OnNewLevelFullyLoaded -= this.FloorReset;
            player.OnEnteredCombat -= EnterRoom;
            ActiveVFX = false;
            return base.Drop(player);
        }

        private void FloorReset()
        {
            newFloor = true;
        }

        private bool ActiveVFX;

        protected override void Update()
        {
            if (Owner)
            {
                EvaluateStats(Owner);

                if ((Owner.HasPickupID(260) && Owner.HasPickupID(262) && Owner.HasPickupID(263) && Owner.HasPickupID(264) && Owner.HasPickupID(269) && Owner.HasPickupID(270) && Owner.HasPickupID(466)) && ActiveVFX == false)
                {
                    ActiveVFX = true;
                    Owner.PlayEffectOnActor(ResourceCache.Acquire("Global VFX/VFX_ghostrevive") as GameObject, Vector3.zero, true, false, false);
                    //GameManager.Instance.StartCoroutine(SecondVFX());

                }

                else if ((!Owner.HasPickupID(260) || !Owner.HasPickupID(262) || !Owner.HasPickupID(263) || !Owner.HasPickupID(264) || !Owner.HasPickupID(269) || !Owner.HasPickupID(270) || !Owner.HasPickupID(466)) && ActiveVFX == true)
                {
                    ActiveVFX = false;
                }

                currentItems = Owner.passiveItems.Count;
                currentGuns = Owner.inventory.AllGuns.Count;
                if ((currentItems != lastItems) || (currentGuns != lastGuns))
                {
                    RemoveStat(PlayerStats.StatType.RateOfFire);
                    if (Owner.HasPickupID(ETGMod.Databases.Items["Infinigun"].PickupObjectId))
                    {
                        foreach (PassiveItem item in Owner.passiveItems)
                        {
                            if (item.PickupObjectId == 260 || item.PickupObjectId == 262 || item.PickupObjectId == 263 || item.PickupObjectId == 264 || item.PickupObjectId == 269 || item.PickupObjectId == 270 || item.PickupObjectId == 466)
                            {
                                AddStat(PlayerStats.StatType.RateOfFire, 1.05f, StatModifier.ModifyMethod.MULTIPLICATIVE);
                            }
                        }
                    }
                    lastItems = currentItems;
                    lastGuns = currentGuns;
                    Owner.stats.RecalculateStats(Owner, true, false);
                }
            }

            else { return; }
        }

        private int currentItems, currentGuns, lastItems, lastGuns;

        private void AddStat(PlayerStats.StatType statType, float amount, StatModifier.ModifyMethod method = StatModifier.ModifyMethod.ADDITIVE)
        {

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


        private void EvaluateStats(PlayerController player, bool force = false)
        {
            if (player.HasPickupID(565) && ((newFloor == true) || (player.HasPickupID(ETGMod.Databases.Items["Infinigun"].PickupObjectId))))
            {
                if (!(Owner.HasPickupID(260) && Owner.HasPickupID(262) && Owner.HasPickupID(263) && Owner.HasPickupID(264) && Owner.HasPickupID(269) && Owner.HasPickupID(270) && Owner.HasPickupID(466)))
                {
                    this.CreateGuon(player);
                    newFloor = false;
                    return;
                }
            }
        }


        private void CreateGuon(PlayerController player)
        {

            if (!player.HasPickupID(264))
            {
                player.healthHaver.ApplyDamage(0.5f, Vector2.zero, "The Infinity Guontlet", CoreDamageTypes.None, DamageCategory.Normal, true, null, false);
                LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(264).gameObject, player);
                if (player.HasPickupID(565))
                {
                    player.RemovePassiveItem(565);
                }
            }
            else if (!player.HasPickupID(270))
            {
                player.healthHaver.ApplyDamage(0.5f, Vector2.zero, "The Infinity Guontlet", CoreDamageTypes.None, DamageCategory.Normal, true, null, false);
                LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(270).gameObject, player);
                if (player.HasPickupID(565))
                {
                    player.RemovePassiveItem(565);
                }
            }
            else if (!player.HasPickupID(262))
            {
                player.healthHaver.ApplyDamage(0.5f, Vector2.zero, "The Infinity Guontlet", CoreDamageTypes.None, DamageCategory.Normal, true, null, false);
                LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(262).gameObject, player);
                if (player.HasPickupID(565))
                {
                    player.RemovePassiveItem(565);
                }
            }
            else if (!player.HasPickupID(269))
            {
                player.healthHaver.ApplyDamage(0.5f, Vector2.zero, "The Infinity Guontlet", CoreDamageTypes.None, DamageCategory.Normal, true, null, false);
                LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(269).gameObject, player);
                if (player.HasPickupID(565))
                {
                    player.RemovePassiveItem(565);
                }

            }
            else if (!player.HasPickupID(260))
            {
                player.healthHaver.ApplyDamage(0.5f, Vector2.zero, "The Infinity Guontlet", CoreDamageTypes.None, DamageCategory.Normal, true, null, false);
                LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(260).gameObject, player);
                if (player.HasPickupID(565))
                {
                    player.RemovePassiveItem(565);
                }
            }
            else if (!player.HasPickupID(263))
            {
                player.healthHaver.ApplyDamage(0.5f, Vector2.zero, "The Infinity Guontlet", CoreDamageTypes.None, DamageCategory.Normal, true, null, false);
                LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(263).gameObject, player);
                if (player.HasPickupID(565))
                {
                    player.RemovePassiveItem(565);
                }
            }
            else if (!player.HasPickupID(466))
            {
                player.healthHaver.ApplyDamage(0.5f, Vector2.zero, "The Infinity Guontlet", CoreDamageTypes.None, DamageCategory.Normal, true, null, false);
                LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(466).gameObject, player);
                if (player.HasPickupID(565))
                {
                    player.RemovePassiveItem(565);
                }
            }
        }



        private void EnterRoom()
        {
            PlayerController player = this.Owner;
            if (player.HasPickupID(260) && player.HasPickupID(262) && player.HasPickupID(263) && player.HasPickupID(264) && player.HasPickupID(269) && player.HasPickupID(270) && player.HasPickupID(466) && newRoom == false)
            {
                AkSoundEngine.PostEvent("Play_ITM_Table_Time_Freeze_01", base.gameObject);
                GameManager.Instance.StartCoroutine(this.Detonation());
                newRoom = true;
            }

            else if (newRoom == true)
            {
                newRoom = false;
            }
        }

        private IEnumerator Detonation()
        {
            PlayerController player = this.Owner;
            yield return new WaitForSeconds(1f);
            player.ForceBlank();
            this.KillAllEnemies();
            yield break;
        }

        private void KillAllEnemies()
        {
            RoomHandler currentRoom = GameManager.Instance.BestActivePlayer.CurrentRoom;
            currentRoom.ClearReinforcementLayers();
            List<AIActor> activeEnemies = currentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
            if (activeEnemies != null)
            {
                List<AIActor> list = new List<AIActor>(activeEnemies);
                for (int i = 0; i < list.Count; i++)
                {
                    AIActor aiactor = list[i];
                    {
                        if (!aiactor.healthHaver.IsBoss)
                        {
                            GameManager.Instance.Dungeon.StartCoroutine(this.HandleEnemyDeath(aiactor));
                            SpawnEnemyOnDeath component = aiactor.GetComponent<SpawnEnemyOnDeath>();
                            if (component)
                            {
                                UnityEngine.Object.Destroy(component);
                            }

                            aiactor.healthHaver.minimumHealth = 0f;
                            aiactor.healthHaver.ApplyDamage(10000f, Vector2.zero, "Infinity Guontlet", CoreDamageTypes.None, DamageCategory.Unstoppable, true, null, false);
                        }

                        if (aiactor.healthHaver.IsBoss)
                        {
                            float maxhealth = aiactor.healthHaver.GetCurrentHealth();
                            aiactor.healthHaver.ApplyDamage(maxhealth / 2, Vector2.zero, "Infinity Guontlet", CoreDamageTypes.None, DamageCategory.Unstoppable, true, null, false);
                        }
                    }
                }
            }
        }


        private static CombineEvaporateEffect GetCombineEffect()
        {
            Gun component = Game.Items["combined_rifle"].GetComponent<Gun>();
            return component.alternateVolley.projectiles[0].projectiles[0].GetComponent<CombineEvaporateEffect>();
        }

        private static CombineEvaporateEffect effect;

        private IEnumerator HandleEnemyDeath(AIActor target)
        {
            target.EraseFromExistenceWithRewards(false);
            Transform copyTransform = this.CreateEmptySprite(target);
            tk2dSprite copySprite = copyTransform.GetComponentInChildren<tk2dSprite>();
            GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(InfinityGuontletPassive.effect.ParticleSystemToSpawn, copySprite.WorldCenter.ToVector3ZisY(0f), Quaternion.identity);
            gameObject.transform.parent = copyTransform;
            float elapsed = 0f;
            float duration = 2.5f;
            copySprite.renderer.material.DisableKeyword("TINTING_OFF");
            copySprite.renderer.material.EnableKeyword("TINTING_ON");
            copySprite.renderer.material.DisableKeyword("EMISSIVE_OFF");
            copySprite.renderer.material.EnableKeyword("EMISSIVE_ON");
            copySprite.renderer.material.DisableKeyword("BRIGHTNESS_CLAMP_ON");
            copySprite.renderer.material.EnableKeyword("BRIGHTNESS_CLAMP_OFF");
            copySprite.renderer.material.SetFloat("_EmissiveThresholdSensitivity", 5f);
            copySprite.renderer.material.SetFloat("_EmissiveColorPower", 1f);
            int emId = Shader.PropertyToID("_EmissivePower");
            while (elapsed < duration)
            {
                elapsed += BraveTime.DeltaTime;
                float t = elapsed / duration;
                copySprite.renderer.material.SetFloat(emId, Mathf.Lerp(1f, 10f, t));
                copySprite.renderer.material.SetFloat("_BurnAmount", t);
                copyTransform.position += Vector3.up * BraveTime.DeltaTime * 1f;
                yield return null;
            }
            UnityEngine.Object.Destroy(copyTransform.gameObject);
            yield break;
        }


        private Transform CreateEmptySprite(AIActor target)
        {
            GameObject gameObject = new GameObject("suck image");
            gameObject.layer = target.gameObject.layer;
            tk2dSprite tk2dSprite = gameObject.AddComponent<tk2dSprite>();
            gameObject.transform.parent = SpawnManager.Instance.VFX;
            tk2dSprite.SetSprite(target.sprite.Collection, target.sprite.spriteId);
            tk2dSprite.transform.position = target.sprite.transform.position;
            GameObject gameObject2 = new GameObject("image parent");
            gameObject2.transform.position = tk2dSprite.WorldCenter;
            tk2dSprite.transform.parent = gameObject2.transform;
            tk2dSprite.usesOverrideMaterial = true;
            bool flag = target.optionalPalette != null;
            if (flag)
            {
                tk2dSprite.renderer.material.SetTexture("_PaletteTex", target.optionalPalette);
            }
            bool flag2 = tk2dSprite.renderer.material.shader.name.Contains("ColorEmissive");
            if (flag2)
            {
            }
            return gameObject2.transform;
        }

    }
}
