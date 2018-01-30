using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheSwimTimeSite.ObjectModels
{
    public class Visits
    {
        public List<Visit> visits { get; set; }
        public DateTime start { get; set; }
        public DateTime end { get; set; }
    }
}