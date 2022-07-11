using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalProject_Group3.Models.ViewModels
{
    public class UpdateStaff
    {
        public StaffDto SelectedStaff { get; set; }

        public IEnumerable<RoleDto> RoleOptions { get; set; }
    }
}