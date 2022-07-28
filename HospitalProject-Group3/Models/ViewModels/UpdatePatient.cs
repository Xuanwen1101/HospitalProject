using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalProject_Group3.Models.ViewModels
{
    public class UpdatePatient
    {
        public PatientDto SelectedPatient { get; set; }

        public IEnumerable<InsuranceDto> InsuranceOptions { get; set; }
    }
}