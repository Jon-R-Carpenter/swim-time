using System;
using System.Collections.Generic;
using System.Data;
using TheSwimTimeSite.ObjectModels;

namespace TheSwimTimeSite.Models
{
    public interface IReportsRepository
    {
        Report GenerateReport(Report creating);
        Report GenerateIndividualReport(Report creating);
        List<Visit> GenerateCourseReportVisits(int courseID);
        List<User> ListAllUsersComplete();
        User GetCompleteUserByID(int id);
        List<User> ListUsers();
        Dictionary<string, DataRowCollection> GetTrends(DateTime start, DateTime end);
        Dictionary<int, int> GetTrendByDayOfWeek(DateTime start, DateTime end);
        Dictionary<int, int> GetTrendByHourOfDay(DateTime start, DateTime end);
        Dictionary<String, int> GetTrendByDay(DateTime start, DateTime end);
        List<Visit> GetUserVisitsHistoryByID(int id);
        List<CertificationHist> GetUserCertHistoryByID(int id);
        List<Course> GetUserCourseHistoryByID(int id);
        List<User> GetUserHistoryByID(int id);
        List<Visit> ListVisitsForReport(DateTime start, DateTime end);
        bool UpdateVisitValidation(int visitID, bool valid);
        Dictionary<String, int> GetUserTrends(DateTime start, DateTime end, int userID);
        List<Course> ListAllCourses();
        List<SwimTime> GetRulesByCourse(int courseID);
        UTReport GenerateUTReport(UTReport creating);
    }
}
