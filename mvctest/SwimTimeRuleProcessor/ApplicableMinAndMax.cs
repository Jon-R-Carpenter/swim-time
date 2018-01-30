namespace TheSwimTimeSite.SwimTimeRuleProcessor
{
    public class ApplicableMinAndMax
    {
        private readonly double minimumHoursThatCounts;
        private readonly double maximumHoursAllowedInTimePeriod;

        public ApplicableMinAndMax(double min, double max)
        {
            this.minimumHoursThatCounts = min;
            this.maximumHoursAllowedInTimePeriod = max;
        }

        public double GetMinimumVisitLengthInHoursThatCounts()
        {
            return this.minimumHoursThatCounts;
        }

        public double GetMaximumHoursAllowedInThisTimePeriod()
        {
            return this.maximumHoursAllowedInTimePeriod;
        }

        public override string ToString()
        {
            return "< min: " + this.minimumHoursThatCounts + " max:" + this.maximumHoursAllowedInTimePeriod +">";
        }
    }
}