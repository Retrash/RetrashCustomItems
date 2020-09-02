using System;
using System.Collections.Generic;
using UnityEngine;
using ItemAPI;
using Dungeonator;

namespace Blunderbeast
{
    public class Boombox : PlayerItem
    {
        public static void Init()
        {
            //The name of the item
            string itemName = "Boombox";

            //Refers to an embedded png in the project. Make sure to embed your resources!
            string resourceName = "Blunderbeast/Resources/boombox";

            //Create new GameObject
            GameObject obj = new GameObject();

            //Add a ActiveItem component to the object
            var item = obj.AddComponent<Boombox>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Funky Beats";
            string longDesc = "Plays music, stuns unaware enemies.\n\n" +
                "Unfortunately, this boombox seems to be stuck on shuffle play... At least the song selection is pretty solid.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //"example_pool" here is the item pool. In the console you'd type "give example_pool:sweating_bullets"
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "rtr");

            //Set the cooldown type and duration of the cooldown
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Timed, 0.5f);
            //Adds a passive modifier, like curse, coolness, damage, etc. to the item. Works for passives and actives.

            //Set some other fields
            item.consumable = false;
            item.quality = PickupObject.ItemQuality.B;
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Coolness, 2f);
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.OnEnteredCombat += HandleStunEffect;
        }

        private void Notify(string header, string text)
        {
            tk2dBaseSprite notificationObjectSprite = GameUIRoot.Instance.notificationController.notificationObjectSprite;
            GameUIRoot.Instance.notificationController.DoCustomNotification(header, text, notificationObjectSprite.Collection, notificationObjectSprite.spriteId, UINotificationController.NotificationColor.PURPLE, true, true);
        }

        public bool boomboxactive;

        protected override void DoEffect(PlayerController user)
        {
            AkSoundEngine.PostEvent("Play_OBJ_plate_press_01", base.gameObject);
            string text;
            string header;
            float value = UnityEngine.Random.Range(0.0f, 1.3f);
            if (value < 0.1)
            {
                AkSoundEngine.PostEvent("Stop_MUS_All", base.gameObject);
                AkSoundEngine.PostEvent("Play_MUS_RatPunch_Theme_03", base.gameObject);
                header = "Fist for Guns";
                text = "Punched-out";
                this.Notify(header, text);

            }
            else
            {
                if (value < 0.2)
                {
                    AkSoundEngine.PostEvent("Stop_MUS_All", base.gameObject);
                    AkSoundEngine.PostEvent("Play_MUS_Anthem_Winner_Short_01", base.gameObject);
                    header = "Enter the Gungeon";
                    text = "Enter the Gungeon";
                    this.Notify(header, text);
                }
                else
                {
                    if (value < 0.3)
                    {
                        AkSoundEngine.PostEvent("Stop_MUS_All", base.gameObject);
                        AkSoundEngine.PostEvent("Play_MUS_Lich_Double_01", base.gameObject);
                        header = "Paradox";
                        text = "Gunslinger's Anthem";
                        this.Notify(header, text);
                    }
                    else
                    {
                        if (value < 0.4)
                        {
                            AkSoundEngine.PostEvent("Stop_MUS_All", base.gameObject);
                            AkSoundEngine.PostEvent("Play_MUS_Anthem", base.gameObject);
                            header = "Gungeon Lite";
                            text = "Remixed";
                            this.Notify(header, text);
                        }
                        else
                        {
                            if (value < 0.5)
                            {
                                AkSoundEngine.PostEvent("Stop_MUS_All", base.gameObject);
                                AkSoundEngine.PostEvent("Play_MUS_Sewer_Theme_01", base.gameObject);
                                header = "Filthy Oubliette";
                                text = "Toxic Jam";
                                this.Notify(header, text);
                            }
                            else
                            {
                                if (value < 0.6)
                                {
                                    AkSoundEngine.PostEvent("Stop_MUS_All", base.gameObject);
                                    AkSoundEngine.PostEvent("Play_MUS_Catacombs_Theme_01", base.gameObject);
                                    header = "Hollow Howl";
                                    text = "Chilled";
                                    this.Notify(header, text);
                                }
                                else
                                {
                                    if (value < 0.7)
                                    {
                                        AkSoundEngine.PostEvent("Stop_MUS_All", base.gameObject);
                                        AkSoundEngine.PostEvent("Play_MUS_Office_Theme_01", base.gameObject);
                                        header = "Office Party Massacre";
                                        text = "R&G";
                                        this.Notify(header, text);
                                    }
                                    else
                                    {
                                        if (value < 0.8)
                                        {
                                            AkSoundEngine.PostEvent("Stop_MUS_All", base.gameObject);
                                            AkSoundEngine.PostEvent("Play_MUS_Cathedral_Theme_01", base.gameObject);
                                            header = "Abbey or Die";
                                            text = "Orgun Melody";
                                            this.Notify(header, text);
                                        }
                                        else
                                        {
                                            if (value < 0.9)
                                            {
                                                AkSoundEngine.PostEvent("Stop_MUS_All", base.gameObject);
                                                AkSoundEngine.PostEvent("Play_MUS_Forge_Theme_01", base.gameObject);
                                                header = "Forge Forgives Not";
                                                text = "Hot Mix";
                                                this.Notify(header, text);
                                            }
                                            else
                                            {
                                                if (value < 1)
                                                {
                                                    AkSoundEngine.PostEvent("Stop_MUS_All", base.gameObject);
                                                    AkSoundEngine.PostEvent("Play_MUS_Space_Theme_01", base.gameObject);
                                                    header = "Space Jams";
                                                    text = "Robotic Tunes";
                                                    this.Notify(header, text);
                                                }
                                                else
                                                {
                                                    if (value < 1.1)
                                                    {
                                                        AkSoundEngine.PostEvent("Stop_MUS_All", base.gameObject);
                                                        AkSoundEngine.PostEvent("Play_MUS_BulletHell_Theme_01", base.gameObject);
                                                        header = "Trapped in Bullet Hell";
                                                        text = "Deep Down";
                                                        this.Notify(header, text);
                                                    }
                                                    else
                                                    {
                                                        if (value < 1.2)
                                                        {
                                                            AkSoundEngine.PostEvent("Stop_MUS_All", base.gameObject);
                                                            AkSoundEngine.PostEvent("Play_MUS_Mines_Theme_01", base.gameObject);
                                                            header = "Black Powder Stomp";
                                                            text = "In the Mines";
                                                            this.Notify(header, text);
                                                        }
                                                        else
                                                        {
                                                            AkSoundEngine.PostEvent("Stop_MUS_All", base.gameObject);
                                                            AkSoundEngine.PostEvent("Play_MUS_Boss_Theme_Dragun_02", base.gameObject);
                                                            header = "Dragun Tooth";
                                                            text = "No More Playing Around";
                                                            this.Notify(header, text);
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
        }


        private string m_cachedMusicEventCore;

        public void ResetMusic(Dungeon d)
        {
            if (!string.IsNullOrEmpty(d.musicEventName))
            {
                this.m_cachedMusicEventCore = d.musicEventName;
            }
            else
            {
                this.m_cachedMusicEventCore = "Play_MUS_Dungeon_Theme_01";
            }
        }

        private void DropMusic()
        {
            AkSoundEngine.PostEvent("Stop_MUS_All", base.gameObject);
            AkSoundEngine.PostEvent("Play_OBJ_plate_press_01", base.gameObject);
            this.ResetMusic(GameManager.Instance.Dungeon);
            AkSoundEngine.PostEvent(m_cachedMusicEventCore, GameManager.Instance.gameObject);
        }

        private void HandleStunEffect()
        {
            PlayerController player = this.LastOwner;
            RoomHandler currentRoom = player.CurrentRoom;
            List<AIActor> activeEnemies = currentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
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
            if (target && target.behaviorSpeculator)
            {
                target.behaviorSpeculator.Stun(1.6f, true);
            }
        }

        protected override void OnPreDrop(PlayerController user)
        {
            boomboxactive = false;
            user.OnEnteredCombat -= HandleStunEffect;
            this.DropMusic();
        }
    }
}
