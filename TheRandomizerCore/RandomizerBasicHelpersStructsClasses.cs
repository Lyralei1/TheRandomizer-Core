using Lyralei.TheRandomizerCore;
using Sims3.Gameplay.Actors;
using Sims3.Gameplay.ActorSystems;
using Sims3.Gameplay.Autonomy;
using Sims3.Gameplay.CAS;
using Sims3.Gameplay.Controllers;
using Sims3.Gameplay.Core;
using Sims3.Gameplay.EventSystem;
using Sims3.Gameplay.Interactions;
using Sims3.Gameplay.Objects.Electronics;
using Sims3.Gameplay.Situations;
using Sims3.Gameplay.Socializing;
using Sims3.Gameplay.Utilities;
using Sims3.SimIFace;
using Sims3.UI;
using Sims3.UI.Controller;
using System;
using System.Collections.Generic;
using System.Text;
using static Sims3.Gameplay.Objects.Electronics.Phone;
using Environment = System.Environment;

namespace Sims3.Gameplay.Lyralei.TheRandomizerCore
{
    public class RandomizerBasicHelpersStructsClasses
    {
        public struct AdviceType
        {
            public string mQuestion;
            public string mAnswerA;
            public string mAnswerB;
            public string mAnswerC;
            public RandomEventType EventType;

            // Is answer correct and not offensive?
            public bool mAnswerAValidation;
            public bool mAnswerBValidation;
            public bool mAnswerCValidation;
        }

        public struct RumorType
        {
            public string mRumor;
            public RandomEventType EventType;
            public bool isBadRumor;
            /// <summary>
            /// is only taken into account when isBadRumor = true.
            /// </summary>
            public bool isControversialRumor;
        }

        public class CallGhoster : Call
        {
            public class Definition : CallDefinition<CallEverybodyHome>
            {
                public override bool Test(Sim a, Phone target, bool isAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
                {
                    if (!base.Test(a, target, isAutonomous, ref greyedOutTooltipCallback))
                    {
                        return false;
                    }
                    foreach (Sim sim in a.Household.Sims)
                    {
                        if (sim != a && sim.LotCurrent != sim.LotHome)
                        {
                            return true;
                        }
                    }
                    return false;
                }
            }

            public static InteractionDefinition Singleton = new Definition();

            public override Sim.WalkStyle GetWalkStyleToRouteToPhone()
            {
                return Sim.WalkStyle.AutoSelect;
            }

            public override DialBehavior GetDialBehavior()
            {
                return DialBehavior.Pickup;
            }

            public override ConversationBehavior OnCallConnected()
            {
                return ConversationBehavior.TalkBriefly;
            }

            public override void OnCallFinished()
            {
                base.OnCallFinished();
                foreach (Sim sim in base.Actor.Household.Sims)
                {
                    if (sim.LotCurrent != sim.LotHome)
                    {
                        Sim.MakeSimGoHome(sim, false);
                    }
                }
            }
        }

        //public class InvitationWithFunctionCall : PhoneCall
        //{
        //    public static Sim mCallee;

        //    public static SimDescription mCaller;

        //    public bool mbInvitationAccepted;

        //    public override Sim TargetSim
        //    {
        //        get
        //        {
        //            return mCallee;
        //        }
        //    }

        //    public InvitationWithFunctionCall(Sim callee, SimDescription caller)
        //    {
        //        mCallee = callee;
        //        mCaller = caller;
        //    }

        //    public InvitationWithFunctionCall()
        //    {
        //    }

        //    public override AnswerType GetAnswerType()
        //    {
        //        return AnswerType.BeQueried;
        //    }

        //    public override QueryResponse GetQueryResponse(Sim actor)
        //    {
        //        bool flag = false;

        //        if (mCaller.Household.LotHome == null || mCaller.Household.LotHome == LotManager.GetWorldLot())
        //        {
        //            flag = true;
        //        }
        //        string name = flag ? "InvitationQueryYesIn" : "InvitationQueryYesOut";

        //        if (TwoButtonDialog.Show(LocalizeString("InvitationQuery", mCallee, mCaller), LocalizeString(name), LocalizeString("InvitationQueryNo")))
        //        {
        //            mbInvitationAccepted = true;
        //            return QueryResponse.RespondPositively;
        //        }
        //        return QueryResponse.RespondPositively;
        //    }

        //    public new static string LocalizeString(string name, params object[] parameters)
        //    {
        //        return Localization.LocalizeString("Gameplay/Objects/Electronics/Phone:" + name, parameters);
        //    }

