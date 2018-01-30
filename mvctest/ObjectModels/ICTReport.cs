using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheSwimTimeSite.ObjectModels
{
    public class ICTReport
    {
        public String HeaderRow { get; set; }
        public List<ICTReportRow> Data { get; set; }
        public Course CourseID { get; set; }

        public ICTReport()
        {
            HeaderRow = "UserID, Date, Time, Last Name, First Name, In/Out, Total";
            Data = new List<ICTReportRow>();
        }

        public ICTReport(Course CourseID)
        {
            this.CourseID = CourseID;
            HeaderRow = "UserID, Date, Time, Last Name, First Name, In/Out, Total";
            Data = new List<ICTReportRow>();
        }
    }
}