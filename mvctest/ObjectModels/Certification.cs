using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheSwimTimeSite.ObjectModels
{
    public class Certification
    {
        public int CertificationID { get; set; }
        public String Title { get; set; }
        public String Description { get; set; }
        public bool Active { get; set; }

        //constructors
        public Certification()
        {

        }

        public Certification(int certID, String title, bool active)
        {
            CertificationID = certID;
            Title = title;
            Active = active;
        }
        public Certification(int certID, String title, String desc, bool active)
        {
            CertificationID = certID;
            Title = title;
            Description = desc;
            Active = active;
        }
        public Certification(int certID)
        {
            CertificationID = certID;
        }

    }
}