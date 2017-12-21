using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Configuration;

namespace EmWebApp.Models
{
    public class ConsularApptVM
    {
        public int ID { get; set; }

        [Display(Name = "F0102_AppointmentDate", ResourceType = typeof(Form_Resource))]
        [Required(ErrorMessageResourceName = "F0102_AppointmentDate", ErrorMessageResourceType = typeof(Form_Resource))]
        //[DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM, yyyy  [dddd]}", ApplyFormatInEditMode = true)]
        public DateTime? AppointmentDate { get; set; }

        [Display(Name = "F0122_AppointmentType", ResourceType = typeof(Form_Resource))]
        [Required(ErrorMessageResourceName = "F0122_AppointmentType", ErrorMessageResourceType = typeof(Form_Resource))]
        public int AppointmentType { get; set; }        

        [Display(Name = "F0103_QueNumber", ResourceType = typeof(Form_Resource))]
        public int QueueNumber { get; set; }

        [Display(Name = "F0104_Name", ResourceType = typeof(Form_Resource))]
        [Required(ErrorMessageResourceName = "F0104_Name", ErrorMessageResourceType = typeof(Form_Resource))]
        public string Name { get; set; }

        [Display(Name = "F0105_Gender", ResourceType = typeof(Form_Resource))]
        [Required(ErrorMessageResourceName = "F0105_Gender", ErrorMessageResourceType = typeof(Form_Resource))]
        public string Gender { get; set; }

        [Display(Name = "F0106_DateOfBirth", ResourceType = typeof(Form_Resource))]
        [Required(ErrorMessageResourceName = "F0106_DateOfBirth", ErrorMessageResourceType = typeof(Form_Resource))]
        // [DataType(DataType.Date)] Note: Removed for Chrome/IE datepicker issues
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [RegularExpression(@"^\d{4}$|^\d{4}-((0?\d)|(1[012]))-(((0?|[12])\d)|3[01])$", ErrorMessage = "Date must be in valid (yyyy-mm-dd) format.")]
        public DateTime? DateOfBirth { get; set; }

        [Display(Name = "F0107_PlaceOfBirth", ResourceType = typeof(Form_Resource))]
        [Required(ErrorMessageResourceName = "F0107_PlaceOfBirth", ErrorMessageResourceType = typeof(Form_Resource))]
        public string PlaceOfBirth { get; set; }

        [Display(Name = "F0108_Nationality", ResourceType = typeof(Form_Resource))]
        public string Nationality { get; set; }

        [Display(Name = "F0113_NRIC_No", ResourceType = typeof(Form_Resource))]
        [Required(ErrorMessageResourceName = "F0113_NRIC_No", ErrorMessageResourceType = typeof(Form_Resource))]
        public string NRIC_No { get; set; }

        [Display(Name = "F0109_PassportNumber", ResourceType = typeof(Form_Resource))]
        [Required(ErrorMessageResourceName = "F0109_PassportNumber", ErrorMessageResourceType = typeof(Form_Resource))]
        public string PassportNumber { get; set; }

        [Display(Name = "F0110_PassportIssuedDate", ResourceType = typeof(Form_Resource))]
        [Required(ErrorMessageResourceName = "F0110_PassportIssuedDate", ErrorMessageResourceType = typeof(Form_Resource))]
        // [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = @"{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [RegularExpression(@"^\d{4}$|^\d{4}-((0?\d)|(1[012]))-(((0?|[12])\d)|3[01])$", ErrorMessage = "Date must be in valid (yyyy-mm-dd) format.")]

        public DateTime? PassportIssuedDate { get; set; }        
        public string ConsulateLocation { get; set; }

        [Display(Name = "F0112_ResidentStayPermitNo", ResourceType = typeof(Form_Resource))]
        [Required(ErrorMessageResourceName = "F0112_ResidentStayPermitNo", ErrorMessageResourceType = typeof(Form_Resource))]
        public string StayPermitNumber { get; set; }


        [Display(Name = "F0125_StayType", ResourceType = typeof(Form_Resource))]
        //[Required(ErrorMessageResourceName = "F0125_StayType", ErrorMessageResourceType = typeof(Form_Resource))]
        public int StayType { get; set; }        

        [Display(Name = "F0123_CompanyName", ResourceType = typeof(Form_Resource))]
        public string EmployerName { get; set; }

        [Display(Name = "F0124_Occupation", ResourceType = typeof(Form_Resource))]
        public string Occupation { get; set; }

        [Display(Name = "F0114_ContactAddr", ResourceType = typeof(Form_Resource))]
        [Required(ErrorMessageResourceName = "F0114_ContactAddr", ErrorMessageResourceType = typeof(Form_Resource))]
        public string ContactAddr1 { get; set; }

        [Display(Name = "F011402_ContactAddr", ResourceType = typeof(Form_Resource))]
        public string ContactAddr2 { get; set; }
        
        [Display(Name = "F0116_ContactPhone", ResourceType = typeof(Form_Resource))]
        [Required(ErrorMessageResourceName = "F0116_ContactPhone", ErrorMessageResourceType = typeof(Form_Resource))]
        public string ContactPhone { get; set; }

        [Display(Name = "F0115_ContactEmail", ResourceType = typeof(Form_Resource))]
        [Required(ErrorMessageResourceName = "F0115_ContactEmail", ErrorMessageResourceType = typeof(Form_Resource))]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string ContactEmail { get; set; }

        [Display(Name = "F0117_HomeAddr", ResourceType = typeof(Form_Resource))]
        [Required(ErrorMessageResourceName = "F0117_HomeAddr", ErrorMessageResourceType = typeof(Form_Resource))]
        public string HomeAddr1 { get; set; }

        [Display(Name = "F011702_HomeAddr", ResourceType = typeof(Form_Resource))]
        public string HomeAddr2 { get; set; }        

        [Display(Name = "F0118_HomePhone", ResourceType = typeof(Form_Resource))]
        public string HomePhone { get; set; }
        
        public int AppointmentStatus { get; set; }
        
        public string ActivationCode { get; set; }

        [Display(Name = "F0119_Note", ResourceType = typeof(Form_Resource))]
        public string Note { get; set; }
    }
}