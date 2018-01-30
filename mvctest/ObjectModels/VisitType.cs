using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheSwimTimeSite.ObjectModels
{
    public class VisitType
    {
        public int VisitTypeID { get; set; }
        public Certification RequiredCert { get; set; }
        public String Title { get; set; }
        public String Description { get; set; }
        public bool Active { get; set; }

        //constructors
        public VisitType()
        {
            //RequiredCert = new Certification();//default to having a blank certification
            //RequiredCert.CertificationID = -1;
        }

        public VisitType(int visitTypeID,  String title, bool active)
        {
            VisitTypeID = visitTypeID;
          
            Title = title;
            Active = active;
        }
        public VisitType(int visitTypeID,String title, String desc, bool active)
        {
            VisitTypeID = visitTypeID;
        
            Description = desc;
            Title = title;
            Active = active;
        }
    }
}