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

            Insurances.ForEach(s => InsuranceDtos.Add(new InsuranceDto()
            {
                InsuranceID=s.InsuranceID,
                InsuranceCompany=s.InsuranceCompany,
                InsurancePlan=s.InsurancePlan
            }));


            return Ok(InsuranceDtos);

        }

        /// <summary>
        /// Adds a Insurance to the system
        /// </summary>
        /// <param name="Insurance">JSON FORM DATA of a Insurance</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: Insurance ID, Insurance Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/InsuranceData/AddInsurance
        /// FORM DATA: Insurance JSON Object
        /// </example>
        [ResponseType(typeof(Insurance))]
        [HttpPost]
        /*[Authorize]*/
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

    }
}
