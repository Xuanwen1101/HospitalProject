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
    public class PrescriptionDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns all Prescriptions in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all Prescriptions in the database.
        /// </returns>
        /// <example>
        /// GET: api/PrescriptionData/ListPrescriptions
        /// </example>
        [HttpGet]
        [ResponseType(typeof(PrescriptionDto))]
        public IHttpActionResult ListPrescriptions()
        {
            List<Prescription> Prescriptions = db.Prescriptions.ToList();
            List<PrescriptionDto> PrescriptionDtos = new List<PrescriptionDto>();

            Prescriptions.ForEach(r => PrescriptionDtos.Add(new PrescriptionDto()
            {
                PrescriptionID = r.PrescriptionID,
                PrescriptionDate = r.PrescriptionDate,
                DoctorNote = r.DoctorNote,
                StaffID = r.Staff.StaffID,
                StaffFName = r.Staff.StaffFName,
                StaffLName = r.Staff.StaffLName,
                PatientID = r.Patient.PatientID,
                PatientFName = r.Patient.PatientFName,
                PatientLName = r.Patient.PatientLName,
                MedicationID = r.Medication.MedicationID,
                MedicationName = r.Medication.MedicationName
            }));


            return Ok(PrescriptionDtos);

        }


        /// <summary>
        /// Gathers information about all Prescriptions related to the selected Patient ID
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all Prescriptions in the database matched with the selected Patient ID
        /// </returns>
        /// <param name="id">Patient ID.</param>
        /// <example>
        /// GET: api/PrescriptionData/ListPrescriptionsForPatient/3
        /// </example>
        [HttpGet]
        [ResponseType(typeof(PrescriptionDto))]
        public IHttpActionResult ListPrescriptionsForPatient(int id)
        {
            List<Prescription> Prescriptions = db.Prescriptions.Where(m => m.PatientID == id).ToList();
            List<PrescriptionDto> PrescriptionDtos = new List<PrescriptionDto>();

            Prescriptions.ForEach(r => PrescriptionDtos.Add(new PrescriptionDto()
            {
                PrescriptionID = r.PrescriptionID,
                PrescriptionDate = r.PrescriptionDate,
                DoctorNote = r.DoctorNote,
                StaffID = r.Staff.StaffID,
                StaffFName = r.Staff.StaffFName,
                StaffLName = r.Staff.StaffLName,
                PatientID = r.Patient.PatientID,
                PatientFName = r.Patient.PatientFName,
                PatientLName = r.Patient.PatientLName,
                MedicationID = r.Medication.MedicationID,
                MedicationName = r.Medication.MedicationName
            }));

            return Ok(PrescriptionDtos);
        }


        /// <summary>
        /// Returns the Prescription info for the given Medication ID.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: A Prescription in the system matching up to the Medication ID primary key
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <param name="id">The primary key of the Medication</param>
        /// <example>
        /// GET: api/PrescriptionData/FindPrescription/5
        /// </example>
        [ResponseType(typeof(Prescription))]
        [HttpGet]
        public IHttpActionResult FindMedication(int id)
        {
            Prescription Prescription = db.Prescriptions.Find(id);
            PrescriptionDto PrescriptionDto = new PrescriptionDto()
            {
                PrescriptionID = Prescription.PrescriptionID,
                PrescriptionDate = Prescription.PrescriptionDate,
                DoctorNote = Prescription.DoctorNote,
                StaffID = Prescription.Staff.StaffID,
                StaffFName = Prescription.Staff.StaffFName,
                StaffLName = Prescription.Staff.StaffLName,
                PatientID = Prescription.Patient.PatientID,
                PatientFName = Prescription.Patient.PatientFName,
                PatientLName = Prescription.Patient.PatientLName,
                MedicationID = Prescription.Medication.MedicationID,
                MedicationName = Prescription.Medication.MedicationName
            };
            if (Prescription == null)
            {
                return NotFound();
            }

            return Ok(PrescriptionDto);
        }


        /// <summary>
        /// Updates a particular Prescription in the system with POST Data input
        /// </summary>
        /// <param name="id">Represents the Prescription ID primary key</param>
        /// <param name="Prescription">JSON FORM DATA of an Prescription</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// PUT: api/PrescriptionData/UpdatePrescription/5
        /// FORM DATA: Prescription JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        /*[Authorize]*/
        public IHttpActionResult UpdatePrescription(int id, Prescription prescription)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != prescription.PrescriptionID)
            {
                return BadRequest();
            }

            db.Entry(prescription).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PrescriptionExists(id))
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
        /// Adds a Prescription to the system
        /// </summary>
        /// <param name="Prescription">JSON FORM DATA of an Prescription</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: Prescription ID, Prescription Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/PrescriptionData/AddPrescription
        /// FORM DATA: Prescription JSON Object
        /// </example>
        [ResponseType(typeof(Prescription))]
        [HttpPost]
        /*[Authorize]*/
        public IHttpActionResult AddPrescription(Prescription prescription)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Prescriptions.Add(prescription);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = prescription.PrescriptionID }, prescription);
        }


        /// <summary>
        /// Deletes a Prescription from the system by it's ID.
        /// </summary>
        /// <param name="id">The primary key of the Prescription</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// DELETE: api/PrescriptionData/DeletePrescription/5
        /// FORM DATA: (empty)
        /// </example>
        [ResponseType(typeof(Prescription))]
        [HttpPost]
        /*[Authorize]*/
        public IHttpActionResult DeletePrescription(int id)
        {
            Prescription prescription = db.Prescriptions.Find(id);
            if (prescription == null)
            {
                return NotFound();
            }

            db.Prescriptions.Remove(prescription);
            db.SaveChanges();

            return Ok(prescription);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PrescriptionExists(int id)
        {
            return db.Prescriptions.Count(e => e.PrescriptionID == id) > 0;
        }
    }
}