        //    public override void OnAnswered()
        //    {
        //        if (mbInvitationAccepted)
        //        {
        //            Relationship.Get(mCallee.SimDescription, mCaller, true);
        //            EventTracker.SendEvent(new SimDescriptionTargetEvent(EventTypeId.kWasInvitedOver, mCallee, mCaller));
        //            bool flag = false;

        //            if (mCaller.Household.LotHome == null || mCaller.Household.LotHome == LotManager.GetWorldLot())
        //            {
        //                flag = true;
        //            }

        //            if (flag && mCaller != null && mCallee.Household.LotHome != null && mCaller.CreatedSim != null)
        //            {
        //                Sim createdSim = mCaller.CreatedSim;
        //                Sim target = mCallee;


        //                RandomizerBasicHelperFunctions.GetSimToActorHomeLotWithCallback(target, createdSim, createdSim.LotHome, createdSim.LotCurrent, null, OnGoingToHouseConfessionAnnounced, null);

        //                //createdSim.InteractionQueue.Add(VisitLot.Singleton.CreateInstance(mCallee.Household.LotHome, createdSim, new InteractionPriority(InteractionPriorityLevel.UserDirected), false, true));
        //            }
        //        }
        //    }

        //    public static void OnGoingToHouseConfessionAnnounced(Sim s, float x)
        //    {
        //        Sim CreatedSim = mCaller.CreatedSim;
        //        Sim Target = mCallee;

        //        if (CreatedSim.LotHome.LotId != CreatedSim.LotCurrent.LotId)
        //        {
        //            Target.InteractionQueue.CancelAllInteractions();
        //            Target.InteractionQueue.Add(GoHome.Singleton.CreateInstance(Target.LotHome, Target, new InteractionPriority(InteractionPriorityLevel.UserDirected), false, true));
        //            return;
        //        }

        //        StyledNotification.Format format = new StyledNotification.Format(string.Format("Hey {1}! Thanks that I was able to come around. I've been wondering how to tell you this, but I really like you.", Target.FirstName, CreatedSim.FirstName), Target.ObjectId, StyledNotification.NotificationStyle.kSimTalking);
        //        StyledNotification.Show(format);

        //        Sim.ForceSocial(Target, CreatedSim, "Confess Attraction", Sims3.Gameplay.Interactions.InteractionPriorityLevel.UserDirected, false);
        //    }
        //}

        public class HandleRumorRevealCall : SimPhoneCall
        {
            public RumorType mRumor;
            public Sim mTarget = null;
            public Sim mRumorFriend = null; // The friend who spread the rumor
            public Sim mActor = null;

            public HandleRumorRevealCall(Sim sim, Sim target, Sim rumorFriend, RumorType rumor)
                : base(sim)
            {
                mTarget = target;
                mRumorFriend = rumorFriend;

                mActor = sim;
                mRumor = rumor;
            }

            public HandleRumorRevealCall()
            {
            }

            public override AnswerType GetAnswerType()
            {
                return AnswerType.BeQueried;
            }

            public override QueryResponse GetQueryResponse(Sim actor)
            {
                if (mTarget == null || mActor == null || mRumorFriend == null || string.IsNullOrEmpty(mRumor.mRumor))
                {
                    return QueryResponse.JustHangUp;
                }
                else
                {
                    StyledNotification.Format format = new StyledNotification.Format(Localization.LocalizeString("Lyralei/Gameplay/RumorSystem/RumorPhone:CalledNotification", new object[] { mTarget, mRumorFriend, mActor }) + Localization.LocalizeString(mRumor.mRumor, new object[] { mActor }), mTarget.ObjectId, mActor.ObjectId, StyledNotification.NotificationStyle.kSimTalking);
                    StyledNotification.Show(format);

                    RandomizerBasicHelperFunctions.HandleRumorWithSimsKnowing(mRumor, mActor, false);

                    if (mRumor.isControversialRumor && mRumor.isBadRumor)
                    {
                        return QueryResponse.RespondNegatively;
                    }
                    else
                    {
                        return QueryResponse.RespondPositively;
                    }
                }
            }

            public override void OnMissed()
            {
                PhoneService.ReceiveText(mTarget.SimDescription, mActor, Sims3.SimIFace.Enums.PhoneTextingAnimStates.ReceiveFrustrated, "RandomRomantic", "WarnFriendRumor");

                mActor.ShowTNSIfSelectable(Localization.LocalizeString("Lyralei/Gameplay/RumorSystem/RumorPhone:CalledMissedNotification", new object[] { mRumorFriend, mActor }) + Localization.LocalizeString(mRumor.mRumor, new object[] { mActor }), StyledNotification.NotificationStyle.kSimTalking);

                RandomizerBasicHelperFunctions.HandleRumorWithSimsKnowing(mRumor, mActor, false);
                base.OnMissed();
            }
        }

