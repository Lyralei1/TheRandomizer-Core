using Sims3.Gameplay.Abstracts;
using Sims3.Gameplay.Actors;
using Sims3.Gameplay.ActorSystems;
using Sims3.Gameplay.Autonomy;
using Sims3.Gameplay.CAS;
using Sims3.Gameplay.Controllers;
using Sims3.Gameplay.Core;
using Sims3.Gameplay.EventSystem;
using Sims3.Gameplay.Interactions;
using Sims3.Gameplay.Objects.Electronics;
using Sims3.Gameplay.Opportunities;
using Sims3.Gameplay.Socializing;
using Sims3.Gameplay.Utilities;
using Sims3.SimIFace;
using Sims3.SimIFace.CAS;
using Sims3.UI;
using Sims3.UI.Controller;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sims3.Gameplay.Lyralei.TheRandomizerCore
{
    public class RandomizerBasicHelperFunctions
    {

        public static void GetSimToActorHomeLot(Sim target, Sim actor, Lot actorHome, Lot actorLotCurrent)
        {
            // If sim is an NPC with no home in the direct world (so a townie) get their ass into the world :p
            if (target == null)
            {
                SimOutfit outfit = target.SimDescription.GetOutfit(OutfitCategories.Everyday, 0);
                target = ((!outfit.IsValid) ? target.SimDescription.Instantiate(actorHome) : target.SimDescription.Instantiate(actorHome, outfit.Key));
            }
            if (target != null)
            {
                target.SocialComponent.SetInvitedOver(actorHome);
                bool flag;
                if (actorLotCurrent == actorHome && !actorLotCurrent.IsBaseCampLotType)
                {
                    VisitLot visitLot = VisitLot.Singleton.CreateInstance(actorLotCurrent, target, target.ForcePushPriority(), false, false) as VisitLot;
                    flag = target.InteractionQueue.PushAsContinuation(visitLot, true);
                }
            }
        }


        /// <summary>
        /// Handles getting the target to the active household's house. IF sim is home. Also handles any callbacks that need to happen. If you only need one out of 3 callbacks, just parse in a null
        /// </summary>
        /// <param name="target">randomly chosen target for random event</param>
        /// <param name="actor">Active sim</param>
        /// <param name="actorHome">The house of the active sim</param>
        /// <param name="actorLotCurrent">Where the active sim currently is. </param>
        /// <param name="callbackStarted">needs to be a function like so: public (static) void FunctionnameHere(Sim s, float x);</param>
        /// <param name="callbackOnCompleted">needs to be a function like so: public (static) void FunctionnameHere(Sim s, float x);</param>
        /// <param name="callbackOnFailed">needs to be a function like so: public (static) void FunctionnameHere(Sim s, float x);</param>
        public static void GetSimToActorHomeLotWithCallback(Sim target, Sim actor, Lot actorHome, Lot actorLotCurrent, Callback callbackStarted, Callback callbackOnCompleted, Callback callbackOnFailed)
        {
            // If sim is an NPC with no home in the direct world (so a townie) get their ass into the world :p
            if (target == null)
            {
                SimOutfit outfit = target.SimDescription.GetOutfit(OutfitCategories.Everyday, 0);
                target = ((!outfit.IsValid) ? target.SimDescription.Instantiate(actorHome) : target.SimDescription.Instantiate(actorHome, outfit.Key));
            }
            if (target != null)
            {
                target.SocialComponent.SetInvitedOver(actorHome);
                bool flag;
                if (actorLotCurrent == actorHome && !actorLotCurrent.IsBaseCampLotType)
                {
                    VisitLot visitLot = VisitLot.Singleton.CreateInstanceWithCallbacks(actorLotCurrent, target, target.ForcePushPriority(), false, false, callbackStarted, callbackOnCompleted, callbackOnFailed) as VisitLot;
                    flag = target.InteractionQueue.Add(visitLot);
                }
            }
        }
        
        public static bool GetSimToActorHomeWithoutRingingBell(Sim target, Sim actor, Lot actorHome, Callback callbackOnArrived)
        {
            if(target == null || actor == null)
            {
                return false;
            }

            GoToLotWithoutRingingDoorbell visitLot = GoToLotWithoutRingingDoorbell.Singleton.CreateInstanceWithCallbacks(actorHome, target, new InteractionPriority(InteractionPriorityLevel.UserDirected), false, false, null, callbackOnArrived, null) as GoToLotWithoutRingingDoorbell;

            return target.InteractionQueue.Add(visitLot);

            //float minDistance = Sim.MinDistanceFromDoorWhenVisiting;
            //Door door = null;
            //float maxDistance = minDistance * 2;
            //return actorHome.RouteToFrontDoorOrMailbox(target, minDistance, maxDistance, ref door, true, true, null, 3.40282347E+38f, null);
        }

        public static bool isAgeAppriopiate(Sim target, Sim Actor, bool hasNoTargetOnPurpose)
        {
            if (Actor == null || Actor.SimDescription == null)
            {
                return false;
            }

            if (hasNoTargetOnPurpose)
            {
                return true;
            }
            else if (!hasNoTargetOnPurpose && target == null)
            {
                return false;
            }
            else if(!hasNoTargetOnPurpose && target.SimDescription == null)
            {
                return false;
            }

            if (target.IsPet || Actor.IsPet)
            {
                return false;
            }

            if ((Actor.SimDescription.YoungAdultOrAbove && target.SimDescription.TeenOrBelow) || (Actor.SimDescription.TeenOrBelow && target.SimDescription.YoungAdultOrAbove))
            {
                return false;
            }

            if ((Actor.SimDescription.Teen && target.SimDescription.ChildOrBelow) || (Actor.SimDescription.ChildOrBelow && target.SimDescription.Teen))
            {
                return false;
            }

            if(Actor.SimDescription.ToddlerOrBelow || target.SimDescription.ToddlerOrBelow)
            {
                return false;
            }
            return true;
        }

        public static void PhoneOrTextActorRumorThatTargetSpread(Sim actor, Sim target, List<RandomizerBasicHelpersStructsClasses.RumorType> rumors)
        {
            if (actor != null && target != null && rumors != null)
            {
                // Have friend tell them that the rumor was spread.
                Relationship[] relationships = Relationship.GetRelationships(actor);
                LTRData lTRData = LTRData.Get(LongTermRelationshipTypes.Friend);

                RandomizerBasicHelpersStructsClasses.RumorType randomRumor = RandomUtil.GetRandomObjectFromList(rumors);

                foreach (Relationship relationship in relationships)
                {
                    // If they're the same sim...
                    if (relationship.SimDescriptionA.SimDescriptionId == actor.SimDescription.SimDescriptionId && relationship.SimDescriptionB.SimDescriptionId == actor.SimDescription.SimDescriptionId)
                    {
                        continue;
                    }

                    if (relationship.LTR.Liking > (float)lTRData.Liking || relationship.AreFriendsOrRomantic() && (relationship.SimDescriptionB.CreatedSim != null || relationship.SimDescriptionA.CreatedSim != null))
                    {
                        // text or phone call...
                        if (RandomUtil.CoinFlip())
                        {
                            PhoneService.ReceiveText(relationship.SimDescriptionB, relationship.SimDescriptionA.CreatedSim, Sims3.SimIFace.Enums.PhoneTextingAnimStates.ReceiveFrustrated, "RandomRomantic", "WarnFriendRumor");
                            actor.ShowTNSIfSelectable(Localization.LocalizeString("Lyralei/Gameplay/Phone/PhoneTextMessages:ForwardedText", new object[] { target }) + Localization.LocalizeString(randomRumor.mRumor, new object[] { actor }), StyledNotification.NotificationStyle.kSystemMessage);

                            HandleRumorWithSimsKnowing(randomRumor, actor, true);

                            Relationship rel = Relationship.Get(actor, target, true);
                            rel.UpdateSTCFromOutsideConversation(rel.SimDescriptionA, rel.SimDescriptionB, CommodityTypes.Insulting, -20f);
                            rel.LTR.UpdateLiking(-20f);
                            break;
                        }
                        else
                        {
                            Phone phone = relationship.SimDescriptionB.CreatedSim.Inventory.Find<Phone>();

                            if (phone != null)
                            {
                                PhoneService.PlaceCall(new RandomizerBasicHelpersStructsClasses.HandleRumorRevealCall(actor, relationship.SimDescriptionB.CreatedSim, target, randomRumor), OpportunityManager.PhoneCallTimeout);
                                //phone.PushCallChat(relationship.SimDescriptionB.CreatedSim, relationship.SimDescriptionA.GetMiniSimDescription());
                                //HandleRumorWithSimsKnowing(randomRumor, actor, true);

                                Relationship rel = Relationship.Get(actor, target, true);
                                rel.UpdateSTCFromOutsideConversation(rel.SimDescriptionA, rel.SimDescriptionB, CommodityTypes.Insulting, -20f);
                                rel.LTR.UpdateLiking(-20f);
                                break;
                            }
                        }
                    }
                    else
                    {
                        StyledNotification.Format format1 = new StyledNotification.Format(Localization.LocalizeString("Lyralei/Gameplay/Phone/PhoneTextMessages:TextRumor", new object[] { target }) + Localization.LocalizeString(randomRumor.mRumor, new object[] { actor }), target.ObjectId, actor.ObjectId, StyledNotification.NotificationStyle.kSystemMessage);
                        StyledNotification.Show(format1);

                        HandleRumorWithSimsKnowing(randomRumor, actor, true);

                        Relationship rel = Relationship.Get(actor, target, true);
                        rel.UpdateSTCFromOutsideConversation(rel.SimDescriptionA, rel.SimDescriptionB, CommodityTypes.Insulting, -20f);
                        rel.LTR.UpdateLiking(-20f);
                        break;
                    }
                }
            }
        }


        /// <summary>
        /// Handles rumors to return which friends/family members (and enemies!) Approve/disapprove/don't care of the actor. Rumor must be bad Rumor AND controversial.
        /// </summary>
        /// <param name="mRumor">Needs to be Rumor Type.</param>
        /// <param name="actor"></param>
        /// <param name="target"></param>
        public static void HandleRumorWithSimsKnowing(RandomizerBasicHelpersStructsClasses.RumorType mRumor, Sim actor, bool shouldReturnDontCareString)
        {
            TraitNames[] traits = new TraitNames[] { TraitNames.Good, TraitNames.GoodSenseOfHumor, TraitNames.Friendly, TraitNames.Proper, TraitNames.Childish, TraitNames.Inappropriate, TraitNames.Insane };

            if (mRumor.isBadRumor && mRumor.isControversialRumor)
            {
                Relationship[] relationships = Relationship.GetRelationships(actor);

                if(relationships.Length == 0)
                {
                    return;
                }

                List<string> mDisapproveNames = new List<string>();
                List<string> mDontCareNames = new List<string>();
                List<string> mApproveNames = new List<string>();
                SimDescription mTarget = null;
                foreach (Relationship rel in relationships)
                {
                    if (rel == null)
                    {
                        continue;
                    }

                    // If they're the same sim...
                    if (rel.SimDescriptionA.SimDescriptionId == actor.SimDescription.SimDescriptionId && rel.SimDescriptionB.SimDescriptionId == actor.SimDescription.SimDescriptionId)
                    {
                        continue;
                    }

                    if (rel.SimDescriptionA.SimDescriptionId != actor.SimDescription.SimDescriptionId)
                    {
                        mTarget = rel.SimDescriptionA;
                    }
                    if (rel.SimDescriptionB.SimDescriptionId != actor.SimDescription.SimDescriptionId)
                    {
                        mTarget = rel.SimDescriptionB;
                    }

                    if(mTarget.CreatedSim == null || actor.SimDescription == null) { continue; }

                    LongTermRelationshipTypes ltr = Relationship.GetLongTermRelationship(actor, mTarget.CreatedSim);

                    switch (ltr)
                    {
                        // Types that don't care:
                        case LongTermRelationshipTypes.Undefined:
                        case LongTermRelationshipTypes.Stranger:
                        case LongTermRelationshipTypes.Default:
                            break;

                        case LongTermRelationshipTypes.Disliked:
                        case LongTermRelationshipTypes.Enemy:
                        case LongTermRelationshipTypes.OldEnemies:
                            mTarget.CreatedSim.BuffManager.ForceAddBuff(BuffNames.FiendishlyDelighted, Origin.FromWatchingSimSuffer);
                            mDisapproveNames.Add(mTarget.FullName);

                            rel.UpdateSTCFromOutsideConversation(actor.SimDescription, mTarget, CommodityTypes.Insulting, -20f);
                            rel.LTR.UpdateLiking(-20f);
                            break;
                        case LongTermRelationshipTypes.Acquaintance:
                        case LongTermRelationshipTypes.DistantFriend:
                        case LongTermRelationshipTypes.Friend:
                        case LongTermRelationshipTypes.GoodFriend:
                        case LongTermRelationshipTypes.ExSpouse:
                        case LongTermRelationshipTypes.Ex:
                        case LongTermRelationshipTypes.OldFriend:
                            // there's a chance that this sim doesn't get to know.
                            if(RandomUtil.RandomChance(40))
                            {
                                continue;
                            }
                            if (mTarget.CreatedSim.TraitManager.HasAnyElement(traits))
                            {
                                if (RandomUtil.CoinFlip())
                                {
                                    mApproveNames.Add(mTarget.FullName);
                                }
                                else
                                {
                                    mDontCareNames.Add(mTarget.FullName);
                                }
                                continue;
                            }
                            if (rel.CurrentLTRLiking > 40 && rel.CurrentLTRLiking < 80)
                            {
                                if (RandomUtil.CoinFlip())
                                {
                                    mApproveNames.Add(mTarget.FullName);
                                }
                                else
                                {
                                    mDisapproveNames.Add(mTarget.FullName);
                                    rel.UpdateSTCFromOutsideConversation(actor.SimDescription, mTarget, CommodityTypes.Insulting, RandomUtil.GetFloat(-70, -5));
                                    rel.LTR.UpdateLiking(RandomUtil.GetFloat(-70, -5));
                                }
                            }
                            break;
                        case LongTermRelationshipTypes.BestFriend:
                        case LongTermRelationshipTypes.BestFriendsForever:
                            mApproveNames.Add(mTarget.FullName);
                            break;
                        case LongTermRelationshipTypes.RomanticInterest:
                        case LongTermRelationshipTypes.Partner:
                        case LongTermRelationshipTypes.Fiancee:
                        case LongTermRelationshipTypes.Spouse:
                            if (mTarget.CreatedSim.TraitManager.HasAnyElement(traits))
                            {
                                if (RandomUtil.CoinFlip())
                                {
                                    mApproveNames.Add(mTarget.FullName);
                                }
                                else
                                {
                                    mDontCareNames.Add(mTarget.FullName);
                                }
                                continue;
                            }
                            if (rel.CurrentLTRLiking > 40 && rel.CurrentLTRLiking < 80)
                            {
                                if (RandomUtil.CoinFlip())
                                {
                                    mApproveNames.Add(mTarget.FullName);
                                }
                                else
                                {
                                    mDisapproveNames.Add(mTarget.FullName);
                                    rel.UpdateSTCFromOutsideConversation(actor.SimDescription, mTarget, CommodityTypes.Insulting, RandomUtil.GetFloat(-70, -5));
                                    rel.LTR.UpdateLiking(RandomUtil.GetFloat(-70, -5));
                                }
                            }
                            break;
                    }
                }

                StringBuilder sb = new StringBuilder();

                if (mDisapproveNames.Count > 0)
                {
                    sb.AppendLine(Localization.LocalizeString("Lyralei/Gameplay/RumorSystem/RumorReaction:SimsDisapprove"));

                    foreach (string name in mDisapproveNames)
                    {
                        sb.AppendLine(name + ",");
                    }
                    sb.AppendLine();
                }

                if (mApproveNames.Count > 0)
                {
                    sb.AppendLine(Localization.LocalizeString("Lyralei/Gameplay/RumorSystem/RumorReaction:SimsOkay"));

                    foreach (string name in mApproveNames)
                    {
                        sb.AppendLine(name + ",");
                    }
                    sb.AppendLine();

                }

                if (shouldReturnDontCareString && mDontCareNames.Count > 0)
                {
                    sb.AppendLine(Localization.LocalizeString("Lyralei/Gameplay/RumorSystem/RumorReaction:SimsDontCare"));

                    foreach (string name in mDontCareNames)
                    {
                        sb.AppendLine(name + ",");
                    }
                    sb.AppendLine();

                }

                if (string.IsNullOrEmpty(sb.ToString()))
                {
                    return;
                }

                StyledNotification.Format format1 = new StyledNotification.Format(sb.ToString(), actor.ObjectId, StyledNotification.NotificationStyle.kSimTalking);
                StyledNotification.Show(format1);

            }
        }


        public static void HandleAdviceOnPhone(Sim actor, Sim target, List<RandomizerBasicHelpersStructsClasses.AdviceType> advices)
        {
            if (advices.Count == 0)
            {
                return;
            }

            RandomizerBasicHelpersStructsClasses.AdviceType randomAdvice = RandomUtil.GetRandomObjectFromList(advices);
            PhoneService.PlaceCall(new RandomizerBasicHelpersStructsClasses.AdviceFriendCall(actor, target, randomAdvice), OpportunityManager.PhoneCallTimeout);
        }


        public class GoToLotWithoutRingingDoorbell : Interaction<Sim, Lot>
        {
            public class Definition : InteractionDefinition<Sim, Lot, GoToLotWithoutRingingDoorbell>
            {
                public override bool Test(Sim a, Lot target, bool isAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
                {
                    return true;
                }
            }

            public static InteractionDefinition Singleton = new Definition();

            public override bool Run()
            {
                float minDistance = Sim.MinDistanceFromDoorWhenVisiting;

                Door door = null;
                return base.Target.RouteToFrontDoorOrMailbox(base.Actor, minDistance, Sims3.Gameplay.Situations.TrickOrTreatSituation.GoToLotForTrickOrTreating.kMaxDistance, ref door, true, true, null, 3.40282347E+38f, null);
            }
        }

        public static void print(string text)
        {
            SimpleMessageDialog.Show("Lyralei's Randomizer [CORE module]", text);
        }

    }
}
