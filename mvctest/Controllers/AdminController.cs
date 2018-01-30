using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheSwimTimeSite.Models;
using TheSwimTimeSite.ObjectModels;
using TheSwimTimeSite.Helpers;
using System.Web.Security;
using mvctest.Security;

namespace mvctest.Controllers
{
    //[CustomAuthorize(Roles="Admin")]
    public class AdminController : Controller
    {

        private ICourseService Service;

        public AdminController()
        {
            Service = new CourseService(new ModelStateWrapper(this.ModelState), new CourseRepository());
        }

        public AdminController(ICourseService service)
        {
            Service = service;
        }



        // GET: Admin
    
        public ActionResult AdminIndex()
        {
            MembershipUserCollection users = Membership.GetAllUsers();
            List<StaffUser> su = new List<StaffUser>();

            foreach (MembershipUser mu in users)
            {
                StaffUser user = new StaffUser(mu.UserName, mu.LastLoginDate, mu.IsApproved);
                user.IsAdmin = Roles.IsUserInRole(mu.UserName, "Administrators");
                user.IsStaff = Roles.IsUserInRole(mu.UserName, "Staff");
                su.Add(user);
            }
            return View(su);
        }
        public ActionResult DeleteStaffUser(string UserName)
        {
            StaffUser model = new StaffUser();
            model.UserName = UserName;
            return View(model);
        }
        public ActionResult DeleteUserr(string UserName)
        {
            if (UserName == User.Identity.Name)
            {
                this.ShowMessage(MessageType.Warning, "You must login as someone else to delete your own account!", true);
                return RedirectToAction("ManageUsers", "Admin");
            }
            else
            {
                if (Membership.DeleteUser(UserName))
                {
                    this.ShowMessage(MessageType.Success, "User " + UserName + " deleted.", true);
                    return RedirectToAction("AdminIndex", "Admin");
                }
                else
                {
                    this.ShowMessage(MessageType.Error, "Cannot delete user " + UserName, true);
                    return RedirectToAction("AdminIndex", "Admin");
                }
            }
        }
        public ActionResult EditStaffUser(string UserName)
        {
            MembershipUser user = Membership.GetUser(UserName);
            StaffUser model = new StaffUser();
            model.UserName = user.UserName;
            model.LastLoginDate = user.LastLoginDate;
            model.ActiveAccount = user.IsApproved;
            model.IsAdmin = Roles.IsUserInRole(user.UserName, "Administrators");
            model.IsStaff = Roles.IsUserInRole(user.UserName, "Staff");
            return View(model);
        }
        [HttpPost]
        public ActionResult EditStaffUser(StaffUser model)
        {
            MembershipUser user = Membership.GetUser(model.UserName);
            user.IsApproved = model.ActiveAccount;
            Membership.UpdateUser(user);
            if (Roles.IsUserInRole(model.UserName, "Administrators") && !model.IsAdmin)
            {
                //if user was an admin but we delected admin
                Roles.RemoveUserFromRole(model.UserName, "Administrators");
            }
            else if (!Roles.IsUserInRole(model.UserName, "Administrators") && model.IsAdmin)
            {
                Roles.AddUserToRole(model.UserName, "Administrators");
            }
            //Membership.UpdateUser(user);
            if (Roles.IsUserInRole(model.UserName, "Staff") && !model.IsStaff)
            {
                Roles.RemoveUserFromRole(model.UserName, "Staff");
            }
            else if (!Roles.IsUserInRole(model.UserName, "Staff") && model.IsStaff)
            {
                Roles.AddUserToRole(model.UserName, "Staff");
            }
            Membership.UpdateUser(user);
            return RedirectToAction("AdminIndex", "Admin");
        }
        public ActionResult RegisterUser()
        {
            return View();
        }
        [HttpPost]
        public ActionResult RegisterUser(RegisterModel model, FormCollection collection)
        {
            if (ModelState.IsValid)
            {
                MembershipCreateStatus createStatus;
                Membership.CreateUser(model.UserName, "password!", "fake@fake.com", null, null, true, out createStatus);
                if (createStatus == MembershipCreateStatus.Success)
                {
                    MembershipUser user = Membership.GetUser(model.UserName);
                    if (model.IsStaff)
                    {


                        if (!Roles.RoleExists("Staff"))
                        {
                            Roles.CreateRole("Staff");
                        }

                        Roles.AddUserToRole(model.UserName, "Staff");
                    }
                    else if (model.IsAdmin)
                    {
                        if (!Roles.RoleExists("Administrators"))
                        {
                            Roles.CreateRole("Administrators");
                        }
                        Roles.AddUserToRole(model.UserName, "Administrators");
                    }
                    Membership.UpdateUser(user);
                    this.ShowMessage(MessageType.Success, "Staff user " + model.UserName + " created.", true);
                    return RedirectToAction("AdminIndex", "Admin");
                }
                else
                {
                    //ModelState.AddModelError("", ErrorCodeToString(createStatus));
                    return View();
                }
            }
            return View(model);
        }

        public ActionResult DeleteUser(string UserName)
        {

            StaffUser model = new StaffUser();
            model.UserName = UserName;
            return View(model);
        }
       
            
        
    }
}