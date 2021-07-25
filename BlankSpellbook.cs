using UnityEngine;
using ItemAPI;
using System.Collections;
using Dungeonator;
using System.Collections.Generic;
using System;
using MonoMod.RuntimeDetour;
using System.Reflection;

namespace Blunderbeast
{
    public class BlankSpellbook : PassiveItem
    {

        //Call this method from the Start() method of your ETGModule extension
        public static void Init()
        {
            //The name of the item
            string itemName = "Blank Spellbook";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "Blunderbeast/Resources/spellbook";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<BlankSpellbook>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Alakazam!";
            string longDesc = "Old grimoire emanating magical energy.\n\n" + "The first page describes a spell that can be used to transmogrify enemies. The rest of its pages are blank.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "rtr");

            //Adds the actual passive effect to the item

            //Set the rarity of the item
            item.quality = PickupObject.ItemQuality.B;
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.AdditionalBlanksPerFloor, 1f, StatModifier.ModifyMethod.ADDITIVE);
            SpellbookID = item.PickupObjectId;
        }

        private static int SpellbookID;

        public override void Pickup(PlayerController player)
        {
            player.OnUsedPlayerItem += ItemChicken;
            player.OnUsedBlank += BlankChicken;
            player.OnTableFlipped += TableChicken;
            base.Pickup(player);
        }

        private void TableChicken(FlippableCover obj)
        {
            PlayerController player = this.Owner;
            if (player.HasPickupID(400))
            {
                HandleChickenEffect();
            }
        }

        private static Hook SpellHook = new Hook(
              typeof(AIActor).GetMethod("Transmogrify", BindingFlags.Instance | BindingFlags.Public),
              typeof(BlankSpellbook).GetMethod("TransmogrifyHook")
          );


        public static void TransmogrifyHook(Action<AIActor, AIActor, GameObject> orig, AIActor aiactor, AIActor EnemyPrefab, GameObject EffectVFX)
        {
            orig(aiactor, EnemyPrefab, EffectVFX);

            if (GameManager.Instance.PrimaryPlayer != null)
            {
                if ((GameManager.Instance.PrimaryPlayer.HasPickupID(SpellbookID) && ((GameManager.Instance.PrimaryPlayer.HasPickupID(145) || GameManager.Instance.PrimaryPlayer.HasPickupID(61)))))
                {
                    GameObject silencerVFX = (GameObject)ResourceCache.Acquire("Global VFX/BlankVFX_Ghost");
                    AkSoundEngine.PostEvent("Play_OBJ_silenceblank_small_01", GameManager.Instance.PrimaryPlayer.gameObject);
                    GameObject gameObject = new GameObject("silencer");
                    SilencerInstance silencerInstance = gameObject.AddComponent<SilencerInstance>();
                    float additionalTimeAtMaxRadius = 0.25f;
                    silencerInstance.TriggerSilencer(aiactor.CenterPosition, 20f, 5f, silencerVFX, 0f, 3f, 3f, 3f, 30f, 3f, additionalTimeAtMaxRadius, GameManager.Instance.PrimaryPlayer, true, false);
                }
            }
        }


        public override DebrisObject Drop(PlayerController player)
        {
            player.OnUsedPlayerItem -= ItemChicken;
            player.OnUsedBlank -= BlankChicken;
            player.OnTableFlipped -= TableChicken;
            return base.Drop(player);
        }

        public float InternalCooldown;

        private float m_lastUsedTime;

        private void ItemChicken(PlayerController player, PlayerItem obj)
        {
            this.InternalCooldown = 10f;

            if (Time.realtimeSinceStartup - this.m_lastUsedTime < this.InternalCooldown && !player.HasPickupID(ETGMod.Databases.Items["Crystal Ball"].PickupObjectId))
            {
                return;
            }
            this.m_lastUsedTime = Time.realtimeSinceStartup;
            HandleChickenEffect();
        }

        private void BlankChicken(PlayerController player, int blank)
        {
            HandleChickenEffect();              
        }

        private void HandleChickenEffect()
        {
            RoomHandler absoluteRoom = Owner.CurrentRoom;

            AIActor randomActiveEnemy = absoluteRoom.GetRandomActiveEnemy(false);

            if (randomActiveEnemy != null && randomActiveEnemy.IsNormalEnemy && randomActiveEnemy.healthHaver && !randomActiveEnemy.healthHaver.IsBoss)
            {
                AffectEnemy(randomActiveEnemy);
            }

        }

        protected void AffectEnemy(AIActor randomActiveEnemy)
        {
            if (randomActiveEnemy && randomActiveEnemy.behaviorSpeculator)
            {
                randomActiveEnemy.Transmogrify(EnemyDatabase.GetOrLoadByName("chicken"), (GameObject)ResourceCache.Acquire("Global VFX/VFX_Item_Spawn_Poof"));
            }
        }
    }
}

