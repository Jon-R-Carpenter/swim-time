using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheSwimTimeSite.ObjectModels
{
    public class UTReportRow
    {
        public int TotalVisits { get; set; }
        public User Current { get; set; }

        public UTReportRow(int TotalVisits, User Current)
        {
            this.TotalVisits = TotalVisits;
            this.Current = Current;
        }

        public String CSVToString()
        {
            String csv = "";
            /*if (this.Current.StudentID == null)
                csv += " ,";
            else
                csv += this.Current.StudentID.ToString() + ",";*/
            csv += this.Current.LastName + ","
            + this.Current.FirstName + ","
            + this.Current.UserType + ","
            + this.TotalVisits.ToString();


            csv += "\n";

            return csv;
        }
    }
}