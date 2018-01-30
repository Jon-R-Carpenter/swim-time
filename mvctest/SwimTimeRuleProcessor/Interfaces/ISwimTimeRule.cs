using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheSwimTimeSite.SwimTimeRuleProcessor.Interfaces
{
    //Will be fleshed out in the future, maybe made into a single class instead of an interface
    public interface ISwimTimeRule
    {
        ApplicableMinAndMax GetMinAndMaxHoursForVisitStartingAt(DateTime startTime);
        ApplicableMinAndMax GetMinAndMaxHours();

        DateTime GetAbsoluteStartingDate();
        DateTime GetAbsoluteEndingDate();
        DateTime GetRuleStartDate();
        double GetRuleRepeatabilityPeriodInHours();
    }
}