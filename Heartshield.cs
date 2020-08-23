using System.Collections;
using UnityEngine;
using ItemAPI;

namespace Blunderbeast
{
    public class Heartshield : PassiveItem
    {
        public bool IsFlipped { get; private set; }

        //Call this method from the Start() method of your ETGModule extension
        public static void Init()
        {
            //The name of the item
            string itemName = "Heart Shield";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "Blunderbeast/Resources/heartshield";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<Heartshield>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Vital Aegis";
            string longDesc = "Taking damage turns health into armor while it's wielder is healthy.\n\n" +
                "Shield in the shape of a heart. This ancient artifact remains pristine by leeching the life of its wielder.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "rtr");

            //Adds the actual passive effect to the item
            //Set the rarity of the item
            item.quality = PickupObject.ItemQuality.C;
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Health, 1f, StatModifier.ModifyMethod.ADDITIVE);
        }

        public override void Pickup(PlayerController player)
        {
            if (!this.m_pickedUpThisRun)
            {
                player.healthHaver.Armor += 1f;
            }
            EvaluateStats(player, true);
            base.Pickup(player);      
        }

        protected override void Update()
        {
            if (Owner)
            {
                EvaluateStats(Owner);
            }

            else { return; }
        }

        private bool hasCrest, hadCrest, shouldRestat;
        private void EvaluateStats(PlayerController player, bool force = false)
        {
            hasCrest = (player.healthHaver.HasCrest);
            shouldRestat = (hadCrest != hasCrest);
            if (!(shouldRestat || force)) return; //don't restat if player already has correct stats

            if (hasCrest)
            {
                float curHealth = player.healthHaver.GetCurrentHealth();
                if (curHealth < 0.5f)
                {
                    player.healthHaver.OnDamaged -= this.RobotTookDamage;
                    player.healthHaver.OnDamaged += this.RobotTookCrestDamage;
                }
                else
                {
                    player.healthHaver.OnDamaged -= this.PlayerTookDamage;
                    player.healthHaver.OnDamaged += this.PlayerTookCrestDamage;
                }
                hadCrest = true;
            }

            else
            {
                float curHealth = player.healthHaver.GetCurrentHealth();
                if (curHealth < 0.5f)
                {
                    player.healthHaver.OnDamaged -= this.RobotTookCrestDamage;
                    player.healthHaver.OnDamaged += this.RobotTookDamage;
                }
                else
                {
                    player.healthHaver.OnDamaged -= this.PlayerTookCrestDamage;
                    player.healthHaver.OnDamaged += this.PlayerTookDamage;
                }
                hadCrest = false;
            }
            player.stats.RecalculateStats(player, true, false);
        }

        public override DebrisObject Drop(PlayerController player)
        {
            player.healthHaver.OnDamaged -= this.PlayerTookDamage;
            player.healthHaver.OnDamaged -= this.PlayerTookCrestDamage;
            player.healthHaver.OnDamaged -= this.RobotTookDamage;
            player.healthHaver.OnDamaged -= this.RobotTookCrestDamage;
            return base.Drop(player);
        }

        private void PlayerTookDamage(float resultValue, float maxValue, CoreDamageTypes damageTypes, DamageCategory damageCategory, Vector2 damageDirection)
        {
            PlayerController player = this.Owner;
            float curHealth = player.healthHaver.GetCurrentHealth();
            if (curHealth > 0.5f)
            {
                player.healthHaver.ForceSetCurrentHealth(curHealth - 0.5f);
                GameManager.Instance.StartCoroutine(this.Armorgain());
            }
            else { return; }
        }

        private void PlayerTookCrestDamage(float resultValue, float maxValue, CoreDamageTypes damageTypes, DamageCategory damageCategory, Vector2 damageDirection)
        {
            PlayerController player = this.Owner;
            float curHealth = player.healthHaver.GetCurrentHealth();
            if (curHealth > 0.5f)
            {
                player.healthHaver.ForceSetCurrentHealth(curHealth - 0.5f);
                GameManager.Instance.StartCoroutine(this.Crestgain());
            }
            else { return; }
        }

        private void RobotTookDamage(float resultValue, float maxValue, CoreDamageTypes damageTypes, DamageCategory damageCategory, Vector2 damageDirection)
        {
            PlayerController player = this.Owner;
            float curArmor = player.healthHaver.Armor;
            if (curArmor > 1f)
            {
            }
        }

        private void RobotTookCrestDamage(float resultValue, float maxValue, CoreDamageTypes damageTypes, DamageCategory damageCategory, Vector2 damageDirection)
        {
            PlayerController player = this.Owner;
            float curArmor = player.healthHaver.Armor;
            GameManager.Instance.StartCoroutine(this.RobotCrestgain());
        }


        private IEnumerator Armorgain()
        {
            PlayerController player = this.Owner;
            yield return new WaitForSeconds(1f);
            AkSoundEngine.PostEvent("Play_OBJ_metalskin_deflect_01", base.gameObject);
            player.healthHaver.Armor += 1f;
            yield break;
        }

        private IEnumerator Crestgain()
        {
            PlayerController player = this.Owner;
            yield return new WaitForSeconds(1f);
            AkSoundEngine.PostEvent("Play_OBJ_metalskin_deflect_01", base.gameObject);
            PickupObject byId = PickupObjectDatabase.GetById(305);
            player.AcquirePassiveItemPrefabDirectly(byId as PassiveItem);
            yield break;
        }

        private IEnumerator RobotCrestgain()
        {
            PlayerController player = this.Owner;
            yield return new WaitForSeconds(1f);
            AkSoundEngine.PostEvent("Play_OBJ_metalskin_deflect_01", base.gameObject);
            PickupObject byId = PickupObjectDatabase.GetById(305);
            player.AcquirePassiveItemPrefabDirectly(byId as PassiveItem);
            player.healthHaver.Armor -= 1f;
            yield break;
        }
    }
}
