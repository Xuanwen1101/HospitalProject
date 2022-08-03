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
    public class MedicalRecordDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/MedicalRecordsData/Listmedicalrecords
        [HttpGet]
        public IEnumerable<MedicalRecordDto> ListMedicalRecords()
        {
            List<MedicalRecord> MedicalRecord = db.MedicalRecords.ToList();

            List<MedicalRecordDto> MedicalRecordDtos = new List<MedicalRecordDto>();


            MedicalRecord.ForEach(d => MedicalRecordDtos.Add(new MedicalRecordDto()
            {
                RecordID = d.RecordID,
                RecordDetail = d.RecordDetail,
                RecordDate = d.RecordDate,
                PatientID = d.Patient.PatientID,
                PatientFName = d.Patient.PatientFName,
                PatientLName = d.Patient.PatientLName
            }));

            return (MedicalRecordDtos);

        }
        // GET: api/MedicalRecordsData/FindMedicalRecords/5
        [ResponseType(typeof(MedicalRecord))]
        [HttpGet]
        public IHttpActionResult FindMedicalRecord(int id)
        {
            MedicalRecord medicalRecord = db.MedicalRecords.Find(id);
            MedicalRecordDto MedicalRecordDto = new MedicalRecordDto()
            {
                RecordID = medicalRecord.RecordID,
                RecordDetail = medicalRecord.RecordDetail,
                RecordDate = medicalRecord.RecordDate,
                PatientID = medicalRecord.Patient.PatientID,
                PatientFName = medicalRecord.Patient.PatientFName,
                PatientLName = medicalRecord.Patient.PatientLName
            };

            if (medicalRecord == null)
            {
                return NotFound();
            }

            return Ok(MedicalRecordDto);
        }

        // PUT: api/MedicalRecordsData/UpdateMedicalRecords/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateMedicalRecord(int id, MedicalRecord medicalRecord)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != medicalRecord.RecordID)
            {
                return BadRequest();
            }

            db.Entry(medicalRecord).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MedicalRecordExists(id))
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

        // POST: api/MedicalRecordsData/AddMedicalRecords
        [ResponseType(typeof(MedicalRecord))]
        [HttpPost]
        public IHttpActionResult AddMedicalRecord(MedicalRecord medicalRecord)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.MedicalRecords.Add(medicalRecord);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = medicalRecord.RecordID }, medicalRecord);
        }

        // DELETE: api/MedicalRecordsData/DeleteMedicalRecords/5
        [ResponseType(typeof(MedicalRecord))]
        [HttpPost]
        public IHttpActionResult DeleteMedicalRecord(int id)
        {
            MedicalRecord medicalRecord = db.MedicalRecords.Find(id);
            if (medicalRecord == null)
            {
                return NotFound();
            }

            db.MedicalRecords.Remove(medicalRecord);
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

        private bool MedicalRecordExists(int id)
        {
            return db.MedicalRecords.Count(e => e.RecordID == id) > 0;
        }
    }
}
