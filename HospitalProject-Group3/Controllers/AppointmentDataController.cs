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
    public class AppointmentDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/AppointmentData/ListAppointment
        [HttpGet]
        public IEnumerable<AppointmentDto> ListAppointments()
        {
            List<Appointment> Appointments = db.Appointments.ToList();
            List<AppointmentDto> AppointmentDtos = new List<AppointmentDto>();

            Appointments.ForEach(a => AppointmentDtos.Add(new AppointmentDto()
            {
                AppointmentID = a.AppointmentID,
                AppointmentDateTime = a.AppointmentDateTime,
                StaffID = a.Staff.StaffID,
                StaffFName = a.Staff.StaffFName,
                StaffLName = a.Staff.StaffLName,
                PatientID = a.Patient.PatientID,
                PatientFName = a.Patient.PatientFName,
                PatientLName = a.Patient.PatientLName

            }));


            return AppointmentDtos;
        }

        /// <summary>
        /// Gathers information about all Appointments related to the selected Staff ID
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all Appointments in the database matched with the selected Staff ID
        /// </returns>
        /// <param name="id">Staff ID.</param>
        /// <example>
        /// GET: api/AppointmentData/ListAppointmentsForStaff/3
        /// </example>
        [HttpGet]
        [ResponseType(typeof(AppointmentDto))]
        public IHttpActionResult ListAppointmentsForStaff(int id)
        {
            List<Appointment> Appointments = db.Appointments.Where(m => m.StaffID == id).ToList();
            List<AppointmentDto> AppointmentDtos = new List<AppointmentDto>();

            Appointments.ForEach(a => AppointmentDtos.Add(new AppointmentDto()
            {
                AppointmentID = a.AppointmentID,
                AppointmentDateTime = a.AppointmentDateTime,
                PatientID = a.Patient.PatientID,
                PatientFName = a.Patient.PatientFName,
                PatientLName = a.Patient.PatientLName
            }));

            return Ok(AppointmentDtos);
        }

        // GET: api/AppointmentData/FindAppointment/5
        [ResponseType(typeof(Appointment))]
        [HttpGet]
        public IHttpActionResult FindAppointment(int id)
        {
            Appointment Appointment = db.Appointments.Find(id);
            AppointmentDto AppointmentDto = new AppointmentDto()
            {
                AppointmentID = Appointment.AppointmentID,
                AppointmentDateTime = Appointment.AppointmentDateTime,
                StaffID = Appointment.Staff.StaffID,
                StaffFName = Appointment.Staff.StaffFName,
                StaffLName = Appointment.Staff.StaffLName,
                PatientID = Appointment.Patient.PatientID,
                PatientFName = Appointment.Patient.PatientFName,
                PatientLName = Appointment.Patient.PatientLName

            };

            if (Appointment == null)
            {
                return NotFound();
            }

            return Ok(AppointmentDto);
        }

        // PUT: api/AppointmentData/UpdateAppointment/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateAppointment(int id, Appointment appointment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != appointment.AppointmentID)
            {
                return BadRequest();
            }

            db.Entry(appointment).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AppointmentExists(id))
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

        // POST: api/AppointmentData/AddAppointment
        [ResponseType(typeof(Appointment))]
        [HttpPost]
        public IHttpActionResult AddAppointment(Appointment appointment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Appointments.Add(appointment);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = appointment.AppointmentID }, appointment);
        }

        // POST: api/AppointmentData/DeleteAppointment/5
        [ResponseType(typeof(Appointment))]
        [HttpPost]
        public IHttpActionResult DeleteAppointment(int id)
        {
            Appointment appointment = db.Appointments.Find(id);
            if (appointment == null)
            {
                return NotFound();
            }

            db.Appointments.Remove(appointment);
            db.SaveChanges();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AppointmentExists(int id)
        {
            return db.Appointments.Count(e => e.AppointmentID == id) > 0;
        }
    }
}