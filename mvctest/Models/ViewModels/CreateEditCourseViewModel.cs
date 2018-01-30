using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheSwimTimeSite.ObjectModels;

namespace TheSwimTimeSite.Models.ViewModels
{
    public class CreateEditCourseViewModel
    {
        public Course Creating { get; set; }
        public List<KeyPair> Days = new List<KeyPair>{ new KeyPair("Monday", 1), new KeyPair("Tuesday", 2), new KeyPair("Wednesday", 3), 
            new KeyPair("Thursday", 4), new KeyPair("Friday", 5), new KeyPair("Saturday", 6), new KeyPair("Sunday", 7) };
        public IEnumerable<SelectListItem> DaysList { get; set; }
        public List<KeyPair> Times = new List<KeyPair>{new KeyPair("2 hours", 8), new KeyPair("4 hours", 9), new KeyPair("6 hours", 10), new KeyPair("8 hours", 11), 
            new KeyPair("10 hours", 12), new KeyPair("12 hours", 13), new KeyPair("14 hours", 14), new KeyPair("16 hours", 15), new KeyPair("18 hours", 16), 
            new KeyPair("20 hours", 17), new KeyPair("22 hours", 18)};
        public IEnumerable<SelectListItem> TimeList { get; set; }
        public List<KeyPair> Durations = new List<KeyPair> { new KeyPair("HalfDay", 1),new KeyPair("AllDay",2)};
        public IEnumerable<SelectListItem> DurationList { get; set; }
        public IEnumerable<SelectListItem> TermList { get; set; }


        //constructors
        public CreateEditCourseViewModel()
        {
            DaysList = Days.Select(x => new SelectListItem
            {
                Text = x.Obj,
                Value = x.Val.ToString()
            });
            TimeList = Times.Select(x => new SelectListItem
            {
                Text = x.Obj,
                Value = x.Val.ToString()
            });
            DurationList = Durations.Select(x => new SelectListItem
            {
                Text = x.Obj,
                Value = x.Val.ToString()
            });
            TermList = new List<SelectListItem>();
        }

        //constructors
        public CreateEditCourseViewModel(List<Term> terms)
        {
            DaysList = Days.Select(x => new SelectListItem
            {
                Text = x.Obj,
                Value = x.Val.ToString()
            });
            TimeList = Times.Select(x => new SelectListItem
            {
                Text = x.Obj,
                Value = x.Val.ToString()
            });
            DurationList = Durations.Select(x => new SelectListItem
            {
                Text = x.Obj,
                Value = x.Val.ToString()
            });
            //terms
            TermList = terms.Select(x => new SelectListItem
            {
                Text = x.Title,
                Value = x.TermID.ToString()
            });
        }

        public CreateEditCourseViewModel(Course creating, List<Term> terms)
        {
            Creating = creating;
            DaysList = Days.Select(x => new SelectListItem
            {
                Text = x.Obj,
                Value = x.Val.ToString()
            });
            TimeList = Times.Select(x => new SelectListItem
            {
                Text = x.Obj,
                Value = x.Val.ToString()
            });
            DurationList = Durations.Select(x => new SelectListItem
            {
                Text = x.Obj,
                Value = x.Val.ToString()
            });

            TermList = terms.Select(x => new SelectListItem
            {
                Text = x.Title,
                Value = x.TermID.ToString()
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