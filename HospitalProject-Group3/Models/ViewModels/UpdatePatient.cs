using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalProject_Group3.Models.ViewModels
{
    public class UpdatePatient
    {
        public DonorTransplantDto DonorTransplantID { get; set; }

        public IEnumerable<DonorTransplantDto> Options { get; set; }
    }
}