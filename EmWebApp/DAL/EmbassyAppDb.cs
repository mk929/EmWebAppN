using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace EmWebApp.Models.Data
{
    public class EmbassyAppDb
    {
        public static string EmWebConfig { get; private set; }

        private static SqlConnection GetDBConnection()
        {
            SqlConnection conn = null;
            string connString = EmWebAppConfig.DbConnectionString;
            conn = new SqlConnection(connString);
            return conn;
        }

        public static List<DateTime> GetApptDates()
        {
            using (SqlConnection conn = GetDBConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT distinct AppointmentDate from [dbo].[ConsularAppointments] where AppointmentDate >= convert(date, getdate()) and AppointmentStatus = 1 order by AppointmentDate", conn);
                using (SqlDataReader sdr = cmd.ExecuteReader())
                {
                    var dt = new DataTable();
                    dt.Load(sdr);
                    var list = (from dr in dt.AsEnumerable()
                                select dr.Field<DateTime>("AppointmentDate")).ToList();
                    return list;
                }
            }
        }

        public static List<ConsularApptVM> GetConsularApptsAdmin(string passportNo = null, DateTime? apptDate = null)
        {
            using (SqlConnection conn = GetDBConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("GetActiveConsularApptsForAdmin", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                if (apptDate != null )
                    cmd.Parameters.Add(new SqlParameter("@AppointmentDate", apptDate));
                if (passportNo != null)
                    cmd.Parameters.Add(new SqlParameter("@PassportNumber", passportNo));
                using (SqlDataReader sdr = cmd.ExecuteReader())
                {
                    var dt = new DataTable();
                    dt.Load(sdr);
                    var list = (from dr in dt.AsEnumerable()
                                select GetConsularApptVM(dr)).ToList();

                    return list;
                }
            }
        }

        public static ConsularApptVM GetConsularApptById(int id)
        {
            using (SqlConnection conn = GetDBConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM [dbo].[ConsularAppointments] where id = @id", conn);
                cmd.Parameters.Add(new SqlParameter("@id", id));
                //.....
                using (SqlDataReader sdr = cmd.ExecuteReader())
                {
                    var dt = new DataTable();
                    dt.Load(sdr);
                    if (dt.Rows.Count == 1)
                        return GetConsularApptVM(dt.Rows[0]);
                }
            }
            return new ConsularApptVM();
        }

        public static ConsularApptVM ConfirmConsularAppt(int appointmentId, string activationCode,
                                ref DateTime? confirmedApptDate, ref int? confirmedQueNumber)
        {
            ConsularApptVM consularAppt = null;
            confirmedQueNumber = 0;
            using (SqlConnection conn = GetDBConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("ConfirmConsularAppointment", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@ApplicationID", appointmentId));
                cmd.Parameters.Add(new SqlParameter("@ActivationCode", activationCode));
                cmd.Parameters.Add(new SqlParameter("@Note", "Confirmation Note"));

                SqlParameter sp = new SqlParameter("@ConfirmedApptDate", SqlDbType.DateTime);
                sp.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(sp);

                sp = new SqlParameter("@ConfirmedQueueNumber", SqlDbType.Int);
                sp.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(sp);

                // cmd.ExecuteNonQuery();

                SqlDataAdapter sqlAdptr = new SqlDataAdapter();
                sqlAdptr.SelectCommand = cmd;
                DataSet ds = new DataSet();
                sqlAdptr.Fill(ds);

                if (ds.Tables[0].Rows.Count == 1)
                {
                    confirmedApptDate = Convert.ToDateTime(cmd.Parameters["@ConfirmedApptDate"].Value);
                    confirmedQueNumber = Convert.ToInt32(cmd.Parameters["@ConfirmedQueueNumber"].Value);
                    consularAppt = GetConsularApptVM(ds.Tables[0].Rows[0]);
                }
            }
            return consularAppt;
        }

        private static ConsularApptVM GetConsularApptVM( DataRow dr )
        {
            return new ConsularApptVM
            {
                ID = dr.Field<int>("Id"),
                AppointmentDate = dr.Field<DateTime>("AppointmentDate"),
                AppointmentType = dr.Field<int>("AppointmentType"),
                QueueNumber = dr.Field<int>("QueueNumber"),
                Name = dr.Field<String>("Name"),
                Gender = dr.Field<String>("Gender"),
                DateOfBirth = dr.Field<DateTime>("DateOfBirth"),
                PlaceOfBirth = dr.Field<String>("PlaceOfBirth"),
                Nationality = dr.Field<String>("Nationality"),
                NRIC_No = dr.Field<String>("NRIC_No"),
                PassportNumber = dr.Field<String>("PassportNumber"),
                PassportIssuedDate = dr.Field<DateTime>("PassportIssuedDate"),
                ConsulateLocation = dr.Field<String>("ConsulateLocation"),
                StayType = dr.Field<int>("StayType"),
                StayPermitNumber = dr.Field<String>("StayPermitNumber"),
                EmployerName = dr.Field<String>("EmployerName"),
                Occupation = dr.Field<String>("Occupation"),
                ContactAddr1 = dr.Field<String>("ContactAddr1"),
                ContactAddr2 = dr.Field<String>("ContactAddr2"),
                ContactPhone = dr.Field<String>("ContactPhone"),
                ContactEmail = dr.Field<String>("ContactEmail"),
                HomeAddr1 = dr.Field<String>("HomeAddr1"),
                HomeAddr2 = dr.Field<String>("HomeAddr2"),
                HomePhone = dr.Field<String>("HomePhone"),
                AppointmentStatus = dr.Field<int>("AppointmentStatus"),
                ActivationCode = dr.Field<String>("ActivationCode"),
                Note = dr.Field<String>("Note")
            };

        }

        public static bool AddConsularAppt(ConsularApptVM ca, int initalQueueNumber)
        {
            ca.QueueNumber = initalQueueNumber;

            SqlConnection conn = GetDBConnection();
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("AddConsularAppointment", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@AppointmentDate", ca.AppointmentDate));
                cmd.Parameters.Add(new SqlParameter("@AppointmentType", ca.AppointmentType));
                cmd.Parameters.Add(new SqlParameter("@QueueNumber", ca.QueueNumber));
                cmd.Parameters.Add(new SqlParameter("@Name", ca.Name));
                cmd.Parameters.Add(new SqlParameter("@Gender", ca.Gender));
                cmd.Parameters.Add(new SqlParameter("@DateOfBirth", ca.DateOfBirth));
                cmd.Parameters.Add(new SqlParameter("@PlaceOfBirth", ca.PlaceOfBirth));
                cmd.Parameters.Add(new SqlParameter("@Nationality", ca.Nationality));
                cmd.Parameters.Add(new SqlParameter("@NRIC_No", ca.NRIC_No));
                cmd.Parameters.Add(new SqlParameter("@PassportNumber", ca.PassportNumber));
                cmd.Parameters.Add(new SqlParameter("@PassportIssuedDate", ca.PassportIssuedDate));
                cmd.Parameters.Add(new SqlParameter("@ConsulateLocation", ca.ConsulateLocation));
                cmd.Parameters.Add(new SqlParameter("@StayType", ca.StayType));
                cmd.Parameters.Add(new SqlParameter("@StayPermitNumber", ca.StayPermitNumber));
                cmd.Parameters.Add(new SqlParameter("@EmployerName", ca.EmployerName));
                cmd.Parameters.Add(new SqlParameter("@Occupation", ca.Occupation));
                cmd.Parameters.Add(new SqlParameter("@ContactAddr1", ca.ContactAddr1));
                cmd.Parameters.Add(new SqlParameter("@ContactAddr2", ca.ContactAddr2));
                cmd.Parameters.Add(new SqlParameter("@ContactPhone", ca.ContactPhone));
                cmd.Parameters.Add(new SqlParameter("@ContactEmail", ca.ContactEmail));
                cmd.Parameters.Add(new SqlParameter("@HomeAddr1", ca.HomeAddr1));
                cmd.Parameters.Add(new SqlParameter("@HomeAddr2", ca.HomeAddr2));
                cmd.Parameters.Add(new SqlParameter("@HomePhone", ca.HomePhone));
                cmd.Parameters.Add(new SqlParameter("@Note", ca.Note));

                // Output Parameters
                SqlParameter sp = new SqlParameter("@ApplicationId", SqlDbType.Int);
                sp.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(sp);

                sp = new SqlParameter("@ActivationCode", SqlDbType.NVarChar, 256);
                sp.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(sp);

                cmd.ExecuteNonQuery();

                ca.ID = Convert.ToInt32(cmd.Parameters["@ApplicationId"].Value);
                ca.ActivationCode = Convert.ToString(cmd.Parameters["@ActivationCode"].Value);
            }
            catch (Exception ex)
            {
                // log
                throw ex;
            }
            finally
            {
                if (conn != null)
                    conn.Close();
            }
            return true;
        }
    }

}

