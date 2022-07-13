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

            Medications.ForEach(s => MedicationDtos.Add(new MedicationDto()
            {
                MedicationID=s.MedicationID,
                MedicationName=s.MedicationName,
                MedicationBrand=s.MedicationBrand,
                MedicationDetail=s.MedicationDetail,
                Price=s.Price
            }));


            return Ok(MedicationDtos);

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
        /// Deletes an Role from the system by it's ID.
        /// </summary>
        /// <param name="id">The primary key of the Role</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// DELETE: api/MedicationData/DeleteMedication/5
        /// FORM DATA: (empty)
        /// </example>
        [ResponseType(typeof(Role))]
        [HttpPost]
        /*[Authorize]*/
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
