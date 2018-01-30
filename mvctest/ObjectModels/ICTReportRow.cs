using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheSwimTimeSite.ObjectModels
{
    public class ICTReportRow
    {
        public int UserID { get; set; }
        public DateTime VisitDateTime { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public int InOut { get; set; }
        public double Total { get; set; }
        public String VisitName { get; set; }
        private enum VisitTypes { InValid = -1, In = 1, Out = 0 }

        public ICTReportRow(int UserId, String FirstName, String LastName, int InOut, double Total, DateTime dateTime)
        {
            this.UserID = UserId;
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.InOut = InOut;
            this.Total = Total;
            this.VisitName = ((VisitTypes)InOut).ToString();
            this.VisitDateTime = dateTime;
        }

        public String CSVToString()
        {
            String csv = this.UserID.ToString() + ","
                    + this.VisitDateTime.ToShortDateString() + ","
                    + this.VisitDateTime.ToShortTimeString() + ","
                    + this.LastName + ","
                    + this.FirstName + ","
                    + this.VisitName + ",";
            if (this.Total >= 0)
            {
                int[] times = Helpers.ClimbTimeCalculations.ConvertToHoursMins(this.Total);
                csv += times[0] + ":" + times[1];
            }
            /*else
            {
                csv += "";
            }*/

            csv += "\n";

            return csv;
        }
    }
}