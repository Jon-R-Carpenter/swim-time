using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheSwimTimeSite.ObjectModels;
using TheSwimTimeSite.Models;
using TheSwimTimeSite.Models.ViewModels;
using TheSwimTimeSite.Helpers;

namespace mvctest.Controllers
{
    public class HomeController : Controller
    {
        private IHomeService Service;
        //
        // Dependency Injection enabled constructors

        public HomeController()
        {
            Service = new HomeService(new ModelStateWrapper(this.ModelState), new HomeRepository());
        }

        public HomeController(IHomeService service)
        {
            Service = service;
        }


        // GET: Test
        public ActionResult Index()
      {
            ViewBag.IP = Request.UserHostAddress;
            return View(new HomeViewModel(Service.ListComments(), Service.ListUsersIn()));
        }
        // GET: Welcomee
      
        public ActionResult Welcomee()
        {
            return View(new HomeViewModel(Service.ListComments(), Service.ListUsersIn()));
        }
        //GET:Construction
        public ActionResult Construction()
        {
            return View();
        }
    }
}