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
                       select new { Name = @"> " + ele.Description, Value = ele.Code };

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
        public static SelectList WorkingDaysSelectList(this DateTime self, int noOfdaysInFuture, bool todayIncluded)
        {
            IEnumerable<DateTime> wdays = self.WorkingDays(self.AddDays(noOfdaysInFuture), todayIncluded);

            var list = from dt in wdays
                       select new { Name = dt.ToString("dd MMM, yyyy  [dddd]"), Value = dt.Date };

            return new SelectList(list, "Value", "Name");
        }

        public static IEnumerable<DateTime> WorkingDays(this DateTime self, int noOfdaysInFuture, bool todayIncluded)
        {
            return self.WorkingDays(self.AddDays(noOfdaysInFuture), todayIncluded);
        }
        public static IEnumerable<DateTime> WorkingDays(this DateTime self, DateTime toDate, bool todayIncluded)
        {
            // if afternoon, skip today.
            //var range = Enumerable.Range(
            //    ((self.Hour > 12) ? 1 : 0), new TimeSpan(toDate.Ticks - self.Ticks).Days);
            
            var range = Enumerable.Range(
                (todayIncluded ? 0 : 1), new TimeSpan(toDate.Ticks - self.Ticks).Days);

            var Holidays = from dt in BLL.Holidays.List
                    select dt.DT;
            
            var exclude = new List<DayOfWeek> { DayOfWeek.Saturday, DayOfWeek.Sunday };

            return from i in range
                   let date = self.Date.AddDays(i)
                   where !exclude.Contains(date.DayOfWeek) && !Holidays.Contains(date)
                   select date;
        }
    }
}