using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalProject_Group3.Models.ViewModels
{
    public class DonorTransplant
    {
        public ShiftDto SelectedDonorTransplantID { get; set; }

        public IEnumerable< DonorTransplantDto> DonorTransplantDto { get; set; }     

    }
}