using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalProject_Group3.Models.ViewModels
{
    public class DetailsDepartment
    {
        public DepartmentDto SelectedDepartment { get; set; }
        //all staff members that are working within the selected Department
        public IEnumerable<StaffDto> DeptStaff { get; set; }

    }
}