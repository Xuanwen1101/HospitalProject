using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalProject_Group3.Models.ViewModels
{
    public class DetailsStaff
    {
        public StaffDto SelectedStaff { get; set; }

        //all of Shifts that are working the selected Staff
        public IEnumerable<ShiftDto> WorkingingShifts { get; set; }

        public IEnumerable<ShiftDto> AvailableShifts { get; set; }

        //all of Appointments that are had the selected Staff
        public IEnumerable<AppointmentDto> HadAppointments { get; set; }

        //all of Prescriptions that are created by the selected Staff
        public IEnumerable<PrescriptionDto> CreatedPrescriptions { get; set; }

    }
}