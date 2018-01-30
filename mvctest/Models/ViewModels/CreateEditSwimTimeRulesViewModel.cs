using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TheSwimTimeSite.ObjectModels;

namespace TheSwimTimeSite.Models.ViewModels
{
    public class CreateEditSwimTimeRulesViewModel
    {
        public SwimTime Creating { get; set; }
        public List<KeyPair> Days = new List<KeyPair>{ new KeyPair("Sunday", 0), new KeyPair("Monday", 24), new KeyPair("Tuesday", 48), new KeyPair("Wednesday", 72), 
            new KeyPair("Thursday", 96), new KeyPair("Friday", 120), new KeyPair("Saturday", 144) };
        public IEnumerable<SelectListItem> DaysList { get; set; }
        public IEnumerable<SelectListItem> Course { get; set; }
        private List<KeyPair> Repetition = new List<KeyPair>{ new KeyPair("Constant", 0), new KeyPair("Daily", 24), new KeyPair("Weekly", 168)};
        public IEnumerable<SelectListItem> RepetitionSchedule { get; set; }

        public CreateEditSwimTimeRulesViewModel()
        {
            DaysList = Days.Select(x => new SelectListItem
            {
                Text = x.Obj,
                Value = x.Val.ToString()
            });
            RepetitionSchedule = Repetition.Select(x => new SelectListItem
            {
                Text = x.Obj,
                Value = x.Val.ToString()
            });
        }

        public CreateEditSwimTimeRulesViewModel(SwimTime creating, List<Course> courseList)
        {
            Creating = creating;
            DaysList = Days.Select(x => new SelectListItem
            {
                Text = x.Obj,
                Value = x.Val.ToString()
            });
            RepetitionSchedule = Repetition.Select(x => new SelectListItem
            {
                Text = x.Obj,
                Value = x.Val.ToString()
            });
            IEnumerable<Course> CTOnly = from co in courseList
                                         where co.CheckOut
                                         select co;
            Course = CTOnly.Select(x => new SelectListItem
            {
                Text = x.Title + " " + x.CRN,
                Value = x.CRN.ToString()
            });
        }
        //for creation view
        public CreateEditSwimTimeRulesViewModel(List<Course> courseList)
        {
            DaysList = Days.Select(x => new SelectListItem
            {
                Text = x.Obj,
                Value = x.Val.ToString()
            });
            IEnumerable<Course> CTOnly = from co in courseList
                                         where co.CheckOut
                                         select co;
            Course = CTOnly.Select(x => new SelectListItem
            {
                Text = x.Title + " " + x.CRN,
                Value = x.CRN.ToString()
            });
            RepetitionSchedule = Repetition.Select(x => new SelectListItem
            {
                Text = x.Obj,
                Value = x.Val.ToString()
            });
        }

            //for creation view
        public CreateEditSwimTimeRulesViewModel(List<Course> courseList, SwimTime input)
        {
            Creating = input;
            DaysList = Days.Select(x => new SelectListItem
            {
                Text = x.Obj,
                Value = x.Val.ToString()
            });
            IEnumerable<Course> CTOnly = from co in courseList
                                         where co.CheckOut
                                         select co;
            Course = CTOnly.Select(x => new SelectListItem
            {
                Text = x.Title + " " + x.CRN,
                Value = x.CRN.ToString()
            });
            RepetitionSchedule = Repetition.Select(x => new SelectListItem
            {
                Text = x.Obj,
                Value = x.Val.ToString()
            });
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