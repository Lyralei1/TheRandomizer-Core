using System;
using System.Collections.Generic;
using Sims3.Gameplay.Abstracts;
using Sims3.Gameplay.Actors;
using Sims3.Gameplay.Core;
using Sims3.Gameplay.EventSystem;
using Sims3.Gameplay.Lyralei.TheRandomizerCore;
using Sims3.Gameplay.Objects.Appliances;
using Sims3.Gameplay.Objects.Electronics;
using Sims3.Gameplay.Objects.Environment;
using Sims3.Gameplay.Objects.Lighting;
using Sims3.Gameplay.Objects.PerformanceObjects;
using Sims3.Gameplay.Objects.Register;
using Sims3.Gameplay.Utilities;
using Sims3.SimIFace;

//Template Created by Battery

namespace Lyralei.TheRandomizerBase
{
    public class TheRandomizerMod
	{
		[Tunable] static bool init;
		
		static TheRandomizerMod()
		{
            World.OnWorldLoadFinishedEventHandler += new EventHandler(OnWorldLoadFinished);
            World.sOnStartupAppEventHandler += new EventHandler(OnStartupApp);
        }

        public static List<GameObject> mCachedElectronicDevices = new List<GameObject>();
        public static List<GameObject> mCachedPlumbingObjects = new List<GameObject>();
        public static bool mHasGeneralInstalled = false;
        public static bool mHasDisasterInstalled = false;

        public static void OnStartupApp(object sender, EventArgs e)
        {
            XmlDbData xmlWorkEvents = XmlDbData.ReadData(new ResourceKey(0xB2D2AC269C47BF2A, 0x0333406C, 0x00000000), false);
            if (xmlWorkEvents != null)
            {
                RandomizerCoreModule.ParseRandomizerDataFromXML(xmlWorkEvents);
            }

            XmlDbData xmlDiseaseEvents = XmlDbData.ReadData(new ResourceKey(0xB44568B961AF137E, 0x0333406C, 0x00000000), false);
            if (xmlDiseaseEvents != null)
            {
                RandomizerCoreModule.ParseRandomizerDataFromXML(xmlDiseaseEvents);
            }

            XmlDbData xmlFriendshipEvents = XmlDbData.ReadData(new ResourceKey(0xB9D3A2E68BD01F32, 0x0333406C, 0x00000000), false);
            if (xmlFriendshipEvents != null)
            {
                RandomizerCoreModule.ParseRandomizerDataFromXML(xmlFriendshipEvents);
            }

            XmlDbData xmlDeathEvents = XmlDbData.ReadData(new ResourceKey(0xE991E35BC71BAFED, 0x0333406C, 0x00000000), false);
            if (xmlDeathEvents != null)
            {
                RandomizerCoreModule.ParseRandomizerDataFromXML(xmlDeathEvents);
            }
            
            XmlDbData xmlGeneralEvents = XmlDbData.ReadData(new ResourceKey(0xD4D43D80046D8883, 0x0333406C, 0x00000000), false);
            if (xmlGeneralEvents != null)
            {
                RandomizerCoreModule.ParseRandomizerDataFromXML(xmlGeneralEvents);
                mHasGeneralInstalled = true;
            }

            XmlDbData xmlMoneyEvents = XmlDbData.ReadData(new ResourceKey(0x881B69095055588B, 0x0333406C, 0x00000000), false);
            if (xmlMoneyEvents != null)
            {
                RandomizerCoreModule.ParseRandomizerDataFromXML(xmlMoneyEvents);
            }

            XmlDbData xmlRelationshipEvents = XmlDbData.ReadData(new ResourceKey(0xD39FEFB2B0AC0AAD, 0x0333406C, 0x00000000), false);
            if (xmlRelationshipEvents != null)
            {
                RandomizerCoreModule.ParseRandomizerDataFromXML(xmlRelationshipEvents);
            }

            XmlDbData xmlEnemyEvents = XmlDbData.ReadData(new ResourceKey(0xE62E30B49B6F33E7, 0x0333406C, 0x00000000), false);
            if (xmlEnemyEvents != null)
            {
                RandomizerCoreModule.ParseRandomizerDataFromXML(xmlEnemyEvents);
            }


            XmlDbData xmlSchoolEvents = XmlDbData.ReadData(new ResourceKey(0x80664FA34CF46A3F, 0x0333406C, 0x00000000), false);
            if (xmlSchoolEvents != null)
            {
                RandomizerCoreModule.ParseRandomizerDataFromXML(xmlSchoolEvents);
            }

            XmlDbData xmlDisasterEvents = XmlDbData.ReadData(new ResourceKey(0x222438AC6DAB94F4, 0x0333406C, 0x00000000), false);
            if (xmlDisasterEvents != null)
            {
                RandomizerCoreModule.ParseRandomizerDataFromXML(xmlDisasterEvents);
                mHasDisasterInstalled = true;
            }

            // PARSE ALL RUMORS
            XmlDbData xmlLoadRumors = XmlDbData.ReadData(new ResourceKey(0xC79496873153AEE2, 0x0333406C, 0x00000000), false);
            if (xmlLoadRumors != null)
            {
                RandomizerCoreModule.ParseRumorDataFromXML(xmlLoadRumors);
            }
            // PARSE ALL Advice
            XmlDbData xmlLoadAdvices = XmlDbData.ReadData(new ResourceKey(0x5F5F8BB8A3966B7C, 0x0333406C, 0x00000000), false);
            if (xmlLoadAdvices != null)
            {
                RandomizerCoreModule.ParseAdviceDataFromXML(xmlLoadAdvices);
            }
        }


