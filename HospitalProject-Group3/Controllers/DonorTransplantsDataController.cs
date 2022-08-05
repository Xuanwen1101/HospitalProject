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
    public class DonorTransplantsDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/DonorTransplantsData/ListDonors
        [HttpGet]
        public IHttpActionResult ListDonors()
        {
            List<DonorTransplant> DonorTransplants = db.DonorTransplants.ToList();
            List<DonorTransplantDto> DonorTransplantDtos = new List<DonorTransplantDto>();

            DonorTransplants.ForEach(d => DonorTransplantDtos.Add(new DonorTransplantDto()
            {
                DonorTransplantID = d.DonorTransplantID,
                OrganType = d.OrganType,
                SurgeryPlan = d.SurgeryPlan,
                WaitListNumber = d.WaitListNumber,
                PatientID = d.Patient.PatientID,
                PatientFName = d.Patient.PatientFName,
                PatientLName = d.Patient.PatientLName
            }));

            return Ok(DonorTransplantDtos);
        }

        // GET: api/DonorTransplantsData/FindDonorTransplant/5
        [ResponseType(typeof(DonorTransplant))]
        [HttpGet]
        public IHttpActionResult FindDonorTransplant(int id)
        {
            DonorTransplant DonorTransplant = db.DonorTransplants.Find(id);
            DonorTransplantDto DonorTransplantDto = new DonorTransplantDto()
            {
                DonorTransplantID = DonorTransplant.DonorTransplantID,
                OrganType = DonorTransplant.OrganType,
                SurgeryPlan = DonorTransplant.SurgeryPlan,
                WaitListNumber = DonorTransplant.WaitListNumber,
                PatientID = DonorTransplant.Patient.PatientID,
                PatientFName = DonorTransplant.Patient.PatientFName,
                PatientLName = DonorTransplant.Patient.PatientLName

            };
            if (DonorTransplant == null)
            {
                return NotFound();
            }

            return Ok(DonorTransplantDto);
        }

        // PUT: api/DonorTransplantsData/UpdateDonorTransplant/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateDonorTransplant(int id, DonorTransplant DonorTransplant)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != DonorTransplant.DonorTransplantID)
            {
                return BadRequest();
            }

            db.Entry(DonorTransplant).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DonorTransplantExists(id))
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

        // POST: api/DonorTransplantsData/AddDonorTransplant
        [ResponseType(typeof(DonorTransplant))]
        [HttpPost]
        public IHttpActionResult AddDonorTransplant(DonorTransplant DonorTransplant)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.DonorTransplants.Add(DonorTransplant);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = DonorTransplant.DonorTransplantID }, DonorTransplant);
        }

        // POST: api/DonorTransplantsData/DeleteDonorTransplant/5
        [ResponseType(typeof(DonorTransplant))]
        [HttpPost]
        public IHttpActionResult DeleteDonorTransplant(int id)
        {
            DonorTransplant DonorTransplant = db.DonorTransplants.Find(id);
            if (DonorTransplant == null)
            {
                return NotFound();
            }

            db.DonorTransplants.Remove(DonorTransplant);
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

        private bool DonorTransplantExists(int id)
        {
            return db.DonorTransplants.Count(e => e.DonorTransplantID == id) > 0;
        }
    }
}