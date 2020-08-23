using UnityEngine;
using ItemAPI;
using Dungeonator;
using System.Collections;
using System;
using MonoMod.RuntimeDetour;
using System.Reflection;

namespace Blunderbeast
{
    public class BarrierAmmolet : BlankModificationItem
    {

        //Call this method from the Start() method of your ETGModule extension
        public static void Init()
        {
            //The name of the item
            string itemName = "Barrier Ammolet";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "Blunderbeast/Resources/barrierammolet";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<BarrierAmmolet>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Blanks Protect";
            string longDesc = "Blanks will create a protective shield.\n\n" +
                "This unusual amulet was once in the possession of the betrayer Blockner, who fashioned it to his liking.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "rtr");

            //Adds the actual passive effect to the item

            //Set the rarity of the item
            item.quality = PickupObject.ItemQuality.C;

            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.AdditionalBlanksPerFloor, 1f, StatModifier.ModifyMethod.ADDITIVE);

            //ID of the item if you need it to be used in other methods
            BarrierID = item.PickupObjectId;
        }

        private static int BarrierID;

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
        }

        private static bool onCooldown;


        private static Hook BlankHook = new Hook(
        typeof(SilencerInstance).GetMethod("ProcessBlankModificationItemAdditionalEffects", BindingFlags.Instance | BindingFlags.NonPublic),
        typeof(BarrierAmmolet).GetMethod("BlankModHook")
    );

        public static void BlankModHook(Action<SilencerInstance, BlankModificationItem, Vector2, PlayerController> orig, SilencerInstance silencer, BlankModificationItem bmi, Vector2 centerPoint, PlayerController user)
        {
            orig(silencer, bmi, centerPoint, user);

            if (user.HasPickupID(BarrierID))
            {
                if (onCooldown == false)
                {
                    Gun gun = PickupObjectDatabase.GetById(380) as Gun;
                    Gun currentGun = user.CurrentGun;
                    GameObject shield = gun.ObjectToInstantiateOnReload.gameObject;
                    GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(shield, user.sprite.WorldCenter, Quaternion.identity);
                    SingleSpawnableGunPlacedObject m_extantAmp = gameObject.GetInterface<SingleSpawnableGunPlacedObject>();
                    BreakableShieldController shieldOptions = gameObject.GetComponent<BreakableShieldController>();
                    if (gameObject)
                    {
                        m_extantAmp.Initialize(currentGun);
                        shieldOptions.Initialize(currentGun);
                        onCooldown = true;
                        GameManager.Instance.StartCoroutine(StartCooldown());
                    }
                }
            }
        }

        private static IEnumerator StartCooldown()
        {
            yield return new WaitForSeconds(5f);
            onCooldown = false;
            yield break;
        }


        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            return debrisObject;
        }
    }
}

