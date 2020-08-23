using System;
using UnityEngine;
using ItemAPI;

namespace Blunderbeast
{
    public class Raccoon : PassiveItem
    {

        //Call this method from the Start() method of your ETGModule extension
        public static void Init()
        {
            //The name of the item
            string itemName = "Raccoon";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "Blunderbeast/Resources/raccoon";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<Raccoon>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Junk Collector";
            string longDesc = "Provides safe transport across trap rooms if junk is dropped in exchange.\n\n" +
                "In another era, this Raccoon was a very successful thief. However, after dabbling in time travel, he became obsessed and put an end to his thieving career in favor of the infinite treasure of the Gungeon.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "rtr");

            //Adds the actual passive effect to the item

            //Set the rarity of the item
            item.quality = PickupObject.ItemQuality.D;
            RaccoonBehavior.behaviorSpeculator.OverrideBehaviors.Add(new ChaosRaccoonManager());
        }

        private GameObject m_extantCompanion;

        public static AIActor RaccoonBehavior = EnemyDatabase.GetOrLoadByName("raccoon");

        public override void Pickup(PlayerController player)
        {
           
            base.Pickup(player);

            player.OnNewFloorLoaded = (Action<PlayerController>)Delegate.Combine(player.OnNewFloorLoaded, new Action<PlayerController>(this.HandleNewFloor));
            SpawnRaccoon(player);
        }

        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            this.DestroyRaccoon();
            return debrisObject;
        }

        private void SpawnRaccoon(PlayerController player)
        {
            AIActor orLoadByName = EnemyDatabase.GetOrLoadByName("raccoon");
            Vector3 vector = player.transform.position;
            if (GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.FOYER)
            {
                vector += new Vector3(1.125f, -0.3125f, 0f);
            }
            GameObject extantCompanion2 = UnityEngine.Object.Instantiate<GameObject>(orLoadByName.gameObject, vector, Quaternion.identity);
            this.m_extantCompanion = extantCompanion2;
            CompanionController orAddComponent = this.m_extantCompanion.GetOrAddComponent<CompanionController>();
            orAddComponent.Initialize(player);
            if (orAddComponent.specRigidbody)
            {
                PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(orAddComponent.specRigidbody, null, false);
            }
        }

        private void DestroyRaccoon()
        {
            if (!this.m_extantCompanion)
            {
                return;
            }
            UnityEngine.Object.Destroy(this.m_extantCompanion);
            this.m_extantCompanion = null;
        }

        private void HandleNewFloor(PlayerController obj)
        {
            this.DestroyRaccoon();
            if (!this.PreventRespawnOnFloorLoad)
            {
                this.SpawnRaccoon(obj);
            }
        }

        public bool PreventRespawnOnFloorLoad;
    }
}

