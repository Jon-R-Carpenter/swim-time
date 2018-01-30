using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheSwimTimeSite.Models;
using TheSwimTimeSite.ObjectModels;
using TheSwimTimeSite.Models.ViewModels;
using Newtonsoft.Json;
using mvctest.Security;

namespace mvctest.Controllers
{
    //[CustomAuthorize(Roles = "Admin")]

    public class SetUpController : Controller
    {
        private ISetupService Service;


        public SetUpController()
        {
            Service = new SetupService(new ModelStateWrapper(this.ModelState), new SetupRepository());
        }

        public SetUpController(ISetupService service)
        {
            Service = service;
        }
        
        // GET: SetUp
        public ActionResult SetUpIndex()
        {
            return View();
        }
        public ActionResult Certifications()
        {
            return View();
        }
        public ActionResult SwimTime()
        {
            return View(Service.ListAllSwimTime());
        }
        public ActionResult Comment()
        {
            return View();
        }
        public ActionResult createCertification()
        {
            return View();
        }
        [HttpGet]
        public ActionResult createSwimTime()
        {
            CreateEditSwimTimeRulesViewModel model = new CreateEditSwimTimeRulesViewModel(Service.ListAllCourses());
            return View(model);
        }
        [HttpPost]
        public ActionResult createSwimTime(CreateEditSwimTimeRulesViewModel input)
        {
            TryUpdateModel(input);
            if (!Service.CreateSwimTime(input.Creating))
            {
                CreateEditSwimTimeRulesViewModel model = new CreateEditSwimTimeRulesViewModel(Service.ListAllCourses(), input.Creating);
                return View(model);
            }
            else
            {
                return RedirectToAction("SwimTime");//Default valud here now
            }
        }
        [HttpGet]
        public ActionResult createTerm()
        {
            CreateEditTermViewModel model = new CreateEditTermViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult createTerm(CreateEditTermViewModel input)
        {
            TryUpdateModel(input);
            if (!Service.CreateTerm(input.Creating))
            {
                CreateEditTermViewModel model = new CreateEditTermViewModel();
                return View(model);
            }
            else
            {
                return RedirectToAction("Terms");//Default valud here now
            }
        }
        [HttpGet]
        public ActionResult createVisitType()
        {
            return View(new VisitType());
        }
        [HttpPost]
        public ActionResult CreateVisitType(VisitType creating)
        {
            if (!Service.CreateVisitType(creating))
            {
              
                return View();
            }

            return RedirectToAction("visitType");//redirect to equipment list

        }

        public ActionResult editCertification()
        {
            return View();
        }
        [HttpGet]
        public ActionResult editSwimTime(int id)
        {
            SwimTime editing = Service.GetSwimTimeByID(id);
            return View(new CreateEditSwimTimeRulesViewModel(editing, Service.ListAllCourses()));
        }
        [HttpPost]
        public ActionResult editSwimTime(CreateEditSwimTimeRulesViewModel input)
        {
            TryUpdateModel(input);
            if (!Service.EditSwimTime(input.Creating))
            {
                CreateEditSwimTimeRulesViewModel model = new CreateEditSwimTimeRulesViewModel(Service.ListAllCourses(), input.Creating);
                return View(model);
            }
            else
            {
                return RedirectToAction("SwimTime");//Default valud here now
            }
        }

        [HttpGet]
        public ActionResult editTerm(int id)
        {
            Term editing = Service.GetTermByID(id);

            return View(new CreateEditTermViewModel(editing));
        }
        [HttpPost]
        public ActionResult editTerm(CreateEditTermViewModel input)
        {
            Service.EditTerm(input.Creating);

             if (ModelState.IsValid)
             {
                 return RedirectToAction("Terms");//redirect to term list
             }
             else
             {
                 return View(input);//return view with validation summary
             }

        }
        [HttpGet]
        public ActionResult editVisitType(int id)
        {
            return View(Service.GetVisitTypeByID(id));
        }
        [HttpPost]
        public ActionResult EditVisitType(VisitType editing)
        {
            if (!Service.EditVisitType(editing))
            {
               
                return View();
            }

            return RedirectToAction("VisitType");//redirect to equipment list
        }

        public ActionResult Terms()
        {
        
            return View(Service.ListAllTerms());
        }
        public ActionResult visitType()
        {
            return View(Service.ListAllVisitTypes());
        }
    }
}