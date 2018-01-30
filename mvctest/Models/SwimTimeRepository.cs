using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TheSwimTimeSite.ObjectModels;

namespace TheSwimTimeSite.Models
{
    public class SwimTimeRepository : ISwimTimeRepository
    {
        private ThePoolDBService Service = new ThePoolDBService();

        public List<SwimTime> ListAllSwimTimeRules()
        {
            return Service.ListAllSwimTimeRules();
        }

        public bool CreateSwimTimeRule(SwimTime swimTimeRulesToCreate)
        {
            return Service.CreateSwimTimeRule(swimTimeRulesToCreate);
        }

        public bool EditSwimTimeRule(SwimTime swimTimeRulesToEdit)
        {
            return Service.EditSwimTimeRule(swimTimeRulesToEdit);
        }

        public List<Course> ListAllCourses()
        {
            return Service.ListAllCourses();
        }
    }
}