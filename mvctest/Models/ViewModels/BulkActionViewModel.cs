using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TheSwimTimeSite.ObjectModels;

namespace TheSwimTimeSite.Models.ViewModels
{
    public class BulkActionViewModel
    {
        public String Message { get; set; }
        public IEnumerable<SelectListItem> SelectedList { get; set; }
        public IEnumerable<SelectListItem> FilterList { get; set; }
        public IEnumerable<SelectListItem> ActionList { get; set; }
        public IEnumerable<SelectListItem> CertList { get; set; }
        public IEnumerable<SelectListItem> ClassList { get; set; }
        public IEnumerable<SelectListItem> UserTypesList { get; set; }
        public bool Invert { get; set; }
        public IEnumerable<SelectListItem> Staff { get; set; }

        public string JsonUsers;

        //constructors
        public BulkActionViewModel()
        {

        }

        public BulkActionViewModel(List<String> userTypes, String message, List<Certification> certs, List<User> staff, List<User> users)
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

            List<KeyPair> actions = new List<KeyPair> { { new KeyPair("Change User Activation", 1) }, {new KeyPair("Add/Remove User Staff", 2)},
            {new KeyPair("Add/Remove User Certifications", 3)}, {new KeyPair("Change UserType", 4)}, {new KeyPair("Change Class in School", 5)}}; 
            Message = message;
            //user type list without classes for filtering
            UserTypesList = userTypes.Select(x => new SelectListItem
            {
                Text = x,
                Value = x
            });

            List<String> classes = new List<String> { "Freshman", "Sophomore", "Junior", "Senior", "Graduate", "Other" };//create class selection list
            FilterList = userTypes.Select(x => new SelectListItem
            {
                Text = x,
                Value = x
            });

            ClassList = classes.Select(x => new SelectListItem
            {
                Text = x,
                Value = x
            });

            CertList = certs.Select(x => new SelectListItem
            {
                Text = x.Title,
                Value = x.CertificationID.ToString()
            });

            ActionList = actions.Select(x => new SelectListItem
            {
                Text = x.Obj,
                Value = x.Val.ToString()
            });

            //user list managed by json at view level
            //staff list
            Staff = staff.Select(x => new SelectListItem
            {
                Text = x.LastName + " " + x.FirstName,
                Value = x.UserID.ToString()
            });

            SelectedList = new List<SelectListItem>();
        }

        public class KeyPair
        {
            public String Obj { get; set; }
            public int Val { get; set; }

            public KeyPair(String obj, int val)
            {
                Obj = obj;
                Val = val;
            }
        }

    }
}