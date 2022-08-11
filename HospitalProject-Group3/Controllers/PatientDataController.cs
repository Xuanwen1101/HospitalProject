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
    public class PatientDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/PatientData/ListPatients
        [HttpGet]
        public IEnumerable<PatientDto> ListPatients()
        {
            List<Patient> Patients = db.Patients.ToList();
            List<PatientDto> PatientDtos = new List<PatientDto>();

            Patients.ForEach(a => PatientDtos.Add(new PatientDto()
            {
                PatientID = a.PatientID,
                PatientFName = a.PatientFName,
                PatientLName = a.PatientLName,
                Gender = a.Gender,
                PhoneNumber = a.PhoneNumber,
                BloodType = a.BloodType,
                Email = a.Email,
                PatientHasPhoto = a.PatientHasPhoto,
                PicExtension = a.PicExtension,
                InsuranceID = a.Insurance.InsuranceID,
                InsuranceCompany = a.Insurance.InsuranceCompany


            }));

            return PatientDtos;
        }

        // GET: api/PatientData/FindPatient/5
        [ResponseType(typeof(Patient))]
        [HttpGet]
        public IHttpActionResult FindPatient(int id)
        {
            Patient Patient = db.Patients.Find(id);
            PatientDto PatientDto = new PatientDto()
            {
                PatientID = Patient.PatientID,
                PatientFName = Patient.PatientFName,
                PatientLName = Patient.PatientLName,
                Gender = Patient.Gender,
                PhoneNumber = Patient.PhoneNumber,
                BloodType = Patient.BloodType,
                Email = Patient.Email,
                PatientHasPhoto = Patient.PatientHasPhoto,
                PicExtension = Patient.PicExtension,
                InsuranceID = Patient.Insurance.InsuranceID,
                InsuranceCompany = Patient.Insurance.InsuranceCompany

            };
            if (Patient == null)
            {
                return NotFound();
            }

            return Ok(PatientDto);
        }

        [HttpGet]
        [ResponseType(typeof(PatientDto))]
        public IHttpActionResult ListPatientsforInsurance(int id)
        {
            List<Patient> Patients = db.Patients.Where(
                c => c.InsuranceID == id
                   ).ToList();
            List<PatientDto> PatientDtos = new List<PatientDto>();

            Patients.ForEach(s => PatientDtos.Add(new PatientDto()
            {
                PatientID = s.PatientID,
                PatientFName = s.PatientFName,
                PatientLName = s.PatientLName
            }));

            return Ok(PatientDtos);
        }

        // PUT: api/PatientData/UpdatePatient/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdatePatient(int id, Patient patient)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != patient.PatientID)
            {
                return BadRequest();
            }

            /*Debug.WriteLine(id);
            Debug.WriteLine(patient);*/

            db.Entry(patient).State = EntityState.Modified;
            // Picture update is handled by another method
            db.Entry(patient).Property(m => m.PatientHasPhoto).IsModified = false;
            db.Entry(patient).Property(m => m.PicExtension).IsModified = false;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PatientExists(id))
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
        /*public IHttpActionResult UpdatePatient(int id, Patient patient)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != patient.PatientID)
            {
                return BadRequest();
            }

            db.Entry(patient).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PatientExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }*/

        /// <summary>
        /// Receives patient picture data, uploads it to the webserver and updates the patient's HasPic option
        /// </summary>
        /// <param name="id">the patient id</param>
        /// <returns>status code 200 if successful.</returns>
        /// <example>
        /// curl -F PatientPicture=@file.jpg "https://localhost:44342/api/PatientData/UploadPatientPicture/5"
        /// POST: api/PatientData/UploadPatientPicture/5
        /// HEADER: enctype=multipart/form-data
        /// FORM-DATA: image
        /// </example>
        /// https://stackoverflow.com/questions/28369529/how-to-set-up-a-web-api-controller-for-multipart-form-data

        [HttpPost]
        public IHttpActionResult UploadPatientPicture(int id)
        {

            bool haspic = false;
            string picextension;
            if (Request.Content.IsMimeMultipartContent())
            {
                //Debug.WriteLine("Received multipart form data.");

                int numfiles = HttpContext.Current.Request.Files.Count;
                //Debug.WriteLine("Files Received: " + numfiles);

                //Check if a file is posted
                if (numfiles == 1 && HttpContext.Current.Request.Files[0] != null)
                {
                    var patientPicture = HttpContext.Current.Request.Files[0];
                    //Check if the file is empty
                    if (patientPicture.ContentLength > 0)
                    {
                        //establish valid file types (can be changed to other file extensions if desired!)
                        var valtypes = new[] { "jpeg", "jpg", "png", "gif" };
                        var extension = Path.GetExtension(patientPicture.FileName).Substring(1);
                        //Check the extension of the file
                        if (valtypes.Contains(extension))
                        {
                            try
                            {
                                //file name is the id of the image
                                string fn = id + "." + extension;

                                //get a direct file path to ~/Content/Images/Patients/{id}.{extension}
                                string path = Path.Combine(HttpContext.Current.Server.MapPath("~/Content/Images/Patients/"), fn);

                                //save the file
                                patientPicture.SaveAs(path);

                                //if these are all successful then we can set these fields
                                haspic = true;
                                picextension = extension;

                                //Update the patient haspic and picextension fields in the database
                                Patient SelectedPatient = db.Patients.Find(id);
                                SelectedPatient.PatientHasPhoto = haspic;
                                SelectedPatient.PicExtension = extension;
                                db.Entry(SelectedPatient).State = EntityState.Modified;

                                db.SaveChanges();

                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine("Patient Image was not saved successfully.");
                                Debug.WriteLine("Exception:" + ex);
                                return BadRequest();
                            }
                        }
                    }

                }

                return Ok();
            }
            else
            {
                //not multipart form data
                return BadRequest();
            }
        }


        // POST: api/PatientData/AddPatient
        [ResponseType(typeof(Patient))]
        [HttpPost]
        public IHttpActionResult AddPatient(Patient patient)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Patients.Add(patient);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = patient.PatientID }, patient);
        }

        // POST: api/PatientData/DeletePatient/5
        [ResponseType(typeof(Patient))]
        [HttpPost]
        public IHttpActionResult DeletePatient(int id)
        {
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return NotFound();
            }

            db.Patients.Remove(patient);
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

        private bool PatientExists(int id)
        {
            return db.Patients.Count(e => e.PatientID == id) > 0;
        }
    }
}