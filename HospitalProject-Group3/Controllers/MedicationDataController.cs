using System;
using System.IO;
using System.Web;
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
using System.Diagnostics;

namespace HospitalProject_Group3.Controllers
{
    public class MedicationDataController : ApiController
    {

        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns all Medication in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all Medication in the database.
        /// </returns>
        /// <example>
        /// GET: api/MedicationData/ListMedications
        /// </example>
        [HttpGet]
        [ResponseType(typeof(MedicationDto))]
        public IHttpActionResult ListMedications()
        {
            List<Medication> Medications = db.Medications.ToList();
            List<MedicationDto> MedicationDtos = new List<MedicationDto>();

            Medications.ForEach(m => MedicationDtos.Add(new MedicationDto()
            {
                MedicationID= m.MedicationID,
                MedicationName= m.MedicationName,
                MedicationBrand= m.MedicationBrand,
                MedicationDetail= m.MedicationDetail,
                Price=m.Price
            }));


            return Ok(MedicationDtos);

        }

        /// <summary>
        /// Returns the Medication info for the given Role id.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: An Medication in the system matching up to the Medication ID primary key
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <param name="id">The primary key of the Medication</param>
        /// <example>
        /// GET: api/MedicationData/FindMedication/5
        /// </example>
        [ResponseType(typeof(Medication))]
        [HttpGet]
        public IHttpActionResult FindMedication(int id)
        {
            Medication Medication = db.Medications.Find(id);
            MedicationDto MedicationDto = new MedicationDto()
            {
                MedicationID = Medication.MedicationID,
                MedicationBrand= Medication.MedicationBrand,
                MedicationName = Medication.MedicationName,
                MedicationDetail = Medication.MedicationDetail,
                Price = Medication.Price
            };
            if (Medication == null)
            {
                return NotFound();
            }

            return Ok(MedicationDto);
        }

        /// <summary>
        /// Adds a Medication to the system
        /// </summary>
        /// <param name="Medication">JSON FORM DATA of a Medication</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: Medication ID, Medication Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/MedicationData/AddMedication
        /// FORM DATA: Medication JSON Object
        /// </example>
        [ResponseType(typeof(Medication))]
        [HttpPost]
        /*[Authorize]*/
        public IHttpActionResult AddMedication(Medication medication)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Medications.Add(medication);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = medication.MedicationID }, medication);
        }

        /// <summary>
        /// Updates the selected Medication in the system with POST Data input
        /// </summary>
        /// <param name="id">Represents the Medication ID primary key</param>
        /// <param name="Medication">JSON FORM DATA of an Medication</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// PUT: api/MedicationData/UpdateMedication/5
        /// FORM DATA: Medication JSON Object
        /// </example>
        [HttpPost]
        [ResponseType(typeof(void))]
        /*[Authorize]*/
        public IHttpActionResult UpdateMedication(int id, Medication medication)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != medication.MedicationID)
            {
                return BadRequest();
            }

            db.Entry(medication).State = EntityState.Modified;
            
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MedicationExists(id))
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
        /// Deletes an Medication from the system by it's ID.
        /// </summary>
        /// <param name="id">The primary key of the Medication</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// DELETE: api/MedicationData/DeleteMedication/5
        /// FORM DATA: (empty)
        /// </example>
        [ResponseType(typeof(Medication))]
        [HttpPost]
        /*[Authorize]*/
        public IHttpActionResult DeleteMedication(int id)
        {
            Medication medication = db.Medications.Find(id);
            if (medication == null)
            {
                return NotFound();
            }

            db.Medications.Remove(medication);
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

        private bool MedicationExists(int id)
        {
            return db.Medications.Count(e => e.MedicationID == id) > 0;
        }
    }
}
