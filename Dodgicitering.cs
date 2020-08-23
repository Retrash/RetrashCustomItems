using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ItemAPI;
using System.Collections;

namespace Blunderbeast
{
    public class Dodgicitering : PlayerItem
    {
        public static void Init()
        {
            //The name of the item
            string itemName = "Unstable Dodgicite Ring";

            //Refers to an embedded png in the project. Make sure to embed your resources!
            string resourceName = "Blunderbeast/Resources/dodgicite";

            //Create new GameObject
            GameObject obj = new GameObject();

            //Add a ActiveItem component to the object
            var item = obj.AddComponent<Dodgicitering>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Ser Manuel's Lost Treasure";
            string longDesc = "Disables the usage of guns but greatly increases the offensive capability of dodge rolls. Can be turned on and off at will.\n\n" +
                "When pure dodgicite is left immobile for an extended period of time, it starts releasing the energy built up inside of it. In most cases, this phenomenon causes all firearms in proximity to malfunction.\n\n" +
                "This ring was once Ser Manuel's greatest possession until his life came to an abrupt end.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //"example_pool" here is the item pool. In the console you'd type "give example_pool:sweating_bullets"
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "rtr");

            //Set the cooldown type and duration of the cooldown
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Timed, 2f);
            //Adds a passive modifier, like curse, coolness, damage, etc. to the item. Works for passives and actives.

            //Set some other fields
            item.consumable = false;
            item.quality = PickupObject.ItemQuality.D;
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
        }

        protected override void DoEffect(PlayerController user)
        {
            if (GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.END_TIMES)
            {
                return;
            }
            base.IsCurrentlyActive = true;
            user.ChangeSpecialShaderFlag(2, 1f);
            AkSoundEngine.PostEvent("Play_ITM_Macho_Brace_Active_01", this.gameObject);
            this.DisableVFX(user);
            this.EnableVFX(user);
            if (!user.HasPickupID(190))
            {
                user.IsGunLocked = true;
            }
            AddStat(PlayerStats.StatType.DodgeRollDamage, 20f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            AddStat(PlayerStats.StatType.DodgeRollSpeedMultiplier, 1.1f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            AddStat(PlayerStats.StatType.DodgeRollDistanceMultiplier, 1.1f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            user.stats.RecalculateStats(user, true, false);
            user.ReceivesTouchDamage = false;
            user.healthHaver.OnDamaged -= this.PlayerTookDamageN;
            user.healthHaver.OnDamaged += this.PlayerTookDamageO;
        }

        protected override void DoActiveEffect(PlayerController user)
        {
            base.IsCurrentlyActive = false;
            user.ChangeSpecialShaderFlag(2, 0f);
            AkSoundEngine.PostEvent("Play_ITM_Macho_Brace_Fade_01", this.gameObject);
            this.DisableVFX(user);
            user.IsGunLocked = false;
            RemoveStat(PlayerStats.StatType.DodgeRollDistanceMultiplier);
            RemoveStat(PlayerStats.StatType.DodgeRollSpeedMultiplier);
            RemoveStat(PlayerStats.StatType.DodgeRollDamage);
            user.stats.RecalculateStats(user, true, false);
            user.ReceivesTouchDamage = true;
            user.healthHaver.OnDamaged -= this.PlayerTookDamageO;
            user.healthHaver.OnDamaged += this.PlayerTookDamageN;
        }

        private void PlayerTookDamageO(float resultValue, float maxValue, CoreDamageTypes damageTypes, DamageCategory damageCategory, Vector2 damageDirection)
        {
            GameManager.Instance.StartCoroutine(this.GainOutline());
        }

        private void PlayerTookDamageN(float resultValue, float maxValue, CoreDamageTypes damageTypes, DamageCategory damageCategory, Vector2 damageDirection)
        {
            GameManager.Instance.StartCoroutine(this.LoseOutline());
        }

        private IEnumerator GainOutline()
        {
            PlayerController user = this.LastOwner;
            yield return new WaitForSeconds(0.05f);
            EnableVFX(user);
            yield break;
        }

        private IEnumerator LoseOutline()
        {
            PlayerController user = this.LastOwner;
            yield return new WaitForSeconds(0.05f);
            DisableVFX(user);
            yield break;
        }

        protected override void OnPreDrop(PlayerController user)
        {
            if (base.IsCurrentlyActive)
            {
                this.DoActiveEffect(user);
            }
        }

        public DebrisObject Drop(PlayerController user)
        {
            DebrisObject debrisObject = base.Drop(user);
            this.DoActiveEffect(user);
            return debrisObject;
        }

        public override void OnItemSwitched(PlayerController user)
        {
            if (base.IsCurrentlyActive)
            {
                this.DoActiveEffect(user);
            }
        }

        private void EnableVFX(PlayerController user)
        {
                Material outlineMaterial = SpriteOutlineManager.GetOutlineMaterial(user.sprite);
                outlineMaterial.SetColor("_OverrideColor", new Color(0f, 10f, 99f));
        }

        private void DisableVFX(PlayerController user)
        {
                Material outlineMaterial = SpriteOutlineManager.GetOutlineMaterial(user.sprite);
                outlineMaterial.SetColor("_OverrideColor", new Color(0f, 0f, 0f));
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
