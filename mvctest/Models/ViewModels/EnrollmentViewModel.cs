using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using TheSwimTimeSite.ObjectModels;

//contains a viewmodel that contains the course and select lists for students enrolled and not enrolled

namespace TheSwimTimeSite.Models.ViewModels
{
    public class EnrollmentViewModel
    {
        public Course Editing { get; set; }
        public IEnumerable<SelectListItem> Enrolled { get; set; }
        public int CourseID { get; set; }

        public string JsonUsers;

        //constructors
        public EnrollmentViewModel()
        {
        }

        public EnrollmentViewModel(Course editing, List<User> users)
        {
            List<object> temp = new List<object>();
            temp.AddRange(from x in users
                          select new
                          {
                              UserID = x.UserID,
                              FirstName = x.FirstName,
                              LastName = x.LastName,
                              ClassInSchool = x.ClassInSchool,
                              UserType = x.UserType
                          });


            JsonUsers = JsonConvert.SerializeObject(temp.ToArray());

            Enrolled = editing.Enrolled.Select(x => new SelectListItem
            {
                Text = x.FirstName + " " + x.LastName,
                Value = x.UserID.ToString()
            });
            
            Editing = editing;
            CourseID = editing.CRN;//for passing back to controller through hidden
        }
    }
}