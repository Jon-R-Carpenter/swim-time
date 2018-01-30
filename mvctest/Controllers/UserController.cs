using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheSwimTimeSite.Models;
using TheSwimTimeSite.ObjectModels;
using TheSwimTimeSite.Models.ViewModels;
using TheSwimTimeSite.Helpers;
using TheSwimTimeSite.SwimTimeRuleProcessor.Factories;
using TheSwimTimeSite.SwimTimeRuleProcessor.Interfaces;
using TheSwimTimeSite.SwimTimeRuleProcessor;
using mvctest.Security;

namespace mvctest.Controllers
{
    //[CustomAuthorize(Roles = "Admin,Staff")]
    public class UserController : Controller
    {
        private IUserService Service;
        private DateTime dayOfWeekMonday;
        private DateTime dayOfWeekSunday;
        private int dateTimesCalculated = 0;
        private int totalHours = 0;
        public UserController()
        {
            Service = new UserService(new ModelStateWrapper(this.ModelState), new UserRepository());
        }

        public UserController(IUserService service)
        {
            Service = service;
        }


        // GET: User
       
        public ActionResult Certify()
        {
            return View();
        }
        //HTTP:GET
        [HttpGet]
        public ActionResult CheckIn(int id)
        {
            List<int> usersIn = (List<int>)Service.ListUsersIn().Select(x => x.UserID).ToList();
            if (usersIn.Contains(id))
                return RedirectToAction("Checkout", new { userID = id });
            //build checkin viewmodel
            User temp = Service.GetCompleteUserByID(id);
            ViewBag.UserName = temp.FirstName + " " + temp.LastName;
            CheckInViewModel ret = new CheckInViewModel(id, Service.ListAvailableVisitTypes(id), temp.Classes,
               temp.Comments);
            return View(ret);
        }
        //HTTP:POST
      [HttpPost]
        public ActionResult CheckIn(FormCollection input)//takes class of button that was pressed, and type of button pressed
        {
           
            int userID = int.Parse(input.GetValues("UserID").FirstOrDefault().ToString());
            String selected = input.GetValues("Selected").FirstOrDefault().ToString();//get selected button text
            User temp = Service.GetCompleteUserByID(userID);//get user information
            List<Course> classes = temp.Classes;
            List<VisitType> types = Service.ListAvailableVisitTypes(userID);
           
            int visitID = -1;//visit ID to attach equipment use to
           
            var ip = Request.UserHostAddress;
            //this.ShowMessage(MessageType.Success, "User " + User.Identity.Name + " logged in from IP." + ip, true);
            //process selection
            foreach (Course row in classes)
            {
                if (row.Title == selected)//process course visit here with redirect to action
                {
                    visitID = Service.SubmitCourseVisit(userID, row.CRN, row.CheckOut, User.Identity.Name, ip);//submit user in if applicable
                
                    this.ShowMessage(MessageType.Success, temp.FirstName + " " + temp.LastName + " Checked In For: " + row.Title, true);
                    return RedirectToAction("Welcomee", "Home");
                }
            }
            foreach (VisitType row in types)
            {
                if (row.Title == selected)//process visit type here with redirect to action
                {
                    visitID = Service.SubmitGeneralVisit(userID, row.VisitTypeID, User.Identity.Name, ip);//add selected value
                  

                    this.ShowMessage(MessageType.Success, temp.FirstName + " " + temp.LastName + " Checked In For: " + row.Title, true);
                    return RedirectToAction("Welcomee", "Home");
                }
            }
            this.ShowMessage(MessageType.Warning, "Error checking in : " + temp.FirstName + " " + temp.LastName, true);
            return RedirectToAction("Welcomee", "Home");
        }

