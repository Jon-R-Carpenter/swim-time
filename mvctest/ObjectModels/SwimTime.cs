using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TheSwimTimeSite.Helpers;

namespace TheSwimTimeSite.ObjectModels
{
    public class SwimTime
    {
        [System.ComponentModel.DataAnnotations.ScaffoldColumn(false)]
        public int SwimTimeID { get; set; }
        [Required(ErrorMessage = "Course is required")]
        public Course Course { get; set; }
        [Required(ErrorMessage = "Start Day is required")]
        public int StartOffset { get; set; }
        [Required(ErrorMessage = "Min Hours is required")]
        public int MinHoursPerVisit { get; set; }
        [Required(ErrorMessage = "Max Hours is required")]
        public int MaxHoursPerVisit { get; set; }
        [Required(ErrorMessage = "Start Date is required")]
        public DateTime Start { get; set; }
        [Required(ErrorMessage = "End Date is required")]
        public DateTime End { get; set; }
        [Required(ErrorMessage = "Frequency is required")]
        public double RepetitionPeriod { get; set; }
        public RuleType Type { get; set; }
        public String StartOffSetDay { get; set; }
        public String RepitionPeriodName { get; set; }
        private enum Repitition
        {
            Constant = 0, Daily = 24, Weekly = 168
        }
        private enum DayTypes
        {
            Sunday = 0, Monday = 24, Tuesday = 48, Wednesday = 72, Thursday = 96, Friday = 120, Saturday = 144
        }

        public SwimTime()
        {
        }

        public SwimTime(int id, String title, Course Course, int offset,
            int type, int minPerVisit, int maxPerVisit, DateTime start, DateTime end)
        {
            this.SwimTimeID = id;
            this.Course = Course;
            this.StartOffset = offset;
            this.MinHoursPerVisit = minPerVisit;
            this.MaxHoursPerVisit = maxPerVisit;
            this.Type = (RuleType)type;
            this.Start = start;
            this.End = end;
           
            DayTypes dayTypes = ((DayTypes)offset);
            this.StartOffSetDay = dayTypes.ToString();
        }
    }
}