using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalProject_Group3.Models
{
    public class Role
    {
        [Key]
        public int RoleID { get; set; }
        public string RoleType { get; set; }

        //A Role can belong to one Department.
        [ForeignKey("Department")]
        public int DepartmentID { get; set; }
        public virtual Department Department { get; set; }

        //A Role can be taken by many Staffs.
        public ICollection<Staff> Staffs { get; set; }
    }


    public class RoleDto
    {

        public int RoleID { get; set; }
        public string RoleType { get; set; }


        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }

    }
}