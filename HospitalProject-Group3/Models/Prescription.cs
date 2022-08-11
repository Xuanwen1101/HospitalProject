using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalProject_Group3.Models
{
    public class Prescription
    {
        [Key]
        public int PrescriptionID { get; set; }
        public DateTime PrescriptionDate { get; set; }

        [AllowHtml]
        public string DoctorNote { get; set; }

        //A Prescription is created by one Staff.
        [ForeignKey("Staff")]
        public int StaffID { get; set; }
        public virtual Staff Staff { get; set; }

        //A Prescription belongs to one Patient.
        [ForeignKey("Patient")]
        public int PatientID { get; set; }
        public virtual Patient Patient { get; set; }

        //A Prescription is included with one Medication.
        [ForeignKey("Medication")]
        public int MedicationID { get; set; }
        public virtual Medication Medication { get; set; }
    }

    public class PrescriptionDto
    {
        public int PrescriptionID { get; set; }
        public DateTime PrescriptionDate { get; set; }

        [AllowHtml]
        public string DoctorNote { get; set; }


        public int StaffID { get; set; }
        public string StaffFName { get; set; }
        public string StaffLName { get; set; }


        public int PatientID { get; set; }
        public string PatientFName { get; set; }
        public string PatientLName { get; set; }


        public int MedicationID { get; set; }
        public string MedicationName { get; set; }
    }
}