using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheSwimTimeSite.ObjectModels;

namespace TheSwimTimeSite.Models.ViewModels
{
    public class CreateEditTermViewModel
    {
        public Term Creating { get; set; }
        public List<KeyPair> Quarters = new List<KeyPair>{ new KeyPair("Fall", "Fall"), new KeyPair("Winter", "Winter"), new KeyPair("Spring", "Spring"), new KeyPair("Summer", "Summer") };
        public IEnumerable<SelectListItem> QuartersList { get; set; }


        //constructors
        public CreateEditTermViewModel()
        {
            QuartersList = Quarters.Select(x => new SelectListItem
            {
                Text = x.Obj,
                Value = x.Val.ToString()
            });
        }

        public CreateEditTermViewModel(Term creating)
        {
            Creating = creating;
            QuartersList = Quarters.Select(x => new SelectListItem
            {
                Text = x.Obj,
                Value = x.Val.ToString()
            });
        }

        public class KeyPair
        {
            public String Obj { get; set; }
            public String Val { get; set; }

            public KeyPair(String obj, String val)
            {
                Obj = obj;
                Val = val;
            }
        }
    }
}