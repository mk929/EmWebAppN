using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EmDbXml
{
    class Program
    {

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

        static void Main(string[] args)
        {
            try
            {
                Run(args);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        static void Run(string[] args)
        {
            string apptDateStr = DateTime.Today.ToString("yyyy-MM-dd");
            if (args.Length > 0)
            {
                string[] formats = { "yyyy-MM-dd", "yyyy/MM/dd", "yyyy-M-d", "yyyy/M/d" };
                DateTime date;
                if ( DateTime.TryParseExact(args[0], formats, new CultureInfo(""), DateTimeStyles.None, out date))
                    apptDateStr = date.ToString("yyyy/MM/dd");
                else
                {
                    Console.WriteLine("[EmDbXml] Invalid Appointment Date: {0}. Use 'yyyy-mm-dd' format. ", args[0]);
                    return;
                }
            }  

            Console.WriteLine("[EmDbXml] Appointment Date: {0} ", apptDateStr);

            var sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            var cmd = new SqlCommand("SELECT * FROM [dbo].[ConsularAppointments] where [AppointmentDate] = @AppointmentDate and [AppointmentStatus] = 1", sqlConn);
            cmd.Parameters.Add(new SqlParameter("@AppointmentDate", DateTime.Parse(apptDateStr)));
            var adptr = new SqlDataAdapter(cmd);
            var dt = new DataTable();
            adptr.Fill(dt);

            Console.WriteLine("[EmDbXml] Number of Appointments: {0} ", dt.Rows.Count);

            var xE = new XElement("Appointments",
                   from cnslrApptList in dt.AsEnumerable()
                   orderby cnslrApptList.Field<string>("Name") descending
                   select new XElement("Appointment",
                        new XAttribute("Name", cnslrApptList.Field<string>("Name")),
                        // new XAttribute("AppointmentType", cnslrApptList.Field<int>("AppointmentType")),
                        new XElement("PassportNumber", cnslrApptList.Field<string>("PassportNumber")),
                        new XElement("PassportIssuedDate", cnslrApptList.Field<DateTime>("PassportIssuedDate").ToString("yyyy-MM-dd")),
                        new XElement("Gender", cnslrApptList.Field<string>("Gender")),
                        new XElement("DateOfBirth", cnslrApptList.Field<DateTime>("DateOfBirth").ToString("yyyy-MM-dd")),
                        new XElement("PlaceOfBirth", cnslrApptList.Field<string>("PlaceOfBirth")),
                        new XElement("NRIC_No", cnslrApptList.Field<string>("NRIC_No")),
                        new XElement("StayType", StayTypeDict[cnslrApptList.Field<int>("StayType")]),
                        new XElement("StayPermitNumber", cnslrApptList.Field<string>("StayPermitNumber")),
                        new XElement("ContactEmail", cnslrApptList.Field<string>("ContactEmail")))
                   );

            System.IO.Directory.CreateDirectory(@".\out");
            string fileName = String.Format(@".\out\ConsularAppts_{0}.xml", DateTime.Now.ToString("yyyyMMdd_HHmmssff"));
            xE.Save(fileName);

            Console.WriteLine("[EmDbXml] Completed. file: {0} ", fileName);
        }


    }
}
