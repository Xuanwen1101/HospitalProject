using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalProject_Group3.Models.ViewModels
{
    public class UpdateAppointment
    {
        public AppointmentDto SelectedAppointment { get; set; }

        public IEnumerable<PatientDto> PatientOptions { get; set; }

        public IEnumerable<StaffDto> StaffOptions { get; set; }
    }
}