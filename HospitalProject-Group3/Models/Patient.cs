using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace HospitalProject_Group3.Models
{
    public enum Genders
    {
        Female,
        Male,
        [Description("Prefer Not To Tell")]
        PreferNotToTell,
    }

    public class Patient
    {
        [Key]
        public int PatientID { get; set; }
        public string PatientFName { get; set; }
        public string PatientLName { get; set; }

        public Genders Gender { get; set; }
        public string PhoneNumber { get; set; }
        public string BloodType { get; set; }
        public string Email { get; set; }
        public DateTime BornDate { get; set; }

        //data needed for keeping track of Patient images uploaded
        //images deposited into /Content/Images/Patients/{id}.{extension}
        public bool PatientHasPhoto { get; set; }
        public string PicExtension { get; set; }

        //A Patient can have one Insurance.
        [ForeignKey("Insurance")]
        public int InsuranceID { get; set; }
        public virtual Insurance Insurance { get; set; }

        //A Patient can have many Bills.
        public ICollection<Bill> Bills { get; set; }
        //A Patient can make many Appointments.
        public ICollection<Appointment> Appointments { get; set; }
        //A Patient can have many Prescriptions.
        public ICollection<Prescription> Prescriptions { get; set; }
        //A Patient can request many DonorTransplants.
        public ICollection<DonorTransplant> DonorTransplants { get; set; }
        //A Patient can have many MedicalRecord.
        public ICollection<MedicalRecord> MedicalRecords { get; set; }
    }


    public class PatientDto
    {
        public int PatientID { get; set; }
        public string PatientFName { get; set; }
        public string PatientLName { get; set; }

        public Genders Gender { get; set; }
        public string PhoneNumber { get; set; }
        public string BloodType { get; set; }
        public string Email { get; set; }
        public DateTime BornDate { get; set; }

        //data needed for keeping track of Patient images uploaded
        //images deposited into /Content/Images/Patients/{id}.{extension}
        public bool PatientHasPhoto { get; set; }
        public string PicExtension { get; set; }


        public int InsuranceID { get; set; }
        public string InsuranceCompany { get; set; }

    }
}