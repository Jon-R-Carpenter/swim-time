using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheSwimTimeSite.ObjectModels;

namespace TheSwimTimeSite.Models.ViewModels
{
    public class SwimTimeReportViewModel
    {
        public IEnumerable<SelectListItem> Course { get; set; }
        public String FileName { get; set; }
        public int CourseID { get; set; }

        public SwimTimeReportViewModel()
        {
        }

        public SwimTimeReportViewModel(List<Course> courseList)
        {
            IEnumerable<Course> CTOnly = from co in courseList
                                         where co.CheckOut
                                         select co;
            Course = CTOnly.Select(x => new SelectListItem
            {
                Text = x.Title + " " + x.CRN,
                Value = x.CRN.ToString()
            });
        }
    }
}