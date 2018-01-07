using System;
using System.ComponentModel.DataAnnotations;

namespace EmWebApp.Data
{
    public class ConsularApptVM
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Appointment Date (တွေ့ဆုံလိုသည့်ရက်စွဲ)*:")]
        public DateTime? AppointmentDate { get; set; }

        [Required]
        [Display(Name = "Service Type (အကြောင်းအရာ)*:")]
        public int AppointmentType { get; set; }
        public int QueueNumber { get; set; }

        [Required]
        [StringLength(128)]
        [Display(Name = "Name (အမည်)*:")]
        public string Name { get; set; }

        [Required]
        [StringLength(1)]
        [Display(Name = "Gender(ကျား/မ)*:")]
        public string Gender { get; set; }

        [Required]
        [Display(Name = "Date of Birth (မွေးသက္ကရာဇ်)*:")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [RegularExpression(@"^\d{4}$|^\d{4}-((0?\d)|(1[012]))-(((0?|[12])\d)|3[01])$", ErrorMessage = "Date must be in valid (yyyy-mm-dd) format.")]
        public DateTime? DateOfBirth { get; set; }

        [Required]
        [StringLength(128)]
        [Display(Name = "Place of Birth (မွေးရပ်)*:")]
        public string PlaceOfBirth { get; set; }

        [StringLength(2)]
        [Display(Name = "Nationality (နိုင်ငံသား):")]
        public string Nationality { get; set; }

        [Required]
        [StringLength(128)]
        [Display(Name = "NRIC No (နိုင်ငံသားမှတ်ပုံတင်အမှတ်)*:")]
        public string NRIC_No { get; set; }

        [Required]
        [StringLength(128)]
        [Display(Name = "Passport No (နိုင်ငံကူးလက်မှတ် #)*:")]
        public string PassportNumber { get; set; }

        [Required]
        [Display(Name = "Issue Date (ထုတ်ပေးရက်စွဲ)*:")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [RegularExpression(@"^\d{4}$|^\d{4}-((0?\d)|(1[012]))-(((0?|[12])\d)|3[01])$", ErrorMessage = "Date must be in valid (yyyy-mm-dd) format.")]
        public DateTime? PassportIssuedDate { get; set; }

        [StringLength(128)]
        public string ConsulateLocation { get; set; }

        [Required]
        [StringLength(128)]
        [Display(Name = "Sgp IC/ Permit No (စင်္ကာပူ IC/ ပါမစ် အမှတ်)*:")]
        public string StayPermitNumber { get; set; }

        [Required]
        [Display(Name = "Current Stay Type (လက်ရှိ နေထိုင်ခွင့် အမျိုးအစား)*:")]
        public int StayType { get; set; }

        [StringLength(128)]
        [Display(Name = "Company/Employer Name (ကုမ္ပဏီ/အလုပ်ရှင်အမည်):")]
        public string EmployerName { get; set; }

        [StringLength(128)]
        [Display(Name = "Occupation (လုပ်ငန်း ရာထူးအမည်):")]
        public string Occupation { get; set; }

        [Required]
        [StringLength(512)]
        [Display(Name = "Singapore Address (စင်္ကာပူ နေရပ်လိပ်စာ)*:")]
        public string ContactAddr1 { get; set; }

        [StringLength(512)]
        public string ContactAddr2 { get; set; }

        [Required]
        [StringLength(128)]
        [Display(Name = "Contact Phone  (ဆက်သွယ်ရန်-ဖုန်း)*:")]
        public string ContactPhone { get; set; }

        [Required]
        [StringLength(128)]
        [Display(Name = "Contact Email  (ဆက်သွယ်ရန်-အီးမေးလ်)*:")]
        [EmailAddress(ErrorMessage = "Invalid Email Address. (အီးမေးလ် မှားနေပါသည်)")]
        public string ContactEmail { get; set; }

        [Required]

        [StringLength(512)]
        [Display(Name = "Myanmar Address (မြန်မာ နေရပ်လိပ်စာ)*:")]
        public string HomeAddr1 { get; set; }

        [StringLength(512)]
        public string HomeAddr2 { get; set; }

        [StringLength(128)]
        [Display(Name = "Myanmar Phone (မြန်မာ ဖုန်း):")]
        public string HomePhone { get; set; }
        public int AppointmentStatus { get; set; }

        [Required]
        [StringLength(512)]
        public string ActivationCode { get; set; }

        public string Note { get; set; }
    }
}