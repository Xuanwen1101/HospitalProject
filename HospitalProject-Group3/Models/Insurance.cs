using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HospitalProject_Group3.Models
{
    public class Insurance
    {
        [Key]
        public int InsuranceID { get; set; }
        public string InsuranceCompany { get; set; }
        public string InsurancePlan { get; set; }


        //A Insurance can be bought by many Patients.
        public ICollection<Patient> Patients { get; set; }
    }

    public class InsuranceDto
    {
        public int InsuranceID { get; set; }
        public string InsuranceCompany { get; set; }
        public string InsurancePlan { get; set; }

    }
}