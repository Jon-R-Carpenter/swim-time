using System.Collections.Generic;
using TheSwimTimeSite.ObjectModels;

namespace TheSwimTimeSite.Models
{
    public interface ISwimTimeService
    {
        List<SwimTime> ListAllSwimTimeRules();
        bool CreateSwimTimeRule(SwimTime swimTimeRulesToCreate);
        bool EditSwimTimeRule(SwimTime swimTimeRulesToEdit);
        List<Course> ListAllCourses();
    }
}