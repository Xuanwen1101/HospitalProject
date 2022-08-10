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
    public class ShiftDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns all Shifts in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all Shifts in the database.
        /// </returns>
        /// <example>
        /// GET: api/ShiftData/ListShifts
        /// </example>
        [HttpGet]
        [ResponseType(typeof(ShiftDto))]
        public IHttpActionResult ListShifts()
        {
            List<Shift> Shifts = db.Shifts.ToList();
            List<ShiftDto> ShiftDtos = new List<ShiftDto>();

            Shifts.ForEach(s => ShiftDtos.Add(new ShiftDto()
            {
                ShiftID = s.ShiftID,
                ShiftDay = s.ShiftDay,
                ShiftTime = s.ShiftTime
            }));

            return Ok(ShiftDtos);
        }


        /// <summary>
        /// Returns all Shifts in the system associated with the selected Staff.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all Shifts in the database showing the selected Staff.
        /// </returns>
        /// <param name="id">Staff Primary Key</param>
        /// <example>
        /// GET: api/ShiftData/ListShiftsWorkingForStaff/1
        /// </example>
        [HttpGet]
        [ResponseType(typeof(ShiftDto))]
        public IHttpActionResult ListShiftsWorkingForStaff(int id)
        {
            List<Shift> Shifts = db.Shifts.Where(
                c => c.Staffs.Any(
                    m => m.StaffID == id)
                ).ToList();
            List<ShiftDto> ShiftDtos = new List<ShiftDto>();

            Shifts.ForEach(s => ShiftDtos.Add(new ShiftDto()
            {
                ShiftID = s.ShiftID,
                ShiftDay = s.ShiftDay,
                ShiftTime = s.ShiftTime
            }));

            return Ok(ShiftDtos);
        }


        /// <summary>
        /// Returns Shifts in the system not working for the selected Staff.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all Shifts in the database not working the selected Staff.
        /// </returns>
        /// <param name="id">Staff Primary Key</param>
        /// <example>
        /// GET: api/ShiftData/ListShiftsNotWorkingForStaff/1
        /// </example>
        [HttpGet]
        [ResponseType(typeof(ShiftDto))]
        public IHttpActionResult ListShiftsNotWorkingForStaff(int id)
        {
            List<Shift> Shifts = db.Shifts.Where(
                c => !c.Staffs.Any(
                    m => m.StaffID == id)
                ).ToList();
            List<ShiftDto> ShiftDtos = new List<ShiftDto>();

            Shifts.ForEach(s => ShiftDtos.Add(new ShiftDto()
            {
                ShiftID = s.ShiftID,
                ShiftDay = s.ShiftDay,
                ShiftTime = s.ShiftTime
            }));

            return Ok(ShiftDtos);
        }


        /// <summary>
        /// Returns the Shift info for the given Shift id.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: An Shift in the system matching up to the Shift ID primary key
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <param name="id">The primary key of the Shift</param>
        /// <example>
        /// GET: api/ShiftData/FindShift/5
        /// </example>
        [ResponseType(typeof(Shift))]
        [HttpGet]
        public IHttpActionResult FindShift(int id)
        {
            Shift Shift = db.Shifts.Find(id);
            ShiftDto ShiftDto = new ShiftDto()
            {
                ShiftID = Shift.ShiftID,
                ShiftDay = Shift.ShiftDay,
                ShiftTime = Shift.ShiftTime
            };
            if (Shift == null)
            {
                return NotFound();
            }

            return Ok(ShiftDto);
        }


        /// <summary>
        /// Updates a particular Shift in the system with POST Data input
        /// </summary>
        /// <param name="id">Represents the Shift ID primary key</param>
        /// <param name="Shift">JSON FORM DATA of an Shift</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// PUT: api/ShiftData/UpdateShift/5
        /// FORM DATA: Shift JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        [Authorize]
        public IHttpActionResult UpdateShift(int id, Shift shift)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != shift.ShiftID)
            {
                return BadRequest();
            }

            db.Entry(shift).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ShiftExists(id))
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
        /// Adds an Shift to the system
        /// </summary>
        /// <param name="Shift">JSON FORM DATA of an Shift</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: Shift ID, Shift Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/ShiftData/AddShift
        /// FORM DATA: Shift JSON Object
        /// </example>
        [ResponseType(typeof(Shift))]
        [HttpPost]
        [Authorize]
        public IHttpActionResult AddShift(Shift shift)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Shifts.Add(shift);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = shift.ShiftID }, shift);
        }


        /// <summary>
        /// Deletes an Shift from the system by it's ID.
        /// </summary>
        /// <param name="id">The primary key of the Shift</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// DELETE: api/ShiftData/DeleteShift/5
        /// FORM DATA: (empty)
        /// </example>
        [ResponseType(typeof(Shift))]
        [HttpPost]
        [Authorize]
        public IHttpActionResult DeleteShift(int id)
        {
            Shift shift = db.Shifts.Find(id);
            if (shift == null)
            {
                return NotFound();
            }

            db.Shifts.Remove(shift);
            db.SaveChanges();

            return Ok(shift);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ShiftExists(int id)
        {
            return db.Shifts.Count(e => e.ShiftID == id) > 0;
        }
    }
}