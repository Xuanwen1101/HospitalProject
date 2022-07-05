using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalProject_Group3.Models
{
    public class Staff
    {
        [Key]
        public int StaffID { get; set; }
        public string StaffFName { get; set; }
        public string StaffLName { get; set; }

        public string StaffBio { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public DateTime HireDate { get; set; }

        //data needed for keeping track of Staff images uploaded
        //images deposited into /Content/Images/Staffs/{id}.{extension}
        public bool StaffHasPhoto { get; set; }
        public string PicExtension { get; set; }

        //A Staff can have one Role.
        [ForeignKey("Role")]
        public int RoleID { get; set; }
        public virtual Role Role { get; set; }

        //A Staff can have many Shifts.
        public ICollection<Shift> Shifts { get; set; }
        //A Staff can have many Appointments.
        public ICollection<Appointment> Appointments { get; set; }
        //A Staff can create many Prescriptions.
        public ICollection<Prescription> Prescriptions { get; set; }
    }


    public class StaffDto
    {
        public int StaffID { get; set; }
        public string StaffFName { get; set; }
        public string StaffLName { get; set; }

        public string StaffBio { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public DateTime HireDate { get; set; }

        //data needed for keeping track of Staff images uploaded
        //images deposited into /Content/Images/Staffs/{id}.{extension}
        public bool StaffHasPhoto { get; set; }
        public string PicExtension { get; set; }


    }
}