using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Gungeon;
using UnityEngine;

namespace ItemAPI
{
    // Token: 0x0200001D RID: 29
    public static class ItemBuilder
    {
        public static void Init()
        {
            FakePrefabHooks.Init();
            try
            {
                MethodBase method = new StackFrame(1, false).GetMethod();
                Type declaringType = method.DeclaringType;
                ResourceExtractor.SetAssembly(declaringType);
            }
            catch (Exception ex)
            {
                ETGModConsole.Log(ex.Message, false);
                ETGModConsole.Log(ex.StackTrace, false);
            }
        }

        public static GameObject AddSpriteToObject(string name, string resourcePath, GameObject obj = null, bool copyFromExisting = true)
        {
            GameObject gameObject = SpriteBuilder.SpriteFromResource(resourcePath, obj, copyFromExisting);
            FakePrefab.MarkAsFakePrefab(gameObject);
            obj.SetActive(false);
            gameObject.name = name;
            return gameObject;
        }

        public static void SetupItem(PickupObject item, string shortDesc, string longDesc, string idPool = "customItems")
        {
            try
            {
                item.encounterTrackable = null;
                ETGMod.Databases.Items.SetupItem(item, item.name);
                SpriteBuilder.AddToAmmonomicon(item.sprite.GetCurrentSpriteDef());
                item.encounterTrackable.journalData.AmmonomiconSprite = item.sprite.GetCurrentSpriteDef().name;
                GunExt.SetName(item, item.name);
                GunExt.SetShortDescription(item, shortDesc);
                GunExt.SetLongDescription(item, longDesc);
                bool flag = item is PlayerItem;
                if (flag)
                {
                    (item as PlayerItem).consumable = false;
                }
                Game.Items.Add(idPool + ":" + item.name.ToLower().Replace(" ", "_"), item);
                ETGMod.Databases.Items.Add(item, false, "ANY");
            }
            catch (Exception ex)
            {
                ETGModConsole.Log(ex.Message, false);
                ETGModConsole.Log(ex.StackTrace, false);
            }
        }

        public static void SetCooldownType(PlayerItem item, ItemBuilder.CooldownType cooldownType, float value)
        {
            item.damageCooldown = -1f;
            item.roomCooldown = -1;
            item.timeCooldown = -1f;
            switch (cooldownType)
            {
                case ItemBuilder.CooldownType.Timed:
                    item.timeCooldown = value;
                    break;
                case ItemBuilder.CooldownType.Damage:
                    item.damageCooldown = value;
                    break;
                case ItemBuilder.CooldownType.PerRoom:
                    item.roomCooldown = (int)value;
                    break;
            }
        }

        public static void AddPassiveStatModifier(PickupObject po, PlayerStats.StatType statType, float amount, StatModifier.ModifyMethod method = StatModifier.ModifyMethod.ADDITIVE)
        {
            StatModifier statModifier = new StatModifier();
            statModifier.amount = amount;
            statModifier.statToBoost = statType;
            statModifier.modifyType = method;
            bool flag = po is PlayerItem;
            if (flag)
            {
                PlayerItem playerItem = po as PlayerItem;
                bool flag2 = playerItem.passiveStatModifiers == null;
                if (flag2)
                {
                    playerItem.passiveStatModifiers = new StatModifier[]
                    {
                        statModifier
                    };
                }
                else
                {
                    playerItem.passiveStatModifiers = playerItem.passiveStatModifiers.Concat(new StatModifier[]
                    {
                        statModifier
                    }).ToArray<StatModifier>();
                }
            }
            else
            {
                bool flag3 = po is PassiveItem;
                if (!flag3)
                {
                    throw new NotSupportedException("Object must be of type PlayerItem or PassiveItem");
                }
                PassiveItem passiveItem = po as PassiveItem;
                bool flag4 = passiveItem.passiveStatModifiers == null;
                if (flag4)
                {
                    passiveItem.passiveStatModifiers = new StatModifier[]
                    {
                        statModifier
                    };
                }
                else
                {
                    passiveItem.passiveStatModifiers = passiveItem.passiveStatModifiers.Concat(new StatModifier[]
                    {
                        statModifier
                    }).ToArray<StatModifier>();
                }
            }
        }

        public static IEnumerator HandleDuration(PlayerItem item, float duration, PlayerController user, Action<PlayerController> OnFinish)
        {
            bool isCurrentlyActive = item.IsCurrentlyActive;
            if (isCurrentlyActive)
            {
                yield break;
            }
            ItemBuilder.SetPrivateType<PlayerItem>(item, "m_isCurrentlyActive", true);
            ItemBuilder.SetPrivateType<PlayerItem>(item, "m_activeElapsed", 0f);
            ItemBuilder.SetPrivateType<PlayerItem>(item, "m_activeDuration", duration);
            Action<PlayerItem> onActivationStatusChanged = item.OnActivationStatusChanged;
            if (onActivationStatusChanged != null)
            {
                onActivationStatusChanged(item);
            }
            float elapsed = ItemBuilder.GetPrivateType<PlayerItem, float>(item, "m_activeElapsed");
            float dur = ItemBuilder.GetPrivateType<PlayerItem, float>(item, "m_activeDuration");
            while (ItemBuilder.GetPrivateType<PlayerItem, float>(item, "m_activeElapsed") < ItemBuilder.GetPrivateType<PlayerItem, float>(item, "m_activeDuration") && item.IsCurrentlyActive)
            {
                yield return null;
            }
            ItemBuilder.SetPrivateType<PlayerItem>(item, "m_isCurrentlyActive", false);
            Action<PlayerItem> onActivationStatusChanged2 = item.OnActivationStatusChanged;
            if (onActivationStatusChanged2 != null)
            {
                onActivationStatusChanged2(item);
            }
            if (OnFinish != null)
            {
                OnFinish(user);
            }
            yield break;
        }

        private static void SetPrivateType<T>(T obj, string field, bool value)
        {
            FieldInfo field2 = typeof(T).GetField(field, BindingFlags.Instance | BindingFlags.NonPublic);
            field2.SetValue(obj, value);
        }

        private static void SetPrivateType<T>(T obj, string field, float value)
        {
            FieldInfo field2 = typeof(T).GetField(field, BindingFlags.Instance | BindingFlags.NonPublic);
            field2.SetValue(obj, value);
        }

        private static T2 GetPrivateType<T, T2>(T obj, string field)
        {
            FieldInfo field2 = typeof(T).GetField(field, BindingFlags.Instance | BindingFlags.NonPublic);
            return (T2)((object)field2.GetValue(obj));
        }

        public enum CooldownType
        {
            Timed,
            Damage,
            PerRoom,
            None
        }
    }
}