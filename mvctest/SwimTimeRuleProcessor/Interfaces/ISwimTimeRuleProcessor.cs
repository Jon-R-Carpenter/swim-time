using System.Collections.Generic;

namespace TheSwimTimeSite.SwimTimeRuleProcessor.Interfaces
{
    public interface ISwimTimeRuleProcessor
    {
        //For reporting
        //IE: give it all the visits by that person during the term and it will do all the work.
        SwimTimeRuleProcessingInformation ProcessMultipleVisits(IList<SwimTimeVisit> theVisits);
    }
}
