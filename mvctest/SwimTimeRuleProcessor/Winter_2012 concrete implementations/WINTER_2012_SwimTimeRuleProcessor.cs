using System;
using System.Collections.Generic;
using System.Linq;
using TheSwimTimeSite.SwimTimeRuleProcessor.Factories;
using TheSwimTimeSite.SwimTimeRuleProcessor.Interfaces;

namespace TheSwimTimeSite.SwimTimeRuleProcessor.Winter_2012
{
    public class WINTER_2012_SwimTimeRuleProcessor : ISwimTimeRuleProcessor
    {
        private readonly IList<SwimTimeRuleBucket> ruleBucketTree;

        public WINTER_2012_SwimTimeRuleProcessor(IList<ISwimTimeRule> theRules)
        {
            if (theRules == null)
            {
                throw new ArgumentException("Cannot have a null list of rules");
            }

            this.ruleBucketTree = new List<SwimTimeRuleBucket>();

            IList<ISwimTimeRule> rulesDecomposedIntoDiscreteTimePeriods = new List<ISwimTimeRule>();

            foreach (ISwimTimeRule aRule in theRules)
            {
                if (Math.Sqrt(aRule.GetRuleRepeatabilityPeriodInHours() * aRule.GetRuleRepeatabilityPeriodInHours()) <= 1.0 / 60.0)//Rules less than 1 minute in repeatability are considered constant rules
                {
                    rulesDecomposedIntoDiscreteTimePeriods.Add(aRule);
                }
                else//repeated rule, so explcitly make it a concrete one
                {
                    DateTime startDate;//When the rule starts
                    DateTime endDate;//When the rule ends
                    DateTime repDate;//The "zero" date; the repition period "hops" forwards from this date.
                    double repPeriod;
                    ISwimTimeRule temp;
                    ApplicableMinAndMax ruleMinAndMax = aRule.GetMinAndMaxHours();

                    startDate = aRule.GetAbsoluteStartingDate();
                    endDate = aRule.GetAbsoluteEndingDate();
                    repDate = aRule.GetRuleStartDate();
                    repPeriod = aRule.GetRuleRepeatabilityPeriodInHours();

                    DateTime hopDate = repDate;
                    while(hopDate < startDate)//The hop date could be "funky"; move it forward until it is inside the time period we care about.
                    {
                        hopDate = hopDate.Add(TimeSpan.FromHours(repPeriod));
                    }
                    DateTime currentConcreteRuleStartDate = startDate;
                    DateTime currentConcreteRuleEndDate = (hopDate < endDate) ? hopDate : endDate;
                    
                    while (currentConcreteRuleStartDate < endDate)
                    {
                        temp = SwimTimeRuleFactory.CreateSwimTimeRule(ruleMinAndMax.GetMinimumVisitLengthInHoursThatCounts(), ruleMinAndMax.GetMaximumHoursAllowedInThisTimePeriod(), currentConcreteRuleStartDate, currentConcreteRuleEndDate);
                        rulesDecomposedIntoDiscreteTimePeriods.Add(temp);

                        //Now move forward one repition period
                        currentConcreteRuleStartDate = currentConcreteRuleEndDate;
                        hopDate = hopDate.Add(TimeSpan.FromHours(repPeriod));
                        currentConcreteRuleEndDate = (hopDate < endDate) ? hopDate : endDate;
                    }
                }
            }

            SwimTimeRuleBucket tempBucket;
            foreach (ISwimTimeRule decoRules in rulesDecomposedIntoDiscreteTimePeriods)
            {
                tempBucket = new SwimTimeRuleBucket(decoRules);
                ruleBucketTree.Add(tempBucket);
            }

        }

        public SwimTimeRuleProcessingInformation ProcessMultipleVisits(IList<SwimTimeVisit> validAndInvalidVisits)
        {
            if (validAndInvalidVisits == null)
            {
                throw new ArgumentException("Cannot handle a null list of visits");
            }
            
            if (this.ruleBucketTree.Count == 0)
            {
                string tempReason = "No rules";
                if (validAndInvalidVisits.Count == 0)
                {
                    tempReason += " or visits";
                }
                tempReason += ".";
                return new SwimTimeRuleProcessingInformation(0.0, 0.0, tempReason);
            }

            IList<SwimTimeVisit> validVisitsOnly = new List<SwimTimeVisit>();

            //Strip out invalid visits
            foreach (var aVisit in validAndInvalidVisits)
            {
                if(aVisit.GetWasAValidCheckout())
                {
                    validVisitsOnly.Add(aVisit);
                }
            }

            validVisitsOnly = OrderValidVisitsAscendingByStartDate(validVisitsOnly);

            double validTime = 0.0;
            double invalidTime = 0.0;
            double actualTimeAdded;

            ZeroOutRuleBucketTree();//Even though it was probably cleared before, make sure it is cleared now before using it.
            Console.Out.WriteLine("The entire bucket tree:");
            Console.Out.WriteLine("-------------------------------------");
            PrintOutRuleBucketTree(false);
            Console.Out.WriteLine("-------------------------------------");
            Console.Out.WriteLine();
            Console.Out.WriteLine();
            Console.Out.WriteLine("Processing visits");
            Console.Out.WriteLine("-------------------------------------");
            foreach (var aVisit in validVisitsOnly)
            {
                ApplicableMinAndMax applicableMinAndMax = GetMinAndMaxHoursForVisitStartingAt(aVisit.GetStartDateTime());
                Console.Out.WriteLine();
                Console.Out.WriteLine("Visit: " + aVisit);
                Console.Out.WriteLine("Applicable min and max: " + applicableMinAndMax);
                if(aVisit.GetWasAValidCheckout() && VisitStartedWithinARulePeriod(aVisit))
                {
                    double visitElapsedTime = aVisit.GetElapsedTime();
                    if (visitElapsedTime >= applicableMinAndMax.GetMinimumVisitLengthInHoursThatCounts())
                    {
                        actualTimeAdded = Math.Min(visitElapsedTime, applicableMinAndMax.GetMaximumHoursAllowedInThisTimePeriod());
                        validTime += actualTimeAdded;
                        invalidTime += visitElapsedTime - actualTimeAdded;
                        FillRuleBucketTree(actualTimeAdded, aVisit.GetStartDateTime());
                        PrintOutRuleBucketTree(true);
                    }
                }
            }//end visit
            Console.Out.WriteLine("-------------------------------------");
            Console.Out.WriteLine("END Processing visits");

            ZeroOutRuleBucketTree();//Cleans up the tree for reuse next time.
            return new SwimTimeRuleProcessingInformation(validTime, invalidTime, "");
        }

