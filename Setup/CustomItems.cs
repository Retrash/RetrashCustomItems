using ItemAPI;
using MonoMod.RuntimeDetour;
using System;
using System.Linq;
using System.Reflection;

namespace Blunderbeast
{
    public class CustomItems : ETGModule
    {
        public override void Init()
        {
        }

        public override void Start()
        {
            FakePrefabHooks.Init();
            ItemBuilder.Init();
            Blunderbeastblight.Init();
            Beastbloodinjection.Init();
            Panacea.Init();
            Vampirecloak.Init();
            Tabletechstealth.Init();
            Glasssmelter.Init();
            Heartshield.Init();
            Recycloader.Init();
            Dodgicitering.Init();
            Tinybullets.Init();
            Boombox.Init();
            Challengerbelt.Init();
            CrackedEgg.Init();
            LockBullets.Init();
            UnlockedBullets.Init();
            MatryoshkaChest.Init();
            Raccoon.Init();
            BarrelCharger.Init();
            TwinPins.Init();
            Ouroboros.Init();
            MaledictionRounds.Init();
            CrystalBall.Init();
            BlankSpellbook.Init();
            CounterfeitCrown.Init();
            InfinityGuontletPassive.Init();
            DisciplineRing.Init();
            Invoker.Init();
            MindControlDevice.Init();
            SharpGuon.Init();
            Roshambo.Init();
            LootBox.Init();
            WarVase.Init();
            WishCoupon.Init();
            GravityGlove.Init();          
            OrnatePistol.Add();
            GravityDisc.Add();
            Leafblower.Add();
            Artemis.Add();
            Alchemiser.Add();
            Craftsman.Add();
            TrapCard.Init();
            BarrierAmmolet.Init();
            NOU.Init();
            Infinigun.Add();
            RageRifle.Add();
            BrittleArmor.Init();
            FragGrenade.Init();
            IcySkull.Init();
            RepositoryDamned.Init();

            //JOKE ITEMS
            BigRedButtonGag.Init();
            GrenadeOnAStick.Add();

            Tools.Print<string>("Retrash's Custom Items Collection v" + CustomItems.version, "FFA500", true);

            //CUSTOM SHOP POOLS SETUP
            ShopTool.Init();

            //SYNERGY SETUP

            Hook synergyStringHook = new Hook(
                typeof(StringTableManager).GetMethod("GetSynergyString", BindingFlags.Public | BindingFlags.Static),
                typeof(CustomItems).GetMethod("SynergyStringHook")
            );


            foreach (AdvancedSynergyEntry synergy in GameManager.Instance.SynergyManager.synergies)
            {

                if (synergy.NameKey == "#RECYCLING")
                {
                    synergy.OptionalItemIDs.Add(ETGMod.Databases.Items["Recycloader"].PickupObjectId);

                    if (synergy.MandatoryGunIDs.Contains(507))
                    {
                        synergy.MandatoryGunIDs.Remove(507);
                    }
                    if (!synergy.OptionalItemIDs.Contains(507))
                    {
                        synergy.OptionalItemIDs.Add(507);
                    }
                }

                if (synergy.NameKey == "#MINORBLANKABLES1")
                {
                    synergy.OptionalItemIDs.Add(ETGMod.Databases.Items["Barrier Ammolet"].PickupObjectId);
                }

                if (synergy.NameKey == "#RELODESTAR")
                {
                    synergy.OptionalItemIDs.Add(ETGMod.Databases.Items["Barrier Ammolet"].PickupObjectId);
                }

                if (synergy.NameKey == "#CRISISROCK")
                {
                    synergy.OptionalItemIDs.Add(ETGMod.Databases.Items["Barrier Ammolet"].PickupObjectId);

                    if (synergy.MandatoryItemIDs.Contains(634))
                    {
                        synergy.MandatoryItemIDs.Remove(634);
                    }
                    if (!synergy.OptionalItemIDs.Contains(634))
                    {
                        synergy.OptionalItemIDs.Add(634);
                    }
                }

                if (synergy.NameKey == "#CEREBRAL_BROS")
                {
                    synergy.OptionalItemIDs.Add(ETGMod.Databases.Items["Mind Control Device"].PickupObjectId);
                }

                if (synergy.NameKey == "#PAPERWORK")
                {
                    synergy.OptionalItemIDs.Add(ETGMod.Databases.Items["Table Tech Stealth"].PickupObjectId);
                }

                if (synergy.NameKey == "#TWOEGGS")
                {
                    synergy.OptionalItemIDs.Add(ETGMod.Databases.Items["Crackling Egg"].PickupObjectId);

                    if (synergy.MandatoryItemIDs.Contains(637))
                    {
                        synergy.MandatoryItemIDs.Remove(637);
                    }
                    if (!synergy.OptionalItemIDs.Contains(637))
                    {
                        synergy.OptionalItemIDs.Add(637);
                    }
                }

                if (synergy.NameKey == "#LEAFBUSTER")
                {
                    synergy.OptionalGunIDs.Add(PickupObjectDatabase.GetByEncounterName("Verdant Blaster").PickupObjectId);

                    if (synergy.MandatoryGunIDs.Contains(339))
                    {
                        synergy.MandatoryGunIDs.Remove(339);
                    }
                    if (!synergy.OptionalGunIDs.Contains(339))
                    {
                        synergy.OptionalGunIDs.Add(339);
                    }
                }

                if (synergy.NameKey == "#IDOLHANDS")
                {
                    
                    synergy.OptionalItemIDs.Add(PickupObjectDatabase.GetByEncounterName("Exalted Armbow").PickupObjectId);

                    if (synergy.MandatoryItemIDs.Contains(457))
                    {
                        synergy.MandatoryItemIDs.Remove(457);
                    }
                    if (!synergy.OptionalItemIDs.Contains(457))
                    {
                        synergy.OptionalItemIDs.Add(457);
                    }
                }

                if (synergy.NameKey == "#YETIDUNK")
                {

                    synergy.OptionalGunIDs.Add(PickupObjectDatabase.GetByEncounterName("Icy Skull").PickupObjectId);

                    if (synergy.MandatoryGunIDs.Contains(387))
                    {
                        synergy.MandatoryGunIDs.Remove(387);
                    }
                    if (!synergy.OptionalGunIDs.Contains(387))
                    {
                        synergy.OptionalGunIDs.Add(387);
                    }

                    if (synergy.MandatoryGunIDs.Contains(223))
                    {
                        synergy.MandatoryGunIDs.Remove(223);
                    }
                    if (!synergy.OptionalGunIDs.Contains(223))
                    {
                        synergy.OptionalGunIDs.Add(223);
                    }
                }
            }


            GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[] { new CustomSynergies.VeryHungrySynergy() }).ToArray();
            GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[] { new CustomSynergies.SmeltingHardSynergy() }).ToArray();
            GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[] { new CustomSynergies.TheTinyAndBigSynergy() }).ToArray();
            GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[] { new CustomSynergies.StableDodgeciteRingSynergy() }).ToArray();
            GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[] { new CustomSynergies.SmallAndStrong() }).ToArray();
            GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[] { new CustomSynergies.ShopkeeperCapsuleSynergy() }).ToArray();
            GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[] { new CustomSynergies.DeadlyBulletsSynergy() }).ToArray();
            GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[] { new CustomSynergies.ChestFamilySynergy() }).ToArray();
            GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[] { new CustomSynergies.StormChargedSynergy() }).ToArray();
            GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[] { new CustomSynergies.TheGoodSynergy() }).ToArray();
            GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[] { new CustomSynergies.TheBadSynergy() }).ToArray();
            GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[] { new CustomSynergies.SoulFiendSynergy() }).ToArray();
            GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[] { new CustomSynergies.MagicTablesSynergy() }).ToArray();
            GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[] { new CustomSynergies.BlankEnchanterSynergy() }).ToArray();
            GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[] { new CustomSynergies.EvenSharperSynergy() }).ToArray();
            GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[] { new CustomSynergies.WarBarrelSynergy() }).ToArray();
            GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[] { new CustomSynergies.FourthWishSynergy() }).ToArray();
            GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[] { new CustomSynergies.TrapperCardSynergy() }).ToArray();
            GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[] { new CustomSynergies.YesRSynergy() }).ToArray();
            GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[] { new CustomSynergies.InfinityGuontletSynergy() }).ToArray();
            GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[] { new CustomSynergies.TheUglySynergy() }).ToArray();
            GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[] { new CustomSynergies.ArtemisSynergy() }).ToArray();
            GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[] { new CustomSynergies.AlchemySynergy() }).ToArray();
            GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[] { new CustomSynergies.CardHeartSynergy() }).ToArray();
            GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[] { new CustomSynergies.GForceSynergy() }).ToArray();
            GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[] { new CustomSynergies.GreedyJarsSynergy() }).ToArray();
            GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[] { new CustomSynergies.EggRollSynergy() }).ToArray();
            GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[] { new CustomSynergies.FutureSightSynergy() }).ToArray();
            GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[] { new CustomSynergies.TigerGenieSynergy() }).ToArray();
            GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[] { new CustomSynergies.SoulTriggerSynergy() }).ToArray();
            GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[] { new CustomSynergies.IceAgeSynergy() }).ToArray();
            GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[] { new CustomSynergies.GoldenRatioSynergy() }).ToArray();
            GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[] { new CustomSynergies.AngerIssuesSynergy() }).ToArray();
            GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[] { new CustomSynergies.AbsoluteChaosSynergy() }).ToArray();
            GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[] { new CustomSynergies.ArmorMaintenanceSynergy() }).ToArray();
            GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[] { new CustomSynergies.FormerGlorySynergy() }).ToArray();
            GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[] { new CustomSynergies.RockPaperCrossBow() }).ToArray();
            GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[] { new CustomSynergies.BatterUpSynergy() }).ToArray();
            GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[] { new CustomSynergies.ChuckingNadesSynergy() }).ToArray();
            GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[] { new CustomSynergies.RocketPitchSynergy() }).ToArray();
            GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[] { new CustomSynergies.FreezePlusSynergy() }).ToArray();
            GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[] { new CustomSynergies.FrozenCoreSynergy() }).ToArray();
            GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[] { new CustomSynergies.BoneheadSynergy() }).ToArray();
            GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[] { new CustomSynergies.ToolKitSynergy() }).ToArray();
            GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[] { new CustomSynergies.PotceptionSynergy() }).ToArray();
            GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[] { new CustomSynergies.AntiquatedSynergy() }).ToArray();
            GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[] { new CustomSynergies.DendrologySynergy() }).ToArray();
            GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[] { new CustomSynergies.PlagueDoctorSynergy() }).ToArray();
            GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[] { new CustomSynergies.ControllerSynergy() }).ToArray();
            GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[] { new CustomSynergies.ChickenSeerSynergy() }).ToArray();


            SynergyFormInitialiser.AddSynergyForms();
        }


        public static string SynergyStringHook(Func<string, int, string> action, string key, int index = -1)
        {
            string value = action(key, index);
            if (string.IsNullOrEmpty(value))
            {
                value = key;
            }
            return value;
        }

        public bool isRetrashCollection;

        private static string version = "5.0.1";

        public override void Exit()
        {
        }
    }
}
