using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheSwimTimeSite.ObjectModels
{
    [Serializable]
    public partial class User
    {
        public int UserID { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String StudentID { get; set; }
        public String CardScan { get; set; }
   
        public String Phone { get; set; }
        public String Email { get; set; }
        public String ClassInSchool { get; set; }
        public String UserType { get; set; }
        public bool Active { get; set; }
        public bool Staff { get; set; }
        public String Comments { get; set; }
        public int IsInCourse { get; set; }//current course the user is checked in for
        public int AssociatedVisitID { get; set; }
        public DateTime CheckInAt { get; set; }//current course the user is checked in for
        public int CheckOutFor { get; set; }
        public DateTime CreatedDate { get; set; }

        public List<Certification> Certs { get; set; }
        public List<Course> Classes { get; set; }

        //constructors
        public User()
        {
            //Certs = new List<Certification>();//default to not null
            Classes = new List<Course>();
        }
        public User(DateTime d)
        {
            CheckInAt = d;
        }
        public User(int userID, String firstName, String lastName)
        {
            UserID = userID;
            FirstName = firstName;
            LastName = lastName;//display name upper first case
            //Certs = new List<Certification>();
            Classes = new List<Course>();
        }

        public User(int userID, String firstName, String lastName, int isInCourse, int assocaitedVisit)//with course in
        {
            UserID = userID;
            FirstName = firstName;
            LastName = lastName;//display name upper first case
            IsInCourse = isInCourse;//set current course
            AssociatedVisitID = assocaitedVisit;
            //Certs = new List<Certification>();
            Classes = new List<Course>();
        }

        public User(int userID, String firstName, String lastName, String userType)//with user type
        {
            UserID = userID;
            FirstName = firstName;
            LastName = lastName;//display name upper first case
            UserType = userType;
           // Certs = new List<Certification>();
            Classes = new List<Course>();
        }

        public User(int userID, String firstName, String lastName, String studentID, String cardscan,
          String phone, String email, String classInSchool, String userType,
            bool active, bool staff, String comments)
        {
            UserID = userID;
            FirstName = firstName;
            LastName = lastName;//display name upper first case
            StudentID = studentID;
            CardScan = cardscan;
           
            Phone = phone;
            Email = email;
            ClassInSchool = classInSchool;
            UserType = userType;
            Active = active;
            Staff = staff;
            Comments = comments;

            //Certs = new List<Certification>();
            Classes = new List<Course>();
        }

        //for user history
        public User(int userID, String firstName, String lastName, String studentID, String cardscan,
            String phone, String email, String classInSchool, String userType,
            bool active, bool staff, String comments, DateTime created)
        {
            UserID = userID;
            FirstName = firstName;
            LastName = lastName;//display name upper first case
            StudentID = studentID;
            CardScan = cardscan;
          
            Phone = phone;
            Email = email;
            ClassInSchool = classInSchool;
            UserType = userType;
            Active = active;
            Staff = staff;
            Comments = comments;
            CreatedDate = created;

            //Certs = new List<Certification>();
            Classes = new List<Course>();
        }

        public String SelfSerialize()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}