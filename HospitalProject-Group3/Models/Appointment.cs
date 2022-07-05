using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace HospitalProject_Group3.Models
{
    public class Appointment
    {
        [Key]
        public int AppointmentID { get; set; }
        public DateTime AppointmentDateTime { get; set; }

        //A Appointment belongs to one Staff.
        [ForeignKey("Staff")]
        public int StaffID { get; set; }
        public virtual Staff Staff { get; set; }

        //A Appointment belongs to one Patient.
        [ForeignKey("Patient")]
        public int PatientID { get; set; }
        public virtual Patient Patient { get; set; }
    }

    public class AppointmentDto
    {
        public int AppointmentID { get; set; }
        public DateTime AppointmentDateTime { get; set; }


        public int StaffID { get; set; }
        public string StaffFName { get; set; }
        public string StaffLName { get; set; }


        public int PatientID { get; set; }
        public string PatientFName { get; set; }
        public string PatientLName { get; set; }
    }
}