        private IList<SwimTimeVisit> OrderValidVisitsAscendingByStartDate(IList<SwimTimeVisit> validVisitsOnly)
        {
            IEnumerable<SwimTimeVisit> sorted = validVisitsOnly.OrderBy(visit => visit.GetStartDateTime());
            IList<SwimTimeVisit> sortedIList = sorted.ToList();
            return sortedIList;
        }

        private void ZeroOutRuleBucketTree()
        {
            foreach (SwimTimeRuleBucket ctrb in ruleBucketTree)
            {
                ctrb.ResetToZeroHoursFilled();
            }
        }

        private void FillRuleBucketTree(double timeAdded, DateTime startDate)
        {
            foreach (SwimTimeRuleBucket ctrb in ruleBucketTree)
            {
                if(startDate >= ctrb.GetStartDate() && startDate <= ctrb.GetEndDate())
                {
                    ctrb.FillHours(timeAdded);
                }
            }//end foreach
        }

        private ApplicableMinAndMax GetMinAndMaxHoursForVisitStartingAt(DateTime startTime)
        {
            double min = Double.MinValue;
            double max = Double.MaxValue;
            
            //Check to see what is already filled
            double minHoursUnfilled = Double.MaxValue;
            Console.Out.WriteLine("     Applicable Buckets: ");
            foreach (SwimTimeRuleBucket ctrb in ruleBucketTree)
            {
                if (RuleAppliesToThisVisit(startTime, ctrb))
                {
                    ApplicableMinAndMax temp2 = ctrb.GetMinAndMaxHoursForVisitStartingAt(startTime);
                    Console.Out.WriteLine("         " + temp2);
                    if (temp2 != null)
                    {
                        min = Math.Max(min, temp2.GetMinimumVisitLengthInHoursThatCounts());
                        max = Math.Min(max, temp2.GetMaximumHoursAllowedInThisTimePeriod());
                    }
                    Console.Out.WriteLine("         " + ctrb);
                    double hoursUnfilled = ctrb.GetHowManyHoursUnfilled();
                    //hoursUnfilled = Math.Max(0.0, hoursUnfilled);
                    Console.Out.WriteLine("             Hours unfilled: " + hoursUnfilled);
                    minHoursUnfilled = Math.Min(minHoursUnfilled, hoursUnfilled);
                    Console.Out.WriteLine("             Min Hours unfilled: " + minHoursUnfilled);
                }
            }//end foreach

            max = Math.Min(max, minHoursUnfilled);
             
            return new ApplicableMinAndMax(min, max);
        }

        private bool RuleAppliesToThisVisit(DateTime startTime, SwimTimeRuleBucket ctrb)
        {
            return startTime >= ctrb.GetStartDate() && startTime <= ctrb.GetEndDate();
        }


        private bool VisitStartedWithinARulePeriod(SwimTimeVisit theSwimTimeVisit)
        {
            DateTime visitStartDate = theSwimTimeVisit.GetStartDateTime();
            foreach (SwimTimeRuleBucket rb in ruleBucketTree)
            {
                if (visitStartDate >= rb.GetStartDate() && visitStartDate <= rb.GetEndDate())
                {
                    return true;
                }
            }
            return false;
        }

        private void PrintOutRuleBucketTree(bool skipZeroFilledBuckets)
        {
            Console.Out.WriteLine("-------------------------------------");
            foreach (SwimTimeRuleBucket rb in ruleBucketTree)
            {
                if (skipZeroFilledBuckets && Math.Sqrt(Math.Pow(rb.GetHowManyHoursFilled() - 0.0, 2.0)) <= 0.01)
                {
                }
                else
                {
                    Console.Out.WriteLine("     Concrete RuleBucket:" + rb);
                }
            }
            Console.Out.WriteLine("-------------------------------------");
            Console.Out.WriteLine();
        }
    }
}