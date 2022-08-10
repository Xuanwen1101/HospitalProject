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
    public class InsuranceDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns all Insurance in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all Insurance in the database.
        /// </returns>
        /// <example>
        /// GET: api/InsuranceData/ListInsurances
        /// </example>
        [HttpGet]
        [ResponseType(typeof(InsuranceDto))]
        public IHttpActionResult ListInsurances()
        {
            List<Insurance> Insurances = db.Insurances.ToList();
            List<InsuranceDto> InsuranceDtos = new List<InsuranceDto>();

            Insurances.ForEach(i => InsuranceDtos.Add(new InsuranceDto()
            {
                InsuranceID=i.InsuranceID,
                InsuranceCompany=i.InsuranceCompany,
                InsurancePlan=i.InsurancePlan
            }));

            return Ok(InsuranceDtos);
        }

        /// <summary>
        /// Returns the Insurance.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: An Insurance in the system matching up to the Insurance ID primary key
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <param name="id">The primary key of the Insurance</param>
        /// <example>
        /// GET: api/InsuranceData/FindInsurance/5
        /// </example>
        [ResponseType(typeof(Insurance))]
        [HttpGet]
        public IHttpActionResult FindInsurance(int id)
        {
            Insurance Insurance = db.Insurances.Find(id);
            InsuranceDto InsuranceDto = new InsuranceDto()
            {
                InsuranceID = Insurance.InsuranceID,
                InsuranceCompany = Insurance.InsuranceCompany,
                InsurancePlan = Insurance.InsurancePlan
            };

            if (Insurance == null)
            {
                return NotFound();
            }

            return Ok(InsuranceDto);
        }

        /// <summary>
        /// Adds a Insurance to the system
        /// </summary>
        /// <param name="Insurance">JSON FORM DATA of a Insurance</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: Insurance Company, Insurance Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/InsuranceData/AddInsurance
        /// FORM DATA: Insurance JSON Object
        /// </example>
        [ResponseType(typeof(Insurance))]
        [HttpPost]
        [Authorize]
        public IHttpActionResult AddInsurance(Insurance insurance)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Insurances.Add(insurance);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = insurance.InsuranceID }, insurance);
        }

        /// <summary>
        /// Updates the selected Insurances in the system with POST Data input
        /// </summary>
        /// <param name="id">Represents the Insurances ID primary key</param>
        /// <param name="Insurances">JSON FORM DATA of an Insurances</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// PUT: api/InsurancesData/UpdateInsurances/5
        /// FORM DATA: Insurances JSON Object
        /// </example>
        [HttpPost]
        [ResponseType(typeof(void))]
        [Authorize]
        public IHttpActionResult UpdateInsurance(int id, Insurance insurance)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != insurance.InsuranceID)
            {
                return BadRequest();
            }


            db.Entry(insurance).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InsuranceExists(id))
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
        /// Deletes an Insurance from the system by it's ID.
        /// </summary>
        /// <param name="id">The primary key of the Insurance</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// DELETE: api/InsuranceData/DeleteInsurance/5
        /// FORM DATA: (empty)
        /// </example>
        [ResponseType(typeof(Insurance))]
        [HttpPost]
        [Authorize]
        public IHttpActionResult DeleteInsurance(int id)
        {
            Insurance Insurance = db.Insurances.Find(id);

            if (Insurance == null)
            {
                return NotFound();
            }

            db.Insurances.Remove(Insurance);
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

        private bool InsuranceExists(int id)
        {
            return db.Insurances.Count(e => e.InsuranceID == id) > 0;
        }

    }
}
