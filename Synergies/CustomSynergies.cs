using Dungeonator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blunderbeast
{
    class CustomSynergies
    {

        public class VeryHungrySynergy : AdvancedSynergyEntry
        {
            public VeryHungrySynergy()
            {
                this.NameKey = "The Crimson Stone";
                this.MandatoryItemIDs = new List<int>
                {
                    ETGMod.Databases.Items["Vampire Cloak"].PickupObjectId,
                    285
                };
                this.IgnoreLichEyeBullets = true;
                this.statModifiers = new List<StatModifier>(0);
                this.bonusSynergies = new List<CustomSynergyType>();
            }
        }

        public class SmeltingHardSynergy : AdvancedSynergyEntry
        {
            public SmeltingHardSynergy()
            {
                this.NameKey = "Optimized Production";
                this.MandatoryItemIDs = new List<int>
                {
                    ETGMod.Databases.Items["Glass Smelter"].PickupObjectId
                };
                this.MandatoryGunIDs = new List<int>
                {
                    152
                };
                this.IgnoreLichEyeBullets = true;
                this.statModifiers = new List<StatModifier>(0);
                this.bonusSynergies = new List<CustomSynergyType>();
            }
        }

        public class TheTinyAndBigSynergy : AdvancedSynergyEntry
        {
            public TheTinyAndBigSynergy()
            {
                this.NameKey = "K & J";
                this.MandatoryItemIDs = new List<int>
                {
                    ETGMod.Databases.Items["Tiny Bullets"].PickupObjectId,
                };

                this.MandatoryGunIDs = new List<int>
                {
                    180
                };
                this.IgnoreLichEyeBullets = true;
                this.statModifiers = new List<StatModifier>(0);
                this.bonusSynergies = new List<CustomSynergyType>();
            }
        }

        public class InfinityGuontletSynergy : AdvancedSynergyEntry
        {
            public InfinityGuontletSynergy()
            {
                this.NameKey = "As All Things Should Be";
                this.MandatoryItemIDs = new List<int>
                {
                    ETGMod.Databases.Items["Infinity Guontlet"].PickupObjectId,
                    260,
                    262,
                    263,
                    264,
                    269,
                    270,
                    466
                };
                this.IgnoreLichEyeBullets = true;
                this.statModifiers = new List<StatModifier>(0);
                this.bonusSynergies = new List<CustomSynergyType>();
            }
        }


        public class StableDodgeciteRingSynergy : AdvancedSynergyEntry
        {
            public StableDodgeciteRingSynergy()
            {
                this.NameKey = "Dodgicite Alloy";
                this.MandatoryItemIDs = new List<int>
                {
                    ETGMod.Databases.Items["Unstable Dodgicite Ring"].PickupObjectId,
                    190
                };
                this.IgnoreLichEyeBullets = true;
                this.statModifiers = new List<StatModifier>(0);
                this.bonusSynergies = new List<CustomSynergyType>();
            }
        }

        public class SmallAndStrong : AdvancedSynergyEntry
        {
            public SmallAndStrong()
            {
                this.NameKey = "Biggie Smalls";
                this.MandatoryItemIDs = new List<int>
                {
                    ETGMod.Databases.Items["Tiny Bullets"].PickupObjectId,
                    277
                };
                this.IgnoreLichEyeBullets = true;
                this.statModifiers = new List<StatModifier>(0);
                this.bonusSynergies = new List<CustomSynergyType>();
            }
        }

        public class ShopkeeperCapsuleSynergy : AdvancedSynergyEntry
        {
            public ShopkeeperCapsuleSynergy()
            {
                this.NameKey = "Bello's Secret Stash";
                this.MandatoryItemIDs = new List<int>
                {
                    ETGMod.Databases.Items["Loot Capsule"].PickupObjectId,
                    476
                };
                this.IgnoreLichEyeBullets = true;
                this.statModifiers = new List<StatModifier>(0);
                this.bonusSynergies = new List<CustomSynergyType>();
            }
        }

        public class DeadlyBulletsSynergy : AdvancedSynergyEntry
        {
            public DeadlyBulletsSynergy()
            {
                this.NameKey = "Unlocked";
                this.MandatoryItemIDs = new List<int>
                {
                    166
                };

                this.OptionalItemIDs = new List<int>
                {
                    ETGMod.Databases.Items["Deadly Bullets"].PickupObjectId,
                    ETGMod.Databases.Items["Locked Bullets"].PickupObjectId,
                };

                
                this.IgnoreLichEyeBullets = true;
                this.statModifiers = new List<StatModifier>(0);
                this.bonusSynergies = new List<CustomSynergyType>();
            }
        }

        public class ChestFamilySynergy : AdvancedSynergyEntry
        {
            public ChestFamilySynergy()
            {
                this.NameKey = "Nested Knowledge";
                this.MandatoryItemIDs = new List<int>
                {
                    ETGMod.Databases.Items["Nesting Chest"].PickupObjectId,
                    487
                };
                this.IgnoreLichEyeBullets = true;
                this.statModifiers = new List<StatModifier>(0);
                this.bonusSynergies = new List<CustomSynergyType>();
            }
        }

        public class StormChargedSynergy : AdvancedSynergyEntry
        {
            public StormChargedSynergy()
            {
                this.NameKey = "Lightning Discharge";
                this.MandatoryItemIDs = new List<int>
                {
                    ETGMod.Databases.Items["Storm Charger"].PickupObjectId
                };
                this.OptionalGunIDs = new List<int>
                {
                    13,
                    153
                };
                this.IgnoreLichEyeBullets = true;
                this.statModifiers = new List<StatModifier>(0);
                this.bonusSynergies = new List<CustomSynergyType>();
            }
        }

        public class TheGoodSynergy : AdvancedSynergyEntry
        {
            public TheGoodSynergy()
            {
                this.NameKey = "The Good";
                this.MandatoryItemIDs = new List<int>
                {
                    ETGMod.Databases.Items["Twin Pins"].PickupObjectId,
                    35
                };
                this.IgnoreLichEyeBullets = true;
                this.statModifiers = new List<StatModifier>(0);
                this.bonusSynergies = new List<CustomSynergyType>();
            }
        }

        public class TheBadSynergy : AdvancedSynergyEntry
        {
            public TheBadSynergy()
            {
                this.NameKey = "The Bad";
                this.MandatoryItemIDs = new List<int>
                {
                    ETGMod.Databases.Items["Twin Pins"].PickupObjectId,
                    22
                };
                this.IgnoreLichEyeBullets = true;
                this.statModifiers = new List<StatModifier>(0);
                this.bonusSynergies = new List<CustomSynergyType>();
            }
        }

        public class TheUglySynergy : AdvancedSynergyEntry
        {
            public TheUglySynergy()
            {
                this.NameKey = "The Ugly";
                this.MandatoryItemIDs = new List<int>
                {
                    ETGMod.Databases.Items["Twin Pins"].PickupObjectId,
                    148
                };

                this.IgnoreLichEyeBullets = true;
                this.statModifiers = new List<StatModifier>(0);
                this.bonusSynergies = new List<CustomSynergyType>();
            }
        }

        public class ArtemisSynergy : AdvancedSynergyEntry
        {
            public ArtemisSynergy()
            {
                this.NameKey = "Heaven and Hell";
                this.MandatoryGunIDs = new List<int>
                {
                    PickupObjectDatabase.GetByEncounterName("Exalted Armbow").PickupObjectId
                };
                this.MandatoryItemIDs = new List<int>
                {
                    538
                };
                this.OptionalItemIDs = new List<int>
                {
                    407,
                    631
                };

                this.NumberObjectsRequired = 3;
                this.RequiresAtLeastOneGunAndOneItem = true;
                this.IgnoreLichEyeBullets = false;
                this.statModifiers = new List<StatModifier>(0);
                this.bonusSynergies = new List<CustomSynergyType>();
                
            }
        }

        public class AlchemySynergy : AdvancedSynergyEntry
        {
            public AlchemySynergy()
            {
                this.NameKey = "Overflowing Vial";
                this.MandatoryGunIDs = new List<int>
                {
                    PickupObjectDatabase.GetByEncounterName("Alchemical Gun").PickupObjectId                  
                };

                this.OptionalItemIDs = new List<int>
                {
                    205,
                    313
                };

                this.IgnoreLichEyeBullets = false;
                this.statModifiers = new List<StatModifier>(0);
                this.bonusSynergies = new List<CustomSynergyType>();
            }
        }

        public class SoulFiendSynergy : AdvancedSynergyEntry
        {
            public SoulFiendSynergy()
            {
                this.NameKey = "Reaper Of Souls";
                this.MandatoryItemIDs = new List<int>
                {
                    ETGMod.Databases.Items["Malediction Rounds"].PickupObjectId,
                    571
                };
                this.IgnoreLichEyeBullets = true;
                this.statModifiers = new List<StatModifier>(0);
                this.bonusSynergies = new List<CustomSynergyType>();
            }
        }

        public class MagicTablesSynergy : AdvancedSynergyEntry
        {
            public MagicTablesSynergy()
            {
                this.NameKey = "Hidden Tech Magic";
                this.MandatoryItemIDs = new List<int>
                {
                    ETGMod.Databases.Items["Blank Spellbook"].PickupObjectId,
                    400
                };
                this.IgnoreLichEyeBullets = true;
                this.statModifiers = new List<StatModifier>(0);
                this.bonusSynergies = new List<CustomSynergyType>();
            }
        }

        public class BlankEnchanterSynergy : AdvancedSynergyEntry
        {
            public BlankEnchanterSynergy()
            {
                this.NameKey = "Blank Magician";
                this.MandatoryItemIDs = new List<int>
                {
                    ETGMod.Databases.Items["Blank Spellbook"].PickupObjectId
                };
                this.OptionalGunIDs = new List<int>
                {
                    145,
                    61
                };
                this.IgnoreLichEyeBullets = true;
                this.statModifiers = new List<StatModifier>(0);
                this.bonusSynergies = new List<CustomSynergyType>();
            }
        }

        public class EvenSharperSynergy : AdvancedSynergyEntry
        {
            public EvenSharperSynergy()
            {
                this.NameKey = "Bleeding Edge";
                this.MandatoryItemIDs = new List<int>
                {
                    ETGMod.Databases.Items["Colorless Guon Stone"].PickupObjectId
                };
                this.OptionalItemIDs = new List<int>
                {
                    822,
                    457
                };
                this.IgnoreLichEyeBullets = true;
                this.statModifiers = new List<StatModifier>(0);
                this.bonusSynergies = new List<CustomSynergyType>();
            }
        }

        public class WarBarrelSynergy : AdvancedSynergyEntry
        {
            public WarBarrelSynergy()
            {
                this.NameKey = "Fish Bowl";
                this.MandatoryItemIDs = new List<int>
                {
                    ETGMod.Databases.Items["War Vase"].PickupObjectId,
                    
                };
                this.OptionalGunIDs = new List<int>
                {
                    7
                };

                this.IgnoreLichEyeBullets = true;
                this.statModifiers = new List<StatModifier>(0);
                this.bonusSynergies = new List<CustomSynergyType>();
            }
        }

        public class FourthWishSynergy : AdvancedSynergyEntry
        {
            public FourthWishSynergy()
            {
                this.NameKey = "Third Time's A Charm";
                this.MandatoryItemIDs = new List<int>
                {
                    ETGMod.Databases.Items["Wish Charm"].PickupObjectId,
                    0
                };
                this.IgnoreLichEyeBullets = true;
                this.statModifiers = new List<StatModifier>(0);
                this.bonusSynergies = new List<CustomSynergyType>();
            }
        }

        public class TrapperCardSynergy : AdvancedSynergyEntry
        {
            public TrapperCardSynergy()
            {
                this.NameKey = "No D-ice";
                this.MandatoryItemIDs = new List<int>
                {
                    ETGMod.Databases.Items["Trap Card"].PickupObjectId,

                };
                this.OptionalItemIDs = new List<int>
                {
                    109,
                    170,
                    364
                };

                this.IgnoreLichEyeBullets = true;
                this.statModifiers = new List<StatModifier>(0);
                this.bonusSynergies = new List<CustomSynergyType>();
            }
        }

        public class YesRSynergy : AdvancedSynergyEntry
        {
            public YesRSynergy()
            {
                this.NameKey = "u r not okay";
                this.MandatoryItemIDs = new List<int>
                {
                    ETGMod.Databases.Items["No-U"].PickupObjectId,
                    340
                };
                this.IgnoreLichEyeBullets = true;
                this.statModifiers = new List<StatModifier>(0);
                this.bonusSynergies = new List<CustomSynergyType>();
            }
        }

        public class CardHeartSynergy : AdvancedSynergyEntry
        {
            public CardHeartSynergy()
            {
                this.NameKey = "Heart of the Card";
                this.MandatoryItemIDs = new List<int>
                {
                    ETGMod.Databases.Items["Trap Card"].PickupObjectId,
                    423
                };
                this.IgnoreLichEyeBullets = true;
                this.statModifiers = new List<StatModifier>(0);
                this.bonusSynergies = new List<CustomSynergyType>();
            }
        }

        public class GreedyJarsSynergy : AdvancedSynergyEntry
        {
            public GreedyJarsSynergy()
            {
                this.NameKey = "Greedy Jars";
                this.MandatoryItemIDs = new List<int>
                {
                    ETGMod.Databases.Items["War Vase"].PickupObjectId,
                };

                this.OptionalItemIDs = new List<int>
                {
                    532,
                    214
                };

                this.IgnoreLichEyeBullets = true;
                this.statModifiers = new List<StatModifier>(0);
                this.bonusSynergies = new List<CustomSynergyType>();
            }
        }

        public class GForceSynergy : AdvancedSynergyEntry
        {
            public GForceSynergy()
            {
                this.NameKey = "G-Forces";
                this.MandatoryItemIDs = new List<int>
                {
                    ETGMod.Databases.Items["Gravity Glove"].PickupObjectId
                };
                this.MandatoryGunIDs = new List<int>
                {
                    519
                };

                this.IgnoreLichEyeBullets = true;
                this.statModifiers = new List<StatModifier>(0);
                this.bonusSynergies = new List<CustomSynergyType>();
            }
        }

        public class EggRollSynergy : AdvancedSynergyEntry
        {
            public EggRollSynergy()
            {
                this.NameKey = "Egg Roll";
                this.MandatoryItemIDs = new List<int>
                {
                    ETGMod.Databases.Items["Crackling Egg"].PickupObjectId,
                    817
                };
                this.IgnoreLichEyeBullets = true;
                this.statModifiers = new List<StatModifier>(0);
                this.bonusSynergies = new List<CustomSynergyType>();
            }
        }

        public class FutureSightSynergy : AdvancedSynergyEntry
        {
            public FutureSightSynergy()
            {
                this.NameKey = "Future Sight";
                this.MandatoryItemIDs = new List<int>
                {
                    ETGMod.Databases.Items["Crystal Ball"].PickupObjectId
                };
                this.MandatoryGunIDs = new List<int>
                {
                    595
                };

                this.IgnoreLichEyeBullets = true;
                this.statModifiers = new List<StatModifier>(0);
                this.bonusSynergies = new List<CustomSynergyType>();
            }
        }

        public class TigerGenieSynergy : AdvancedSynergyEntry
        {
            public TigerGenieSynergy()
            {
                this.NameKey = "Arabian Nights";
                this.MandatoryItemIDs = new List<int>
                {
                    ETGMod.Databases.Items["Wish Charm"].PickupObjectId
                };
                this.MandatoryGunIDs = new List<int>
                {
                    369
                };

                this.IgnoreLichEyeBullets = true;
                this.statModifiers = new List<StatModifier>(0);
                this.bonusSynergies = new List<CustomSynergyType>();
            }
        }

        public class SoulTriggerSynergy : AdvancedSynergyEntry
        {
            public SoulTriggerSynergy()
            {
                this.NameKey = "Spirit Trigger";
                this.MandatoryItemIDs = new List<int>
                {
                    ETGMod.Databases.Items["Invoker"].PickupObjectId,
                    489
                };

                this.IgnoreLichEyeBullets = true;
                this.statModifiers = new List<StatModifier>(0);
                this.bonusSynergies = new List<CustomSynergyType>();
            }
        }

        public class GoldenRatioSynergy : AdvancedSynergyEntry
        {
            public GoldenRatioSynergy()
            {
                this.NameKey = "The Golden Ratio";
                this.MandatoryItemIDs = new List<int>
                {
                    ETGMod.Databases.Items["Infinity Guontlet"].PickupObjectId
                };
                this.MandatoryGunIDs = new List<int>
                {
                    PickupObjectDatabase.GetByEncounterName("Infinigun").PickupObjectId
                };
                this.IgnoreLichEyeBullets = true;
                this.statModifiers = new List<StatModifier>(0);
                this.bonusSynergies = new List<CustomSynergyType>();
            }
        }

        public class AngerIssuesSynergy : AdvancedSynergyEntry
        {
            public AngerIssuesSynergy()
            {
                this.NameKey = "Anger Issues";
                this.OptionalItemIDs = new List<int>
                {
                    323
                };
                this.MandatoryGunIDs = new List<int>
                {
                    PickupObjectDatabase.GetByEncounterName("Berserker Rifle").PickupObjectId
                };
                this.IgnoreLichEyeBullets = false;
                this.statModifiers = new List<StatModifier>(0);
                this.bonusSynergies = new List<CustomSynergyType>();
            }
        }

        public class AbsoluteChaosSynergy : AdvancedSynergyEntry
        {
            public AbsoluteChaosSynergy()
            {
                this.NameKey = "Absolute Chaos";
                this.OptionalItemIDs = new List<int>
                {
                    524
                };
                this.MandatoryGunIDs = new List<int>
                {
                    PickupObjectDatabase.GetByEncounterName("Berserker Rifle").PickupObjectId
                };
                this.IgnoreLichEyeBullets = false;
                this.statModifiers = new List<StatModifier>(0);
                this.bonusSynergies = new List<CustomSynergyType>();
            }
        }

        public class ArmorMaintenanceSynergy : AdvancedSynergyEntry
        {
            public ArmorMaintenanceSynergy()
            {
                this.NameKey = "Armor Maintenance";
                this.MandatoryItemIDs = new List<int>
                {
                    ETGMod.Databases.Items["Rusted Armor"].PickupObjectId,
                    450
                };
                this.IgnoreLichEyeBullets = true;
                this.statModifiers = new List<StatModifier>(0);
                this.bonusSynergies = new List<CustomSynergyType>();
            }
        }

        public class FormerGlorySynergy : AdvancedSynergyEntry
        {
            public FormerGlorySynergy()
            {
                this.NameKey = "Former Glory";
                this.MandatoryItemIDs = new List<int>
                {
                    ETGMod.Databases.Items["Rusted Armor"].PickupObjectId,
                    384
                };

                this.OptionalItemIDs = new List<int>
                {
                    267,
                    160,
                    161,
                    162
                };

                this.NumberObjectsRequired = 3;
                this.IgnoreLichEyeBullets = true;
                this.statModifiers = new List<StatModifier>(0);
                this.bonusSynergies = new List<CustomSynergyType>();
            }
        }

        public class RockPaperCrossBow : AdvancedSynergyEntry
        {
            public RockPaperCrossBow()
            {
                this.NameKey = "Rock Paper Crossbow";
                this.MandatoryItemIDs = new List<int>
                {
                    ETGMod.Databases.Items["Roshambo"].PickupObjectId
                };

                this.OptionalGunIDs = new List<int>
                {
                    381
                };

                this.IgnoreLichEyeBullets = true;
                this.statModifiers = new List<StatModifier>(0);
                this.bonusSynergies = new List<CustomSynergyType>();
            }
        }

        public class BatterUpSynergy : AdvancedSynergyEntry
        {
            public BatterUpSynergy()
            {
                this.NameKey = "Batter Up";
                this.MandatoryItemIDs = new List<int>
                {
                    ETGMod.Databases.Items["Frag Grenade"].PickupObjectId
                };

                this.MandatoryGunIDs = new List<int>
                {
                    541
                };

                this.IgnoreLichEyeBullets = true;
                this.statModifiers = new List<StatModifier>(0);
                this.bonusSynergies = new List<CustomSynergyType>();
            }
        }

        public class ChuckingNadesSynergy : AdvancedSynergyEntry
        {
            public ChuckingNadesSynergy()
            {
                this.NameKey = "Chucking Nades";
                this.MandatoryItemIDs = new List<int>
                {
                    ETGMod.Databases.Items["Frag Grenade"].PickupObjectId
                };

                this.MandatoryGunIDs = new List<int>
                {
                    19
                };

                this.IgnoreLichEyeBullets = true;
                this.statModifiers = new List<StatModifier>(0);
                this.bonusSynergies = new List<CustomSynergyType>();
            }
        }

        public class FreezePlusSynergy : AdvancedSynergyEntry
        {
            public FreezePlusSynergy()
            {
                this.NameKey = "Winter Is Coming";
                this.MandatoryItemIDs = new List<int>
                {
                    ETGMod.Databases.Items["Icy Skull"].PickupObjectId
                };

                this.MandatoryGunIDs = new List<int>
                {
                    40
                };

                this.IgnoreLichEyeBullets = true;
                this.statModifiers = new List<StatModifier>(0);
                this.bonusSynergies = new List<CustomSynergyType>();
            }
        }

        public class FrozenCoreSynergy : AdvancedSynergyEntry
        {
            public FrozenCoreSynergy()
            {
                this.NameKey = "Frozen Core";
                this.MandatoryItemIDs = new List<int>
                {
                    ETGMod.Databases.Items["Icy Skull"].PickupObjectId
                };

                this.OptionalItemIDs = new List<int>
                {
                    364
                };

                this.IgnoreLichEyeBullets = true;
                this.statModifiers = new List<StatModifier>(0);
                this.bonusSynergies = new List<CustomSynergyType>();
            }
        }

        public class BoneheadSynergy : AdvancedSynergyEntry
        {
            public BoneheadSynergy()
            {
                this.NameKey = "Boneheads";
                this.MandatoryItemIDs = new List<int>
                {
                    ETGMod.Databases.Items["Icy Skull"].PickupObjectId
                };

                this.OptionalGunIDs = new List<int>
                {
                    45,
                    464
                };

                this.IgnoreLichEyeBullets = true;
                this.statModifiers = new List<StatModifier>(0);
                this.bonusSynergies = new List<CustomSynergyType>();
            }
        }

        public class ToolKitSynergy : AdvancedSynergyEntry
        {
            public ToolKitSynergy()
            {
                this.NameKey = "Builder's Toolkit";
                this.MandatoryGunIDs = new List<int>
                {
                    PickupObjectDatabase.GetByEncounterName("The Constructor").PickupObjectId
                };

                this.OptionalItemIDs = new List<int>
                {
                    131,
                    239
                };

                this.IgnoreLichEyeBullets = false;
                this.statModifiers = new List<StatModifier>(0);
                this.bonusSynergies = new List<CustomSynergyType>();
            }
        }

        public class PotceptionSynergy : AdvancedSynergyEntry
        {
            public PotceptionSynergy()
            {
                this.NameKey = "Potception";
                this.MandatoryGunIDs = new List<int>
                {
                    PickupObjectDatabase.GetByEncounterName("The Kiln").PickupObjectId
                };

                this.MandatoryItemIDs = new List<int>
                {
                    ETGMod.Databases.Items["War Vase"].PickupObjectId
                };

                this.IgnoreLichEyeBullets = true;
                this.statModifiers = new List<StatModifier>(0);
                this.bonusSynergies = new List<CustomSynergyType>();
            }
        }

        public class AntiquatedSynergy : AdvancedSynergyEntry
        {
            public AntiquatedSynergy()
            {
                this.NameKey = "Antiquated";
                this.MandatoryGunIDs = new List<int>
                {
                    PickupObjectDatabase.GetByEncounterName("Ornate Pistol").PickupObjectId,
                    9                  
                };

                this.IgnoreLichEyeBullets = true;
                this.statModifiers = new List<StatModifier>(0);
                this.bonusSynergies = new List<CustomSynergyType>();
            }

        }




    }
}
