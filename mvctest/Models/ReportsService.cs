using System;
using System.Collections.Generic;
using System.Data;
using TheSwimTimeSite.ObjectModels;
using TheSwimTimeSite.SwimTimeRuleProcessor;
using TheSwimTimeSite.SwimTimeRuleProcessor.Interfaces;
using TheSwimTimeSite.SwimTimeRuleProcessor.Factories;
using TheSwimTimeSite.Models.ViewModels;

namespace TheSwimTimeSite.Models
{
    public class ReportsService : IReportsService
    {
        private IReportsRepository Repository;
        private IValidationDictonary Validator;
        
        public ReportsService(IValidationDictonary dictonary, IReportsRepository repository)
        {
            Validator = dictonary;
            Repository = repository;
        }


        public Report GenerateReport(Report creating)
        {
            if(ValidateReport(creating))
                return Repository.GenerateReport(creating);
            
            return creating;
        }
        public Report GenerateIndividualReport(Report creating)
        {
            if (ValidateIndividualReport(creating))
                return Repository.GenerateIndividualReport(creating);

            return creating;
        }
        protected bool ValidateIndividualReport(Report input)
        {
            if (input.Start.Year == 0001)
                Validator.AddError("Start", "Start date is required");
            if (input.Finish.Year == 0001)
                Validator.AddError("Finish", "End date is required");
            if (input.Start > input.Finish)
                Validator.AddError("Finish", "Finish date must be after Start date");
            if (String.IsNullOrEmpty(input.First))
                Validator.AddError("First", "First Name is required");
            if (String.IsNullOrEmpty(input.Last))
                Validator.AddError("Last", "Last Name is required");
            if (String.IsNullOrEmpty(input.FileName))
                Validator.AddError("FileName", "File Name is required");

            return Validator.IsValid;
        }



    public List<User> ListAllUsersComplete()
        {
            return Repository.ListAllUsersComplete();
        }

        public List<User> ListUsers()
        {
            return Repository.ListUsers();
        }

        public User GetCompleteUserByID(int id)
        {
            return Repository.GetCompleteUserByID(id);
        }

        //Trends
        public Dictionary<string, DataRowCollection> GetTrends(DateTime start, DateTime end)
        {
            return Repository.GetTrends(start, end);
        }
        public Dictionary<int, int> GetTrendByDayOfWeek(DateTime start, DateTime end)
        {
            return Repository.GetTrendByDayOfWeek(start, end);
        }
        public Dictionary<String, int> GetTrendByDay(DateTime start, DateTime end)
        {
            return Repository.GetTrendByDay(start, end);
        }
        public Dictionary<int, int> GetTrendByHourOfDay(DateTime start, DateTime end)
        {
            return Repository.GetTrendByHourOfDay(start, end);
        }
        public List<Visit> ListVisitsForReport(DateTime start, DateTime end)
        {
            return Repository.ListVisitsForReport(start, end);
        }

        public bool UpdateVisitValidation(int? visitID, bool valid)
        {
            int id = (int)((visitID == null) ? -1 : visitID);
            return Repository.UpdateVisitValidation(id, valid);
        }

        public void ValidateViewModel(SwimTimeReportViewModel input)
        {
            if(input.CourseID == 0)
                Validator.AddError("input.CourseID", "A Course must be selected");
            if(String.IsNullOrWhiteSpace(input.FileName))
                Validator.AddError("FileName", "A File Name must be entered");
        }

        public List<Course> ListAllCourses()
        {
            return Repository.ListAllCourses();
        }

        //** Assumption! All visits are paired in/out/invalid except the students currently in...
        public List<ICTReportRow> GenerateCourseReportVisits(int courseID)
        {
            List<ICTReportRow> ret = new List<ICTReportRow>();                  //working space
            IList<Visit> list = Repository.GenerateCourseReportVisits(courseID);//get list of valid visits grouped by userID

            //rules
            List<SwimTime> baseRules = Repository.GetRulesByCourse(courseID);
            IList<ISwimTimeRule> converted = SwimTimeToISwimTimeAdaptor.ConvertListOfSwimTimeToListOfISwimTimeRule();
            ISwimTimeRuleProcessor temp = SwimTimeRuleProcessorFactory.CreateSwimTimeRuleProcessor(converted);

            IEnumerator<Visit> i = list.GetEnumerator();
            i.Reset();
            i.MoveNext();//initialize iterator
            while(i.Current != null && i.Current.Visitor != null)                                 //iterate through all visits
            {
                int currentUserID = i.Current.Visitor.UserID;
                List<SwimTimeVisit> cleanVisits = new List<SwimTimeVisit>();  //this users running batch of visits
                double runningTotal = 0.0;
                while (i.Current != null && currentUserID == i.Current.Visitor.UserID) //while we are on this userID
                {
                    Visit prev = i.Current;
                    i.MoveNext();

                    if (i.Current != null && i.Current.InOutFlag != 1 && prev.InOutFlag == 1)        //we have two valid line items
                    {
                        Visit cur = i.Current;                                  //get second visit
                        ret.Add(new ICTReportRow(prev.Visitor.UserID, prev.Visitor.FirstName, prev.Visitor.LastName,
                            prev.InOutFlag, -1.0, prev.VisitTime));                              //no display for checkin time...?
                        cleanVisits.Add(new SwimTimeVisit(prev.VisitTime, cur.VisitTime, cur.InOutFlag == 0));
                        SwimTimeRuleProcessingInformation answer = temp.ProcessMultipleVisits(cleanVisits);
                        runningTotal = answer.GetValidTime();                   //get valid time total for this batch of visits
                        ret.Add(new ICTReportRow(cur.Visitor.UserID, cur.Visitor.FirstName, cur.Visitor.LastName,
                            cur.InOutFlag, runningTotal, cur.VisitTime));
                        i.MoveNext();                                               //advance iterator
                    }
                    
                }
            }
            return ret;
        }

