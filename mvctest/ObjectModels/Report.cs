using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheSwimTimeSite.ObjectModels
{
    public class Report
    {
        public String HeaderRow { get; set; }
        public List<ReportRow> Data { get; set; }
        public DateTime Start { get; set; }
        public DateTime Finish { get; set; }
        public String First { get; set; }
        public String Last { get; set; }
        public Boolean IncludeOuts { get; set; }
        //[Required]
        public String FileName { get; set; }

        public Report()
        {
            HeaderRow = "UserID, Last Name, First Name, User Type, Class In School, Day of Week, Month, Day, Year,  CourseID";
            Data = new List<ReportRow>();
        }

        public Report(DateTime Start, DateTime Finish, String FileName, Boolean IncludeOuts)
        {
            this.Start = Start;
            this.Finish = Finish;
            this.FileName = FileName;
            this.IncludeOuts = IncludeOuts;
            HeaderRow = "UserID, Last Name, First Name, User Type, Class In School, Day of Week, Month, Day, Year, CourseID ";
            Data = new List<ReportRow>();
        }
        public Report(DateTime Start, DateTime Finish, String FirstName, String LastName, String FileName, Boolean IncludeOuts)
        {
            this.Start = Start;
            this.Finish = Finish;
            this.First = FirstName;
            this.Last = LastName;
            this.FileName = FileName;
            this.IncludeOuts = IncludeOuts;
            HeaderRow = "UserID, Last Name, First Name, User Type, Class In School, Day of Week, Month, Day, Year, Hour, Minutes";
            Data = new List<ReportRow>();
        }



    }
}