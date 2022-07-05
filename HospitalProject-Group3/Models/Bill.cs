using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalProject_Group3.Models
{
    public class Bill
    {
        [Key]
        public int BillID { get; set; }
        public string BillDetail { get; set; }
        public decimal TotalCost { get; set; }

        //A Bill belongs to one Patient.
        [ForeignKey("Patient")]
        public int PatientID { get; set; }
        public virtual Patient Patient { get; set; }
    }

    public class BillDto
    {
        public int BillID { get; set; }
        public string BillDetail { get; set; }
        public decimal TotalCost { get; set; }


        public int PatientID { get; set; }
        public virtual Patient Patient { get; set; }
        public string PatientFName { get; set; }
        public string PatientLName { get; set; }
    }
}