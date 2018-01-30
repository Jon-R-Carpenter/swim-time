using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Newtonsoft.Json;

namespace TheSwimTimeSite.Models.ViewModels
{
    public class TrendViewModel
    {
        public Dictionary<string, DataRowCollection> Trends { get; set; }
        public String JsonVisitsByDay { get; set; }
        public String JsonVisitsByDayOfWeek { get; set; }
        public String JsonVisitsByHourInDay { get; set; }
        public String JsonTrendsByType { get; set; }
        public String JsonTrendsUserType { get; set; }
        public DateTime Start { get; set; }
        public DateTime Finish { get; set; }

        //paramaterless constructor for model binding
        public TrendViewModel()
        {

        }

        public TrendViewModel(DateTime start, DateTime finish, Dictionary<string, DataRowCollection> trends, Dictionary<String, int> visitsByDay,
            Dictionary<int, int> visitsByDayOfWeek, Dictionary<int, int> visitsByHour)
        {
            this.Start = start;//set times
            this.Finish = finish;
            Trends = trends;            
            List<String> temp = new List<string>();
            foreach(var x in visitsByDay.ToArray())
            {
                temp.Add(JsonConvert.SerializeObject(x));
            }
            JsonVisitsByDay = JsonConvert.SerializeObject(temp.ToArray());
            temp = new List<string>();
            foreach (var x in visitsByDayOfWeek.ToArray())
            {
                temp.Add(JsonConvert.SerializeObject(x));
            }
            JsonVisitsByDayOfWeek = JsonConvert.SerializeObject(temp.ToArray());
            temp = new List<string>();
            foreach (var x in visitsByHour.ToArray())
            {
                temp.Add(JsonConvert.SerializeObject(x));
            }
            JsonVisitsByHourInDay = JsonConvert.SerializeObject(temp.ToArray());

            temp = new List<string>();
            foreach (DataRow row in trends["Type"])
            {
                temp.Add(JsonConvert.SerializeObject(row.ItemArray));
            }
            JsonTrendsByType = JsonConvert.SerializeObject(temp.ToArray());

            temp = new List<string>();
            foreach (DataRow row in trends["UserType"])
            {
                temp.Add(JsonConvert.SerializeObject(row.ItemArray));
            }
            JsonTrendsUserType = JsonConvert.SerializeObject(temp.ToArray());

        }

        public TrendViewModel(Dictionary<string, DataRowCollection> input)
        {
            Trends = input;
        }
    }
}