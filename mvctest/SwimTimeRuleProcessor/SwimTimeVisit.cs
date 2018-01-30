using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheSwimTimeSite.SwimTimeRuleProcessor
{
    //Will be more fully fleshed out (with error checking) in the future
    //Use the factory method to create this class, as the constructor will be likely changing.

    public class SwimTimeVisit
    {
        private DateTime visitStarted;
        private DateTime visitEnded;
        private bool visitWasAValidOne;

        public SwimTimeVisit(DateTime startDateTime, DateTime endDateTime, bool wasAValidCheckout)
        {
            if(endDateTime.CompareTo(startDateTime) < 0)
            {
                throw new ArgumentException("Cannot end visit before the visit started.");
            }
            this.visitStarted = startDateTime;
            this.visitEnded = endDateTime;
            this.visitWasAValidOne = wasAValidCheckout;
        }

        public double GetElapsedTime()
        {
            double millisPerHour = 1000.0*60.0*60.0;
            long startTimeMillis = this.visitStarted.Ticks/TimeSpan.TicksPerMillisecond;
            long endTimeMillis = this.visitEnded.Ticks/TimeSpan.TicksPerMillisecond;
            long elapsedTimeInMillis = endTimeMillis - startTimeMillis;
            double elapsedTimeInHours = ((double)elapsedTimeInMillis) / millisPerHour;
            return elapsedTimeInHours;
        }

        public DateTime GetStartDateTime()
        {
            return this.visitStarted;
        }
        public DateTime GetEndDateTime()
        {
            return this.visitEnded;
        }
        public bool GetWasAValidCheckout()
        {
            return this.visitWasAValidOne;
        }

        public override string ToString()
        {
            return "<(" + visitStarted + " to " + visitEnded + ") valid: " + this.visitWasAValidOne + " totalElapsedTime: " + this.GetElapsedTime() + ">";
        }
    }
}