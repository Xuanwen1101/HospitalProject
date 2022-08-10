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
    public class RoleDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns all Roles in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all Roles in the database.
        /// </returns>
        /// <example>
        /// GET: api/RoleData/ListRoles
        /// </example>
        [HttpGet]
        [ResponseType(typeof(RoleDto))]
        public IHttpActionResult ListRoles()
        {
            List<Role> Roles = db.Roles.ToList();
            List<RoleDto> RoleDtos = new List<RoleDto>();

            Roles.ForEach(r => RoleDtos.Add(new RoleDto()
            {
                RoleID = r.RoleID,
                RoleType = r.RoleType,
                DepartmentID = r.Department.DepartmentID,
                DepartmentName = r.Department.DepartmentName
            }));


            return Ok(RoleDtos);

        }

        /// <summary>
        /// Gathers information about all Roles related to the selected Department ID
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all Roles in the database matched with the selected Department ID
        /// </returns>
        /// <param name="id">Department ID.</param>
        /// <example>
        /// GET: api/RoleData/ListRolesForDepartment/3
        /// </example>
        [HttpGet]
        [ResponseType(typeof(RoleDto))]
        public IHttpActionResult ListRolesForDepartment(int id)
        {
            List<Role> Roles = db.Roles.Where(m => m.DepartmentID == id).ToList();
            List<RoleDto> RoleDtos = new List<RoleDto>();

            Roles.ForEach(r => RoleDtos.Add(new RoleDto()
            {
                RoleID = r.RoleID,
                RoleType = r.RoleType,
                DepartmentID = r.Department.DepartmentID,
                DepartmentName = r.Department.DepartmentName
            }));

            return Ok(RoleDtos);
        }


        /// <summary>
        /// Returns the Role info for the given Role id.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: An Role in the system matching up to the Role ID primary key
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <param name="id">The primary key of the Role</param>
        /// <example>
        /// GET: api/RoleData/FindRole/5
        /// </example>
        [ResponseType(typeof(Role))]
        [HttpGet]
        public IHttpActionResult FindRole(int id)
        {
            Role Role = db.Roles.Find(id);
            RoleDto RoleDto = new RoleDto()
            {
                RoleID = Role.RoleID,
                RoleType = Role.RoleType,
                DepartmentID = Role.Department.DepartmentID,
                DepartmentName = Role.Department.DepartmentName
            };
            if (Role == null)
            {
                return NotFound();
            }

            return Ok(RoleDto);
        }


        /// <summary>
        /// Updates a particular Role in the system with POST Data input
        /// </summary>
        /// <param name="id">Represents the Role ID primary key</param>
        /// <param name="Role">JSON FORM DATA of an Role</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// PUT: api/RoleData/UpdateRole/5
        /// FORM DATA: Role JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        [Authorize]
        public IHttpActionResult UpdateRole(int id, Role role)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != role.RoleID)
            {
                return BadRequest();
            }

            db.Entry(role).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoleExists(id))
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
        /// Adds an Role to the system
        /// </summary>
        /// <param name="Role">JSON FORM DATA of an Role</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: Role ID, Role Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/RoleData/AddRole
        /// FORM DATA: Role JSON Object
        /// </example>
        [ResponseType(typeof(Role))]
        [HttpPost]
        [Authorize]
        public IHttpActionResult AddRole(Role role)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Roles.Add(role);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = role.RoleID }, role);
        }


        /// <summary>
        /// Deletes an Role from the system by it's ID.
        /// </summary>
        /// <param name="id">The primary key of the Role</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// DELETE: api/RoleData/DeleteRole/5
        /// FORM DATA: (empty)
        /// </example>
        [ResponseType(typeof(Role))]
        [HttpPost]
        [Authorize]
        public IHttpActionResult DeleteRole(int id)
        {
            Role role = db.Roles.Find(id);
            if (role == null)
            {
                return NotFound();
            }

            db.Roles.Remove(role);
            db.SaveChanges();

            return Ok(role);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RoleExists(int id)
        {
            return db.Roles.Count(e => e.RoleID == id) > 0;
        }
    }
}