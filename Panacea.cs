using UnityEngine;
using ItemAPI;

namespace Blunderbeast
{
    public class Panacea : PlayerItem
    {
        public static void Init()
        {
            //The name of the item
            string itemName = "Panacea";

            //Refers to an embedded png in the project. Make sure to embed your resources!
            string resourceName = "Blunderbeast/Resources/panacea";

            //Create new GameObject
            GameObject obj = new GameObject();

            //Add a ActiveItem component to the object
            var item = obj.AddComponent<Panacea>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Cures All";
            string longDesc = "Completely cleanses the body and soul.\n\n" +
                "Concocted by a master alchemist. This potion will get rid of all impurities and strengthen one's natural resistances.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //"example_pool" here is the item pool. In the console you'd type "give example_pool:sweating_bullets"
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "rtr");

            //Set the cooldown type and duration of the cooldown
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Timed, 1.0f);

            //Adds a passive modifier, like curse, coolness, damage, etc. to the item. Works for passives and actives.
            //Set some other fields
            item.consumable = true;
            item.quality = PickupObject.ItemQuality.S;
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
        }


        protected override void DoEffect(PlayerController user)
        {
            AkSoundEngine.PostEvent("Play_OBJ_dice_bless_01", base.gameObject);
            user.PlayEffectOnActor(ResourceCache.Acquire("Global VFX/VFX_Healing_Sparkles_001") as GameObject, Vector3.zero, true, false, false);

            // give items
            PickupObject byId = PickupObjectDatabase.GetById(264);
            user.AcquirePassiveItemPrefabDirectly(byId as PassiveItem);
            LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(264).gameObject, user);

            user.healthHaver.FullHeal();

            // curse cleansing
            StatModifier statModifier = new StatModifier
            {
                amount = Mathf.Min((float)(PlayerStats.GetTotalCurse() * -1)),
                modifyType = StatModifier.ModifyMethod.ADDITIVE,
                statToBoost = PlayerStats.StatType.Curse
            };
            user.ownerlessStatModifiers.Add(statModifier);

            user.stats.RecalculateStats(user, true, false);
        }
    }
}