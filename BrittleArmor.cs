using UnityEngine;
using ItemAPI;
using Dungeonator;
using System.Collections;
using System.Text;
using System;

namespace Blunderbeast
{
    public class BrittleArmor : PassiveItem
    {

        //Call this method from the Start() method of your ETGModule extension
        public static void Init()
        {
            //The name of the item
            string itemName = "Rusted Armor";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "Blunderbeast/Resources/brittlearmor";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<BrittleArmor>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Old and Worn Down";
            string longDesc = "Prevents incoming damage three times before shattering.\n\n" +
                "Knightly armor that strangely seems much older than the Gungeon itself. It has no doubt seen its fair share of adventure, give it a good send-off.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "rtr");

            //Adds the actual passive effect to the item

            //Set the rarity of the item
            item.quality = PickupObject.ItemQuality.D;
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            HealthHaver healthHaver = player.healthHaver;
            healthHaver.ModifyDamage = (Action<HealthHaver, HealthHaver.ModifyDamageEventArgs>)Delegate.Combine(healthHaver.ModifyDamage, new Action<HealthHaver, HealthHaver.ModifyDamageEventArgs>(this.HandleEffect));
        }

        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            HealthHaver healthHaver = player.healthHaver;
            healthHaver.ModifyDamage = (Action<HealthHaver, HealthHaver.ModifyDamageEventArgs>)Delegate.Remove(healthHaver.ModifyDamage, new Action<HealthHaver, HealthHaver.ModifyDamageEventArgs>(this.HandleEffect));
            return debrisObject;
        }

        protected override void OnDestroy()
        {
            if (Owner)
            {
                HealthHaver healthHaver = Owner.healthHaver;
                healthHaver.ModifyDamage = (Action<HealthHaver, HealthHaver.ModifyDamageEventArgs>)Delegate.Remove(healthHaver.ModifyDamage, new Action<HealthHaver, HealthHaver.ModifyDamageEventArgs>(this.HandleEffect));
                Owner.DropPassiveItem(this);
            }
            base.OnDestroy();
        }

        protected override void Update()
        {
            if (Owner)
            {
                if ((damageInstances >= 3 && !Owner.HasPickupID(450)) || (damageInstances >= 5 && Owner.HasPickupID(450)))
                {
                    AkSoundEngine.PostEvent("Play_obj_toilet_break_01", base.gameObject);
                    if (Owner.HasPickupID(384) && ((Owner.HasPickupID(267) || Owner.HasPickupID(160) || Owner.HasPickupID(161) || Owner.HasPickupID(162))))
                    {
                        LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(163).gameObject, Owner);
                    }
                    UnityEngine.Object.Destroy(base.gameObject, 0f);
                }
            }
        }

        private int damageInstances;

        private void HandleEffect(HealthHaver source, HealthHaver.ModifyDamageEventArgs args)
        {
            if (args == EventArgs.Empty)
            {
                return;
            }

            if (args.ModifiedDamage <= 0f)
            {
                return;
            }

            if (!source.IsVulnerable)
            {
                return;
            }

            if (Owner)
            {
                source.StartCoroutine("IncorporealityOnHit");
                source.TriggerInvulnerabilityPeriod(-1f);
                args.ModifiedDamage = 0f;
                damageInstances += 1;
                DoMicroBlank((!source.specRigidbody) ? source.transform.position.XY() : source.specRigidbody.UnitCenter);
            }
        }

        private void DoMicroBlank(Vector2 center)
        {
            PlayerController player = this.Owner;
            GameObject silencerVFX = (GameObject)ResourceCache.Acquire("Global VFX/BlankVFX_Ghost");
            AkSoundEngine.PostEvent("Play_ITM_Crisis_Stone_Impact_01", base.gameObject);
            GameObject gameObject = new GameObject("silencer");
            SilencerInstance silencerInstance = gameObject.AddComponent<SilencerInstance>();
            float additionalTimeAtMaxRadius = 0.25f;
            silencerInstance.TriggerSilencer(center, 20f, 3f, silencerVFX, 0f, 3f, 3f, 3f, 30f, 3f, additionalTimeAtMaxRadius, player, true, false);
        }

    }
}

