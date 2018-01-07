using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using EmWebApp.Domain;
using System.Linq.Expressions;
using AutoMapper;

namespace EmWebApp.Data
{
    public class EmbassyAppDb
    {
        /*
        static EmbassyAppDb()
        {
            ConnectionStringSettings connStrSettings = ConfigurationManager.ConnectionStrings["DefaultConnection"];
            if (connStrSettings == null || string.IsNullOrEmpty(connStrSettings.ConnectionString))
                throw new Exception("Fatal error: missing connecting string in web.config file");
            var connectionString = connStrSettings.ConnectionString;
        }
        */
        public static bool AddConsularAppt(ConsularApptVM dto, int initalQueueNumber)
        {
            var appt = dto.MapToModel();
            appt.QueueNumber = initalQueueNumber;
            using (var context = ApplicationDbContext.Create())
            {
                context.ConsularAppointments.Add(appt);
                context.SaveChanges();
                dto.Id = appt.Id;
                dto.ActivationCode = appt.ActivationCode;
                dto.QueueNumber = appt.QueueNumber;
            }
            return true;
        }
        public static ConsularApptVM ConfirmConsularAppt(int appointmentId, string activationCode,
                                ref DateTime? confirmedApptDate, ref int? confirmedQueNumber)
        {
            confirmedApptDate = null;
            confirmedQueNumber = null;
            ConsularApptVM dto = null;

            using (var context = ApplicationDbContext.Create())
            {
                ConsularAppointment appt = null;
                var query = context.ConsularAppointments;
                appt = query.Where(n => n.Id == appointmentId && n.ActivationCode == activationCode)
                    .DefaultIfEmpty(appt).First();

                if (appt != null && appt.AppointmentStatus != 1) // only if it exists in initial or canceled state
                {
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            appt.QueueNumber = query.Where(n => n.AppointmentDate == appt.AppointmentDate).Max(m => m.QueueNumber);
                            appt.QueueNumber++;
                            appt.AppointmentStatus = 1;

                            context.SaveChanges();
                            transaction.Commit();

                            confirmedQueNumber = appt.QueueNumber;
                            dto = appt.MapToViewModel();
                        }
                        catch (Exception)
                        {
                            transaction.Rollback();
                        }
                    }

                }
            }
            return dto;
        }
        public static ConsularApptVM GetConsularApptById(int id)
        {
            using (var context = ApplicationDbContext.Create())
            {
                var appt = context.ConsularAppointments.Find(id);
                if (appt == null)
                    return new ConsularApptVM();
                return appt.MapToViewModel();
            }
        }
        public static List<DateTime> GetApptDates()
        {
            using (var context = ApplicationDbContext.Create())
            {
                var dates = context.ConsularAppointments
                    .Where(n => n.AppointmentDate >= DateTime.Today && n.AppointmentStatus == 1 && n.AppointmentDate.HasValue )
                    .OrderBy(n => n.AppointmentDate)
                    .Select(n => n.AppointmentDate.Value)
                    .Distinct().ToList();
                return dates;
            }
        }
        public static List<DateTime> GetClosedApptDates(int maxAppts)
        {
            // string sql = String.Format("SELECT AppointmentDate from [dbo].[ConsularAppointments] where AppointmentDate > '{0}  23:59:59' and AppointmentStatus = 1 group by AppointmentDate having count(*) >= {1} order by AppointmentDate", DateTime.Today.ToString("yyyy-MM-dd"), maxAppts);
            using (var context = ApplicationDbContext.Create())
            {
                var dates = context.ConsularAppointments
                    .Where(n => n.AppointmentDate > DateTime.Today && n.AppointmentStatus == 1 && n.AppointmentDate.HasValue)
                    .GroupBy(n => n.AppointmentDate)
                    .Where(grp => grp.Count() >= maxAppts)
                    .Select(n => n.Key.Value).ToList();
                return dates;
            }
        }
        public static List<ConsularApptVM> GetConsularApptsAdmin(string passportNo = null, DateTime? apptDate = null)
        {
            using (var context = ApplicationDbContext.Create())
            {
                var appts = context.ConsularAppointments
                    .Where(ConsularApptsAdminPredicate.CheckForCriteria(passportNo, apptDate))
                    .OrderBy(n => n.AppointmentDate)
                    .ThenBy(n => n.QueueNumber).ToList();
                
                var dto = appts.MapToViewModelList();
                return dto;
            }            
        }
        public static string GetConsularApptsAdminCSV(List<ConsularApptVM> list, char seperator)
        {
            StringBuilder sb = new StringBuilder();

            string header = "Appointment Date|Name|Passport Number|Issued Date|Email|Phone".Replace('|', seperator);
            sb.Append(header);
            sb.AppendLine();
            foreach (var item in list)
            {
                sb.Append(string.Format("{0:yyyy-MM-dd}", item.AppointmentDate));
                sb.Append(seperator);
                sb.Append(item.Name);
                sb.Append(seperator);
                sb.Append(item.PassportNumber);
                sb.Append(seperator);
                sb.Append(string.Format("{0:yyyy-MM-dd}", item.PassportIssuedDate));
                sb.Append(seperator);
                sb.Append(item.ContactEmail);
                sb.Append(seperator);
                sb.Append(item.ContactPhone);
                sb.AppendLine();

            }
            return sb.ToString();
        }

    }

    public static class ConsularApptsAdminPredicate
    {
        public static Expression<Func<ConsularAppointment, bool>> CheckForCriteria(string passportNo = null, DateTime? apptDate = null)
        {

            if (passportNo != null && apptDate != null)
                return (appt => appt.AppointmentDate == apptDate && appt.PassportNumber.StartsWith(passportNo));

            if (passportNo != null && apptDate == null)
                return (appt => appt.PassportNumber.StartsWith(passportNo));

            if (passportNo == null && apptDate != null)
                return (appt => appt.AppointmentDate == apptDate);

            return (appt => appt.Id > 0); // alway true expression
        }
    }

}

