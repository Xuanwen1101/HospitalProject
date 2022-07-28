using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalProject_Group3.Models.ViewModels
{
    public class UpdatePrescription
    {
        public PrescriptionDto SelectedPrescription { get; set; }
        //Ability to change/update the prescribed medication for the selected prescription
        public IEnumerable<MedicationDto> MedicationOptions { get; set; }

    }
}