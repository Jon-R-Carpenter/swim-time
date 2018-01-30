using mvctest.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheSwimTimeSite.Models;
using TheSwimTimeSite.ObjectModels;

namespace mvctest.Controllers
{
    [CustomAuthorize(Roles = "Admin")]
    public class ReportsController : Controller
    {
        private IReportsService Service;

        public ReportsController()
        {
            Service = new ReportsService(new ModelStateWrapper(this.ModelState), new ReportsRepository());
        }
     
        public ReportsController(IReportsService service)
        {
            Service = service;
        }


        [HttpGet]
        public ActionResult ReportIndex()
        {
            return View(new Report(DateTime.Now.AddMonths(-1).Date, DateTime.Now, "", false));
        }
        [HttpPost]
        public ActionResult ReportIndex(Report input)
        {
            Report masterReport = Service.GenerateReport(input);
            if (ModelState.IsValid)
            {
                //HeaderRow = "userID, lastName, firstName, userType, classInSchool, Date, Time, visitType, inOut";
                //Report test = new Report(DateTime.Parse("1/1/1990"), DateTime.Now);
                //Report masterReport = Service.GenerateReport(input);
                String csv = masterReport.HeaderRow + "\n";
                foreach (ReportRow row in masterReport.Data)
                {
                    csv += row.CSVToString();
                }
                System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
                Byte[] bytes = encoding.GetBytes(csv);
                return File(bytes, "text/csv", masterReport.FileName + ".csv");

            }
            else
            {
                return View(input);
            }

        }
        [HttpGet]
        public ActionResult IndividualReport()
        {
            //List<User> ret = Service.ListUsers();
            return View(new Report(DateTime.Now.AddMonths(-1).Date, DateTime.Now, "", false));
        }

        [HttpPost]
        public ActionResult IndividualReport(Report input)
        {
            Report indiReport = Service.GenerateIndividualReport(input);
            if (ModelState.IsValid)
            {
                String csv = indiReport.HeaderRow + "\n";
                foreach (ReportRow row in indiReport.Data)
                {
                    csv += row.CSVToString();
                }
                System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
                Byte[] bytes = encoding.GetBytes(csv);
                return File(bytes, "text/csv", indiReport.FileName + ".csv");

            }
            else
            {
                return View(input);
            }

        }



        public ActionResult TrendReport()
        {
            return View();
        }
       
        public ActionResult classReport()
        {
            return View();  
        }
    }
}