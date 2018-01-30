using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheSwimTimeSite.ObjectModels;
using TheSwimTimeSite.Models;
using TheSwimTimeSite.Models.ViewModels;

namespace mvctest.Controllers
{
   // [Authorize(Roles = "Admin,Staff")]
    public class CourseController : Controller
    {
        public ICourseService Service;

        public CourseController()
        {
            Service = new CourseService(new ModelStateWrapper(this.ModelState), new CourseRepository());
        }
        public CourseController(ICourseService service)
        {
            Service = service;
        }


        // GET: Course
        public ActionResult Index()
        {
            return View(Service.ListAllCourses());
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View(new CreateEditCourseViewModel(Service.ListAllTerms()));
        }
        [HttpPost]
        public ActionResult Create(CreateEditCourseViewModel input)
        {
            bool test = TryUpdateModel(input);
            Service.CreateCourse(input.Creating);

            if (ModelState.IsValid)
            {
                return RedirectToAction("Index");//redirect to course list
            }
            else
            {
                return View(new CreateEditCourseViewModel(input.Creating, Service.ListAllTerms()));//return view with validation summary
            }
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {

            Course editing = Service.GetCourseByID(id);
            return View(new CreateEditCourseViewModel(editing, Service.ListAllTerms()));
        }
        [HttpPost]
        public ActionResult Edit(CreateEditCourseViewModel input)
        {
            Service.EditCourse(input.Creating);

            if (ModelState.IsValid)
            {
                return RedirectToAction("Index");//redirect to course list
            }
            else
            {
                Course editing = Service.GetCourseByID(input.Creating.CRN);
                return View(new CreateEditCourseViewModel(editing, Service.ListAllTerms()));//return view with validation summary
            }
        }
        [HttpGet]
        public ActionResult Enroll(int id)
        {
            Course editing = Service.GetCourseCompleteByID(id);
            return View(new EnrollmentViewModel(editing, Service.ListAllUsers()));

        }
        [HttpPost]
        public ActionResult Enroll(FormCollection input)//gets the list of Enrolled students from Enrolled
        {
            int courseID = int.Parse(input.GetValues("CourseID").FirstOrDefault().ToString());
            List<int> enrolling = new List<int>();//blank list
            String[] sd = input.GetValues("UnEnrolled");

            if (input.GetValues("Enrolled") != null)
            {
                enrolling = input.GetValues("Enrolled").ToList().Select(int.Parse).ToList();
            }

            Service.AdjustEnrollment(courseID, enrolling);
            //adjust enrollment taken from integer translated list of userID's - to course Editing.CourseID
            return RedirectToAction("Index");
        }
        public ActionResult Demo()
        {
            return View();
        }
    }
}