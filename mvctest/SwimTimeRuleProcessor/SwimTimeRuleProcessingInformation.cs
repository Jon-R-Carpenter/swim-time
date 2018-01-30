using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheSwimTimeSite.SwimTimeRuleProcessor
{
    public class SwimTimeRuleProcessingInformation
    {
        private double validTimeInHours;
        private double invalidTimeInHours;
        private string reasoningOnSplit;

        public SwimTimeRuleProcessingInformation(double validTime, double invalidTime, string reasoning)
        {
            if(reasoning == null)
            {
                throw new ArgumentException("Cannot have a null string as a reasoning, use the empty string instead.");
            }
            if (invalidTime < 0.0)
            {
                throw new ArgumentException("Cannot have an invalid time less than zero hours; does not make logical sense.");
            }
            if (validTime < 0.0)
            {
                throw new ArgumentException("Cannot have a valid time less than zero hours; does not make logical sense.");
            }


            this.validTimeInHours = validTime;
            this.invalidTimeInHours = invalidTime;
            this.reasoningOnSplit = reasoning;
        }

        public double GetValidTime()
        {
            return this.validTimeInHours;
        }
        public double GetTimeExcludedByTheRules()
        {
            return this.invalidTimeInHours;
        }
        //The reason why it split the visit(s) into valid and invalid hours this way.  A debugging tool/explain what it is doing tool.
        public string GetReasoning()
        {
            return this.reasoningOnSplit;
        }
    }//end of class
}