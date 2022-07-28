using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalProject_Group3.Models.ViewModels
{
    public class UpdateBill
    {
        public BillDto SelectedBill { get; set; }

        public IEnumerable<PatientDto> PatientOptions { get; set; }
    }
}