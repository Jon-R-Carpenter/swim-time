using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TheSwimTimeSite.SwimTimeRuleProcessor.Interfaces;
using TheSwimTimeSite.SwimTimeRuleProcessor.Winter_2012;

namespace TheSwimTimeSite.SwimTimeRuleProcessor.Factories
{
    public class SwimTimeRuleProcessorFactory
    {
        public static ISwimTimeRuleProcessor CreateSwimTimeRuleProcessor(IList<ISwimTimeRule> theRules)
        {
            return new WINTER_2012_SwimTimeRuleProcessor(theRules);
        }
    }
}