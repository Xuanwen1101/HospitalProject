using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalProject_Group3.Models.ViewModels
{
    public class DetailsMedication
    {
        public MedicationDto SelectedMedication { get; set; }
        public IEnumerable<Prescription> ListPrescription { get; set; }
    }
}