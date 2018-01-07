using System;
using System.ComponentModel.DataAnnotations;

namespace EmWebApp.Domain
{
    public class ConsularAppointment
    {
        public int Id { get; set; }
        public DateTime? FormSubmissionDate { get; set; } = DateTime.Today;

        [Required]
        public DateTime? AppointmentDate { get; set; }
        public int AppointmentType { get; set; }        
        public int QueueNumber { get; set; }

        [Required]
        [StringLength(128)]
        public string Name { get; set; }

        [Required]
        [StringLength(1)]
        public string Gender { get; set; }

        [Required]
        public DateTime? DateOfBirth { get; set; }

        [Required]
        [StringLength(128)]
        public string PlaceOfBirth { get; set; }

        [StringLength(2)]
        public string Nationality { get; set; }

        [Required]
        [StringLength(128)]
        public string NRIC_No { get; set; }

        [Required]
        [StringLength(128)]
        public string PassportNumber { get; set; }

        [Required]
        public DateTime? PassportIssuedDate { get; set; }

        [StringLength(128)]
        public string ConsulateLocation { get; set; }

        [StringLength(128)]
        public string StayPermitNumber { get; set; }
        public int StayType { get; set; }

        [StringLength(128)]
        public string EmployerName { get; set; }

        [StringLength(128)]
        public string Occupation { get; set; }

        [Required]
        [StringLength(512)]
        public string ContactAddr1 { get; set; }

        [StringLength(512)]
        public string ContactAddr2 { get; set; }

        [Required]

        [StringLength(128)]
        public string ContactPhone { get; set; }

        [Required]

        [StringLength(128)]
        public string ContactEmail { get; set; }

        [Required]

        [StringLength(512)]
        public string HomeAddr1 { get; set; }

        [StringLength(512)]
        public string HomeAddr2 { get; set; }

        [StringLength(128)]
        public string HomePhone { get; set; }        
        public int AppointmentStatus { get; set; }

        [Required]
        [StringLength(512)]
        public string ActivationCode { get; set; } = Guid.NewGuid().ToString();
        public string Note { get; set; }
    }
    
}