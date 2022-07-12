using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalProject_Group3.Models.ViewModels
{
    public class UpdateShift
    {
        public ShiftDto SelectedShift { get; set; }
        public String[] ShiftDayOptions = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
        public string[] ShiftTimeOptions = { "Morning", "Afternoon", "Evening", "Midnight" };
    }
}