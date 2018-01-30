using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mvctest.Models.ViewModels;
using mvctest.Models;
using mvctest.Security;


namespace mvctest.Controllers
{
    public class AccountController : Controller
    {
     
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(AccountViewModel m)
        {
            AccountModel am = new AccountModel();
            if (string.IsNullOrEmpty(m.Account.userName)|| string.IsNullOrEmpty(m.Account.Password)
                || am.login(m.Account.userName, m.Account.Password) == null)
            {
                ViewBag.Error = "Account Invalid";
                return View("Index");
            }
            SessionPersister.Username = m.Account.userName;
            return RedirectToAction("Welcomee", "Home"); //change success to welcomee
        }
        public ActionResult Logout()
        {
            SessionPersister.Username = string.Empty;
            return RedirectToAction("Index","Account");
        }
    }
}