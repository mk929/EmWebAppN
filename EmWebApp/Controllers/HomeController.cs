using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EmWebApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Consular Appointment Request.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Myanmar Embassy (Singapore)";

            return View();
        }
        public ActionResult Error()
        {
            ViewBag.Message = "An Error occured. Please contact the Administrator for assistant.";

            return View();
        }
    }
}