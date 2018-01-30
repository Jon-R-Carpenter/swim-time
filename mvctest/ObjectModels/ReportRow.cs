using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheSwimTimeSite.ObjectModels
{
    public class ReportRow
    {
        public int userID { get; set; }
        public String lastName { get; set; }
        public String firstName { get; set; }
        public String studentID { get; set; }
        public String userType { get; set; }
        public String classInSchool { get; set; }
        public bool staffFlag { get; set; }
        public DateTime dateTime { get; set; }
        public String visitType { get; set; }
        public int courseCode { get; set; }
        public int inOut { get; set; }

        public ReportRow(int userID, String lastName, String firstName, String studentID, String userType,
            String classInSchool, bool staffFlag, DateTime dateTime, int courseCode)
        {
            this.userID = userID;
            this.lastName = lastName;
            
            this.firstName = firstName;
            
            this.studentID = studentID;
            this.userType = userType;
            this.classInSchool = classInSchool;
            
            this.staffFlag = staffFlag;
            
            this.dateTime = dateTime;
            
            this.courseCode = courseCode;
     
            
        }

        public String CSVToString()
        {
            /* String csv = this.UserID.ToString() + ","
                     + this.VisitDateTime.ToShortDateString() + ","
                     + this.VisitDateTime.ToShortTimeString() + ","
                     + this.LastName + ","
                     + this.FirstName + ","
                     + this.VisitName + ",";

             int[] times = Helpers.ClimbTimeCalculations.ConvertToHoursMins(this.Total);
             csv += times[0] + ":" + times[1];

             csv += "\n";
             */

            String csv = userID.ToString() + ","
                        + lastName + ","
                        + firstName + ","
                        //+ studentID + ","
                        + userType + ",";
            if (classInSchool != null)
            {
                csv += classInSchool + ",";
            }
            else
            {
                csv += "N/A,";
            }
            //+ row.staffFlag + ","
            //Day of Week, Month, Day, Year, Hour, Minutes
            csv += dateTime.DayOfWeek.ToString() + ",";
            csv += dateTime.Month.ToString() + ",";
            csv += dateTime.Day.ToString() + ",";
            csv += dateTime.Year.ToString() + " ,";
      
           
            
                csv += courseCode + ",";
            
         
            csv += "\n";

            return csv;
        }
    }
}