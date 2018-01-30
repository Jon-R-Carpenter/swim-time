using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TheSwimTimeSite.ObjectModels
{
    [Serializable]
    public class Visit
    {
        [ScaffoldColumn(false)]
        public int VisitID { get; set; }
        public User Visitor { get; set; }
        public String VisitType { get; set; }
        public Course CourseAttendance { get; set; }
        public String CourseName { get; set; }
        public DateTime VisitTime { get; set; }
        [ScaffoldColumn(false)]
        public int? AssociatedVisit { get; set; }
        [ScaffoldColumn(false)]
        public int UserID { get; set; }
        public DateTime InOutTime { get; set; }
        public int InOutFlag { get; set; } //1 in 0 out -1 invalid checkout
        public String InOutString { get; set; }
        public String IP { get; set; }
        public String SSO { get; set; }
        public bool ValidVisit { get; set; }//{ get { return InOutFlag != -1; } set { ValidVisit = value; } }
        public String VisitName { get; set; }
        private enum VisitTypes { InValid = -1, In = 1, Out = 0 }

        public TimeSpan checkInAt { get; set; }

        public TimeSpan checkOut { get; set; }
        //constructors
        public Visit()
        {

        }

        public Visit(int visiID, String type, String course, DateTime visitTime, int inout)
        {
            VisitID = visiID;
            VisitType = type;
            CourseName = course;
            VisitTime = visitTime;
            InOutFlag = inout;
        }

        //for real time climb time rule processing
        public Visit(int visiID, DateTime visitTime, TimeSpan time,TimeSpan checkOutTime)
        {
            VisitID = visiID;
            VisitTime = visitTime;
            this.checkInAt = time;
            this.checkOut = checkOutTime;
        }

        //for climb time reporting
        public Visit(int visiID, DateTime visitTime, int inout, User user)
        {
            VisitID = visiID;
            VisitTime = visitTime;
            InOutFlag = inout;
            Visitor = user;
        }

        public Visit(int visitID, int userID, DateTime dateTime, string type, int inOut, string ip, string sso, bool lineValid, int? associatedVisit)
        {
            // TODO: Complete member initialization
            VisitID = visitID;
            UserID = userID;
            InOutTime = dateTime;
            VisitType = type;
            InOutFlag = inOut;
            IP = ip;
            SSO = sso;
            ValidVisit = lineValid;
            AssociatedVisit = associatedVisit;
            VisitTypes visitType = ((VisitTypes)inOut);
            VisitName = visitType.ToString();

        }

        public Visit(int visitID, int userID, User use, DateTime dateTime, string type, int inOut, string ip, string sso, bool lineValid, int? associatedVisit)
        {
            Visitor = use;
            VisitID = visitID;
            UserID = userID;
            InOutTime = dateTime;
            VisitType = type;
            InOutFlag = inOut;
            IP = ip;
            SSO = sso;
            ValidVisit = lineValid;
            AssociatedVisit = associatedVisit;
            VisitTypes visitType = ((VisitTypes)inOut);
            VisitName = visitType.ToString();

        }
    }
}