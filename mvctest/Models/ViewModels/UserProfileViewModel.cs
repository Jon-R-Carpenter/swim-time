using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheSwimTimeSite.ObjectModels;


//viewmodel for a user profile including all courses this user is enrolled in and a list of certifications this user has
namespace TheSwimTimeSite.Models.ViewModels
{
    public class UserProfileViewModel
    {
        public User Editing { get; set; }
        public IEnumerable<SelectListItem> CertsHeld { get; set; }
        public IEnumerable<SelectListItem> CertsAvailable { get; set; }
        public int UserID { get; set; }//this is really staff ID
        public IEnumerable<SelectListItem> Staff { get; set; }

        public UserProfileViewModel()
        {
        }

        public UserProfileViewModel(User editing, List<Certification> available, List<User> staff)
        {
            CertsHeld = editing.Certs.Select(x => new SelectListItem
            {
                Text = x.Title,
                Value = x.CertificationID.ToString()
            });

            CertsAvailable = available.Select(x => new SelectListItem
            {
                Text = x.Title,
                Value = x.CertificationID.ToString()
            });

            Staff = staff.Select(x => new SelectListItem
            {
                Text = x.LastName + " " + x.FirstName,
                Value = x.UserID.ToString()
            });

            Editing = editing;
            UserID = editing.UserID;
        }
    }
}