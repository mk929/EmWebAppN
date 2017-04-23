using EmWebApp.BLL;
using EmWebApp.Models;
using EmWebApp.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EmWebApp.Controllers
{
    public static class ControllerExts
    {

        public readonly static string Template_ConfirmedEmail = "_Email_Confirmed";
        public readonly static string Template_ConfirmationRequestEmail = "_Email_ConfirmReq";
        public readonly static string Template_ConfirmationLetter = "~/Views/ConsularAppt/_ConfirmationLetter.cshtml";


        public static Email GetConfirmationRequestEmail(this Controller cntlr, ConsularApptVM consularApptVM)
        {
            string emailBody = cntlr.GetConfirmationRequestEmailBody(consularApptVM);
            Email email = new Email
            {
                From = EmWebAppConfig.EmailAddress,
                DisplayName = EmWebAppConfig.EmailUser,
                To = consularApptVM.ContactEmail,
                Subject = EmWebAppConfig.EmailSubj_VerificationRequest,
                Body = emailBody,
                IsHtml = true
            };

            return email;
        }
        private static string GetConfirmationRequestEmailBody(this Controller cntlr, ConsularApptVM consularApptVM)
        {
            String routeName = String.Empty;
            var controllerName = cntlr.RouteData.Values["controller"].ToString();
            var actionName = "Confirmed";
            cntlr.ViewBag.Applicant = consularApptVM.Name;
            cntlr.ViewBag.EmailConfirmationLink = cntlr.Url.RouteUrl(routeName,
                     new // route values, add more if needed
                     {
                         controller = controllerName,
                         action = actionName,
                         id = consularApptVM.ID,
                         code = consularApptVM.ActivationCode
                     }, cntlr.Request.Url.Scheme);

            // Render Html Email
            ViewRenderer vr = new ViewRenderer(cntlr.ControllerContext);
            return vr.RenderPartialView(Template_ConfirmationRequestEmail, null);
        }
        public static string GetConfirmedEmailBody(this Controller cntlr, ConsularApptVM consularApptVM)
        {
            string actionName = cntlr.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = cntlr.ControllerContext.RouteData.Values["controller"].ToString();

            // logo path ( iis express doesn't have project root in Url
            var baseUrl = cntlr.Request.Url.GetLeftPart(UriPartial.Authority);
            string logoPng = String.Format("{0}{1}{2}{3}"
                , baseUrl
                , cntlr.Request.Url.Segments[0]
                , cntlr.Request.Url.Segments[1].IndexOf(controllerName) == 0 ? String.Empty : cntlr.Request.Url.Segments[1]
                , EmWebAppConfig.Emblem_Logo);
            cntlr.ViewBag.Logo = Graphics.GetImgTag(logoPng, "logo");

            // Service Type
            AppointmentType appointmentType = ConsularAppointmentTypes.GetAppointmentType(consularApptVM.AppointmentType);
            cntlr.ViewBag.AppointmentType = appointmentType.Description;
            ViewRenderer vr = new ViewRenderer(cntlr.ControllerContext);
            return vr.RenderPartialView(Template_ConfirmedEmail, consularApptVM);
        }
        public static string GetConfirmationLetter(this Controller cntlr, int id)
        {
            var consularApptVM = EmWebApp.Models.Data.EmbassyAppDb.GetConsularApptById(id);
            SetViewBagForConfirmationLetter(cntlr, consularApptVM);
            return GetConfirmationLetter(cntlr, consularApptVM);
        }
        public static string GetConfirmationLetter(this Controller cntlr, ConsularApptVM consularApptVM)
        {
            ViewRenderer vr = new ViewRenderer(cntlr.ControllerContext);
            SetViewBagForConfirmationLetter(cntlr, consularApptVM);
            return vr.RenderPartialView(Template_ConfirmationLetter, consularApptVM);
        }
        private static void SetViewBagForConfirmationLetter(this Controller cntlr, ConsularApptVM consularApptVM)
        {
            if (cntlr.ViewBag.Logo == null)
            {
                string basePath = AppDomain.CurrentDomain.BaseDirectory;
                cntlr.ViewBag.Logo = Graphics.GetImgSrcForPdf(basePath + EmWebAppConfig.Emblem_Logo);
            }
            // Get Service Type look up
            if (cntlr.ViewBag.AppointmentType == null)
            {
                AppointmentType appointmentType = ConsularAppointmentTypes.GetAppointmentType(consularApptVM.AppointmentType);
                cntlr.ViewBag.AppointmentType = appointmentType.Description;
            }
            // Get QR code
            if (cntlr.ViewBag.QR == null)
            {
                cntlr.ViewBag.QR = Graphics.GenerateRelayQrCode(ConfirmationLetterPdf.GetQrCodeString(consularApptVM));
            }
        }
        public static string GetViewHtml(this Controller ctrlr, object model)
        {
            string emailBody = String.Empty;
            ctrlr.ViewData.Model = model;

            string viewName = ctrlr.ControllerContext.RouteData.GetRequiredString("action");
            using (StringWriter sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ctrlr.ControllerContext, viewName);
                ViewContext viewContext = new ViewContext(ctrlr.ControllerContext, viewResult.View,
                    ctrlr.ViewData, ctrlr.TempData, sw);
                viewResult.View.Render(viewContext, sw);
                emailBody = sw.GetStringBuilder().ToString();
            }
            return emailBody;
        }
        public static void LogModelStateError(this Controller ctrlr)
        {
            var errors = ctrlr.ModelState
                        .Where(x => x.Value.Errors.Count > 0)
                        .Select(x => new { x.Key, x.Value.Errors })
                        .ToArray();
            // log
            // var errors2 = ModelState.Values.SelectMany(v => v.Errors);
        }
    }
}