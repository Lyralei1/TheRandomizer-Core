using Sims3.Gameplay.Actors;
using Sims3.Gameplay.Socializing;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Xml;

namespace Sims3.Gameplay.Lyralei.TheRandomizerCore
{
    public enum RandomEventType
    {
        None,
        WorkRelated,
        GeneralRelated,
        DeathRelated,
        DiseaseRelated,
        FriendshipRelated,
        FameRelated,
        EnemyRelated,
        RelationshipRelated,
        MoneyRelated,
        SchoolRelated,
        DisasterRelated
    }


    public class EventTypeRandomizer : IEquatable<EventTypeRandomizer>
    {
        public RandomEventType type = RandomEventType.None;
        public string mRarity = "COMMON";
        public bool mNeedsOptionMenu = true;
        public bool mNeedsInvolvedTarget = true;
        public bool mIsRomantic = true;
        public bool mIsForNastySims = false;
        public string mTitle = "";
        public string mCaption = "";
        public string mOptionOneText = "";
        public string mOptionTwoText = "";
        public string mOptionThreeText = "";

        public MethodInfo TestMethod = null;

        public MethodInfo OptionGeneralFunction = null;

        public MethodInfo OptionOneFunction = null;
        public MethodInfo OptionTwoFunction = null;
        public MethodInfo OptionThreeFunction = null;

        public EventTypeRandomizer()
        {
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("Type: " + type.ToString());
            sb.AppendLine("mRarity: " + mRarity.ToString());
            sb.AppendLine("     mNeedsOptionMenu: " + mNeedsOptionMenu.ToString());
            sb.AppendLine("     mIsRomantic: " + mIsRomantic.ToString());
            sb.AppendLine("     mIsForNastySims: " + mIsForNastySims.ToString());
            sb.AppendLine("mTitle: " + mTitle.ToString());
            sb.AppendLine("mCaption: " + mCaption.ToString());
            sb.AppendLine("     mOptionOneText: " + mOptionOneText.ToString());
            sb.AppendLine("     mOptionTwoText: " + mOptionTwoText.ToString());
            sb.AppendLine("     mOptionThreeText: " + mOptionThreeText.ToString());

            if (OptionGeneralFunction != null) { sb.AppendLine("             OptionGeneralFunction: " + OptionGeneralFunction.ToString()); }
            if (OptionOneFunction != null) { sb.AppendLine("             OptionOneFunction: " + OptionOneFunction.ToString()); }
            if (OptionTwoFunction != null) { sb.AppendLine("             OptionTwoFunction: " + OptionTwoFunction.ToString()); }
            if (OptionThreeFunction != null) { sb.AppendLine("             OptionThreeFunction: " + OptionThreeFunction.ToString()); }

            return sb.ToString();
        }

        public override bool Equals(object obj)
        {
            var otherAttendance = obj as EventTypeRandomizer;
            if (otherAttendance == null)
            {
                return false;
            }

            return this.Equals(otherAttendance);
        }

        public bool Equals(EventTypeRandomizer other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            if (other != null && other.TestMethod != null && this != null && this.TestMethod != null && !string.IsNullOrEmpty(other.TestMethod.ToString()) && !string.IsNullOrEmpty(this.TestMethod.ToString()) && other.TestMethod.ToString() != this.TestMethod.ToString())
            {
                return true;
            }

            if (other != null && other.OptionGeneralFunction != null && this != null && this.OptionGeneralFunction != null && !string.IsNullOrEmpty(other.OptionGeneralFunction.ToString()) && !string.IsNullOrEmpty(this.OptionGeneralFunction.ToString()) && other.OptionGeneralFunction.ToString() != this.OptionGeneralFunction.ToString())
            {
                return true;
            }
            else if (other != null && other.OptionOneFunction != null && this != null && this.OptionOneFunction != null && !string.IsNullOrEmpty(other.OptionOneFunction.ToString()) && !string.IsNullOrEmpty(this.OptionOneFunction.ToString()) && other.OptionOneFunction.ToString() != this.OptionOneFunction.ToString())
            {
                return true;
            }
            return false;
        }

    }
}
