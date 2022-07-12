using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using Lyralei.TheRandomizerCore;
using Sims3.Gameplay.Actors;
using Sims3.Gameplay.ActorSystems;
using Sims3.Gameplay.Autonomy;
using Sims3.Gameplay.CAS;
using Sims3.Gameplay.Core;
using Sims3.Gameplay.EventSystem;
using Sims3.Gameplay.Interactions;
using Sims3.Gameplay.Socializing;
using Sims3.Gameplay.Utilities;
using Sims3.SimIFace;
using Sims3.UI;
using Sims3.UI.CAS;

//Template Created by Battery

namespace Sims3.Gameplay.Lyralei.TheRandomizerCore
{
	public class RandomizerCoreModule
	{
		[Tunable] static bool init;

        [TunableComment("Should Lyralei's Randomizer Mod notify you if it's installed AND when events are being triggered? (true = yes, false = no)")]
        [Tunable]
        public static bool kEnableDebugMode = false;

        [PersistableStatic]
        public static string sRandomizerSaveData;

        public static List<EventTypeRandomizer> AllRandomOptions = new List<EventTypeRandomizer>();


        // Curated list with actual Random Events we have loaded in. Because this might become an utility, it does make it so it's not always got all types, unlike the official mod.
        public static Dictionary<RandomEventType, float> allRandomEventTypes = new Dictionary<RandomEventType, float>();
        public static List<RandomEventType> mAllInstalledModules = new List<RandomEventType>();

        public static List<EventTypeRandomizer> CareerRelatedOptions = new List<EventTypeRandomizer>();
        public static List<EventTypeRandomizer> GeneralRelatedOptions = new List<EventTypeRandomizer>();
        public static List<EventTypeRandomizer> DeathRelatedOptions = new List<EventTypeRandomizer>();
        public static List<EventTypeRandomizer> DiseaseRelatedOptions = new List<EventTypeRandomizer>();
        public static List<EventTypeRandomizer> FriendshipRelatedOptions = new List<EventTypeRandomizer>();
        public static List<EventTypeRandomizer> FameRelatedOptions = new List<EventTypeRandomizer>();
        public static List<EventTypeRandomizer> EnemyRelatedOptions = new List<EventTypeRandomizer>();
        public static List<EventTypeRandomizer> RelationshipRelatedOptions = new List<EventTypeRandomizer>();
        public static List<EventTypeRandomizer> MoneyRelatedOptions = new List<EventTypeRandomizer>();
        public static List<EventTypeRandomizer> SchoolRelatedOptions = new List<EventTypeRandomizer>();
        public static List<EventTypeRandomizer> DisasterRelatedOptions = new List<EventTypeRandomizer>();

        [Tunable] static float kDaysCooldownForWorkOptions = 1f;
        [Tunable] static float kDaysCooldownForGeneralOption = 2f;
        [Tunable] static float kDaysCooldownForDeathRelatedOption = 10f;
        [Tunable] static float kDaysCooldownForDiseaseRelatedOption = 7f;
        [Tunable] static float kDaysCooldownForFriendshipRelatedOption = 3f;
        [Tunable] static float kDaysCooldownForFameRelatedOption = 4f;
        [Tunable] static float kDaysCooldownForEnemyRelatedOption = 4f;
        [Tunable] static float kDaysCooldownForRelationshipRelatedOption = 3f;
        [Tunable] static float kDaysCooldownForMoneyRelatedOption = 10f;
        [Tunable] static float kDaysCooldownForSchoolRelatedOption = 1f;
        [Tunable] static float kDaysCooldownForDisasterRelatedOption = 10f;

        [Tunable] static float kWhenToClearLastItemListDays = 7f;
        [Tunable] static bool kShouldCheckToNotFireTheSameEvent = true;


        public static List<RandomizerBasicHelpersStructsClasses.RumorType> AllRumors = new List<RandomizerBasicHelpersStructsClasses.RumorType>();
        public static List<RandomizerBasicHelpersStructsClasses.RumorType> GeneralRumors = new List<RandomizerBasicHelpersStructsClasses.RumorType>();
        public static List<RandomizerBasicHelpersStructsClasses.RumorType> WorkRumors = new List<RandomizerBasicHelpersStructsClasses.RumorType>();
        public static List<RandomizerBasicHelpersStructsClasses.RumorType> FriendshipRumors = new List<RandomizerBasicHelpersStructsClasses.RumorType>();
        public static List<RandomizerBasicHelpersStructsClasses.RumorType> FameRumors = new List<RandomizerBasicHelpersStructsClasses.RumorType>();
        public static List<RandomizerBasicHelpersStructsClasses.RumorType> RelationshipRumors = new List<RandomizerBasicHelpersStructsClasses.RumorType>();
        public static List<RandomizerBasicHelpersStructsClasses.RumorType> DeathRumors = new List<RandomizerBasicHelpersStructsClasses.RumorType>();
        public static List<RandomizerBasicHelpersStructsClasses.RumorType> EnemyRumors = new List<RandomizerBasicHelpersStructsClasses.RumorType>();
        public static List<RandomizerBasicHelpersStructsClasses.RumorType> DiseaseRumors = new List<RandomizerBasicHelpersStructsClasses.RumorType>();
        public static List<RandomizerBasicHelpersStructsClasses.RumorType> MoneyRumors = new List<RandomizerBasicHelpersStructsClasses.RumorType>();
        public static List<RandomizerBasicHelpersStructsClasses.RumorType> SchoolRumors = new List<RandomizerBasicHelpersStructsClasses.RumorType>();
        public static List<RandomizerBasicHelpersStructsClasses.RumorType> DisasterRumors = new List<RandomizerBasicHelpersStructsClasses.RumorType>();

        public static List<RandomizerBasicHelpersStructsClasses.AdviceType> AllAdvice = new List<RandomizerBasicHelpersStructsClasses.AdviceType>();
        public static List<RandomizerBasicHelpersStructsClasses.AdviceType> GeneralAdvice = new List<RandomizerBasicHelpersStructsClasses.AdviceType>();
        public static List<RandomizerBasicHelpersStructsClasses.AdviceType> WorkAdvice = new List<RandomizerBasicHelpersStructsClasses.AdviceType>();
        public static List<RandomizerBasicHelpersStructsClasses.AdviceType> FriendshipAdvice = new List<RandomizerBasicHelpersStructsClasses.AdviceType>();
        public static List<RandomizerBasicHelpersStructsClasses.AdviceType> FameAdvice = new List<RandomizerBasicHelpersStructsClasses.AdviceType>();
        public static List<RandomizerBasicHelpersStructsClasses.AdviceType> RelationshipAdvice = new List<RandomizerBasicHelpersStructsClasses.AdviceType>();
        public static List<RandomizerBasicHelpersStructsClasses.AdviceType> DeathAdvice = new List<RandomizerBasicHelpersStructsClasses.AdviceType>();
        public static List<RandomizerBasicHelpersStructsClasses.AdviceType> EnemyAdvice = new List<RandomizerBasicHelpersStructsClasses.AdviceType>();
        public static List<RandomizerBasicHelpersStructsClasses.AdviceType> DiseaseAdvice = new List<RandomizerBasicHelpersStructsClasses.AdviceType>();
        public static List<RandomizerBasicHelpersStructsClasses.AdviceType> MoneyAdvice = new List<RandomizerBasicHelpersStructsClasses.AdviceType>();
        public static List<RandomizerBasicHelpersStructsClasses.AdviceType> SchoolAdvice = new List<RandomizerBasicHelpersStructsClasses.AdviceType>();
        public static List<RandomizerBasicHelpersStructsClasses.AdviceType> DisasterAdvice = new List<RandomizerBasicHelpersStructsClasses.AdviceType>();

        public static Dictionary<RandomEventType, uint> sTimePassed = new Dictionary<RandomEventType, uint>();

        public static uint mDayCounter = 0;

        public static Dictionary<RandomEventType, EventTypeRandomizer> mLastItem = new Dictionary<RandomEventType, EventTypeRandomizer>();
        //public static EventTypeRandomizer mLastItem = null;

        public static Household mCurrentHousehold = null;
        public static EventListener sSimIsInActiveFamily = null;

        public static Dictionary<ulong, AlarmHandle> mDoRandomizerCheck = new Dictionary<ulong, AlarmHandle>();
        public static Dictionary<ulong, AlarmHandle> mRinseLastItemList = new Dictionary<ulong, AlarmHandle>();

        public static List<Sim> sAllSimsInWorld = new List<Sim>();

        static RandomizerCoreModule()
        {
            World.OnWorldLoadFinishedEventHandler += new EventHandler(OnWorldLoadFinished);
            LoadSaveManager.ObjectGroupsPreLoad += LoadSaveManager_ObjectGroupsPreLoad;
            //World.sOnWorldQuitEventHandler += new EventHandler(OnWorldQuit);
        }

        private static void LoadSaveManager_ObjectGroupsPreLoad()
        {
            ExportSaveData();
        }

        public static void SetupAlarm(Household household)
        {
            if (household != null && household.LotHome != null && !mDoRandomizerCheck.ContainsKey(household.HouseholdId))
            {
                //mDoRandomizerCheck.Add(household.HouseholdId, household.LotHome.AddAlarmRepeating(RandomUtil.GetFloat(kTimeBetweenRandomEventsFiredMin, kTimeBetweenRandomEventsFiredMax), TimeUnit.Hours, () => { FindGoodRandomEvent(household); }, 2f, TimeUnit.Hours, "RandomizerCore_FindGoodRandomEvent_Alarm_" + household.HouseholdId.ToString(), AlarmType.NeverPersisted));
                mDoRandomizerCheck.Add(household.HouseholdId, household.LotHome.AddAlarmRepeating(1f, TimeUnit.Days, () => { HandleAlarm(household); }, "RandomizerCore_FindGoodRandomEvent_Alarm_" + household.HouseholdId.ToString(), AlarmType.NeverPersisted));

                // To guarantee that there may be a chance that certain last items do fire, we reset the list after a while.
                mRinseLastItemList.Add(household.HouseholdId, household.LotHome.AddAlarmRepeating(kWhenToClearLastItemListDays, TimeUnit.Days, HandleLastItemList, "RandomizerCore_CleanLastTimeFired_Alarm_" + household.HouseholdId.ToString(), AlarmType.NeverPersisted));

            }
        }

