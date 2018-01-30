using System;

namespace TheSwimTimeSite.SwimTimeRuleProcessor.Factories
{
    public class VisitFactory
    {
        //This factory method is here because I think the constructor for Visit will change
        public static SwimTimeVisit CreateVisit(DateTime start, DateTime end, bool valid)
        {
            return new SwimTimeVisit(start,end,valid);
        }
    }
}