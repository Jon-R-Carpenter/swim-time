using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TheSwimTimeSite.ObjectModels;
using TheSwimTimeSite.SwimTimeRuleProcessor.Factories;
using TheSwimTimeSite.SwimTimeRuleProcessor.Interfaces;

namespace TheSwimTimeSite.SwimTimeRuleProcessor
{
    public class SwimTimeToISwimTimeAdaptor
    {
        public static IList<ISwimTimeRule> ConvertListOfSwimTimeToListOfISwimTimeRule()
        {
             
            IList<ISwimTimeRule> theSecondList = new List<ISwimTimeRule>();
            /*
            foreach (SwimTime swimTime in theFirstList)
            {
                ISwimTimeRule theConveretedRule = null;
                double distanceFrom0RepeatHours = DistanceBetweenTheTwo(swimTime.RepetitionPeriod, 0.0);
                double distanceFromDailyRule = DistanceBetweenTheTwo(swimTime.RepetitionPeriod, 24.0);
                double distanceFromWeeklyRule = DistanceBetweenTheTwo(swimTime.RepetitionPeriod, 168.0);
                
                if(distanceFromWeeklyRule <= 1.0/60.0)
                {
                    theConveretedRule = SwimTimeRuleFactory.CreateRepeatingWeeklySwimTimeRule(swimTime.MinHoursPerVisit,swimTime.MaxHoursPerVisit,swimTime.Start, swimTime.End,swimTime.StartOffset);
                }else if(distanceFromDailyRule <= 1.0/60.0)
                {
                    theConveretedRule = SwimTimeRuleFactory.CreateRepeatingDailySwimTimeRule(swimTime.MinHoursPerVisit, swimTime.MaxHoursPerVisit, swimTime.Start, swimTime.End, swimTime.StartOffset);
                }else
                {
                    theConveretedRule = SwimTimeRuleFactory.CreateSwimTimeRule(swimTime.MinHoursPerVisit, swimTime.MaxHoursPerVisit, swimTime.Start, swimTime.End);
                }

                theSecondList.Add(theConveretedRule);
            }
            */
            return theSecondList;
            
        }

        //private static double DistanceBetweenTheTwo(double alpha, double beta)
        //{
            //double delta = alpha - beta;
            //double deltaSquared = delta*delta;
            //double sqrt = Math.Sqrt(deltaSquared);
            //return sqrt;
        //}
    }
}