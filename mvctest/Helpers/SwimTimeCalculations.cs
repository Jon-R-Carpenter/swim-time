using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheSwimTimeSite.Helpers
{
    public class ClimbTimeCalculations
    {
        public static int[] ConvertToHoursMins(double time)
        {
            int hour = (int)(Math.Floor(time));
            int min = (int)((time - hour) * 60.0);
            return new int[] { hour, min };
        }
    }
}