        public class AdviceFriendCall : SimPhoneCall
        {
            public AdviceType mAdvice;

            public Sim mTarget = null;
            public Sim mActor = null;

            public AdviceFriendCall(Sim sim, Sim target, AdviceType advice)
                : base(sim)
            {
                mTarget = target;
                mActor = sim;
                mAdvice = advice;
            }

            public AdviceFriendCall()
            {
            }

            public override AnswerType GetAnswerType()
            {
                return AnswerType.BeQueried;
            }
            public QueryResponse mResponse;

            public override QueryResponse GetQueryResponse(Sim actor)
            {
                ThreeButtonDialogRandomizer.ButtonPressed mBtnPressedThreeBtnDialog;
                bool mButtonPressedTwoBtnDialog = false;

                if (mTarget == null || mActor == null || string.IsNullOrEmpty(mAdvice.mQuestion))
                {
                    return QueryResponse.JustHangUp;
                }
                else
                {
                    if (!string.IsNullOrEmpty(mAdvice.mAnswerA) && !string.IsNullOrEmpty(mAdvice.mAnswerB) && string.IsNullOrEmpty(mAdvice.mAnswerC))
                    {
                        mButtonPressedTwoBtnDialog = TwoButtonRandomizerDialog.Show(Localization.LocalizeString(mAdvice.mQuestion, new object[] { mTarget }), mAdvice.mAnswerA, mAdvice.mAnswerB);

                        if (mButtonPressedTwoBtnDialog)
                        {
                            if (mAdvice.mAnswerAValidation)
                            {
                                
                                return HandleResponse(mActor, mTarget, Relationship.Get(mActor, mTarget, true), true, Localization.LocalizeString("Lyralei/Gameplay/RumorSystem/AdviceResult:GreatAdvice"), "");
                            }
                            else
                            {
                                
                                return HandleResponse(mActor, mTarget, Relationship.Get(mActor, mTarget, true), false, "", Localization.LocalizeString("Lyralei/Gameplay/RumorSystem/AdviceResult:NegativeAdvice"));
                            }
                        }
                        else
                        {
                            if (mAdvice.mAnswerBValidation)
                            {
                                return HandleResponse(mActor, mTarget, Relationship.Get(mActor, mTarget, true), true, Localization.LocalizeString("Lyralei/Gameplay/RumorSystem/AdviceResult:GreatAdvice"), "");
                            }
                            else
                            {
                                return HandleResponse(mActor, mTarget, Relationship.Get(mActor, mTarget, true), false, "", Localization.LocalizeString("Lyralei/Gameplay/RumorSystem/AdviceResult:NegativeAdvice"));
                            }
                        }
                    }
                    else if (!string.IsNullOrEmpty(mAdvice.mAnswerA) && !string.IsNullOrEmpty(mAdvice.mAnswerB) && !string.IsNullOrEmpty(mAdvice.mAnswerC))
                    {
                        mBtnPressedThreeBtnDialog = ThreeButtonDialogRandomizer.Show(Localization.LocalizeString(mAdvice.mQuestion, new object[] { mTarget }), mAdvice.mAnswerA, mAdvice.mAnswerB, mAdvice.mAnswerC);

                        switch (mBtnPressedThreeBtnDialog)
                        {
                            case ThreeButtonDialogRandomizer.ButtonPressed.FirstButton:
                                if (mAdvice.mAnswerAValidation)
                                {
                                    return HandleResponse(mActor, mTarget, Relationship.Get(mActor, mTarget, true), true, Localization.LocalizeString("Lyralei/Gameplay/RumorSystem/AdviceResult:GreatAdvice"), "");
                                }
                                else
                                {
                                    return HandleResponse(mActor, mTarget, Relationship.Get(mActor, mTarget, true), false, "", Localization.LocalizeString("Lyralei/Gameplay/RumorSystem/AdviceResult:NegativeAdvice"));
                                }
                            case ThreeButtonDialogRandomizer.ButtonPressed.SecondButton:
                                if (mAdvice.mAnswerBValidation)
                                {
                                    return HandleResponse(mActor, mTarget, Relationship.Get(mActor, mTarget, true), true, Localization.LocalizeString("Lyralei/Gameplay/RumorSystem/AdviceResult:GreatAdvice"), "");
                                }
                                else
                                {
                                    return HandleResponse(mActor, mTarget, Relationship.Get(mActor, mTarget, true), false, "", Localization.LocalizeString("Lyralei/Gameplay/RumorSystem/AdviceResult:NegativeAdvice"));
                                }
                            case ThreeButtonDialogRandomizer.ButtonPressed.ThirdButton:
                                if (mAdvice.mAnswerCValidation)
                                {
                                    return HandleResponse(mActor, mTarget, Relationship.Get(mActor, mTarget, true), true, Localization.LocalizeString("Lyralei/Gameplay/RumorSystem/AdviceResult:GreatAdvice"), "");
                                }
                                else
                                {
                                    return HandleResponse(mActor, mTarget, Relationship.Get(mActor, mTarget, true), false, "", Localization.LocalizeString("Lyralei/Gameplay/RumorSystem/AdviceResult:NegativeAdvice"));
                                }
                        }
                    }
                    else if ((!string.IsNullOrEmpty(mAdvice.mAnswerA) && string.IsNullOrEmpty(mAdvice.mAnswerB) && string.IsNullOrEmpty(mAdvice.mAnswerC)) || (!string.IsNullOrEmpty(mAdvice.mAnswerA) && !string.IsNullOrEmpty(mAdvice.mAnswerB) && string.IsNullOrEmpty(mAdvice.mAnswerC)) || (!string.IsNullOrEmpty(mAdvice.mAnswerA) && string.IsNullOrEmpty(mAdvice.mAnswerB) && !string.IsNullOrEmpty(mAdvice.mAnswerC)))
                    {
                        RandomizerCoreModule.print("Attention!! The creator of this event has inncorrectly parsed through the answer buttons in the AdviceFriendCall. " + Environment.NewLine + "Answer B and Answer C cannot be empty when Answer A isn't empty. Either parse in an Answer B or Both Answer B and C.");
                        return QueryResponse.JustHangUp;
                    }
                    return QueryResponse.JustHangUp;
                }
            }

