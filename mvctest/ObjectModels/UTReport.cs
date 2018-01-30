using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheSwimTimeSite.ObjectModels
{
    public class UTReport
    {
        public String HeaderRow { get; set; }
        public List<UTReportRow> Data { get; set; }
        public DateTime Start { get; set; }
        public DateTime Finish { get; set; }
        public String FileName { get; set; }

        public UTReport()
        {
            //HeaderRow = "UserID, Last Name, First Name, User Type, Total Visits";
            HeaderRow = "Last Name, First Name, User Type, Total Visits";
            Data = new List<UTReportRow>();
        }

        public UTReport(DateTime Start, DateTime Finish, String FileName)
        {
            this.Start = Start;
            this.Finish = Finish;
            this.FileName = FileName;
            //HeaderRow = "UserID, Last Name, First Name, User Type, Total Visits";
            HeaderRow = "Last Name, First Name, User Type, Total Visits";
            Data = new List<UTReportRow>();
        }
    }
}