using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using EmWebApp.Models.Data;

namespace EmWebApp.BLL
{
    public static class DropDownLookups
    {
        public static SelectList GetAppointmentDates()
        {
            var ldt = EmbassyAppDb.GetApptDates();
            var list = from dt in ldt
                       select new { Name = String.Format("{0:dd MMM, yyyy [dddd]}",dt), Value = dt };

            return new SelectList(list, "Value", "Name");
        }
        public static SelectList GetAppointmentTypes()
        {
            var elements = ConsularAppointmentTypes.List;
            var list = from ele in elements
                       select new { Name = HttpUtility.HtmlDecode(@"&bull; ") + ele.Description, Value = ele.Code };

            return new SelectList(list, "Value", "Name");
        }
        public static SelectList GetAppointmentTypesFromXml()
        {
            XDocument doc = XDocument.Load(ConsularAppointmentTypes.XmlFileName);
            IEnumerable<XElement> elements = doc.Root.Elements("ConsularAppointmentType");

            var list = from ele in elements
                       select new
                       {
                           Name = @"> " + ele.Element("Description_EN").Value
                                  ,
                           Value = Int32.Parse(ele.Element("Code").Value)
                       };

            return new SelectList(list, "Value", "Name");
        }
    }
    public static class LookupExtensions
    {
        public static SelectList WorkingDaysSelectList(this DateTime self, int noOfdaysInFuture, bool isAdmin)
        {
            IEnumerable<DateTime> wdays = self.WorkingDays(self.AddDays(noOfdaysInFuture), isAdmin);

            var list = from dt in wdays
                       select new { Name = dt.ToString("dd MMM, yyyy  [dddd]"), Value = dt.Date };

            return new SelectList(list, "Value", "Name");
        }

        private static IEnumerable<DateTime> WorkingDays(this DateTime self, int noOfdaysInFuture, bool isAdmin)
        {
            return self.WorkingDays(self.AddDays(noOfdaysInFuture), isAdmin);
        }
        private static IEnumerable<DateTime> WorkingDays(this DateTime self, DateTime toDate, bool isAdmin)
        {
            if (isAdmin)
            {
                var adminRange = Enumerable.Range(0, new TimeSpan(toDate.Ticks - self.Ticks).Days);
                return from i in adminRange
                       let date = self.Date.AddDays(i)
                       select date;
            }
            
            var holidays = from dt in BLL.Holidays.List
                            select dt.DT;

            var weekends = new List<DayOfWeek> { DayOfWeek.Saturday, DayOfWeek.Sunday };

            var maxedOutdates = EmbassyAppDb.GetClosedApptDates(EmWebAppConfig.QueueDailyMax);

            // skip today
            var range = Enumerable.Range(1, new TimeSpan(toDate.Ticks - self.Ticks).Days);

            return from i in range
                   let date = self.Date.AddDays(i)
                   where !weekends.Contains(date.DayOfWeek) 
                       && !holidays.Contains(date) 
                       && !maxedOutdates.Contains(date)
                   select date;
        }
    }
}