        public ActionResult CheckOut(int userID)
        {
            int individualHours = 0,mTracker=0,tTracker=0,wTracker=0,thTracker=0,fTracker=0,satTracker=0,sunTracker=0;
            User checkingOut = Service.GetUserForCheckout(userID);//associated visit contained in user
            //rules
            List<SwimTime> baseRules = Service.GetRulesByCourse(checkingOut.CheckOutFor);

           // IList<ISwimTimeRule> converted = SwimTimeToISwimTimeAdaptor.ConvertListOfSwimTimeToListOfISwimTimeRule();
           // ISwimTimeRuleProcessor temp = SwimTimeRuleProcessorFactory.CreateSwimTimeRuleProcessor(converted);
            //visits
            List<Object> cleanVisits = new List<Object>();
            List<Visit> visits = Service.ListUserVisitsForCheckout(userID, checkingOut.CheckOutFor);
            //visits.Add(new Visit(-1, DateTime.Now, 0));//add current checkout as visit
            for (int i = 0; i < visits.Count; i++)
            {
                int delta = DayOfWeek.Monday - visits[i].VisitTime.DayOfWeek;
                if(delta > 0)
                {
                    delta -= 7;
                }
                DateTime monday = visits[i].VisitTime.AddDays(delta);
                DateTime sunday = monday.AddDays(6);

                while (visits[i].VisitTime.Day >= monday.Day && visits[i].VisitTime.Day <= sunday.Day)
                {
                    visits[i].checkInAt = convertTime(visits[i].checkInAt);
                    visits[i].checkOut = convertTime(visits[i].checkOut);

                    TimeSpan time = visits[i].checkOut.Subtract(visits[i].checkInAt);
                    int hours = time.Hours;
                    int minutes = hours * 60;

                    if (visits[i].VisitTime.DayOfWeek.ToString().Equals("Monday") &&mTracker<=2)
                    {
                        if (minutes > baseRules[0].MaxHoursPerVisit)
                        {
                            individualHours += 1;
                            mTracker++;
                        }
                        else if (minutes >= baseRules[0].MinHoursPerVisit && minutes <= baseRules[0].MaxHoursPerVisit)
                        {
                            minutes = minutes / 60;
                            individualHours += minutes;
                            mTracker++;
                        }

                    }
                    else if (visits[i].VisitTime.Day.Equals("Tuesday")&&tTracker<=2)
                    {
                        if (minutes > baseRules[0].MaxHoursPerVisit)
                        {
                            individualHours += 1;
                            tTracker++;
                        }
                        else if (minutes >= baseRules[0].MinHoursPerVisit && minutes <= baseRules[0].MaxHoursPerVisit)
                        {
                            minutes = minutes / 60;
                            individualHours += minutes;
                            tTracker++;
                        }
                    }
                    else if (visits[i].VisitTime.Day.Equals("Wednesday")&&wTracker<=2)
                    {
                        if (minutes > baseRules[0].MaxHoursPerVisit)
                        {
                            individualHours += 1;
                            wTracker++;
                        }
                        else if (minutes >= baseRules[0].MinHoursPerVisit && minutes <= baseRules[0].MaxHoursPerVisit)
                        {
                            minutes = minutes / 60;
                            individualHours += minutes;
                            wTracker++;
                        }
                    }
                    else if (visits[i].VisitTime.Day.Equals("Thursday")&&thTracker<=2)
                    {
                        if (minutes > baseRules[0].MaxHoursPerVisit)
                        {
                            individualHours += 1;
                            thTracker++;
                        }
                        else if (minutes >= baseRules[0].MinHoursPerVisit && minutes <= baseRules[0].MaxHoursPerVisit)
                        {
                            minutes = minutes / 60;
                            individualHours += minutes;
                            thTracker++;
                        }
                    }
                    else if (visits[i].VisitTime.Day.Equals("Friday")&&fTracker<=2)
                    {
                        if (minutes > baseRules[0].MaxHoursPerVisit)
                        {
                            individualHours += 1;
                            fTracker++;
                        }
                        else if (minutes >= baseRules[0].MinHoursPerVisit && minutes <= baseRules[0].MaxHoursPerVisit)
                        {
                            minutes = minutes / 60;
                            individualHours += minutes;
                            fTracker++;
                        }
                    }
                    else if (visits[i].VisitTime.Day.Equals("Saturday")&&satTracker<=2)
                    {
                        if (minutes > baseRules[0].MaxHoursPerVisit)
                        {
                            individualHours += 1;
                            satTracker++;
                        }
                        else if (minutes >= baseRules[0].MinHoursPerVisit && minutes <= baseRules[0].MaxHoursPerVisit)
                        {
                            minutes = minutes / 60;
                            individualHours += minutes;
                            satTracker++;
                        }

                    }
                    else if (visits[i].VisitTime.Day.Equals("Sunday")&&sunTracker<=2)
                    {
                        if (minutes > baseRules[0].MaxHoursPerVisit)
                        {
                            individualHours += 1;
                            sunTracker++;
                        }
                        else if (minutes >= baseRules[0].MinHoursPerVisit && minutes <= baseRules[0].MaxHoursPerVisit)
                        {
                            minutes = minutes / 60;
                            individualHours += minutes;
                            sunTracker++;
                        }
                    }
                    if(i+1 != visits.Count && visits[i+1].VisitTime.Day >= sunday.Day || i+1 ==visits.Count)
                    {
                        break;
                    }
                    else
                    {
                        i++;
                    }
                }
                if(individualHours > 5)
                {
                    totalHours += 5;
                }
                else if(individualHours <= 5)
                {
                    totalHours += individualHours;
                }
                individualHours = 0;

                



            }//end of for statement

            
           
            
            ViewData["TotalHours"] = totalHours;
            return View(checkingOut);//return checkout page
        }
        private void dayOfWeek(DateTime input,int hours)
        {
            
            int delta = DayOfWeek.Monday - input.DayOfWeek;
            if (delta > 0)
                delta -= 7;
            DateTime monday = input.AddDays(delta);
            DateTime sunday = monday.AddDays(6);


            if (dateTimesCalculated != 1)
            {
                dayOfWeekMonday = monday;
                dayOfWeekSunday = sunday;
                dateTimesCalculated = 1;
            }

            if (input.Day >= dayOfWeekMonday.Day && input.Day <= dayOfWeekSunday.Day)
            {
                //don't reset varibles
            }
            else
            {
                
               //reset mTracker,wTracker,individualHours
                dayOfWeekMonday = monday;
                dayOfWeekSunday = sunday;
            
                //re

            }

        }
        private TimeSpan convertTime(TimeSpan checkIn)
        {
            TimeSpan time = checkIn;
            if(time.Hours > 12)
            {
                int test = time.Hours - 12;
                time = new TimeSpan(test, checkIn.Minutes, checkIn.Seconds);
            }
            return time;
        }
        
       
        public ActionResult ValidCheckout(int userID, int associatedVisitID)
        {

            var ip = Request.UserHostAddress;
            Service.SubmitCheckout(userID, true, User.Identity.Name, ip, associatedVisitID);
            User temp = Service.GetUserByID(userID);
            this.ShowMessage(MessageType.Success, "Valid Checkout for: " + temp.FirstName + " " + temp.LastName, true);
            return RedirectToAction("Welcomee", "Home");//redirect to home
        }
        public ActionResult InvalidCheckout(int userID, int associatedVisitID)
        {
            var ip = Request.UserHostAddress;
            Service.SubmitCheckout(userID, false, User.Identity.Name, ip, associatedVisitID);
            User temp = Service.GetUserByID(userID);
            this.ShowMessage(MessageType.Warning, "Invalid Checkout for: " + temp.FirstName + " " + temp.LastName, true);
            return RedirectToAction("Welcomee", "Home");//redirect to home
        }
        [HttpGet]
        public ActionResult MultipleUsers(String lastname)//accepts list of users
        {
            List<User> ids = Service.SearchUsersByLastname(lastname);//get list
            List<User> users = new List<User>();//working space
            foreach (User use in ids)//get complete user for each user
            {
                users.Add(Service.GetCompleteUserByID(use.UserID));//get complete user id
            }
            return View(users);
        }
        [HttpGet]
        public ActionResult SimilarUsers(String lastname)//accepts list of users
        {
            List<User> ids = Service.SearchBySimilarLastNames(lastname);//get list
            List<User> users = new List<User>();//working space
            foreach (User use in ids)//get complete user for each user
            {
                users.Add(Service.GetCompleteUserByID(use.UserID));//get complete user id
            }
            return View("MultipleUsers", users);//use multiple users view
        }
        [HttpGet]
        public ActionResult VerifyPage(int userID, String message)//clickthrough verify page
        {
            ViewData["Message"] = message;//insert message
            return View(userID);
        }

