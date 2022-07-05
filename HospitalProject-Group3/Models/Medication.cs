using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HospitalProject_Group3.Models
{
    public class Medication
    {
        [Key]
        public int MedicationID { get; set; }
        public string MedicationName { get; set; }
        public string MedicationBrand { get; set; }
        public string MedicationDetail { get; set; }
        public decimal Price { get; set; }


        //A Insurance can be used in many Prescriptions.
        public ICollection<Patient> Patients { get; set; }
    }

    public class MedicationDto
    {
        public int MedicationID { get; set; }
        public string MedicationName { get; set; }
        public string MedicationBrand { get; set; }
        public string MedicationDetail { get; set; }
        public decimal Price { get; set; }

    }
}