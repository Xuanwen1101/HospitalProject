using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalProject_Group3.Models
{
    public class DonorTransplant
    {
        [Key]
        public int DonorTransplantID { get; set; }
        public string OrganType { get; set; }

        [AllowHtml]
        public string SurgeryPlan { get; set; }
        public string WaitListNumber { get; set; }

        //A Donor belongs to one Patient.
        [ForeignKey("Patient")]
        public int PatientID { get; set; }
        public virtual Patient Patient { get; set; }
    }

    public class DonorTransplantDto
    {
        public int DonorTransplantID { get; set; }
        public string OrganType { get; set; }

        [AllowHtml]
        public string SurgeryPlan { get; set; }
        public string WaitListNumber { get; set; }


        public int PatientID { get; set; }
        public string PatientFName { get; set; }
        public string PatientLName { get; set; }
    }
}