        /// <summary>
        /// Parse all the Rumor data and will be added to the appropiate lists (if rumor is of course parsed correctly ;))
        /// </summary>
        /// <param name="xmlDocument"> expects your XMl. Parse this in like so: XmlDbData.ReadData(new ResourceKey(0x2FD1D96B6115155F, 0x0333406C, 0x00000000), false);</param>
        public static void ParseRumorDataFromXML(XmlDbData xmlDocument)
        {
            try
            {
                XmlDbTable xmlDbTable;

                if (xmlDocument != null && xmlDocument.Tables != null && xmlDocument.Tables.TryGetValue("Rumor", out xmlDbTable))
                {
                    foreach (XmlDbRow row in xmlDbTable.Rows)
                    {
                        if(string.IsNullOrEmpty(row.GetString("rumorDesc")))
                        {
                            continue;
                        }

                        RandomizerBasicHelpersStructsClasses.RumorType mRumor = new RandomizerBasicHelpersStructsClasses.RumorType();
                        mRumor.mRumor = row.GetString("rumorDesc");

                        RandomEventType key;
                        if (!row.TryGetEnum("TypeEvent", out key, RandomEventType.None))
                        {
                            break;
                        }
                        mRumor.EventType = key;
                        mRumor.isBadRumor = row.GetBool("isBadRumor");
                        mRumor.isControversialRumor = row.GetBool("isControversialRumor");

                        AllRumors.Add(mRumor);

                        switch (mRumor.EventType)
                        {
                            case RandomEventType.None:
                            case RandomEventType.GeneralRelated:
                                GeneralRumors.Add(mRumor);
                                break;
                            case RandomEventType.WorkRelated:
                                WorkRumors.Add(mRumor);
                                break;
                            case RandomEventType.DeathRelated:
                                DeathRumors.Add(mRumor);
                                break;
                            case RandomEventType.DiseaseRelated:
                                DiseaseRumors.Add(mRumor);
                                break;
                            case RandomEventType.FriendshipRelated:
                                FriendshipRumors.Add(mRumor);
                                break;
                            case RandomEventType.FameRelated:
                                FameRumors.Add(mRumor);
                                break;
                            case RandomEventType.EnemyRelated:
                                EnemyRumors.Add(mRumor);
                                break;
                            case RandomEventType.RelationshipRelated:
                                RelationshipRumors.Add(mRumor);
                                break;
                            case RandomEventType.MoneyRelated:
                                MoneyRumors.Add(mRumor);
                                break;
                            case RandomEventType.SchoolRelated:
                                SchoolRumors.Add(mRumor);
                                break;
                            case RandomEventType.DisasterRelated:
                                DisasterRumors.Add(mRumor);
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                printException(ex);
            }
        }

        /// <summary>
        /// Parse all the Advice data and will be added to the appropiate lists (if Advice is of course parsed correctly ;))
        /// </summary>
        /// <param name="xmlDocument"> expects your XMl. Parse this in like so: XmlDbData.ReadData(new ResourceKey(0x2FD1D96B6115155F, 0x0333406C, 0x00000000), false);</param>

        public static void ParseAdviceDataFromXML(XmlDbData xmlDocument)
        {
            try
            {
                XmlDbTable xmlDbTable;

                if (xmlDocument != null && xmlDocument.Tables != null && xmlDocument.Tables.TryGetValue("Advice", out xmlDbTable))
                {
                    foreach (XmlDbRow row in xmlDbTable.Rows)
                    {
                        if (string.IsNullOrEmpty(Localization.LocalizeString(row.GetString("Question"))))
                        {
                            continue;
                        }

                        RandomizerBasicHelpersStructsClasses.AdviceType mAdvice = new RandomizerBasicHelpersStructsClasses.AdviceType();
                        mAdvice.mQuestion = row.GetString("Question");

                        RandomEventType key;
                        if (!row.TryGetEnum("TypeEvent", out key, RandomEventType.None))
                        {
                            break;
                        }
                        mAdvice.EventType = key;
                        mAdvice.mAnswerA = Localization.LocalizeString(row.GetString("AnswerA"));
                        mAdvice.mAnswerB = Localization.LocalizeString(row.GetString("AnswerB"));
                        mAdvice.mAnswerC = Localization.LocalizeString(row.GetString("AnswerC"));
                        mAdvice.mAnswerAValidation = row.GetBool("AnswerACorrect");
                        mAdvice.mAnswerBValidation = row.GetBool("AnswerBCorrect");
                        mAdvice.mAnswerCValidation = row.GetBool("AnswerCCorrect");

                        AllAdvice.Add(mAdvice);

                        switch (mAdvice.EventType)
                        {
                            case RandomEventType.None:
                            case RandomEventType.GeneralRelated:
                                GeneralAdvice.Add(mAdvice);
                                break;
                            case RandomEventType.WorkRelated:
                                WorkAdvice.Add(mAdvice);
                                break;
                            case RandomEventType.DeathRelated:
                                DeathAdvice.Add(mAdvice);
                                break;
                            case RandomEventType.DiseaseRelated:
                                DiseaseAdvice.Add(mAdvice);
                                break;
                            case RandomEventType.FriendshipRelated:
                                FriendshipAdvice.Add(mAdvice);
                                break;
                            case RandomEventType.FameRelated:
                                FameAdvice.Add(mAdvice);
                                break;
                            case RandomEventType.EnemyRelated:
                                EnemyAdvice.Add(mAdvice);
                                break;
                            case RandomEventType.RelationshipRelated:
                                RelationshipAdvice.Add(mAdvice);
                                break;
                            case RandomEventType.MoneyRelated:
                                MoneyAdvice.Add(mAdvice);
                                break;
                            case RandomEventType.SchoolRelated:
                                SchoolAdvice.Add(mAdvice);
                                break;
                            case RandomEventType.DisasterRelated:
                                DisasterAdvice.Add(mAdvice);
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                printException(ex);
            }
        }


        /// <summary>
        /// Use this function to parse your Randomize Data! 
        /// </summary>
        /// <param name="xmlDocument"> expects your XMl. Parse this in like so: XmlDbData.ReadData(new ResourceKey(0x2FD1D96B6115155F, 0x0333406C, 0x00000000), false);</param>
        public static void ParseRandomizerDataFromXML(XmlDbData xmlDocument)
		{
            try
            {
                XmlDbTable xmlDbTable;

                if (xmlDocument != null && xmlDocument.Tables != null && xmlDocument.Tables.TryGetValue("Event", out xmlDbTable))
                {
                    foreach (XmlDbRow row in xmlDbTable.Rows)
                    {
                        EventTypeRandomizer mEvent = null;

                        RandomEventType key;
                        if (!row.TryGetEnum("TypeEvent", out key, RandomEventType.None))
                        {
                            break;
                        }

                        if (key == RandomEventType.None) break;
                        mEvent = new EventTypeRandomizer();
                        mEvent.type = key;

                        mEvent.mRarity = row.GetString("Rarity").ToUpper();

                        if(mEvent.mRarity == "NONE")
                        {
                            continue;
                        }

                        switch(mEvent.mRarity)
                        {
                            case "COMMON":
                            case "UNCOMMON":
                            case "RARE":
                                break;
                            default:
                                mEvent.mRarity = "COMMON";
                                break;
                        }

                        mEvent.mNeedsOptionMenu = row.GetBool("NeedsOptionMenu");
                        mEvent.mNeedsInvolvedTarget = row.GetBool("NeedsInvolvedTarget");
                        mEvent.mIsRomantic = row.GetBool("IsRomanticOption");
                        mEvent.mIsForNastySims = row.GetBool("IsForEvilDramaQueen");

                        mEvent.mCaption = row.GetString("STBLCaptionKey");


                        string mTestMethod = row.GetString("TestMethod");
                        if (!string.IsNullOrEmpty(mTestMethod))
                        {
                            mEvent.TestMethod = FindMethod(mTestMethod);
                        }


                        if (mEvent.mNeedsOptionMenu)
                        {
                            mEvent.mTitle = Localization.LocalizeString(row.GetString("STBLTitleKey"));
                            mEvent.mOptionOneText = Localization.LocalizeString(row.GetString("FirstOption"));

                            string mMethod = row.GetString("FirstOptionOutcome");
                            if (!string.IsNullOrEmpty(mMethod))
                            {
                                mEvent.OptionOneFunction = FindMethod(mMethod);
                            }
                            mEvent.mOptionTwoText = Localization.LocalizeString(row.GetString("SecondOption"));
                            mMethod = row.GetString("SecondOptionOutcome");

                            if (!string.IsNullOrEmpty(mMethod))
                            {
                                mEvent.OptionTwoFunction = FindMethod(mMethod);
                            }
                            mEvent.mOptionThreeText = Localization.LocalizeString(row.GetString("ThirdOption"));
                            mMethod = row.GetString("ThirdOptionOutcome");

                            if (!string.IsNullOrEmpty(mMethod))
                            {
                                mEvent.OptionThreeFunction = FindMethod(mMethod);
                            }
                        }
                        else
                        {
                            string mMethod = row.GetString("GeneralMethod");
                            if (!string.IsNullOrEmpty(mMethod))
                            {
                                mEvent.OptionGeneralFunction = FindMethod(mMethod);
                            }
                        }

                        AllRandomOptions.Add(mEvent);
                        mAllInstalledModules.Add(mEvent.type);

                        // Filter and add all Definitive event type to their respective lists. And if the type hasn't been recognised yet, also add it to the potential list of event types we can choose from.
                        switch (mEvent.type)
                        {
                            case RandomEventType.None:
                                break;
                            case RandomEventType.WorkRelated: // Skip adding Work as a Random Event type, as we only want to fire these when the sim is at work
                                CareerRelatedOptions.Add(mEvent);
                                AddRarityToCollection(mEvent.type, mEvent.mRarity);
                                break;
                            case RandomEventType.GeneralRelated:
                                if (!allRandomEventTypes.ContainsKey(RandomEventType.GeneralRelated))
                                {
                                    allRandomEventTypes.Add(RandomEventType.GeneralRelated, kDaysCooldownForGeneralOption);
                                }
                                GeneralRelatedOptions.Add(mEvent);
                                AddRarityToCollection(mEvent.type, mEvent.mRarity);

                                break;
                            case RandomEventType.DeathRelated:
                                if (!allRandomEventTypes.ContainsKey(RandomEventType.DeathRelated))
                                {
                                    allRandomEventTypes.Add(RandomEventType.DeathRelated, kDaysCooldownForDeathRelatedOption);
                                }
                                DeathRelatedOptions.Add(mEvent);
                                AddRarityToCollection(mEvent.type, mEvent.mRarity);

                                break;
                            case RandomEventType.DiseaseRelated:
                                if (!allRandomEventTypes.ContainsKey(RandomEventType.DiseaseRelated))
                                {
                                    allRandomEventTypes.Add(RandomEventType.DiseaseRelated, kDaysCooldownForDiseaseRelatedOption);
                                }
                                DiseaseRelatedOptions.Add(mEvent);
                                AddRarityToCollection(mEvent.type, mEvent.mRarity);

                                break;
                            case RandomEventType.FriendshipRelated:
                                if (!allRandomEventTypes.ContainsKey(RandomEventType.FriendshipRelated))
                                {
                                    allRandomEventTypes.Add(RandomEventType.FriendshipRelated, kDaysCooldownForFriendshipRelatedOption);
                                }
                                FriendshipRelatedOptions.Add(mEvent);
                                AddRarityToCollection(mEvent.type, mEvent.mRarity);

                                break;
                            case RandomEventType.FameRelated:
                                if (!allRandomEventTypes.ContainsKey(RandomEventType.FameRelated))
                                {
                                    allRandomEventTypes.Add(RandomEventType.FameRelated, kDaysCooldownForFameRelatedOption);
                                }
                                FameRelatedOptions.Add(mEvent);
                                AddRarityToCollection(mEvent.type, mEvent.mRarity);

                                break;
                            case RandomEventType.EnemyRelated:
                                if (!allRandomEventTypes.ContainsKey(RandomEventType.EnemyRelated))
                                {
                                    allRandomEventTypes.Add(RandomEventType.EnemyRelated, kDaysCooldownForEnemyRelatedOption);
                                }
                                EnemyRelatedOptions.Add(mEvent);
                                AddRarityToCollection(mEvent.type, mEvent.mRarity);

                                break;
                            case RandomEventType.RelationshipRelated:
                                if (!allRandomEventTypes.ContainsKey(RandomEventType.RelationshipRelated))
                                {
                                    allRandomEventTypes.Add(RandomEventType.RelationshipRelated, kDaysCooldownForRelationshipRelatedOption);
                                }
                                RelationshipRelatedOptions.Add(mEvent);
                                AddRarityToCollection(mEvent.type, mEvent.mRarity);

                                break;
                            case RandomEventType.MoneyRelated:
                                if (!allRandomEventTypes.ContainsKey(RandomEventType.MoneyRelated))
                                {
                                    allRandomEventTypes.Add(RandomEventType.MoneyRelated, kDaysCooldownForMoneyRelatedOption);
                                }
                                MoneyRelatedOptions.Add(mEvent);
                                AddRarityToCollection(mEvent.type, mEvent.mRarity);

                                break;
                            case RandomEventType.SchoolRelated:
                                if (!allRandomEventTypes.ContainsKey(RandomEventType.SchoolRelated))
                                {
                                    allRandomEventTypes.Add(RandomEventType.SchoolRelated, kDaysCooldownForSchoolRelatedOption);
                                }
                                SchoolRelatedOptions.Add(mEvent);
                                AddRarityToCollection(mEvent.type, mEvent.mRarity);

                                break;
                            case RandomEventType.DisasterRelated:
                                if (!allRandomEventTypes.ContainsKey(RandomEventType.DisasterRelated))
                                {
                                    allRandomEventTypes.Add(RandomEventType.DisasterRelated, kDaysCooldownForDisasterRelatedOption);
                                }
                                DisasterRelatedOptions.Add(mEvent);
                                AddRarityToCollection(mEvent.type, mEvent.mRarity);

                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                printException(ex);
            }
        }

        private static void AddRarityToCollection(RandomEventType type, string mRarity)
        {
            if (!sTypeOfRarities.ContainsKey(type))
            {
                sTypeOfRarities.Add(type, new List<string>());
            }
            bool hasRarity = false;
            for (int i = 0; i < sTypeOfRarities[type].Count; i++)
            {
                if (sTypeOfRarities[type][i] == mRarity)
                {
                    hasRarity = true;
                    break;
                }
            }
            if (!hasRarity) sTypeOfRarities[type].Add(mRarity);
        }

        public static MethodInfo FindMethod(string methodName)
        {
            if (methodName.Contains(","))
            {
                string[] array = methodName.Split(',');
                string typeName = array[0] + "," + array[1];
                Type type = Type.GetType(typeName, true);
                string text = array[2];
                text = text.Replace(" ", "");

                if (type.GetMethod(text) == null)
                {
                    return null;
                }

                return type.GetMethod(text);
            }
            return null;
            //Type typeFromHandle = typeof(RandomizerFunctions);
            //return typeFromHandle.GetMethod(methodName);
        }

        public static EventTypeRandomizer FindProperEvent(List<EventTypeRandomizer> optionsList, Sim sim)
        {
            try
            {
                RandomEventType tmpType = optionsList[0].type;
                string reason = "";

                if (optionsList != null && optionsList.Count > 0)
                {
                    List<EventTypeRandomizer> found = new List<EventTypeRandomizer>();
                    List<Sim> potentialTargets = new List<Sim>();
                    foreach (EventTypeRandomizer e in optionsList)
                    {
                        if (e == null) { continue; }
                        if (e.mRarity == RarityChosen)
                        {
                            if (kShouldCheckToNotFireTheSameEvent && !mLastItem.ContainsKey(e.type))
                            {
                                // If mLastitem is not yet set, just add it as it should be fine...
                                Sim Target = PrepareForNecessaryTarget(e, sim);
                                if (TestRandomEvent(e, sim, Target))
                                {
                                    found.Add(e);
                                    potentialTargets.Add(Target);
                                }
                                else
                                {
                                    reason = "Test was failure";
                                }
                            }
                            if (kShouldCheckToNotFireTheSameEvent && mLastItem.ContainsKey(e.type) && !mLastItem.ContainsValue(e))
                            {
                                Sim Target = PrepareForNecessaryTarget(e, sim);
                                if (TestRandomEvent(e, sim, Target))
                                {
                                    found.Add(e);
                                    potentialTargets.Add(Target);
                                }
                                else
                                {
                                    reason = "Test was failure";
                                }
                            }
                            else if(!kShouldCheckToNotFireTheSameEvent)
                            {
                                Sim Target = PrepareForNecessaryTarget(e, sim);
                                if (TestRandomEvent(e, sim, Target))
                                {
                                    found.Add(e);
                                    potentialTargets.Add(Target);
                                }
                                else
                                {
                                    reason = "Test was failure";
                                }
                            }
                        }
                    }

                    if (found.Count > 0)
                    {
                        int RanIndex = RandomUtil.GetInt(found.Count - 1);
                        EventTypeRandomizer mEvent = found[RanIndex];
                        mTarget = potentialTargets[RanIndex];
                        return mEvent;
                    }
                }

                if (kEnableDebugMode)
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine("Lyralei's Randomizer Mod:");
                    sb.AppendLine("Internal Reason: " + reason);
                    sb.AppendLine();
                    sb.AppendLine("Couldnt find any Event. See the reason if there is any. Otherwise, it's that the list of collected modules was empty (which it most likely isn't)");

                    StyledNotification.Format format = new StyledNotification.Format(sb.ToString(), StyledNotification.NotificationStyle.kDebugAlert);
                    StyledNotification.Show(format);
                }
                return null;
            }
            catch(Exception ex)
            {
                printException(ex);
                return null;
            }
        }


        public static void OnWorldQuit(object sender, EventArgs e)
        {
            ExportSaveData();
        }

        public static void HandleLastItemList()
        {
            // If the user did actually change the setting, we want to check if we should clear the item.
            if(!kShouldCheckToNotFireTheSameEvent && mLastItem.Count > 0)
            {
                mLastItem.Clear();
                return;
            }

            if(sTimePassed != null && sTimePassed.Count > 0)
            {
                foreach (KeyValuePair<RandomEventType, uint> kpv in sTimePassed)
                {
                    if(mLastItem.ContainsKey(kpv.Key))
                    {
                        float Cooldown = sTimePassed[kpv.Key] + GetRightCooldownForModule(kpv.Key);

                        if (mDayCounter > Cooldown)
                        {
                            mLastItem[kpv.Key] = null;
                            Cooldown = 0;
                        }
                    }
                }
            }

        }

        // 1. Get random sim from list 

        // 2. Check which module would be a valid one for said sim, if so, we add this to a collection of options. For this we check: 
        //          - If it meets the module requirement's (Like with friendship, if the sim has friends) 
        //          - Check for cooldown on module 

        // 3. Randomly get any of the module options from the collection. 
        //          - In the meantime we look for a potential Target that we need. 

        // 4. Fire the event. 

        // 5. Add the module to our cooldown list. 



        public static void HandleAlarm(Household household)
        {
            if(GameUtils.IsOnVacation() || household == null || household.Sims == null || household.Sims.Count <= 0) { return; }

            mDayCounter++;

            Sim sim = RandomUtil.GetRandomObjectFromList(household.Sims);
            if (sim != null && sim.SimDescription != null && sim.SimDescription.SimDescriptionId != 0u)
            {
                FigureOutGoodModules(sim);
            }
        }

        public static List<RandomEventType> sModulesValidToFire = new List<RandomEventType>();
        public static Dictionary<RandomEventType, List<string>> sTypeOfRarities = new Dictionary<RandomEventType, List<string>>();
        public static Sim mTarget = null;

        public static void FigureOutGoodModules(Sim sim)
        {
            try
            {
                // If sim is at school/work only fire those.
                if (CareerRelatedOptions.Count > 0
                            && sim.CareerManager != null
                            && sim.CareerManager.Occupation != null
                            && sim.IsAtWork
                            && sim.School == null)
                {
                    if (mAllInstalledModules.Contains(RandomEventType.WorkRelated))
                    {
                        sModulesValidToFire.Add(RandomEventType.WorkRelated);
                        UpdateListDependingOnCooldowns();
                        UpdateListDependingOnRarityType();

                        RandomEventType mType2 = RandomUtil.GetRandomObjectFromList(sModulesValidToFire);

                        EventTypeRandomizer randomEvent2 = FindRandomEvent(mType2, sim);

                        if (randomEvent2 == null)
                        {
                            if (kEnableDebugMode)
                            {
                                StringBuilder sb = new StringBuilder();

                                sb.AppendLine("Lyralei's Randomizer Mod:");
                                sb.AppendLine("Random event chosen returned invalid...");

                                StyledNotification.Format format = new StyledNotification.Format(sb.ToString(), StyledNotification.NotificationStyle.kDebugAlert);
                                StyledNotification.Show(format);
                            }
                            sModulesValidToFire.Clear();

                            return;
                        }
                        FireChosenEvent(randomEvent2, sim, mTarget);
                        sModulesValidToFire.Clear();
                        return;
                    }
                }
                else if (SchoolRelatedOptions.Count > 0
                            && sim.CareerManager != null
                            && sim.CareerManager.School != null
                            && sim.School != null
                            && sim.CareerManager.School.IsAtWork)
                {
                        if (mAllInstalledModules.Contains(RandomEventType.SchoolRelated))
                        {
                            sModulesValidToFire.Add(RandomEventType.SchoolRelated);
                            UpdateListDependingOnCooldowns();
                            UpdateListDependingOnRarityType();

                            UpdateListDependingOnCooldowns();
                            UpdateListDependingOnRarityType();

                            RandomEventType mType3 = RandomUtil.GetRandomObjectFromList(sModulesValidToFire);

                            EventTypeRandomizer randomEvent3 = FindRandomEvent(mType3, sim);

                            if (randomEvent3 == null)
                            {
                                if (kEnableDebugMode)
                                {
                                    StringBuilder sb = new StringBuilder();

                                    sb.AppendLine("Lyralei's Randomizer Mod:");
                                    sb.AppendLine("Random event chosen returned invalid...");

                                    StyledNotification.Format format = new StyledNotification.Format(sb.ToString(), StyledNotification.NotificationStyle.kDebugAlert);
                                    StyledNotification.Show(format);
                                }
                                sModulesValidToFire.Clear();
                                return;
                            }
                            FireChosenEvent(randomEvent3, sim, mTarget);
                            sModulesValidToFire.Clear();
                            return;
                        }
                }

                // First we check any relationship related requirements.. This is Enemy, Friendship and/or relationship
                Relationship[] relationships = Relationship.GetRelationships(sim);

                if (relationships != null && relationships.Length > 0)
                {

                    foreach (Relationship rel in relationships)
                    {
                        if (rel == null || rel.SimDescriptionA == null || rel.SimDescriptionB == null) { continue; }

                        // If they're the same sim...
                        if (rel.SimDescriptionA.SimDescriptionId == sim.SimDescription.SimDescriptionId && rel.SimDescriptionB.SimDescriptionId == sim.SimDescription.SimDescriptionId)
                        {
                            continue;
                        }

                        if (rel.AreEnemies())
                        {
                            if (mAllInstalledModules.Contains(RandomEventType.EnemyRelated) && !sModulesValidToFire.Contains(RandomEventType.EnemyRelated))
                            {
                                sModulesValidToFire.Add(RandomEventType.EnemyRelated);
                                continue;
                            }
                        }
                        else if (rel.AreFriends())
                        {
                            if (mAllInstalledModules.Contains(RandomEventType.FriendshipRelated) && !sModulesValidToFire.Contains(RandomEventType.FriendshipRelated))
                            {

                                sModulesValidToFire.Add(RandomEventType.FriendshipRelated);
                                continue;
                            }
                        }
                        else if (rel.AreRomantic())
                        {
                            if (mAllInstalledModules.Contains(RandomEventType.RelationshipRelated) && !sModulesValidToFire.Contains(RandomEventType.RelationshipRelated))
                            {

                                sModulesValidToFire.Add(RandomEventType.RelationshipRelated);
                                continue;
                            }
                        }
                    }
                }


                // Check if we can do the death module
                if (sim.CanBeKilled() && (sim.MoodManager != null && sim.MoodManager.IsInNegativeMood))
                {
                    if (mAllInstalledModules.Contains(RandomEventType.DeathRelated) && !sModulesValidToFire.Contains(RandomEventType.DeathRelated))
                    {
                        sModulesValidToFire.Add(RandomEventType.DeathRelated);
                    }
                }

                // Can apply disease?
                if ((sim.SimDescription.HealthManager != null && sim.SimDescription.TraitManager.HasElement(TraitNames.Simmunity)))
                {
                    if (mAllInstalledModules.Contains(RandomEventType.DiseaseRelated) && !sModulesValidToFire.Contains(RandomEventType.DiseaseRelated))
                    {
                        sModulesValidToFire.Add(RandomEventType.DiseaseRelated);
                    }
                }

                if (sim.CelebrityManager != null && sim.CelebrityManager.Level > 0)
                {
                    if (mAllInstalledModules.Contains(RandomEventType.FameRelated) && !sModulesValidToFire.Contains(RandomEventType.FameRelated))
                    {
                        sModulesValidToFire.Add(RandomEventType.FameRelated);
                    }
                }

                // Any random events with no necessary requirements
                if (mAllInstalledModules.Contains(RandomEventType.GeneralRelated) && !sModulesValidToFire.Contains(RandomEventType.GeneralRelated))
                {
                    sModulesValidToFire.Add(RandomEventType.GeneralRelated);
                }
                if (mAllInstalledModules.Contains(RandomEventType.MoneyRelated) && !sModulesValidToFire.Contains(RandomEventType.MoneyRelated))
                {
                    sModulesValidToFire.Add(RandomEventType.MoneyRelated);
                }
                if (mAllInstalledModules.Contains(RandomEventType.DisasterRelated) && !sModulesValidToFire.Contains(RandomEventType.DisasterRelated))
                {
                    sModulesValidToFire.Add(RandomEventType.DisasterRelated);
                }

                if (kEnableDebugMode)
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine("Lyralei's Randomizer Mod:");
                    sb.AppendLine("The following Module(s) will run through the requirements check, for " + sim.FirstName + ":");

                    foreach (RandomEventType type in sModulesValidToFire)
                    {
                        sb.AppendLine("     - " + type.ToString());
                    }

                    StyledNotification.Format format = new StyledNotification.Format(sb.ToString(), StyledNotification.NotificationStyle.kDebugAlert);
                    StyledNotification.Show(format);
                }
                UpdateListDependingOnCooldowns();
                UpdateListDependingOnRarityType();


                // In case all the modules can't be chosen for today...
                if (sModulesValidToFire.Count == 0)
                {
                    if (kEnableDebugMode)
                    {
                        StringBuilder sb = new StringBuilder();

                        sb.AppendLine("Lyralei's Randomizer Mod:");
                        sb.AppendLine("Went through all requirement checks, Such as cooldowns and Whether the module had the correct rarity type, but today is not the day for events...");

                        StyledNotification.Format format = new StyledNotification.Format(sb.ToString(), StyledNotification.NotificationStyle.kDebugAlert);
                        StyledNotification.Show(format);
                    }
                    return;
                }
                else
                {
                    if (kEnableDebugMode)
                    {
                        StringBuilder sb = new StringBuilder();

                        sb.AppendLine("Lyralei's Randomizer Mod:");
                        sb.AppendLine("The following Module(s) may get triggered, for " + sim.FirstName + ":");

                        foreach (RandomEventType type in sModulesValidToFire)
                        {
                            sb.AppendLine("     - " + type.ToString());
                        }

                        StyledNotification.Format format = new StyledNotification.Format(sb.ToString(), StyledNotification.NotificationStyle.kDebugAlert);
                        StyledNotification.Show(format);
                    }
                }

                RandomEventType mType = RandomUtil.GetRandomObjectFromList(sModulesValidToFire);

                // CASES:
                // Module has multiple candidates to return
                // Module only had one candidate, which can no longer be chosen...

                EventTypeRandomizer randomEvent = FindRandomEvent(mType, sim);

                if (randomEvent == null)
                {
                    if (kEnableDebugMode)
                    {
                        StringBuilder sb = new StringBuilder();

                        sb.AppendLine("Lyralei's Randomizer Mod:");
                        sb.AppendLine("Random event chosen returned invalid...");

                        StyledNotification.Format format = new StyledNotification.Format(sb.ToString(), StyledNotification.NotificationStyle.kDebugAlert);
                        StyledNotification.Show(format);
                    }
                    sModulesValidToFire.Clear();

                    return;
                }
                FireChosenEvent(randomEvent, sim, mTarget);
                sModulesValidToFire.Clear();
            }
            catch(Exception ex)
            {
                printException(ex);
            }
        }



        private static float GetRightCooldownForModule(RandomEventType type)
        {
            switch (type)
            {
                case RandomEventType.WorkRelated:
                    return kDaysCooldownForWorkOptions;
                case RandomEventType.GeneralRelated:
                    return kDaysCooldownForGeneralOption;
                case RandomEventType.DeathRelated:
                    return kDaysCooldownForDeathRelatedOption;
                case RandomEventType.DiseaseRelated:
                    return kDaysCooldownForDiseaseRelatedOption;
                case RandomEventType.FriendshipRelated:
                    return kDaysCooldownForFriendshipRelatedOption;
                case RandomEventType.FameRelated:
                    return kDaysCooldownForFameRelatedOption;
                case RandomEventType.EnemyRelated:
                    return kDaysCooldownForEnemyRelatedOption;
                case RandomEventType.RelationshipRelated:
                    return kDaysCooldownForRelationshipRelatedOption;
                case RandomEventType.MoneyRelated:
                    return kDaysCooldownForMoneyRelatedOption;
                case RandomEventType.SchoolRelated:
                    return kDaysCooldownForSchoolRelatedOption;
                case RandomEventType.DisasterRelated:
                    return kDaysCooldownForDisasterRelatedOption;
            }
            return kDaysCooldownForGeneralOption;
        }

        private static void UpdateListDependingOnCooldowns()
        {
            if (sModulesValidToFire != null && sModulesValidToFire.Count > 0)
            {
                // If Cooldown is day 28 + 8 days (cooldown) = 36
                // If we're at day 30, then don't let it go through
                // if we're at day 39, then let it through.

                int howManyRemovedDebug = 0;
                int countBefore = sModulesValidToFire.Count;


                float Cooldown = 0;

                for(int i = 0; i < sModulesValidToFire.Count; i++)
                {
                    if (!sTimePassed.ContainsKey(sModulesValidToFire[i])) { continue; }

                    Cooldown = sTimePassed[sModulesValidToFire[i]] + GetRightCooldownForModule(sModulesValidToFire[i]);

                    if (mDayCounter < Cooldown)
                    {
                        sModulesValidToFire.Remove(sModulesValidToFire[i]);
                        Cooldown = 0;
                        howManyRemovedDebug++;
                    }
                }


                if (kEnableDebugMode)
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine("Lyralei's Randomizer Mod:");
                    sb.AppendLine("Removed " + howManyRemovedDebug.ToString() + " from the list (was: " + countBefore.ToString() + ")");

                    StyledNotification.Format format = new StyledNotification.Format(sb.ToString(), StyledNotification.NotificationStyle.kDebugAlert);
                    StyledNotification.Show(format);
                }
            }
        }


        public static string RarityChosen = "COMMON";

        private static void UpdateListDependingOnRarityType()
        {
            if (sModulesValidToFire != null && sModulesValidToFire.Count > 0)
            {
                List<RandomEventType> TypeToRemove = new List<RandomEventType>();
                int diceRoll = RandomUtil.GetInt(100);

                if(diceRoll >= 60)
                {
                    RarityChosen = "COMMON";
                }
                else if(diceRoll < 60 && diceRoll > 15)
                {
                    RarityChosen = "UNCOMMON";
                }
                else if( diceRoll <= 15)
                {
                    RarityChosen = "RARE";
                }

                foreach (RandomEventType type in sModulesValidToFire)
                {
                    if (sTypeOfRarities[type] != null && sTypeOfRarities[type].Count > 0 )
                            //&& (!sTypeOfRarities[type].Contains(RarityChosen)))
                    {
                        bool hasRarity = false;
                        for(int i = 0; i < sTypeOfRarities[type].Count; i++)
                        {

                            if (sTypeOfRarities[type][i] == RarityChosen)
                            {
                                hasRarity = true;
                                break;
                            }
                        }

                        if (!hasRarity)
                        {
                            TypeToRemove.Add(type);
                        }
                    }
                }

                if (TypeToRemove.Count > 0)
                {
                    if (kEnableDebugMode)
                    {
                        StringBuilder sb = new StringBuilder();

                        sb.AppendLine("Lyralei's Randomizer Mod:");
                        sb.AppendLine("Rarity was: " + RarityChosen);

                        sb.AppendLine("Removing " + TypeToRemove.Count.ToString() + " from the list (was: " + sModulesValidToFire.Count.ToString() + ")");
                        
                        StyledNotification.Format format = new StyledNotification.Format(sb.ToString(), StyledNotification.NotificationStyle.kDebugAlert);
                        StyledNotification.Show(format);
                    }

                    foreach (RandomEventType type in TypeToRemove)
                    {
                        sModulesValidToFire.Remove(type);
                    }
                }
            }
        }

        private static EventTypeRandomizer FindRandomEvent(RandomEventType type, Sim sim)
        {
            switch (type)
            {
                case RandomEventType.WorkRelated:
                    return FindProperEvent(CareerRelatedOptions, sim);
                case RandomEventType.GeneralRelated:
                    return FindProperEvent(GeneralRelatedOptions, sim);
                case RandomEventType.DeathRelated:
                    return FindProperEvent(DeathRelatedOptions, sim);
                case RandomEventType.DiseaseRelated:
                    return FindProperEvent(DiseaseRelatedOptions, sim);
                case RandomEventType.FriendshipRelated:
                    return FindProperEvent(FriendshipRelatedOptions, sim);
                case RandomEventType.FameRelated:
                    return FindProperEvent(FameRelatedOptions, sim);
                case RandomEventType.EnemyRelated:
                    return FindProperEvent(EnemyRelatedOptions, sim);
                case RandomEventType.RelationshipRelated:
                    return FindProperEvent(RelationshipRelatedOptions, sim);
                case RandomEventType.MoneyRelated:
                    return FindProperEvent(MoneyRelatedOptions, sim);
                case RandomEventType.SchoolRelated:
                    return FindProperEvent(SchoolRelatedOptions, sim);
                case RandomEventType.DisasterRelated:
                    return FindProperEvent(DisasterRelatedOptions, sim);
            }
            return null;
        }

        private static Sim PrepareForNecessaryTarget(EventTypeRandomizer Type, Sim sim)
        {
            Sim mTarget = null;
            switch (Type.type)
            {
                case RandomEventType.WorkRelated:

                    if (Type.mNeedsInvolvedTarget)
                    {
                        List<Sim> mCoworkers = new List<Sim>();
                        foreach(SimDescription simDescription in sim.CareerManager.Occupation.Coworkers)
                        {
                            if(simDescription == null || simDescription.CreatedSim == null) { continue; }
                            if(!RandomizerBasicHelperFunctions.isAgeAppriopiate(simDescription.CreatedSim, sim, false))
                            {
                                mCoworkers.Add(simDescription.CreatedSim);
                            }
                        }
                        if (mCoworkers.Count == 0)
                        {
                            return null;
                        }
                        return mTarget = RandomUtil.GetRandomObjectFromList(mCoworkers);
                    }
                    break;
                case RandomEventType.FriendshipRelated:
                    if (Type.mNeedsInvolvedTarget)
                    {
                        Relationship[] relationships = Relationship.GetRelationships(sim);
                        List<Sim> mFriends = new List<Sim>();
                        if(relationships == null && relationships.Length == 0) { return null; }
                        TraitNames[] traitsCompatible = new TraitNames[5] { TraitNames.Dramatic, TraitNames.Evil, TraitNames.EvilChip, TraitNames.Diva, TraitNames.MeanSpirited };

                        foreach (Relationship rel in relationships)
                        {
                            if (rel == null) { continue; }

                            if (!RandomizerBasicHelperFunctions.isAgeAppriopiate(rel.SimDescriptionB.CreatedSim, rel.SimDescriptionA.CreatedSim, false))
                            {
                                continue;
                            }
                            // If they're the same sim...
                            if (rel.SimDescriptionA.SimDescriptionId == sim.SimDescription.SimDescriptionId && rel.SimDescriptionB.SimDescriptionId == sim.SimDescription.SimDescriptionId)
                            {
                                continue;
                            }

                            if (Type.mIsForNastySims && rel.AreFriends())
                            {

                                if (rel.SimDescriptionA.SimDescriptionId != sim.SimDescription.SimDescriptionId
                                    && !rel.SimDescriptionA.CreatedSim.IsInActiveHousehold && rel.SimDescriptionA.CreatedSim.TraitManager.HasAnyElement(traitsCompatible))
                                {
                                    mFriends.Add(rel.SimDescriptionA.CreatedSim);
                                }
                                if (rel.SimDescriptionB.SimDescriptionId != sim.SimDescription.SimDescriptionId
                                    && !rel.SimDescriptionB.CreatedSim.IsInActiveHousehold && rel.SimDescriptionB.CreatedSim.TraitManager.HasAnyElement(traitsCompatible))
                                {
                                    mFriends.Add(rel.SimDescriptionB.CreatedSim);
                                }
                            }
                            else if (!Type.mIsForNastySims && rel.AreFriends())
                            {
                                if (rel.SimDescriptionA.SimDescriptionId != sim.SimDescription.SimDescriptionId
                                    && !rel.SimDescriptionA.CreatedSim.IsInActiveHousehold)
                                {
                                    mFriends.Add(rel.SimDescriptionA.CreatedSim);
                                }
                                if (rel.SimDescriptionB.SimDescriptionId != sim.SimDescription.SimDescriptionId
                                    && !rel.SimDescriptionB.CreatedSim.IsInActiveHousehold)
                                {
                                    mFriends.Add(rel.SimDescriptionB.CreatedSim);
                                }
                            }
                        }

                        if (mFriends.Count == 0)
                        {
                            return null;
                        }
                        return mTarget = RandomUtil.GetRandomObjectFromList(mFriends);
                    }
                    break;
                case RandomEventType.FameRelated:
                    if (Type.mNeedsInvolvedTarget)
                    {
                        Relationship[] relationships = Relationship.GetRelationships(sim);
                        List<Sim> mFriends = new List<Sim>();
                        if (relationships == null && relationships.Length == 0) { return null; }
                        TraitNames[] traitsCompatible = new TraitNames[5] { TraitNames.Dramatic, TraitNames.Evil, TraitNames.EvilChip, TraitNames.Diva, TraitNames.MeanSpirited };

                        foreach (IMiniRelationship item in Relationship.GetAllInAndOutOfWorld(sim.SimDescription))
                        {
                            ulong otherSimDescriptionId = item.GetOtherSimDescriptionId(sim.SimDescription);
                            SimDescription simDescription = SimDescription.Find(otherSimDescriptionId);
                            if (simDescription == null)
                            {
                                // Continue because the target isn't in the world.
                                continue;
                            }
                            else if (simDescription.IsCelebrity)
                            {
                                if (Type.mIsForNastySims)
                                {
                                    mFriends.Add(simDescription.CreatedSim);
                                }
                                else if (Type.mIsRomantic)
                                {
                                    mFriends.Add(simDescription.CreatedSim);
                                }
                                else if(!Type.mIsForNastySims && !Type.mIsRomantic)
                                {
                                    mFriends.Add(simDescription.CreatedSim);
                                }
                            }
                        }
                        if (mFriends.Count == 0)
                        {
                            return null;
                        }

                        return mTarget = RandomUtil.GetRandomObjectFromList(mFriends);
                    }
                    break;
                case RandomEventType.EnemyRelated:
                    if (Type.mNeedsInvolvedTarget)
                    {
                        Relationship[] relationships = Relationship.GetRelationships(sim);
                        List<Sim> mFriends = new List<Sim>();
                        if (relationships == null && relationships.Length == 0) { return null; }
                        TraitNames[] traitsCompatible = new TraitNames[5] { TraitNames.Dramatic, TraitNames.Evil, TraitNames.EvilChip, TraitNames.Diva, TraitNames.MeanSpirited };

                        foreach (Relationship rel in relationships)
                        {
                            if (rel == null) { continue; }

                            if (!RandomizerBasicHelperFunctions.isAgeAppriopiate(rel.SimDescriptionB.CreatedSim, rel.SimDescriptionA.CreatedSim, false))
                            {
                                continue;
                            }

                            // If they're the same sim...
                            if (rel.SimDescriptionA.SimDescriptionId == sim.SimDescription.SimDescriptionId && rel.SimDescriptionB.SimDescriptionId == sim.SimDescription.SimDescriptionId)
                            {
                                continue;
                            }

                            if (rel.AreEnemies() && !Type.mIsForNastySims)
                            {
                                if (rel.SimDescriptionA.SimDescriptionId != sim.SimDescription.SimDescriptionId)
                                {
                                    mFriends.Add(rel.SimDescriptionA.CreatedSim);
                                }
                                if (rel.SimDescriptionB.SimDescriptionId != sim.SimDescription.SimDescriptionId)
                                {
                                    mFriends.Add(rel.SimDescriptionB.CreatedSim);
                                }
                            }
                            else if (rel.AreEnemies() && Type.mIsForNastySims)
                            {
                                if (rel.SimDescriptionA.SimDescriptionId != sim.SimDescription.SimDescriptionId && rel.SimDescriptionA.TraitManager.HasAnyElement(traitsCompatible))
                                {
                                    mFriends.Add(rel.SimDescriptionA.CreatedSim);
                                }
                                if (rel.SimDescriptionB.SimDescriptionId != sim.SimDescription.SimDescriptionId && rel.SimDescriptionA.TraitManager.HasAnyElement(traitsCompatible))
                                {
                                    mFriends.Add(rel.SimDescriptionB.CreatedSim);
                                }
                            }
                            else if (rel.LTR.CurrentLTR == Sims3.UI.Controller.LongTermRelationshipTypes.Disliked && rel.CurrentLTRLiking <= -75 && Type.mIsForNastySims)
                            {
                                if (rel.SimDescriptionA.SimDescriptionId != sim.SimDescription.SimDescriptionId && rel.SimDescriptionA.TraitManager.HasAnyElement(traitsCompatible))
                                {
                                    mFriends.Add(rel.SimDescriptionA.CreatedSim);
                                }
                                if (rel.SimDescriptionB.SimDescriptionId != sim.SimDescription.SimDescriptionId && rel.SimDescriptionA.TraitManager.HasAnyElement(traitsCompatible))
                                {
                                    mFriends.Add(rel.SimDescriptionB.CreatedSim);
                                }
                            }
                        }
                        if (mFriends.Count == 0)
                        {
                            return null;
                        }

                        return mTarget = RandomUtil.GetRandomObjectFromList(mFriends);
                    }
                    break;
                case RandomEventType.RelationshipRelated:
                    if (Type.mNeedsInvolvedTarget)
                    {
                        Relationship[] relationships = Relationship.GetRelationships(sim);
                        List<Sim> mFriends = new List<Sim>();
                        if (relationships == null && relationships.Length == 0) { return null; }
                        TraitNames[] traitsCompatible = new TraitNames[5] { TraitNames.Dramatic, TraitNames.Evil, TraitNames.EvilChip, TraitNames.Diva, TraitNames.MeanSpirited };

                        foreach (Relationship rel in relationships)
                        {
                            if (rel == null) { continue; }

                            if (!RandomizerBasicHelperFunctions.isAgeAppriopiate(rel.SimDescriptionB.CreatedSim, rel.SimDescriptionA.CreatedSim, false))
                            {
                                continue;
                            }

                            // If they're the same sim...
                            if (rel.SimDescriptionA.SimDescriptionId == sim.SimDescription.SimDescriptionId && rel.SimDescriptionB.SimDescriptionId == sim.SimDescription.SimDescriptionId)
                            {
                                continue;
                            }

                            if (Type.mIsRomantic && rel.AreFriendsOrRomantic() && !Type.mIsForNastySims)
                            {
                                if (rel.SimDescriptionA.SimDescriptionId != sim.SimDescription.SimDescriptionId)
                                {
                                    mFriends.Add(rel.SimDescriptionA.CreatedSim);
                                }
                                if (rel.SimDescriptionB.SimDescriptionId != sim.SimDescription.SimDescriptionId)
                                {
                                    mFriends.Add(rel.SimDescriptionB.CreatedSim);
                                }
                            }
                            else if(Type.mIsRomantic && rel.AreFriendsOrRomantic() && Type.mIsForNastySims)
                            {
                                if (rel.SimDescriptionA.SimDescriptionId != sim.SimDescription.SimDescriptionId && rel.SimDescriptionA.TraitManager.HasAnyElement(traitsCompatible))
                                {
                                    mFriends.Add(rel.SimDescriptionA.CreatedSim);
                                }
                                if (rel.SimDescriptionB.SimDescriptionId != sim.SimDescription.SimDescriptionId && rel.SimDescriptionA.TraitManager.HasAnyElement(traitsCompatible))
                                {
                                    mFriends.Add(rel.SimDescriptionB.CreatedSim);
                                }
                            }
                            if (!Type.mIsRomantic && rel.AreFriendsOrRomantic() && !Type.mIsForNastySims)
                            {
                                if (rel.SimDescriptionA.SimDescriptionId != sim.SimDescription.SimDescriptionId)
                                {
                                    mFriends.Add(rel.SimDescriptionA.CreatedSim);
                                }
                                if (rel.SimDescriptionB.SimDescriptionId != sim.SimDescription.SimDescriptionId)
                                {
                                    mFriends.Add(rel.SimDescriptionB.CreatedSim);
                                }
                            }
                            else if (!Type.mIsRomantic && rel.AreFriendsOrRomantic() && Type.mIsForNastySims)
                            {
                                if (rel.SimDescriptionA.SimDescriptionId != sim.SimDescription.SimDescriptionId && rel.SimDescriptionA.TraitManager.HasAnyElement(traitsCompatible))
                                {
                                    mFriends.Add(rel.SimDescriptionA.CreatedSim);
                                }
                                if (rel.SimDescriptionB.SimDescriptionId != sim.SimDescription.SimDescriptionId && rel.SimDescriptionA.TraitManager.HasAnyElement(traitsCompatible))
                                {
                                    mFriends.Add(rel.SimDescriptionB.CreatedSim);
                                }
                            }
                            else if(Type.mIsRomantic && rel.CurrentLTR == Sims3.UI.Controller.LongTermRelationshipTypes.RomanticInterest && !Type.mIsForNastySims)
                            {
                                if (rel.SimDescriptionA.SimDescriptionId != sim.SimDescription.SimDescriptionId)
                                {
                                    mFriends.Add(rel.SimDescriptionA.CreatedSim);
                                }
                                if (rel.SimDescriptionB.SimDescriptionId != sim.SimDescription.SimDescriptionId)
                                {
                                    mFriends.Add(rel.SimDescriptionB.CreatedSim);
                                }
                            }
                            else if (Type.mIsRomantic && rel.CurrentLTR == Sims3.UI.Controller.LongTermRelationshipTypes.RomanticInterest && Type.mIsForNastySims)
                            {
                                if (rel.SimDescriptionA.SimDescriptionId != sim.SimDescription.SimDescriptionId && rel.SimDescriptionA.TraitManager.HasAnyElement(traitsCompatible))
                                {
                                    mFriends.Add(rel.SimDescriptionA.CreatedSim);
                                }
                                if (rel.SimDescriptionB.SimDescriptionId != sim.SimDescription.SimDescriptionId && rel.SimDescriptionA.TraitManager.HasAnyElement(traitsCompatible))
                                {
                                    mFriends.Add(rel.SimDescriptionB.CreatedSim);
                                }
                            }
                            else if (!Type.mIsRomantic && rel.CurrentLTR == Sims3.UI.Controller.LongTermRelationshipTypes.RomanticInterest && !Type.mIsForNastySims)
                            {
                                if (rel.SimDescriptionA.SimDescriptionId != sim.SimDescription.SimDescriptionId)
                                {
                                    mFriends.Add(rel.SimDescriptionA.CreatedSim);
                                }
                                if (rel.SimDescriptionB.SimDescriptionId != sim.SimDescription.SimDescriptionId)
                                {
                                    mFriends.Add(rel.SimDescriptionB.CreatedSim);
                                }
                            }
                            else if (!Type.mIsRomantic && rel.CurrentLTR == Sims3.UI.Controller.LongTermRelationshipTypes.RomanticInterest && Type.mIsForNastySims)
                            {
                                if (rel.SimDescriptionA.SimDescriptionId != sim.SimDescription.SimDescriptionId && rel.SimDescriptionA.TraitManager.HasAnyElement(traitsCompatible))
                                {
                                    mFriends.Add(rel.SimDescriptionA.CreatedSim);
                                }
                                if (rel.SimDescriptionB.SimDescriptionId != sim.SimDescription.SimDescriptionId && rel.SimDescriptionA.TraitManager.HasAnyElement(traitsCompatible))
                                {
                                    mFriends.Add(rel.SimDescriptionB.CreatedSim);
                                }
                            }
                        }

                        if (mFriends.Count == 0)
                        {
                            return null;
                        }

                        return mTarget = RandomUtil.GetRandomObjectFromList(mFriends);
                    }

                    break;
                case RandomEventType.SchoolRelated:
                    if (Type.mNeedsInvolvedTarget)
                    {
                        List<Sim> mCoworkers = new List<Sim>();
                        foreach (SimDescription simDescription in sim.School.Coworkers)
                        {
                            if (simDescription == null || simDescription.CreatedSim == null) { continue; }
                            if (!RandomizerBasicHelperFunctions.isAgeAppriopiate(simDescription.CreatedSim, sim, false))
                            {
                                mCoworkers.Add(simDescription.CreatedSim);
                            }
                        }
                        return mTarget = RandomUtil.GetRandomObjectFromList(mCoworkers);
                    }
                    break;
            }
            return mTarget;
        }

        public static string mReasonWhyCouldntFire = "";

        private static bool TestRandomEvent(EventTypeRandomizer kChosenEvent, Sim actor, Sim target)
        {
            RandomizerFunctionCallback.EventTypeRandomizerFunctionTest proceduralEffectDelegate = Delegate.CreateDelegate(typeof(RandomizerFunctionCallback.EventTypeRandomizerFunctionTest), kChosenEvent.TestMethod) as RandomizerFunctionCallback.EventTypeRandomizerFunctionTest;
            if(proceduralEffectDelegate == null || !proceduralEffectDelegate(actor, target))
            {
                if (kEnableDebugMode)
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine("Lyralei's Randomizer Mod:");
                    sb.AppendLine("Event test failed. This can be intentional by the creator.");
                    sb.AppendLine("NOTE: the code will now try finding a better event. This does NOT mean the event is broken!");

                    sb.AppendLine();
                    if(!string.IsNullOrEmpty(mReasonWhyCouldntFire))
                    {
                        sb.AppendLine("Reason Added by creator: " + mReasonWhyCouldntFire);
                    }
                    sb.AppendLine("Type: " + kChosenEvent.type.ToString());
                    sb.AppendLine("Function name: " + kChosenEvent.TestMethod.ToString());

                    StyledNotification.Format format = new StyledNotification.Format(sb.ToString(), StyledNotification.NotificationStyle.kDebugAlert);
                    StyledNotification.Show(format);
                }
                return false;
            }
            return true;
        }

        private static bool DEBUGTestRandomEvent(MethodInfo method, Sim actor, Sim target)
        {
            RandomizerFunctionCallback.EventTypeRandomizerFunctionTest proceduralEffectDelegate = Delegate.CreateDelegate(typeof(RandomizerFunctionCallback.EventTypeRandomizerFunctionTest), method) as RandomizerFunctionCallback.EventTypeRandomizerFunctionTest;
            if (proceduralEffectDelegate == null || !proceduralEffectDelegate(actor, target))
            {
                return false;
            }
            return true;
        }


        public static void FireChosenEvent(EventTypeRandomizer kChosenEvent, Sim sim, Sim optionalTarget)
        {
            // Store which option we've got 
            if (kChosenEvent.mNeedsOptionMenu && !string.IsNullOrEmpty(kChosenEvent.mOptionThreeText))
            {
                //print("Should fire three button dialog");
                ThreeButtonDialogRandomizer.ButtonPressed mBtnPressed = ThreeButtonDialogRandomizer.ButtonPressed.FirstButton;
                if (optionalTarget != null && kChosenEvent.mNeedsInvolvedTarget)
                {
                    mBtnPressed = ThreeButtonDialogRandomizer.Show(Localization.LocalizeString(kChosenEvent.mCaption, new object[] { optionalTarget }), kChosenEvent.mOptionOneText, kChosenEvent.mOptionTwoText, kChosenEvent.mOptionThreeText);
                }
                else
                {
                    
                    mBtnPressed = ThreeButtonDialogRandomizer.Show(Localization.LocalizeString(kChosenEvent.mCaption), kChosenEvent.mOptionOneText, kChosenEvent.mOptionTwoText, kChosenEvent.mOptionThreeText);
                }

                switch (mBtnPressed)
                {
                    case ThreeButtonDialogRandomizer.ButtonPressed.FirstButton:
                        if (kChosenEvent.OptionOneFunction != null)
                        {
                            FireTheFunction(kChosenEvent, kChosenEvent.OptionOneFunction, sim, optionalTarget != null ? optionalTarget : null);
                        }
                        break;
                    case ThreeButtonDialogRandomizer.ButtonPressed.SecondButton:
                        if (kChosenEvent.OptionTwoFunction != null)
                        {
                            FireTheFunction(kChosenEvent, kChosenEvent.OptionTwoFunction, sim, optionalTarget != null ? optionalTarget : null);
                        }
                        break;
                    case ThreeButtonDialogRandomizer.ButtonPressed.ThirdButton:
                        if (kChosenEvent.OptionThreeFunction != null)
                        {
                            FireTheFunction(kChosenEvent, kChosenEvent.OptionThreeFunction, sim, optionalTarget != null ? optionalTarget : null);
                        }
                        break;
                    default:
                        break;
                }
            }
            else if (kChosenEvent.mNeedsOptionMenu && !string.IsNullOrEmpty(kChosenEvent.mOptionTwoText) && string.IsNullOrEmpty(kChosenEvent.mOptionThreeText))
            {
                bool mAcceptCancel = false;
                if (optionalTarget != null && kChosenEvent.mNeedsInvolvedTarget)
                {
                    mAcceptCancel = TwoButtonRandomizerDialog.Show(Localization.LocalizeString(kChosenEvent.mCaption, new object[] { optionalTarget }), kChosenEvent.mOptionOneText, kChosenEvent.mOptionTwoText);
                }
                else
                {
                    mAcceptCancel = TwoButtonRandomizerDialog.Show(Localization.LocalizeString(kChosenEvent.mCaption), kChosenEvent.mOptionOneText, kChosenEvent.mOptionTwoText);
                }
                //bool mAcceptCancel = TwoButtonDialog.Show(optionalTarget != null ? String.Format(kChosenEvent.mCaption, optionalTarget.FullName) : kChosenEvent.mCaption, kChosenEvent.mOptionOneText, kChosenEvent.mOptionTwoText);

                if (mAcceptCancel)
                {
                    if (kChosenEvent.OptionOneFunction != null)
                    {
                        FireTheFunction(kChosenEvent, kChosenEvent.OptionOneFunction, sim, optionalTarget != null ? optionalTarget : null);
                    }
                }
                else
                {
                    if (kChosenEvent.OptionTwoFunction != null)
                    {
                        FireTheFunction(kChosenEvent, kChosenEvent.OptionTwoFunction, sim, optionalTarget != null ? optionalTarget : null);
                    }
                }
            }
            else
            {
                if (optionalTarget != null && kChosenEvent.mNeedsInvolvedTarget && !string.IsNullOrEmpty(kChosenEvent.mCaption))
                {
                    StyledNotification.Show(new StyledNotification.Format(Localization.LocalizeString(kChosenEvent.mCaption, new object[] { optionalTarget }), sim.ObjectId, StyledNotification.NotificationStyle.kSimTalking));
                }
                else
                {
                    if(!string.IsNullOrEmpty(kChosenEvent.mCaption))
                    {
                        StyledNotification.Show(new StyledNotification.Format(Localization.LocalizeString(kChosenEvent.mCaption), sim.ObjectId, StyledNotification.NotificationStyle.kSimTalking));
                    }
                }

                if (kChosenEvent.OptionGeneralFunction != null)
                {
                    FireTheFunction(kChosenEvent, kChosenEvent.OptionGeneralFunction, sim, optionalTarget != null ? optionalTarget : null);
                }
            }
        }

        public static void FireTheFunction(EventTypeRandomizer kChosenEvent, MethodInfo method, Sim actor, Sim target)
        {
            try
            {
                if (kEnableDebugMode)
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine("Lyralei's Randomizer Mod:");
                    sb.AppendLine("Event is being fired!");
                    sb.AppendLine();
                    sb.AppendLine("Sim: " + actor.FullName);
                    sb.AppendLine("Type: " + kChosenEvent.type.ToString());
                    sb.AppendLine("Function name: " + method.ToString());

                    StyledNotification.Format format = new StyledNotification.Format(sb.ToString(), StyledNotification.NotificationStyle.kDebugAlert);
                    StyledNotification.Show(format);
                }
                UpdateLastFiredDictionary(kChosenEvent);

                RandomizerFunctionCallback.EventTypeRandomizerFunctionOption proceduralEffectDelegate = Delegate.CreateDelegate(typeof(RandomizerFunctionCallback.EventTypeRandomizerFunctionOption), method) as RandomizerFunctionCallback.EventTypeRandomizerFunctionOption;
                proceduralEffectDelegate(actor, target);
            }
            catch (Exception ex)
            {
                printException(ex);
            }
        }

        private static void UpdateLastFiredDictionary(EventTypeRandomizer ChosenEvent)
        {
            if(!sTimePassed.ContainsKey(ChosenEvent.type))
            {
                sTimePassed.Add(ChosenEvent.type, 0);
            }
            sTimePassed[ChosenEvent.type] = mDayCounter;

            if (kShouldCheckToNotFireTheSameEvent) { mLastItem[ChosenEvent.type] = ChosenEvent; }
        }


        /// <summary>
        /// We only use this version of firing the event for Debugging only!
        /// </summary>
        /// <param name="method"></param>
        /// <param name="actor"></param>
        /// <param name="target"></param>
        public static void FireTheFunction(MethodInfo method, Sim actor, Sim target)
        {
            try
            {
                if (kEnableDebugMode)
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine("Lyralei's Randomizer Mod:");
                    sb.AppendLine("Event is being fired!");
                    sb.AppendLine();
                    sb.AppendLine("Function name: " + method.ToString());

                    StyledNotification.Format format = new StyledNotification.Format(sb.ToString(), StyledNotification.NotificationStyle.kDebugAlert);
                    StyledNotification.Show(format);
                }

                RandomizerFunctionCallback.EventTypeRandomizerFunctionOption proceduralEffectDelegate = Delegate.CreateDelegate(typeof(RandomizerFunctionCallback.EventTypeRandomizerFunctionOption), method) as RandomizerFunctionCallback.EventTypeRandomizerFunctionOption;
                proceduralEffectDelegate(actor, target);

            }
            catch (Exception ex)
            {
                printException(ex);
            }
        }

        /* SAVE DATA STRUCTURE:
         * 
         * 1. DateAlarm
         * 2. Amount Of items In sTimePassed
         * 3.       Type event hashed
         * 4.       Day it was fired
         * 5. Amount in mLastItem
         * 6.       Type event hashed
         * 7.           bool for Whether it is a general method or a button method.
         * 8.           method string.
         * 
         */

        private static void ImportSaveData()
        {
            if (sRandomizerSaveData == null)
            {
                sRandomizerSaveData = "";
                return;
            }

            if (!string.IsNullOrEmpty(sRandomizerSaveData))
            {
                try
                {
                    if (sTimePassed.Count > 0) { sTimePassed.Clear(); }
                    if (mLastItem.Count > 0) { mLastItem.Clear(); }

                    StringBuilder sb = new StringBuilder();

                    string str = sRandomizerSaveData;
                    byte[] data = FromHex(str);

                    MemoryStream input = new MemoryStream(data);
                    BinaryReader reader = new BinaryReader(input);

                    mDayCounter = reader.ReadUInt32();
                    sb.AppendLine("Day Counter: " + mDayCounter.ToString());

                    int indexTimePassed = reader.ReadInt32();
                    sb.AppendLine("TimePassed amount: " + indexTimePassed.ToString());

                    for (int i = 0; i < indexTimePassed; i++)
                    {
                        RandomEventType type = (RandomEventType)Enum.Parse(typeof(RandomEventType), reader.ReadString());
                        sb.AppendLine("     Type: " + type.ToString());

                        uint daySaved = reader.ReadUInt32();
                        sb.AppendLine("     Day: " + daySaved.ToString());

                        if(sTimePassed.ContainsKey(type))
                        {
                            sTimePassed[type] = daySaved;
                        }
                        else
                        {
                            sTimePassed.Add(type, daySaved);
                        }
                    }

                    int indexLastItem = reader.ReadInt32();
                    sb.AppendLine("mLastItem amount: " + indexLastItem.ToString());

                    for (int i = 0; i < indexLastItem; i++)
                    {
                        RandomEventType type = (RandomEventType)Enum.Parse(typeof(RandomEventType), reader.ReadString());
                        sb.AppendLine("     Type: " + type.ToString());

                        bool wasGeneral = reader.ReadBoolean();
                        sb.AppendLine("     Was General? : " + wasGeneral.ToString());

                        string method = reader.ReadString();
                        EventTypeRandomizer eventToReturn = null;

                        if (wasGeneral)
                        {
                            foreach(EventTypeRandomizer randomEvent in AllRandomOptions)
                            {
                                if(randomEvent.OptionGeneralFunction.ToString() == method)
                                {
                                    eventToReturn = randomEvent;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            foreach (EventTypeRandomizer randomEvent in AllRandomOptions)
                            {
                                if (randomEvent.OptionOneFunction.ToString() == method)
                                {
                                    eventToReturn = randomEvent;
                                    break;
                                }
                            }
                        }
                        sb.AppendLine("         Last Done Event: " + eventToReturn.ToString());

                        if(mLastItem.ContainsKey(type))
                        {
                            mLastItem[type] = eventToReturn;
                        }
                        else
                        {
                            mLastItem.Add(type, eventToReturn);
                        }
                    }

                    //StyledNotification.Format format = new StyledNotification.Format(sb.ToString(), StyledNotification.NotificationStyle.kDebugAlert);
                    //StyledNotification.Show(format);

                    reader.Close();
                    input.Close();
                    //WriteErrorXMLFile("ImportSaveDataRandomizer_", null, sb.ToString());
                }
                catch (Exception ex)
                {
                    printException(ex);
                }
            }
            else
            {
                if (kEnableDebugMode)
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine("Lyralei's Randomizer Mod");
                    sb.AppendLine("Tried Importing the saved data, but the saved data was empty! This could be as expected, but if not, you know you need to debug more because this thing is just neveer bug free and omfg work for once");

                    StyledNotification.Format format = new StyledNotification.Format(sb.ToString(), StyledNotification.NotificationStyle.kDebugAlert);
                    StyledNotification.Show(format);
                }
            }
        }

        private static void ExportSaveData()
        {
            if (sRandomizerSaveData == null)
            {
                sRandomizerSaveData = "";
            }

            try
            {
                MemoryStream memorystream = new MemoryStream();
                BinaryWriter bw = new BinaryWriter(memorystream);
                bw.Write(mDayCounter);

                bw.Write(sTimePassed.Count);
                foreach(KeyValuePair<RandomEventType, uint> Cooldown in sTimePassed)
                {
                    bw.Write(Cooldown.Key.ToString());
                    bw.Write(Cooldown.Value);
                }

                bw.Write(mLastItem.Count);
                foreach (KeyValuePair<RandomEventType, EventTypeRandomizer> Event in mLastItem)
                {
                    bw.Write(Event.Key.ToString());
                    if(Event.Value.OptionGeneralFunction != null )
                    {
                        // is general method
                        bw.Write(true);
                        bw.Write(Event.Value.OptionGeneralFunction.ToString());
                    }
                    else
                    {
                        bw.Write(false);
                        bw.Write(Event.Value.OptionOneFunction.ToString());
                    }
                }
                bw.Close();

                byte[] bytes = memorystream.ToArray();
                sRandomizerSaveData = BitConverter.ToString(bytes);

                //print("Save was success!");
            }
            catch (Exception ex)
            {
                printException(ex);
            }
            
        }

        public static byte[] FromHex(string hex)
        {
            hex = hex.Replace("-", "");
            byte[] raw = new byte[hex.Length / 2];
            for (int i = 0; i < raw.Length; i++)
            {
                raw[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            }
            return raw;
        }

        public static void OnWorldLoadFinished(object sender, EventArgs e)
        {
            try
            {
                // Get all sims in town
                Sim[] objects = Sims3.Gameplay.Queries.GetObjects<Sim>();

                for (int i = 0; i < objects.Length; i++)
                {
                    Sim sim = objects[i];
                    if (sim != null)
                    {
                        // Check if sim is in the world, if it isn't a pet or if it isn't a service in the world
                        if (sim.InWorld || !sim.IsPet || !sim.IsPerformingAService)
                        {
                            sAllSimsInWorld.Add(sim);
                            AddInteractionsSims(sim);
                            sSimIsInActiveFamily = EventTracker.AddListener(EventTypeId.kHouseholdSelected, new ProcessEventDelegate(OnHouseholdSelected));
                        }
                    }
                }

                // Take active household for now as the event triggerer...
                if (Household.ActiveHousehold != null)
                {
                    ImportSaveData();
                    SetupAlarm(Household.ActiveHousehold);
                    mCurrentHousehold = Household.ActiveHousehold;

                    if(kEnableDebugMode)
                    {
                        StringBuilder sb = new StringBuilder();

                        sb.AppendLine("Lyralei's Randomizer Mod Has successfully loaded!");
                        sb.AppendLine("The following module types are recognised: ");
                        
                        foreach(RandomEventType type in mAllInstalledModules)
                        {
                            sb.AppendLine("- " + type.ToString());
                        }

                        StyledNotification.Format format = new StyledNotification.Format(sb.ToString(), StyledNotification.NotificationStyle.kDebugAlert);
                        StyledNotification.Show(format);
                    }
                }
            }
            catch (Exception ex)
            {
                printException(ex);
            }
        }

        public static void AddInteractionsSims(Sim sim)
        {
            sim.AddInteraction(TestRandomEventDEBUG.Singleton);
            sim.AddInteraction(UpdateDayRandomizer.Singleton);
            sim.AddInteraction(ShowDayRandomizer.Singleton);
        }

        public static ListenerAction OnHouseholdSelected(Event e)
        {
            if (mCurrentHousehold != null && mCurrentHousehold.HouseholdId != Household.ActiveHousehold.HouseholdId)
            {
                // Handle resetting data.
                if (mDoRandomizerCheck != null && mDoRandomizerCheck.ContainsKey(mCurrentHousehold.HouseholdId))
                {
                    mCurrentHousehold.LotHome.RemoveAlarm(mDoRandomizerCheck[mCurrentHousehold.HouseholdId]);
                    mDoRandomizerCheck.Remove(mCurrentHousehold.HouseholdId);
                }
            }

            if(Household.ActiveHousehold != null)
            {
                SetupAlarm(Household.ActiveHousehold);
                mCurrentHousehold = Household.ActiveHousehold;
            }
            return ListenerAction.Keep;
        }

        public static void print(string text)
        {
            SimpleMessageDialog.Show("Lyralei's Randomizer [CORE module]", text);
        }

        public static void printException(Exception e)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("An exception ("+ e.GetType().Name+ ") occurred.");
            sb.AppendLine("   Message:\n"+ e.Message);
            sb.AppendLine("   Stack Trace:\n   "+ e.StackTrace);
            Exception ie = e.InnerException;
            if (ie != null)
            {
                sb.AppendLine("   The Inner Exception:");
                sb.AppendLine("      Exception Name: "+ ie.GetType().Name);
                sb.AppendLine("      Message: " + ie.Message + "\n");
                sb.AppendLine("      Stack Trace:\n   "+ ie.StackTrace + "\n");
            }
            SimpleMessageDialog.Show("Lyralei's Randomizer [CORE module] Error:", sb.ToString());
        }

        public class TestRandomEventDEBUG : ImmediateInteraction<Sim, Sim>
        {
            public static readonly InteractionDefinition Singleton = new Definition();

            public override bool Run()
            {
                bool Userchosen = TwoButtonRandomizerDialog.Show("Want to test in General?", "true", "false");

                if (Userchosen)
                {
                    FigureOutGoodModules(base.Actor);
                    return true;
                }
                else
                {
                    Userchosen = TwoButtonRandomizerDialog.Show("Want to test Rumors?", "true", "false");
                    if (Userchosen)
                    {
                        MethodInfo method = RandomizerCoreModule.FindMethod("Lyralei.TheRandomizerFriendship.RandomFriendshipFunctions, TheRandomizerBase, StartedRumorAboutFriend");
                        RandomizerCoreModule.FireTheFunction(method, base.Actor, RandomUtil.GetRandomObjectFromList(RandomizerCoreModule.sAllSimsInWorld));
                        return true;
                    }
                    else
                    {
                        Userchosen = TwoButtonRandomizerDialog.Show("Want to test Friendship advice?", "true", "false");

                        if (Userchosen)
                        {
                            MethodInfo method = RandomizerCoreModule.FindMethod("Lyralei.TheRandomizerFriendship.RandomFriendshipFunctions, TheRandomizerBase, CallForAdvice");
                            RandomizerCoreModule.FireTheFunction(method, base.Actor, RandomUtil.GetRandomObjectFromList(RandomizerCoreModule.sAllSimsInWorld));
                            return true;
                        }
                        else
                        {
                            Userchosen = TwoButtonRandomizerDialog.Show("Want to test water outage?", "true", "false");

                            if (Userchosen)
                            {
                                MethodInfo method = RandomizerCoreModule.FindMethod("Lyralei.TheRandomizerGeneral.RandomizerGeneralFunctions, TheRandomizerBase, DoWaterShutdown");
                                RandomizerCoreModule.FireTheFunction(method, base.Actor, RandomUtil.GetRandomObjectFromList(RandomizerCoreModule.sAllSimsInWorld));
                                return true;
                            }
                        }
                    }
                }
                return true;
            }

            [DoesntRequireTuning]
            public sealed class Definition : ImmediateInteractionDefinition<Sim, Sim, TestRandomEventDEBUG>
            {
                public override string GetInteractionName(Sim a, Sim target, InteractionObjectPair interaction)
                {
                    return "[DEBUGGER Randomizer] Test Random Event";
                }

                public override bool Test(Sim a, Sim target, bool isAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
                {
                    return kEnableDebugMode;
                }
            }
        }

        public class UpdateDayRandomizer : ImmediateInteraction<Sim, Sim>
        {
            public static readonly InteractionDefinition Singleton = new Definition();

            public override bool Run()
            {
                mDayCounter = uint.Parse(StringInputDialog.Show("Update Randomizer Day", "How many days should have passed in code", mDayCounter.ToString(), true));
                print("Day counter now set to: " + mDayCounter);
                return true;
            }

            [DoesntRequireTuning]
            public sealed class Definition : ImmediateInteractionDefinition<Sim, Sim, UpdateDayRandomizer>
            {
                public override string GetInteractionName(Sim a, Sim target, InteractionObjectPair interaction)
                {
                    return "[DEBUGGER Randomizer] Update Day for Randomizer ";
                }

                public override bool Test(Sim a, Sim target, bool isAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
                {
                    return kEnableDebugMode;
                }
            }
        }

        public class ShowDayRandomizer : ImmediateInteraction<Sim, Sim>
        {
            public static readonly InteractionDefinition Singleton = new Definition();

            public override bool Run()
            {
                print("Day counter " + mDayCounter);
                return true;
            }

            [DoesntRequireTuning]
            public sealed class Definition : ImmediateInteractionDefinition<Sim, Sim, ShowDayRandomizer>
            {
                public override string GetInteractionName(Sim a, Sim target, InteractionObjectPair interaction)
                {
                    return "[DEBUGGER Randomizer] Show Day for Randomizer ";
                }

                public override bool Test(Sim a, Sim target, bool isAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
                {
                    return kEnableDebugMode;
                }
            }
        }

        public static void WriteErrorXMLFile(string fileName, Exception errorToPrint, string additionalinfo)
        {
            uint num = 0u;
            string s = Simulator.CreateExportFile(ref num, fileName);

            if (num != 0)
            {
                CustomXmlWriter customXmlWriter = new CustomXmlWriter(num);

                if (!String.IsNullOrEmpty(additionalinfo))
                {
                    customXmlWriter.WriteToBuffer(additionalinfo);
                }
                if (errorToPrint != null)
                {
                    customXmlWriter.WriteToBuffer(errorToPrint.ToString());
                }
                customXmlWriter.WriteEndDocument();
            }
        }
    }
}
 