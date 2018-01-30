using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheSwimTimeSite.ObjectModels
{
    public class CertificationHist
    {
        public int CertificationID { get; set; }
        public String Title { get; set; }
        public DateTime Added { get; set; }
        public DateTime Removed { get; set; }

        //constructors
        public CertificationHist(int id, String title, DateTime added, DateTime removed)
        {
            CertificationID = id;
            Title = title;
            Added = added;
            Removed = removed;
        }
    }
}