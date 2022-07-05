using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace HospitalProject_Group3.Models
{
    public enum ShiftDays
    {
        Monday,
        Tuesday,
        Wednesday,
        Thursday,
        Friday,
        Saturday,
        Sunday
    }

    public enum ShiftTimes
    {
        Morning,
        Afternoon,
        Evening,
        Midnight
    }


    public class Shift
    {
        [Key]
        public int ShiftID { get; set; }
        public ShiftDays ShiftDay { get; set; }
        public ShiftTimes ShiftTime { get; set; }


        //A Shift can be taken by many Staffs.
        public ICollection<Staff> Staffs { get; set; }
    }

    public class ShiftDto
    {
        public int ShiftID { get; set; }
        public ShiftDays ShiftDay { get; set; }
        public ShiftTimes ShiftTime { get; set; }

    }

}