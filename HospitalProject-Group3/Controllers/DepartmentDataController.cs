using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using HospitalProject_Group3.Models;

namespace HospitalProject_Group3.Controllers
{
    public class DepartmentDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns all Departments in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all Departments in the database.
        /// </returns>
        /// <example>
        /// GET: api/DepartmentData/ListDepartments
        /// </example>
        [HttpGet]
        [ResponseType(typeof(DepartmentDto))]
        public IHttpActionResult ListDepartments()
        {
            List<Department> Departments = db.Departments.ToList();
            List<DepartmentDto> DepartmentDtos = new List<DepartmentDto>();

            Departments.ForEach(r => DepartmentDtos.Add(new DepartmentDto()
            {
                DepartmentID = r.DepartmentID,
                DepartmentType = r.DepartmentType,
                DepartmentID = r.Department.DepartmentID,
                DepartmentName = r.Department.DepartmentName
            }));


            return Ok(DepartmentDtos);

        }

        /// <summary>
        /// Gathers information about all Departments related to the selected Department ID
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all Departments in the database matched with the selected Department ID
        /// </returns>
        /// <param name="id">Department ID.</param>
        /// <example>
        /// GET: api/DepartmentData/ListDepartmentsForDepartment/3
        /// </example>
        [HttpGet]
        [ResponseType(typeof(DepartmentDto))]
        public IHttpActionResult ListDepartmentsForDepartment(int id)
        {
            List<Department> Departments = db.Departments.Where(m => m.DepartmentID == id).ToList();
            List<DepartmentDto> DepartmentDtos = new List<DepartmentDto>();

            Departments.ForEach(r => DepartmentDtos.Add(new DepartmentDto()
            {
                DepartmentID = r.DepartmentID,
                DepartmentName = r.DepartmentName,
                RoomNumber = r.RoomNumber,
                RoleType = r.Role.RoleType
            }));

            return Ok(DepartmentDtos);
        }


        /// <summary>
        /// Returns the Department info for the given Department id.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: A Department in the system matching up to the Department ID primary key
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <param name="id">The primary key of the Department</param>
        /// <example>
        /// GET: api/DepartmentData/FindDepartment/5
        /// </example>
        [ResponseType(typeof(Department))]
        [HttpGet]
        public IHttpActionResult FindDepartment(int id)
        {
            Department Department = db.Departments.Find(id);
            DepartmentDto DepartmentDto = new DepartmentDto()
            {
                DepartmentID = r.DepartmentID,
                DepartmentName = r.DepartmentName,
                RoomNumber = r.RoomNumber,
                RoleType = r.Role.RoleType
            };
            if (Department == null)
            {
                return NotFound();
            }

            return Ok(DepartmentDto);
        }


        /// <summary>
        /// Updates a particular Department in the system with POST Data input
        /// </summary>
        /// <param name="id">Represents the Department ID primary key</param>
        /// <param name="Department">JSON FORM DATA of an Department</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// PUT: api/DepartmentData/UpdateDepartment/5
        /// FORM DATA: Department JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        /*[Authorize]*/
        public IHttpActionResult UpdateDepartment(int id, Department department)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != department.DepartmentID)
            {
                return BadRequest();
            }

            db.Entry(department).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DepartmentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }


        /// <summary>
        /// Adds an Department to the system
        /// </summary>
        /// <param name="Department">JSON FORM DATA of an Department</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: Department ID, Department Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/DepartmentData/AddDepartment
        /// FORM DATA: Department JSON Object
        /// </example>
        [ResponseType(typeof(Department))]
        [HttpPost]
        /*[Authorize]*/
        public IHttpActionResult AddDepartment(Department department)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Departments.Add(department);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = department.DepartmentID }, department);
        }


        /// <summary>
        /// Deletes an Department from the system by it's ID.
        /// </summary>
        /// <param name="id">The primary key of the Department</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// DELETE: api/DepartmentData/DeleteDepartment/5
        /// FORM DATA: (empty)
        /// </example>
        [ResponseType(typeof(Department))]
        [HttpPost]
        /*[Authorize]*/
        public IHttpActionResult DeleteDepartment(int id)
        {
            Department department = db.Departments.Find(id);
            if (department == null)
            {
                return NotFound();
            }

            db.Departments.Remove(department);
            db.SaveChanges();

            return Ok(department);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DepartmentExists(int id)
        {
            return db.Departments.Count(e => e.DepartmentID == id) > 0;
        }
    }
}