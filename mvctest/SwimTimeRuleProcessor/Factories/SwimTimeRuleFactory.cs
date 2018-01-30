using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TheSwimTimeSite.ObjectModels;
using TheSwimTimeSite.SwimTimeRuleProcessor.Interfaces;
using TheSwimTimeSite.SwimTimeRuleProcessor.Winter_2012;

namespace TheSwimTimeSite.SwimTimeRuleProcessor.Factories
{
    public class SwimTimeRuleFactory
    {
        public static ISwimTimeRule CreateSwimTimeRule(double minHours, double maxHours, DateTime startDate, DateTime endDate)
        {
            return new WINTER_2012_SwimTimeRule(startDate, endDate, startDate, minHours, maxHours, 0.0);
        }

        public static ISwimTimeRule CreateRepeatingTermLongDailySwimTimeRule(double minHours, double maxHours, Term containsMyStartAndEndDates, double hoursOffsetFromMidnight)
        {
            DateTime ruleStartDateTime = containsMyStartAndEndDates.Start;
            ruleStartDateTime = AdjustToNearestPreviousMidnight(ruleStartDateTime);
            ruleStartDateTime = AddOffsetToStartDateTime(ruleStartDateTime, hoursOffsetFromMidnight);
            return new WINTER_2012_SwimTimeRule(containsMyStartAndEndDates.Start, containsMyStartAndEndDates.Endd, ruleStartDateTime, minHours, maxHours, 24.0);
        }
        
        public static ISwimTimeRule CreateRepeatingDailySwimTimeRule(double minHours, double maxHours, DateTime startDate, DateTime endDate, double hoursOffsetFromMidnight)
        {
            DateTime ruleStartDateTime = startDate;
            ruleStartDateTime = AdjustToNearestPreviousMidnight(ruleStartDateTime);
            ruleStartDateTime = AddOffsetToStartDateTime(ruleStartDateTime, hoursOffsetFromMidnight);
            return new WINTER_2012_SwimTimeRule(startDate, endDate, ruleStartDateTime, minHours, maxHours, 24.0);
        }

        public static ISwimTimeRule CreateRepeatingTermLongWeeklySwimTimeRule(double minHours, double maxHours, Term containsMyStartAndEndDates, double hoursOffsetFromSundayMidnight)
        {
            DateTime ruleStartDateTime = containsMyStartAndEndDates.Start;
            ruleStartDateTime = AdjustToNearestPreviousSunday(ruleStartDateTime);
            ruleStartDateTime = AdjustToNearestPreviousMidnight(ruleStartDateTime);
            ruleStartDateTime = AddOffsetToStartDateTime(ruleStartDateTime, hoursOffsetFromSundayMidnight);
            return new WINTER_2012_SwimTimeRule(containsMyStartAndEndDates.Start, containsMyStartAndEndDates.Endd, ruleStartDateTime, minHours, maxHours, 168.0);
        }
        public static ISwimTimeRule CreateRepeatingWeeklySwimTimeRule(double minHours, double maxHours, DateTime startDate, DateTime endDate, double hoursOffsetFromSundayMidnight)
        {
            DateTime ruleStartDateTime = startDate;
            ruleStartDateTime = AdjustToNearestPreviousSunday(ruleStartDateTime);
            ruleStartDateTime = AdjustToNearestPreviousMidnight(ruleStartDateTime);
            ruleStartDateTime = AddOffsetToStartDateTime(ruleStartDateTime, hoursOffsetFromSundayMidnight);
            return new WINTER_2012_SwimTimeRule(startDate, endDate, ruleStartDateTime, minHours, maxHours, 168.0);
        }



        private static DateTime AdjustToNearestPreviousSunday(DateTime ruleStartDateTime)
        {
            while(ruleStartDateTime.DayOfWeek != DayOfWeek.Sunday)
            {
                ruleStartDateTime = ruleStartDateTime.Subtract(TimeSpan.FromHours(24.0));
            }
            return ruleStartDateTime;
        }
        private static DateTime AddOffsetToStartDateTime(DateTime ruleStartDateTime, double hoursOffsetFromMidnight)
        {
            TimeSpan toAdd = TimeSpan.FromHours(hoursOffsetFromMidnight);
            DateTime adjustedDateTime = ruleStartDateTime.Add(toAdd);
            return adjustedDateTime;
        }

        private static DateTime AdjustToNearestPreviousMidnight(DateTime ruleStartDateTime)
        {
            DateTime adjustedDateTime = ruleStartDateTime;
            if (ruleStartDateTime.Hour != 0 || ruleStartDateTime.Minute != 0 || ruleStartDateTime.Second != 0 || ruleStartDateTime.Millisecond != 0)
            {
                TimeSpan toSubtract = new TimeSpan(0, ruleStartDateTime.Hour, ruleStartDateTime.Minute, ruleStartDateTime.Second, ruleStartDateTime.Millisecond);
                adjustedDateTime = ruleStartDateTime.Subtract(toSubtract);
            }
            return adjustedDateTime;
        } 
    }
}