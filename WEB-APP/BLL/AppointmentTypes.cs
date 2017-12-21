using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using EmWebApp.Util;
using System.Web.Mvc;
using System.Xml.Linq;
using System.Web.Hosting;

namespace EmWebApp.BLL
{
    public static class ConsularAppointmentTypes
    {
        static List<AppointmentType> _appointmentTypes = null;
        static string _configXmlFileName;

        static ConsularAppointmentTypes()
        {
        }
        public static string XmlFileName
        {
            get
            {
                if (string.IsNullOrEmpty(_configXmlFileName))
                    _configXmlFileName = AppDomain.CurrentDomain.BaseDirectory
                                            + EmWebAppConfig.AppointmentTypeFile;
                return _configXmlFileName;
            }
        }
        public static List<AppointmentType> List
        {
            get
            {
                if (_appointmentTypes == null)
                {
                    AppointmentTypeList atl = XmlUtil.DeserializeXMLFileToObject<AppointmentTypeList>(XmlFileName);
                    _appointmentTypes = atl.AppointmentTypes;
                }

                return _appointmentTypes;
            }
        }
        public static AppointmentType GetAppointmentType(int id)
        {
            AppointmentType appointmentType = new AppointmentType();

            var elements = ConsularAppointmentTypes.List;
            var list = from ele in elements
                       where ele.Code == id
                       select ele;
            if (list.Count() > 0)
                appointmentType = list.First();

            return appointmentType;
        }
    }

    public static class Holidays
    {
        static List<Holiday> _Holidays = null;
        static string _configXmlFileName;
        
        public static string XmlFileName
        {
            get
            {
                if (string.IsNullOrEmpty(_configXmlFileName))
                    _configXmlFileName = AppDomain.CurrentDomain.BaseDirectory
                                            + EmWebAppConfig.HolidaysFile;
                return _configXmlFileName;
            }
        }
        public static List<Holiday> List
        {
            get
            {
                if (_Holidays == null)
                {
                    HolidayList atl = XmlUtil.DeserializeXMLFileToObject<HolidayList>(XmlFileName);
                    _Holidays = atl.List;
                }

                return _Holidays;
            }
        }

    }

    //======================       XML Serialization Classes [AppointmentTypeList] ===========================================


    [XmlRoot("ConsularAppointmentTypes")]
    public class AppointmentTypeList
    {
        [XmlElement("ConsularAppointmentType")]
        public List<AppointmentType> AppointmentTypes { get; set; }
    }

    public class AppointmentType
    {
        [XmlElement("Code")]
        public int Code { get; set; }
        [XmlElement("Description_EN")]
        public string Description { get; set; }

        [XmlElement("ConsularLetterPdfTemplate")]
        public string ConsularLtrPdfTmplFilename { get; set; }        

        [XmlElement("Attachments", IsNullable = false)]
        public AttachmentList Attachments { get; set; }

    }

    public class AttachmentList
    {
        [XmlElement("Attachment", IsNullable = false)]
        public List<Attachment> List { get; set; }
    }
    public class Attachment
    {
        [XmlAttribute("name")]
        public string FileName { get; set; }

        [XmlAttribute("path")]
        public string Path { get; set; }
        public string FullPath
        {
            get
            {
                return HostingEnvironment.ApplicationPhysicalPath + Path + FileName;
            }
        }

        [XmlAttribute("mediaType")]
        public string MediaType { get; set; }
    }


    //======================       XML Serialization Classes [HolidayList] ===========================================

    [XmlRoot("Holidays")]
    public class HolidayList
    {
        [XmlElement("Holiday")]
        public List<Holiday> List { get; set; }
    }
    public class Holiday
    {
        [XmlAttribute("month")]
        public string Month { get; set; }

        [XmlAttribute("day")]
        public string Day { get; set; }

        public DateTime NextHoliday
        {
            get
            {
                var holiday = Convert.ToDateTime(String.Format("{0}/{1}/{2}", DateTime.Today.Year, Month, Day));

                if ( DateTime.Today > holiday)
                    holiday = Convert.ToDateTime(String.Format("{0}/{1}/{2}", DateTime.Today.Year + 1, Month, Day));

                return holiday;
            }
        }
    }
}
    