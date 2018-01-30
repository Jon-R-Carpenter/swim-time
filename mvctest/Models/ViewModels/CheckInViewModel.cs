using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheSwimTimeSite.ObjectModels;

//model for checking view
namespace TheSwimTimeSite.Models.ViewModels
{
    public class CheckInViewModel
    {
        public int UserID { get; set; }
        public String Selected { get; set; }//the text from the button that was selected!
        //list of avaliable visit types for this user
        public List<VisitType> Types { get; set; }
        public List<Course> Classes { get; set; }
        //list of active equipment for use
        public List<Equipment> EquipmentForUse { get; set; }
        //list of comments for this user!
        public String UserComments {get; set;}

        //constructors
        public CheckInViewModel()
        {
        }

        public CheckInViewModel (int checkID, List<VisitType> typesAvailable, List<Course> enrolledIn, String comments)
        {
            Types = new List<VisitType>();
            Classes = new List<Course>();
            UserID = checkID;
            Types.AddRange(typesAvailable);
            Classes.AddRange(enrolledIn);
      
            UserComments = comments;
        }
    }
}