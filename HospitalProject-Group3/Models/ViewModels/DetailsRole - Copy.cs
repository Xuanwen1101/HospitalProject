using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalProject_Group3.Models.ViewModels
{
    public class DetailsRole
    {
        public RoleDto SelectedRole { get; set; }
        //all of staffs that worked as the selected Role
        public IEnumerable<StaffDto> WorkedStaffs { get; set; }
    }
}