using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalProject_Group3.Models.ViewModels
{
    public class UpdateDepartment
    {
        public DepartmentDto SelectedDepartment { get; set; }

        public IEnumerable<DepartmentDto> DepartmentOptions { get; set; }

    }
}