        private static ListenerAction OnObjectChanged(Event e)
        {
            //PhoneSmart phone = e.TargetObject as PhoneSmart;
            //if (phone != null)
            //{
            //    phone.AddInventoryInteraction(CallGnomeFirefighters.Singleton);
            //    phone.AddInteraction(CallGnomeFirefighters.Singleton);
            //}
            //PhoneFuture phone5 = e.TargetObject as PhoneFuture;
            //if (phone5 != null)
            //{
            //    phone5.AddInventoryInteraction(Sims3.Gameplay.Objects.Lyralei.CallGnomeFirefighters.Singleton);
            //    phone5.AddInteraction(CallGnomeFirefighters.Singleton);
            //}

            //Phone phone1 = e.TargetObject as Phone;
            //if (phone1 != null)
            //{
            //    phone1.AddInventoryInteraction(CallGnomeFirefighters.Singleton);
            //    phone1.AddInteraction(CallGnomeFirefighters.Singleton);
            //}
            //PhoneCell phone2 = e.TargetObject as PhoneCell;
            //if (phone2 != null)
            //{
            //    phone2.AddInventoryInteraction(CallGnomeFirefighters.Singleton);
            //    phone2.AddInteraction(CallGnomeFirefighters.Singleton);
            //}
            return ListenerAction.Keep;
        }

        public static EventListener sHasPurchasedNewItem = null;
        public static List<Sim> sAllSims = new List<Sim>();

        public static void OnWorldLoadFinished(object sender, EventArgs e)
        {

            if (mHasGeneralInstalled)
            {
                //Populate cached lists for the poweroutage and water leak function.
                mCachedElectronicDevices = GetAllPlumbingObjects();
                mCachedPlumbingObjects = GetAllElectronicObjects();

                LotManager.ExitingBuildBuyMode += OnExitBuildBuyMode;

            }

            if(mHasDisasterInstalled)
            {
                // Setup phone call firefighter Gnomes:
                //foreach (PhoneCell phone in Sims3.Gameplay.Queries.GetObjects<PhoneCell>())
                //{
                //    if (phone != null)
                //    {
                //        phone.AddInventoryInteraction(CallGnomeFirefighters.Singleton);
                //        phone.AddInteraction(CallGnomeFirefighters.Singleton);
                //    }
                //}
                //foreach (Phone phone in Sims3.Gameplay.Queries.GetObjects<Phone>())
                //{
                //    if (phone != null)
                //    {
                //        phone.AddInventoryInteraction(CallGnomeFirefighters.Singleton);
                //        phone.AddInteraction(CallGnomeFirefighters.Singleton);
                //    }
                //}
                //foreach (PhoneSmart phone in Sims3.Gameplay.Queries.GetObjects<PhoneSmart>())
                //{
                //    if (phone != null)
                //    {
                //        phone.AddInventoryInteraction(CallGnomeFirefighters.Singleton);
                //        phone.AddInteraction(CallGnomeFirefighters.Singleton);
                //    }
                //}

                //EventTracker.AddListener(EventTypeId.kInventoryObjectAdded, new ProcessEventDelegate(OnObjectChanged));
                //EventTracker.AddListener(EventTypeId.kObjectStateChanged, new ProcessEventDelegate(OnObjectChanged));
                //EventTracker.AddListener(EventTypeId.kBoughtObject, new ProcessEventDelegate(OnObjectChanged));

            }

            //foreach (Sim sim in RandomizerCoreModule.sAllSimsInWorld)
            //{
            //    if (sim != null && sim.SimDescription != null)
            //    {
            //        sAllSims.Add(sim);
            //        sim.AddInteraction(TestRandomEvent.Singleton);
            //    }
            //}
        }

        private static void OnExitBuildBuyMode()
        {
            // Recache!
            mCachedElectronicDevices = GetAllPlumbingObjects();
            mCachedPlumbingObjects = GetAllElectronicObjects();
        }


