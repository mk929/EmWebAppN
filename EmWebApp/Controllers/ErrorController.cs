using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EmWebApp.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult General(Exception exception)
        {
            TempData ["Title"] = "General failure.";
            // TempData ["Description"] = exception.Message;
            return RedirectToAction("DisplayError");
        }

        public ActionResult HttpError403(string error)
        {
            TempData ["Title"] = "403: An error occurred while processing your request.";
            TempData ["Description"] = error;
            return RedirectToAction("DisplayError");
        }
        public ActionResult HttpError404(string error)
        {
            TempData ["Title"] = "404: An error occurred while processing your request.";
            TempData ["Description"] = error;
            return RedirectToAction("DisplayError");
            //return Content("Page Not found", "text/plain");
        }
        public ActionResult HttpError500(string error)
        {
            TempData ["Title"] = "500: An error occurred while processing your request. (500)";
            TempData ["Description"] = error;
            return RedirectToAction("DisplayError");
        }
        public ActionResult DisplayError()
        {
            return View();
        }
    }
}