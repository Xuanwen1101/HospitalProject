using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalProject_Group3.Models.ViewModels
{
    public class DetailsShift
    {
        public ShiftDto SelectedShift { get; set; }

        //all of Staffs that Worked at the selected Shift
        public IEnumerable<StaffDto> WorkedStaffs { get; set; }
    }
}