        public static List<GameObject> GetAllPlumbingObjects()
        {
            GameObject[] mGameObjects = Sims3.Gameplay.Queries.GetObjects<GameObject>();
            List<GameObject> mElectronics = new List<GameObject>();

            string[] validNamespaces = new string[]
            {
                "Sims3.Gameplay.Objects.Entertainment",
                "Sims3.Gameplay.Objects.Electronics",
                "Sims3.Gameplay.Objects.Appliances",
                "Sims3.Gameplay.Objects.Lighting",
                "Sims3.Gameplay.Objects.PerformanceObjects"
            };

            foreach(GameObject obj in mGameObjects)
            {
                if(obj.GetType() == typeof(PhoneHome) || 
                    obj.GetType() == typeof(DaylightLight) || 
                    obj.GetType() == typeof(MoodLamp) || 
                    obj.GetType() == typeof(LightInvisible) || 
                    obj.GetType() == typeof(Grill) ||
                    obj.GetType() == typeof(Clothesline) || 
                    obj.GetType() == typeof(FutureBar) || 
                    obj.GetType() == typeof(FutureFoodSynthesizer) || 
                    obj.GetType() == typeof(AlarmClockCheap) || 
                    obj.GetType() == typeof(VideoCamera) || 
                    obj.GetType() == typeof(VRGoggles) ||
                    obj.GetType() == typeof(CrowdMonster) ||
                    obj.GetType() == typeof(PerformanceTips) ||
                    obj.GetType() == typeof(ProprietorWaitingArea) ||
                    obj.GetType() == typeof(ShowFloor) ||
                    obj.GetType() == typeof(ShowStage)
                    )
                {
                    continue;
                }

                string mNamespace = obj.GetType().Namespace;

                if( (mNamespace == validNamespaces[0]) ||
                    (mNamespace == validNamespaces[1]) ||
                    (mNamespace == validNamespaces[2]) ||
                    (mNamespace == validNamespaces[3]) ||
                    (mNamespace == validNamespaces[4]) ||
                    obj.GetType() == typeof(ShoppingRegister) ||
                    obj.GetType() == typeof(AthleticGameObject)
                    )
                {
                    mElectronics.Add(obj);
                }
            }
            return mElectronics;

        }

        public static List<GameObject> GetAllElectronicObjects()
        {
            GameObject[] mGameObjects = Sims3.Gameplay.Queries.GetObjects<GameObject>();
            List<GameObject> mPlumbings = new List<GameObject>();

            string[] validNamespaces = new string[]
            {
                "Sims3.Gameplay.Objects.Plumbing",
                "Sims3.Gameplay.Objects.Plumbing.Mimics"
            };

            foreach (GameObject obj in mGameObjects)
            {
                string mNamespace = obj.GetType().Namespace;

                if ((mNamespace == validNamespaces[0]) ||
                    (mNamespace == validNamespaces[1]) ||
                    obj.GetType() == typeof(Dishwasher) ||
                    obj.GetType() == typeof(WashingMachineCheap) ||
                    obj.GetType() == typeof(WashingMachine) ||
                    obj.GetType() == typeof(WashingMachineExpensive) ||
                    obj.GetType() == typeof(Sprinkler) ||
                    obj.GetType() == typeof(Fountain) ||
                    obj.GetType().IsSubclassOf(typeof(Fountain))
                    )
                {
                    mPlumbings.Add(obj);
                }
            }
            return mPlumbings;
        }

        //public static void OnWorldLoadFinished(object sender, EventArgs e)
        //{
        //    XmlDbData xmlWorkEvents = XmlDbData.ReadData(new ResourceKey(0xB2D2AC269C47BF2A, 0x0333406C, 0x00000000), false);
        //    if(xmlWorkEvents != null)
        //    {
        //        RandomizerCoreModule.ParseRandomizerDataFromXML(xmlWorkEvents);
        //    }

        //    XmlDbData xmlDiseaseEvents = XmlDbData.ReadData(new ResourceKey(0xB44568B961AF137E, 0x0333406C, 0x00000000), false);
        //    if (xmlDiseaseEvents != null)
        //    {
        //        RandomizerCoreModule.ParseRandomizerDataFromXML(xmlDiseaseEvents);
        //    }

        //    XmlDbData xmlFriendshipEvents = XmlDbData.ReadData(new ResourceKey(0xB9D3A2E68BD01F32, 0x0333406C, 0x00000000), false);
        //    if (xmlFriendshipEvents != null)
        //    {
        //        RandomizerCoreModule.ParseRandomizerDataFromXML(xmlFriendshipEvents);
        //    }
        //}

    }
}