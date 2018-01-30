using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mvctest.Security;
using mvctest.Models;

namespace mvctest.Controllers
{
    public class DemoController : Controller
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }
        [CustomAuthorize(Roles="Admin")]
        public ActionResult Work1()
        {
            return View("Work1");
        }
       
    }
}