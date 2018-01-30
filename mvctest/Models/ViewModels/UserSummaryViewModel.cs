using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TheSwimTimeSite.ObjectModels;

namespace TheSwimTimeSite.Models.ViewModels
{
    public class UserSummaryViewModel
    {
        public User Viewing { get; set; }                           //current user with current user information
        public JPager UserVisitsJpager { get; set; }                //visit list for paged list of lists
        public List<CertificationHist> CertHistory { get; set;}     //history of certifications
        public List<Course> CourseHistory { get; set; }             //all courses this user has been in
        public List<User> UserHistory { get; set; }                 //user edit history
        public String JsonTrendsByType { get; set; }                //user activity by type
        //TODO insert breakdown for pie graph of last 90 days (null check on no data)
        //insert breakdown numbers (total number of visits, in last 90 days)
        //constructors
        public UserSummaryViewModel(User input)
        {
            Viewing = input;
        }

        public UserSummaryViewModel(User input, List<User> userHist, List<CertificationHist> certLog,
            List<Course> courseHist, JPager visitHist, Dictionary<String, int> userActivity)
        {
            Viewing = input;
            CertHistory = certLog;
            UserHistory = userHist;
            CourseHistory = courseHist;
            UserVisitsJpager = visitHist;

            List<String> temp = new List<string>();
            foreach (var x in userActivity.ToArray())
            {
                temp.Add(JsonConvert.SerializeObject(x));
            }
            JsonTrendsByType = JsonConvert.SerializeObject(temp.ToArray());
        }
    }
}
