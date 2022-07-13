using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalProject_Group3.Models.ViewModels
{
    public class DetailsMedication
    {
        public MedicationDto SelectedMedication { get; set; }
        //all of staffs that worked as the selected Role
        //public IEnumerable<PatientDto> WorkedStaffs { get; set; }
    }
}