using System;
using System.Collections.Generic;
using System.Data;
using TheSwimTimeSite.ObjectModels;
using TheSwimTimeSite.Models.ViewModels;
using TheSwimTimeSite.Models;


namespace TheSwimTimeSite.Models
{
    public interface IReportsService
    {
        Report GenerateReport(Report creating);
     
        List<ICTReportRow> GenerateCourseReportVisits(int courseID);
        Report GenerateIndividualReport(Report creating);
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
        bool UpdateVisitValidation(int? visitID, bool valid);
        Dictionary<String, int> GetUserTrends(DateTime start, DateTime end, int userID);
        List<Course> ListAllCourses();
        void ValidateViewModel(SwimTimeReportViewModel input);
        UTReport GenerateUTReport(UTReport creating);
    }

}
