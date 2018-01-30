using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheSwimTimeSite.ObjectModels
{
    public class Course
    {
        public int CRN { get; set; }

        public String courseCode { get; set; }
        public String Title { get; set; }
        public int? DayOfWeek { get; set; }//must be kept nullable to avoid excess validation
        public int? Duration { get; set; }
        public int? TimeOfDay { get; set; }
        public bool CheckOut { get; set; }
        public Term Term { get; set; }

        public List<User> Enrolled { get; set; }//users enrolled in this course

        //display methods
        public String DayString()
        {
            List<String> temp = new List<String> { "--", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
            return temp[DayOfWeek ?? 0];
        }
        public String TimeString()
        {
            if (TimeOfDay == 0)
                return "--";
            List<String> temp = new List<String> { "2 hours", "4 hours", "6 hours", "8 hours", "10 hours", "12 hours", "14 hours", "16 hours", "18 hours", "20 hours", "22 hours" };
            return temp[(TimeOfDay ?? 8) - 8];
        }
        public String DurationString()
        {
            List<String> temp = new List<String> {"--","HalfDay","AllDay" };
            return temp[Duration ?? 0];
        }

        //constructors
        public Course()
        {
            Enrolled = new List<User>();//default to non null list
        }
        public Course(String courseCode)
        {
            this.courseCode = courseCode;
        }
        public Course(int crn,String title)
        {
            CRN = crn;
           
            Title = title;
            
            Enrolled = new List<User>();//default to non null list
        }

        public Course(int crn,  String title, bool checkout)
        {
            CRN = crn;
            
            Title = title;
            CheckOut = checkout;
            Enrolled = new List<User>();//default to non null list
        }

        public Course(int crn, String title, int dayofweek, int duration,
            int timeOfDay, bool checkout, Term term)
        {
            CRN = crn;
            
            Title = title;
            DayOfWeek = dayofweek;
            Duration = duration;
            TimeOfDay = timeOfDay;
            CheckOut = checkout;
            Term = term;

            Enrolled = new List<User>();//default to non null list
        }
        public Course(int crn, String code, String title, int dayofweek, int duration,
            int timeOfDay, bool checkout, Term term)
        {
            this.CRN = crn;
            this.courseCode = code;
            this.Title = title;
            this.DayOfWeek = dayofweek;
            this.Duration = duration;
            this.TimeOfDay = timeOfDay;
            this.CheckOut = checkout;

            Term = term;
            Enrolled = new List<User>();

        }
    }
}