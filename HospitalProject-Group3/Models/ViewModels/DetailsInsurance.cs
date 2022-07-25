using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalProject_Group3.Models.ViewModels
{
    public class DetailsInsurance
    {
        public InsuranceDto SelectedInsurance { get; set; }

        public IEnumerable<PatientDto> ListPatient { get; set; }
    }
}