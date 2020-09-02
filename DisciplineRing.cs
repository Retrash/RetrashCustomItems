using UnityEngine;
using ItemAPI;
using Dungeonator;
using System.Collections;
using System;

namespace Blunderbeast
{
    public class DisciplineRing : PassiveItem
    {

        //Call this method from the Start() method of your ETGModule extension
        public static void Init()
        {
            //The name of the item
            string itemName = "Ring of Discipline";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "Blunderbeast/Resources/disciplinering";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<DisciplineRing>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Don't Be Hasty";
            string longDesc = "Adds bonus damage for each room cleared without dodge rolling. Removes one instance of bonus damage each time its wearer dodge rolls in combat.\n\n" +
                "Ring worn only by the most devoted Gun Cultists who have defeated the urge of dodge rolling recklessly.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "rtr");

            //Adds the actual passive effect to the item

            //Set the rarity of the item
            item.quality = PickupObject.ItemQuality.C;
        }

        public bool NoDodge;

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.OnRoomClearEvent += this.RoomCleared;
            player.OnEnteredCombat += TestSkills;
            player.OnPreDodgeRoll += this.DodgeFail;
            AddStats();
            player.stats.RecalculateStats(player, true, false);
        }

        public override DebrisObject Drop(PlayerController player)
        {
            RemoveStats();
            player.stats.RecalculateStats(player, true, false);
            DebrisObject debrisObject = base.Drop(player);
            player.OnRoomClearEvent -= this.RoomCleared;
            player.OnEnteredCombat -= TestSkills;
            player.OnPreDodgeRoll -= this.DodgeFail;
            return debrisObject;
        }

        private void RemoveStats()
        {
            PlayerController player = this.Owner;
            StatModifier statModifier = new StatModifier();
            statModifier.statToBoost = PlayerStats.StatType.Damage;
            statModifier.amount = (disciplineLevel * -0.05f);
            statModifier.modifyType = StatModifier.ModifyMethod.ADDITIVE;
            player.ownerlessStatModifiers.Add(statModifier);
        }

        private void AddStats()
        {
            PlayerController player = this.Owner;
            StatModifier statModifier = new StatModifier();
            statModifier.statToBoost = PlayerStats.StatType.Damage;
            statModifier.amount = (disciplineLevel * 0.05f);
            statModifier.modifyType = StatModifier.ModifyMethod.ADDITIVE;
            player.ownerlessStatModifiers.Add(statModifier);
        }

        private int disciplineLevel;

        private void RoomCleared(PlayerController player)
        {
            if (player.CurrentRoom.PreventStandardRoomReward == false)
            {
                if (NoDodge == true)
                {
                    AkSoundEngine.PostEvent("Play_OBJ_metronome_jingle_01", this.gameObject);

                    if (disciplineLevel < 20)
                    {
                        disciplineLevel += 1;
                        StatModifier statModifier = new StatModifier();
                        statModifier.statToBoost = PlayerStats.StatType.Damage;
                        statModifier.amount = 0.05f;
                        statModifier.modifyType = StatModifier.ModifyMethod.ADDITIVE;
                        player.ownerlessStatModifiers.Add(statModifier);
                        player.stats.RecalculateStats(player, true, false);
                        NoDodge = false;
                    }
                }

                else
                {
                    NoDodge = false;
                }

                base.StartCoroutine(this.MarkRoom());
            }
        }

        private IEnumerator MarkRoom()
        {
            PlayerController player = this.Owner;
            yield return new WaitForSeconds(0.25f);
            player.CurrentRoom.PreventStandardRoomReward = true;
            yield break;
        }

        private void TestSkills()
        {
            NoDodge = true;
        }

        private void DodgeFail(PlayerController player)
        {
            if (disciplineLevel >= 1 && player.IsInCombat)
            {
                disciplineLevel -= 1;
                AkSoundEngine.PostEvent("Play_OBJ_metronome_fail_01", this.gameObject);
                StatModifier statModifier = new StatModifier();
                statModifier.statToBoost = PlayerStats.StatType.Damage;
                statModifier.amount = -0.05f;
                statModifier.modifyType = StatModifier.ModifyMethod.ADDITIVE;
                player.ownerlessStatModifiers.Add(statModifier);
                player.stats.RecalculateStats(player, true, false);
            }
            NoDodge = false;
        }

    }
}

