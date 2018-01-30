using System;
using TheSwimTimeSite.SwimTimeRuleProcessor.Interfaces;

namespace TheSwimTimeSite.SwimTimeRuleProcessor.Winter_2012
{
    public class WINTER_2012_SwimTimeRule : ISwimTimeRule
    {
        private double minimumTimeInHours;
        private double maximumTimeInHours;
        private DateTime absStartTime;
        private DateTime absEndTime;
        private DateTime ruleStartDate;

        private double repeatTime;

        public WINTER_2012_SwimTimeRule(DateTime AbsoluteStartDate, DateTime AbsoluteEndDate, DateTime RuleStartDateTime, double minHours, double maxHours, double ruleRepeatTime)
        {
            if(AbsoluteEndDate < AbsoluteStartDate)
            {
                throw new ArgumentException("Cannot end before I start!");
            }

            this.absStartTime = AbsoluteStartDate;
            this.absEndTime = AbsoluteEndDate;
            this.ruleStartDate = RuleStartDateTime;

            this.minimumTimeInHours = minHours;
            this.maximumTimeInHours = maxHours;
            this.repeatTime = ruleRepeatTime;
        }

        public ApplicableMinAndMax GetMinAndMaxHoursForVisitStartingAt(DateTime visitStartDate)
        {
            if (visitStartDate >= this.absStartTime && visitStartDate <= this.absEndTime)
            {
                return new ApplicableMinAndMax(this.minimumTimeInHours, this.maximumTimeInHours);
            }
            return null;
        }

        public DateTime GetAbsoluteStartingDate()
        {
            return this.absStartTime;
        }

        public DateTime GetAbsoluteEndingDate()
        {
            return this.absEndTime;
        }

        public DateTime GetRuleStartDate()
        {
            return this.ruleStartDate;
        }

        public double GetRuleRepeatabilityPeriodInHours()
        {
            return this.repeatTime;
        }

        public ApplicableMinAndMax GetMinAndMaxHours()
        {
            return new ApplicableMinAndMax(this.minimumTimeInHours, this.maximumTimeInHours);
        }

        public override string ToString()
        {
            String minTimeSTR = minimumTimeInHours.ToString();
            String maxTimeSTR = maximumTimeInHours.ToString();
            String startTimeSTR = absStartTime.ToString();
            String endTimeSTR = absEndTime.ToString();
            String repeatTimeSTR = repeatTime.ToString();
            String repeatTimeSpecialSTR = "UNKNOWN";
            if (Math.Sqrt(Math.Pow(repeatTime - 24.0,2.0)) <= 0.1)
            {
                repeatTimeSpecialSTR = "DAILY";
            }
            if (Math.Sqrt(Math.Pow(repeatTime - 168.0, 2.0)) <= 0.1)
            {
                repeatTimeSpecialSTR = "WEEKLY";
            }
            if (Math.Sqrt(Math.Pow(repeatTime - 0.0, 2.0)) <= 0.1)
            {
                repeatTimeSpecialSTR = "NON-REPEATING";
            }

            return "<(" + startTimeSTR + " to " + endTimeSTR + ")  MinTime: " + minTimeSTR + " MaxTime: " + maxTimeSTR + " repeatTime: " + repeatTimeSTR + " (" +  repeatTimeSpecialSTR + ")" + ">";
        }
    }
}