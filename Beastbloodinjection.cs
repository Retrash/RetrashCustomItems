using UnityEngine;
using ItemAPI;

namespace Blunderbeast
{
    public class Beastbloodinjection : PlayerItem
    {
        public static void Init()
        {
            //The name of the item
            string itemName = "Beastblood Injection";

            //Refers to an embedded png in the project. Make sure to embed your resources!
            string resourceName = "Blunderbeast/Resources/Beastblood-injection";

            //Create new GameObject
            GameObject obj = new GameObject();

            //Add a ActiveItem component to the object
            var item = obj.AddComponent<Beastbloodinjection>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Blood For Blood";
            string longDesc = "Permanently increase damage at the cost of a full heart. Becomes more potent with curse.\n\n" +
                "Blunderbeasts are Kaliber's spawns and their blood inherits the highly dangerous properties of the jammed.\n\n" +
                "This blood sample was originally collected to find a cure to the Blunderbeast scourge.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //"example_pool" here is the item pool. In the console you'd type "give example_pool:sweating_bullets"
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "rtr");

            //Set the cooldown type and duration of the cooldown
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Timed, 1.0f);

            //Adds a passive modifier, like curse, coolness, damage, etc. to the item. Works for passives and actives.

            //Set some other fields
            item.consumable = false;
            item.quality = PickupObject.ItemQuality.C;
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Curse, 1f, StatModifier.ModifyMethod.ADDITIVE);
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
        }

        //Removes one heart from the player, gives them 1 armor
        protected override void DoEffect(PlayerController user)
        {
            float curHealth = user.healthHaver.GetCurrentHealth();
            if (curHealth > 1)
            {
                AkSoundEngine.PostEvent("Play_OBJ_poisonvial_shatter_01", base.gameObject);
                user.healthHaver.ForceSetCurrentHealth(curHealth - 1);
                float curseValue = user.stats.GetStatValue(PlayerStats.StatType.Curse);
                StatModifier statModifier = new StatModifier();
                statModifier.statToBoost = PlayerStats.StatType.Damage;
                statModifier.amount = 0.02f + (0.003f * curseValue);
                statModifier.modifyType = StatModifier.ModifyMethod.ADDITIVE;
                user.ownerlessStatModifiers.Add(statModifier);
                user.stats.RecalculateStats(user, true, false);
            }
        }

        //Disables the item if the player's health is less than or equal to 1 heart
        public override bool CanBeUsed(PlayerController user)
        {
            return user.healthHaver.GetCurrentHealth() > 1;
        }
    }
}