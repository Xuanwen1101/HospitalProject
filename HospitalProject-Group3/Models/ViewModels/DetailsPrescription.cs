using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalProject_Group3.Models.ViewModels
{
    public class DetailsPrescription
    {
        public PrescriptionDto SelectedPrescription { get; set; }
        //all patients that have been presribed medication
        public IEnumerable<PatientsDto> PrescribedPatients { get; set; }

    }
}