        [HttpPost]
        public ActionResult VerifyPage(int userID)//clickthrough verify page
        {
            return RedirectToAction("CheckIn", new { id = userID });//pass this userID to the checkin method
        }
        public JsonResult UserExistsByName(String lastname, String firstname)
        {
            return this.Json(new { exists = (Service.UserExistsByName(lastname, firstname)) }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult UserIndex()
        {
            return View(new CreateEditUserViewModel(new User(), Service.ListStaff(), Service.GetUserTypes()));
        }

        [HttpPost]
        public ActionResult UserIndex( CreateEditUserViewModel creating)
        {
            int ret = Service.CreateUser(creating);
            if (ret <= 0)//if an invalid user id is returned
            {
                return View(new CreateEditUserViewModel(creating.Editing, Service.ListStaff(), Service.GetUserTypes()));
            }
            this.ShowMessage(MessageType.Success, creating.Editing.FirstName + " " + creating.Editing.LastName + " Created", true);
            return RedirectToAction("UserProfile", new { id = ret });
        }
        [HttpGet]
        public ActionResult Edit(int id)//edit user by userID
        {
            return View(new CreateEditUserViewModel(Service.GetUserByID(id), Service.ListStaff(), Service.GetUserTypes()));
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(CreateEditUserViewModel editing)
        {
            this.TryUpdateModel(editing);
            if (!Service.EditUser(editing))
            {
                return View(new CreateEditUserViewModel(editing.Editing, Service.ListStaff(), Service.GetUserTypes()));
            }

            return RedirectToAction("UserProfile", new { id = editing.Editing.UserID });
        }

        public ActionResult UserProfile(int id)
        {
            int n = id;
            return View(Service.GetUserByID(n));
        }

        [HttpGet]
        public ActionResult UserSearchByLastname(String lastname)
        {
            List<User> working = Service.SearchUsersByLastname(lastname);
            if(working.Count > 1)
            {
                return RedirectToAction("MultipleUsers", new { lastname = lastname });
            }
            else if (working.Count == 1)
            {
                return RedirectToAction("Verify", new {id=working.FirstOrDefault().UserID});
            }
            else if(working.Count == 0)
            {
                return RedirectToAction("SimilarUsers", new { lastname = lastname });
            }

            return RedirectToAction("Welcomee", "Home");//redirect to main page
        }
        public ActionResult Verify(int id)
        {
            User working = Service.GetCompleteUserByID(id);
            if (working.UserType != null && working.UserType.ToUpper() == "COMMUNITY MEMBER")
            {
                return RedirectToAction("VerifyPage", new
                {
                    userID = working.UserID,
                    message =
                        "Community Member has a day pass or membership?"
                });//pass to a verify page
            }
            else
            {
                return RedirectToAction("CheckIn", new { id = working.UserID });//pass this userID to the checkin method
            }
        }    

        [HttpGet]
        public ActionResult UserSearchByCardscan(String cardscan)
        {
            User check = Service.SearchUsersByCardscan(cardscan);
            if(check != null)
            {
                return RedirectToAction("CheckIn", new { id = check.UserID });
            }
            this.ShowMessage(MessageType.Warning, "No user with this card exists", true);
            return RedirectToAction("Welcomee", "Home");
        }
        

    }
}