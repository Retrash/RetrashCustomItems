using UnityEngine;
using ItemAPI;

namespace Blunderbeast
{
    public class Ouroboros : PlayerItem
    {
        public static void Init()
        {
            //The name of the item
            string itemName = "Ouroboros";

            //Refers to an embedded png in the project. Make sure to embed your resources!
            string resourceName = "Blunderbeast/Resources/ouroboros";

            //Create new GameObject
            GameObject obj = new GameObject();

            //Add a ActiveItem component to the object
            var item = obj.AddComponent<Ouroboros>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Cycle Of Infinity";
            string longDesc = "This fossilized serpent holds great power. Choose wisely which gun you wish to imbue with its energy.\n\n" +
                "A symbol of infinite potential, it poses a threat to Kaliber herself.";

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
            StatModifier statModifier = new StatModifier();
            statModifier.statToBoost = PlayerStats.StatType.Curse;
            statModifier.amount = 3f;
            statModifier.modifyType = StatModifier.ModifyMethod.ADDITIVE;
            user.ownerlessStatModifiers.Add(statModifier);
            user.CurrentGun.GainAmmo(user.CurrentGun.AdjustedMaxAmmo);
            user.CurrentGun.OnPostFired += OuroborosOnPostFired;
            user.CurrentGun.InfiniteAmmo = true;
            user.CurrentGun.CanGainAmmo = true;

            OnGunDamagedModifier onGunDamagedModifier = user.CurrentGun.gameObject.GetComponent<OnGunDamagedModifier>();
            if (onGunDamagedModifier != null)
            {
                onGunDamagedModifier.BrokenAnimation = null;
                onGunDamagedModifier.DepleteAmmoOnDamage = false;
            }
            user.stats.RecalculateStats(user, true, false);
        }

        public void OuroborosOnPostFired(PlayerController player, Gun gun)
        {
            if (gun.ClipShotsRemaining < 500)
            {
                gun.ammo += 1;
            }
        }

        protected Gun GetValidGun(PlayerController user, Gun excluded = null)
        {
            int num = user.inventory.AllGuns.IndexOf(user.CurrentGun);
            if (num < 0)
            {
                num = 0;
            }
            for (int i = num; i < num + user.inventory.AllGuns.Count; i++)
            {
                int index = i % user.inventory.AllGuns.Count;
                Gun gun = user.inventory.AllGuns[index];
                if (!gun.InfiniteAmmo && gun.CanActuallyBeDropped(user))
                {
                    if (!(gun == excluded))
                    {
                        return gun;
                    }
                }
            }
            return null;
        }

        protected bool IsGunValid(Gun g, Gun excluded)
        {
            return !g.InfiniteAmmo && g.CanActuallyBeDropped(g.CurrentOwner as PlayerController) && !(g == excluded);
        }

        public override bool CanBeUsed(PlayerController user)
        {
            return user.CurrentGun != null && user.CurrentGun.CanActuallyBeDropped(user) && !user.CurrentGun.InfiniteAmmo && user.CurrentGun.DefaultModule.numberOfShotsInClip != 0;
        }
    }
}