            public static QueryResponse HandleResponse(Sim actor, Sim target, Relationship rel, bool positive, string posMessage, string negMessage)
            {
                // Default to just hang up if all goes wrong.
                QueryResponse mResponse = QueryResponse.JustHangUp;

                if (positive)
                {
                    StyledNotification.Format format = new StyledNotification.Format(posMessage, target.ObjectId, actor.ObjectId, StyledNotification.NotificationStyle.kSimTalking);
                    StyledNotification.Show(format);
                    if (RandomUtil.CoinFlip())
                    {
                        mResponse = QueryResponse.RespondPositively;
                    }
                    else
                    {
                        mResponse = QueryResponse.Chat;
                    }
                    rel.UpdateSTCFromOutsideConversation(rel.SimDescriptionA, rel.SimDescriptionB, CommodityTypes.Insulting, RandomUtil.GetFloat(5, 25));
                    rel.LTR.UpdateLiking(RandomUtil.GetFloat(5, 25));
                }
                else
                {
                    StyledNotification.Format format = new StyledNotification.Format(negMessage, target.ObjectId, actor.ObjectId, StyledNotification.NotificationStyle.kSimTalking);
                    StyledNotification.Show(format);
                    if (RandomUtil.CoinFlip())
                    {
                        mResponse = QueryResponse.RespondNegatively;
                    }
                    else
                    {
                        mResponse = QueryResponse.JustHangUp;
                    }
                    rel.UpdateSTCFromOutsideConversation(rel.SimDescriptionA, rel.SimDescriptionB, CommodityTypes.Insulting, RandomUtil.GetFloat(-35, -10));
                    rel.LTR.UpdateLiking(RandomUtil.GetFloat(-35, -10));
                }

                return mResponse;
            }

        }


        public class SimpleSingleBtnNotification
        {
            public Sim mSim;
            public Sim mTarget;
            public StyledNotification mNotification;

            public StyledNotification.Format mNotificationFormat;

            public SimpleSingleBtnNotification(StyledNotification.Format notificationFormat, Sim sim, Sim target)
            {
                mSim = sim;
                mTarget = target;
                mNotificationFormat = notificationFormat;
            }

            // Param: Callback expects a method. See example on how this is parsed through.
            public static void Show(StyledNotification.Format notificationFormat, Sim sim, Sim target)
            {
                SimpleSingleBtnNotification simpleSingleBtnNotification = new SimpleSingleBtnNotification(notificationFormat, sim, target);

                //simpleSingleBtnNotification.mNotificationFormat.mButtonPressed = callback;
                simpleSingleBtnNotification.mNotification = StyledNotification.Show(simpleSingleBtnNotification.mNotificationFormat);
            }
        }

    }
}
