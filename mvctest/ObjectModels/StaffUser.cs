using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TheSwimTimeSite.ObjectModels
{
    [Serializable]
    public class StaffUser
    {
        private DateTime dateTime;
        public String UserName { get; set; }
        public String Email { get; set; }
        [Display(Name = "Active Account")]
        public bool ActiveAccount { get; set; }
        [Display(Name = "Staff ")]
        public bool IsStaff { get; set; }
        [Display(Name = "Administrator ")]
        public bool IsAdmin { get; set; }
        public DateTime LastLoginDate { get; set; }

        public StaffUser()
        { }

        public StaffUser(string userName, DateTime dateTime, bool activeAccount)
        {
            // TODO: Complete member initialization
            this.UserName = userName;
            this.LastLoginDate = dateTime;
            this.ActiveAccount = activeAccount;
        }

        /*
                Sign Up for Your New Account
        User Name:	
        Password:	
        Confirm Password:	
        E-mail:	
        Security Question:	
        Security Answer:	
	
        Roles
        Select roles for this user:
        Administrators
        Staff */
    }
}