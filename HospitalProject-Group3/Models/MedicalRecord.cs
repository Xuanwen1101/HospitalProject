using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalProject_Group3.Models
{
    public class MedicalRecord
    {
        [Key]
        public int RecordID { get; set; }
        public string RecordDetail { get; set; }
        public DateTime RecordDate { get; set; }

        //A Record belongs to one Patient.
        [ForeignKey("Patient")]
        public int PatientID { get; set; }
        public virtual Patient Patient { get; set; }

    }

    public class MedicalRecordDto
    {
        public int RecordID { get; set; }
        public string RecordDetail { get; set; }
        public DateTime RecordDate { get; set; }


        public int PatientID { get; set; }
        public string PatientFName { get; set; }
        public string PatientLName { get; set; }

    }
}