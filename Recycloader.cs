using ItemAPI;
using System.Collections;
using UnityEngine;

namespace Blunderbeast
{
    public class Recycloader : PlayerItem
    {
        public static void Init()
        {
            //The name of the item
            string itemName = "Recycloader";

            //Refers to an embedded png in the project. Make sure to embed your resources!
            string resourceName = "Blunderbeast/Resources/recycloader";

            //Create new GameObject
            GameObject obj = new GameObject();

            //Add a ActiveItem component to the object
            var item = obj.AddComponent<Recycloader>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Good For The Gunvironment";
            string longDesc = "Turns equipped gun's ammo into casings. Recycles empty guns into ammo.\n\n" +
                "A smaller, more docile member of the gun muncher species. This one prefers bullets served on the side.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //"example_pool" here is the item pool. In the console you'd type "give example_pool:sweating_bullets"
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "rtr");

            //Set the cooldown type and duration of the cooldown
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Timed, 0.5f);

            //Adds a passive modifier, like curse, coolness, damage, etc. to the item. Works for passives and actives.

            //Set some other fields
            item.consumable = false;
            item.quality = PickupObject.ItemQuality.B;
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
        }

        private IEnumerator SpewAmmo()
        {
            PlayerController user = this.LastOwner;
            yield return new WaitForSeconds(0.5f);
            LootEngine.SpawnItem(PickupObjectDatabase.GetById(78).gameObject, user.specRigidbody.UnitCenter, Vector2.left, 1f, false, true, false);
            LootEngine.SpawnItem(PickupObjectDatabase.GetById(78).gameObject, user.specRigidbody.UnitCenter, Vector2.right, 1f, false, true, false);
            yield break;
        }

        protected override void DoEffect(PlayerController user)
        {

            if (user.CurrentGun.CurrentAmmo == 0)
            {
                AkSoundEngine.PostEvent("Play_NPC_BabyDragun_Munch_01", base.gameObject);
                GameManager.Instance.StartCoroutine(SpewAmmo());
                user.inventory.DestroyCurrentGun();
                StatModifier statModifier = new StatModifier
                {
                    statToBoost = PlayerStats.StatType.ReloadSpeed,
                    amount = 0.98f,
                    modifyType = StatModifier.ModifyMethod.MULTIPLICATIVE
                };
                user.ownerlessStatModifiers.Add(statModifier);
                user.stats.RecalculateStats(user, true, false);
            }

            else
            {
                if (user.CharacterUsesRandomGuns)
                {
                    user.inventory.DestroyCurrentGun();
                    AkSoundEngine.PostEvent("Play_OBJ_lock_pick_01", base.gameObject);
                }

                else
                {
                    LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(68).gameObject, user);
                    LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(68).gameObject, user);
                    LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(68).gameObject, user);

                    AkSoundEngine.PostEvent("Play_OBJ_lock_pick_01", base.gameObject);

                    if (user.CurrentGun.CurrentAmmo >= 300)
                    {
                        user.CurrentGun.CurrentAmmo -= 300;
                    }
                    else
                    {
                        user.CurrentGun.CurrentAmmo = 0;
                    }
                }
            }
        }

        public override bool CanBeUsed(PlayerController user)
        {
            return user.CurrentGun != null && user.CurrentGun.CanActuallyBeDropped(user) && !user.CurrentGun.InfiniteAmmo && user.CurrentGun.DefaultModule.numberOfShotsInClip != 0 && user.CurrentGun.PickupObjectId != 476;
        }
    }
}