        public UTReport GenerateUTReport(UTReport creating)
        {
            if (ValidateUTReport(creating))
                return Repository.GenerateUTReport(creating);

            return creating;
        }

        #region UserSummary

        public Dictionary<String, int> GetUserTrends(DateTime start, DateTime end, int userID)
        {
            return Repository.GetUserTrends(start, end, userID);
        }
        public List<Visit> GetUserVisitsHistoryByID(int id)
        {
            return Repository.GetUserVisitsHistoryByID(id);
        }
        public List<CertificationHist> GetUserCertHistoryByID(int id)
        {
            return Repository.GetUserCertHistoryByID(id);
        }
        public List<Course> GetUserCourseHistoryByID(int id)
        {
            return Repository.GetUserCourseHistoryByID(id);
        }
        public List<User> GetUserHistoryByID(int id)
        {
            return Repository.GetUserHistoryByID(id);
        }

        #endregion

        //validation
        protected bool ValidateReport(Report input)
        {
            if (input.Start.Year == 0001)
                Validator.AddError("Start", "Start date is required");
            if (input.Finish.Year == 0001)
                Validator.AddError("Finish", "End date is required");
            if (input.Start > input.Finish)
                Validator.AddError("Finish", "Finish date must be after Start date");
            if (String.IsNullOrEmpty(input.FileName))
                Validator.AddError("FileName", "File Name is required");

            return Validator.IsValid;
        }

        protected bool ValidateUTReport(UTReport input)
        {
            if (input.Start.Year == 0001)
                Validator.AddError("Start", "Start date is required");
            if (input.Finish.Year == 0001)
                Validator.AddError("Finish", "End date is required");
            if (input.Start > input.Finish)
                Validator.AddError("Finish", "Finish date must be after Start date");
            if (String.IsNullOrEmpty(input.FileName))
                Validator.AddError("FileName", "File Name is required");

            return Validator.IsValid;
        }













        //DELETE THESE TWO METHODS
        private static DateTime parseStringIntoDateTime(String dateTime)
        {
            int locOfYearMonthSeparator = -1;
            int locOfMonthDaySeparator = -1;
            int locOfDayHourSeparator = -1;
            int locOfHourMinSeparator = -1;
            int locOfMinSecSeparator = -1;
            int locOfSecMilliSeparator = -1;

            locOfYearMonthSeparator = dateTime.IndexOf('/', 0);
            locOfMonthDaySeparator = dateTime.IndexOf('/', locOfYearMonthSeparator + 1);
            locOfDayHourSeparator = dateTime.IndexOf(' ', locOfMonthDaySeparator + 1);
            locOfHourMinSeparator = dateTime.IndexOf(':', locOfDayHourSeparator + 1);
            locOfMinSecSeparator = dateTime.IndexOf(':', locOfHourMinSeparator + 1);
            locOfSecMilliSeparator = dateTime.IndexOf('.', locOfMinSecSeparator + 1);


            string yearStr = dateTime.Substring(0, locOfYearMonthSeparator);
            string monthStr = dateTime.Substring(locOfYearMonthSeparator + 1, locOfMonthDaySeparator - (locOfYearMonthSeparator + 1));
            string dayStr = dateTime.Substring(locOfMonthDaySeparator + 1, locOfDayHourSeparator - (locOfMonthDaySeparator + 1));
            string hourStr = dateTime.Substring(locOfDayHourSeparator + 1, locOfHourMinSeparator - (locOfDayHourSeparator + 1));
            string minStr = dateTime.Substring(locOfHourMinSeparator + 1, locOfMinSecSeparator - (locOfHourMinSeparator + 1));
            string secStr = dateTime.Substring(locOfMinSecSeparator + 1, locOfSecMilliSeparator - (locOfMinSecSeparator + 1));
            string milliStr = dateTime.Substring(locOfSecMilliSeparator + 1);

            int year;
            int month;
            int day;
            int hour;
            int minute;
            int second;
            int millisecond;

            year = int.Parse(yearStr);
            month = getMonthNumber(monthStr);
            day = int.Parse(dayStr);
            hour = int.Parse(hourStr);
            minute = int.Parse(minStr);
            second = int.Parse(secStr);
            millisecond = int.Parse(milliStr);

            
            DateTime gc = new DateTime(year,month,day,hour,minute,second,millisecond);
            
            return gc;
        }
        private static int getMonthNumber(String monthStr)
        {
            string upperMonthStr = monthStr.ToUpper();
            if (upperMonthStr.CompareTo("JAN") == 0)
            {
                return 1;
            }
            if (upperMonthStr.CompareTo("FEB") == 0)
            {
                return 2;
            }
            if (upperMonthStr.CompareTo("MAR") == 0)
            {
                return 3;
            }
            if (upperMonthStr.CompareTo("APR") == 0)
            {
                return 4;
            }
            if (upperMonthStr.CompareTo("MAY") == 0)
            {
                return 5;
            }
            if (upperMonthStr.CompareTo("JUN") == 0)
            {
                return 6;
            }
            if (upperMonthStr.CompareTo("JUL") == 0)
            {
                return 7;
            }
            if (upperMonthStr.CompareTo("AUG") == 0)
            {
                return 8;
            }
            if (upperMonthStr.CompareTo("SEP") == 0)
            {
                return 9;
            }
            if (upperMonthStr.CompareTo("OCT") == 0)
            {
                return 10;
            }
            if (upperMonthStr.CompareTo("NOV") == 0)
            {
                return 11;
            }
            if (upperMonthStr.CompareTo("DEC") == 0)
            {
                return 12;
            }
            else return -1;
        }
    }
}
