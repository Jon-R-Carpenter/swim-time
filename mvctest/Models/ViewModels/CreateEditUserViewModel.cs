using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheSwimTimeSite.ObjectModels;

namespace TheSwimTimeSite.Models.ViewModels
{
    public class CreateEditUserViewModel
    {
        public User Editing { get; set; }
        public List<String> Classes { get; set; }
        public IEnumerable<SelectListItem> Staff { get; set; }
        public List<String> UserTypes { get; set; }
        public int StaffID { get; set; }

        public CreateEditUserViewModel() { }

        public CreateEditUserViewModel(User user, List<User> staffList, List<String> types)
        {
            StaffID = -1;
            Editing = user;
            Staff = staffList.Select(x => new SelectListItem
            {
                Text = x.FirstName + " " + x.LastName,
                Value = x.UserID.ToString()
            });
            Classes = new List<String> { "Freshman", "Sophomore", "Junior", "Senior", "Graduate", "Other" };//create class selection list
            UserTypes = types;
        }
    }
}