using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace EmWebApp
{
    public static class EmWebAppConfig
    {

        public static readonly string DbConnectionString;

        public static readonly int QueueNumberInitial;
        public static readonly int QueueDailyMax;
        
        public static readonly string EmailAddress;
        public static readonly string EmailUser;
        public static readonly string EmailSubj_Confirmed;
        public static readonly string EmailSubj_VerificationRequest;

        public static readonly string AppointmentTypeFile;
        public static readonly string HolidaysFile;
        public static readonly string Emblem_Logo;


        public static readonly bool PrefilledFormTest;

        public static readonly string CnslrLtrPdfTmplPath;

        public static readonly string DomesticWorker_PassportRenew_TmplFileName;

        public static IDictionary<int, string> StayTypeDict = new Dictionary<int, string>()
                                                            {
                                                                {1, "EP"},
                                                                {2, "PR"},
                                                                {3, "Seaman"},
                                                                {4, "SP"},
                                                                {5, "WP (Domestic Worker)"},
                                                                {6, "WP (Normal)"},
                                                                {7, "WP (Nursing Aide/Health Care)"}
                                                            };
        static EmWebAppConfig()
        {
            DbConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            QueueNumberInitial = Convert.ToInt32(ConfigurationManager.AppSettings["QueueNumberInitial"]);
            QueueDailyMax = Convert.ToInt32(ConfigurationManager.AppSettings["QueueDailyMax"]);
            EmailAddress = ConfigurationManager.AppSettings["EmailAddress"];
            EmailUser = ConfigurationManager.AppSettings["EmailUser"];
            EmailSubj_Confirmed = ConfigurationManager.AppSettings["EmailSubj_Confirmed"];
            EmailSubj_VerificationRequest = ConfigurationManager.AppSettings["EmailSubj_VerificationRequest"];
            AppointmentTypeFile = ConfigurationManager.AppSettings["AppointmentTypeFile"];

            HolidaysFile = ConfigurationManager.AppSettings["HolidaysFile"];
            DomesticWorker_PassportRenew_TmplFileName = ConfigurationManager.AppSettings["DomesticWorker_PassportRenew_TmplFileName"];
            Emblem_Logo = ConfigurationManager.AppSettings["Emblem_Logo"];


            //string appVpath = System.Web.HttpRuntime.AppDomainAppVirtualPath;
            //string path = System.Web.Hosting.HostingEnvironment.MapPath(appVpath);
            //string path = HostingEnvironment.ApplicationPhysicalPath;
            //string basePath = AppDomain.CurrentDomain.BaseDirectory;
            //string pdfTmplFilePath = basePath + EmWebAppConfig.ConsularLetterPdfTemplate;
            string basePath = HostingEnvironment.ApplicationPhysicalPath;
            CnslrLtrPdfTmplPath = basePath + ConfigurationManager.AppSettings["ConsularLetterPdfTemplate"];

            PrefilledFormTest = Convert.ToBoolean(ConfigurationManager.AppSettings["PrefilledFormTest"]);
            
        }
    }
}