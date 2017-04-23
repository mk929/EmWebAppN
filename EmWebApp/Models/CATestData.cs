using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmWebApp.Models
{
    public class CATestData
    {
        private static int _i = 0;
        public static ConsularApptVM GetNewAppt()
        {
            _i++;
            Random r = new Random();
            DateTime appointmentDate = DateTime.Today;
            do
            {
                appointmentDate = (DateTime.Now.Hour > 12) ?        // afternoon? 
                        DateTime.Today.AddDays(r.Next(1, 15))       // then next 14 days from tomorrow
                        : DateTime.Today.AddDays(r.Next(0, 14));    // else next 14 days from today

            } while (appointmentDate.DayOfWeek == DayOfWeek.Saturday 
                    || appointmentDate.DayOfWeek == DayOfWeek.Sunday);
            

            return new ConsularApptVM()
            {
                AppointmentDate = appointmentDate,
                AppointmentType = r.Next(1, 16),
                Name = String.Format("User_{0}", _i),
                Gender = r.Next(1, 3) == 1 ? "M" : "F",
                DateOfBirth = DateTime.Today.AddYears(r.Next(-60, -15)).AddDays(r.Next(1,365)),
                PlaceOfBirth = String.Format("City/Town Name: {0}, Myanmar", _i),
                Nationality = "MM",
                NRIC_No = String.Format("NRIC{0}", r.Next(1000000, 9999999).ToString("D8")),
                PassportNumber = String.Format("{0}", r.Next(1, 9999999).ToString("D8")),
                PassportIssuedDate = DateTime.Today.AddYears(r.Next(-20, -1)).AddDays(r.Next(1, 365)),
                StayType = r.Next(1, 7),
                StayPermitNumber = String.Format("StayPermitNumber_{0}", _i),
                EmployerName = String.Format("EmployerName_{0}", _i),
                Occupation = String.Format("Occupation_{0}", _i),
                ContactAddr1 = String.Format("[{0}] Singapore Address Line 1", _i),
                ContactAddr2 = String.Format("[{0}] Singapore Address Line 2", _i),
                ContactPhone = String.Format("+65 {0}", 
                                    String.Format("{0}", r.Next(1000000, 9999999).ToString("D8"))),
                ContactEmail = String.Format("{0}@test.com", String.Format("User_{0}", _i)),
                HomeAddr1 = String.Format("[{0}] Myanmar Address Line 1", _i),
                HomeAddr2 = String.Format("[{0}] Myanmar Address Line 2", _i),
                HomePhone = String.Format("+95 9{0}",
                                    String.Format("{0}", r.Next(1000000, 9999999).ToString("D8"))),
                AppointmentStatus = 0,
                ActivationCode = "",
                Note = "TEST"
                // Note = String.Format("Sumit request at: {0:dd MMM, yyyy [hh:mm:ss]}", DateTime.Now)
            };
        }
    }
}