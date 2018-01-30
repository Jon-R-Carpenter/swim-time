using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TheSwimTimeSite.SwimTimeRuleProcessor.Interfaces;

namespace TheSwimTimeSite.SwimTimeRuleProcessor
{
    public class SwimTimeRuleBucket
    {
        private ISwimTimeRule myConcreteRule;
        private double hoursFilled = 0.0;

        public SwimTimeRuleBucket(ISwimTimeRule theRule)
        {
            this.myConcreteRule = theRule;
        }

        public double GetHowManyHoursFilled()
        {
            return this.hoursFilled;
        }
        public double GetHowManyHoursUnfilled()
        {
            double max = myConcreteRule.GetMinAndMaxHours().GetMaximumHoursAllowedInThisTimePeriod();
            return Math.Max(0.0, max - this.hoursFilled);
        }

        public void FillHours(double howManyHoursToFill)
        {
            if(howManyHoursToFill < 0.0)
            {
                throw new ArgumentException("Cannot fill with negative hours!");
            }
            this.hoursFilled += howManyHoursToFill;
        }

        public void ResetToZeroHoursFilled()
        {
            this.hoursFilled = 0.0;
        }

        public override string ToString()
        {
            return "<" + myConcreteRule + " hours filled: " + hoursFilled + ">";
        }

        public DateTime GetStartDate()
        {
            return this.myConcreteRule.GetAbsoluteStartingDate();
        }

        public DateTime GetEndDate()
        {
            return this.myConcreteRule.GetAbsoluteEndingDate();
        }

        public ApplicableMinAndMax GetMinAndMaxHoursForVisitStartingAt(DateTime startTime)
        {
            return this.myConcreteRule.GetMinAndMaxHoursForVisitStartingAt(startTime);
        }
    }
}