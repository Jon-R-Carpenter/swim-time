using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheSwimTimeSite.Models;
using System.Data;
using TheSwimTimeSite.ObjectModels;


namespace TheSwimTimeSite.Models
{
    public class ReportsRepository : IReportsRepository
    {
        private ThePoolDBService Service = new ThePoolDBService();

        public Report GenerateReport(Report creating)
        {
            return Service.GenerateReport(creating);
        }
        public Report GenerateIndividualReport(Report creating)
        {
            return Service.GenerateIndividualReport(creating);
        }
        public List<Visit> GenerateCourseReportVisits(int courseID)
        {
            return Service.GenerateCourseReportVisits(courseID);
        }

        public List<User> ListAllUsersComplete()
        {
            return Service.ListAllUsersComplete();
        }

        public List<User> ListUsers()
        {
            return Service.ListUsers();
        }

        #region trends

        public Dictionary<string, DataRowCollection> GetTrends(DateTime start, DateTime end)
        {
            return Service.GetTrends(start, end);
        }
        public Dictionary<int, int> GetTrendByDayOfWeek(DateTime start, DateTime end)
        {
            return Service.GetTrendByDayOfWeek(start, end);
        }
        public Dictionary<int, int> GetTrendByHourOfDay(DateTime start, DateTime end)
        {
            return Service.GetTrendByHourOfDay(start, end);
        }
        public Dictionary<String, int> GetTrendByDay(DateTime start, DateTime end)
        {
            return Service.GetTrendByDay(start, end);
        }
        #endregion

        #region UserSummary

        public List<Visit> GetUserVisitsHistoryByID(int id)
        {
            return Service.GetUserVisitsHistoryByID(id);
        }
        public List<CertificationHist> GetUserCertHistoryByID(int id)
        {
            return Service.GetUserCertHistoryByID(id);
        }
        public List<Course> GetUserCourseHistoryByID(int id)
        {
            return Service.GetUserCourseHistoryByID(id);
        }
        public List<User> GetUserHistoryByID(int id)
        {
            return Service.GetUserHistoryByID(id);
        }
        public Dictionary<String, int> GetUserTrends(DateTime start, DateTime end, int userID)
        {
            return Service.GetUserTrends(start, end, userID);
        }

        #endregion

        public User GetCompleteUserByID(int id)
        {
            return Service.GetCompleteUserByID(id);
        }
        public List<Visit> ListVisitsForReport(DateTime start, DateTime end)
        {
            return Service.ListVisitsForReport(start, end);
        }
        public bool UpdateVisitValidation(int visitID, bool valid)
        {
            return Service.UpdateVisitValidation(visitID, valid);
        }
        public List<Course> ListAllCourses()
        {
            return Service.ListAllCourses();
        }
        public List<SwimTime> GetRulesByCourse(int courseID)
        {
            return Service.GetRulesByCourse(courseID);
        }

        public UTReport GenerateUTReport(UTReport creating)
        {
            return Service.GenerateUTReport(creating);
        }
    }
}
