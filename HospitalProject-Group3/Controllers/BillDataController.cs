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
using System.Diagnostics;

namespace HospitalProject_Group3.Controllers
{
    public class BillDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        /// <summary>
        /// list bills
        /// </summary>
        /// <returns>returns the bills</returns>
        // GET: api/BillData/ListBills
        [HttpGet]
        public IEnumerable<BillDto> ListBills()
        {
            List<Bill> Bills = db.Bills.ToList();
            List<BillDto> BillDtos = new List<BillDto>();

            Bills.ForEach(a => BillDtos.Add(new BillDto()
            {
                BillID = a.BillID,
                BillDetail = a.BillDetail,
                TotalCost = a.TotalCost,
                PatientID = a.Patient.PatientID,
                PatientFName = a.Patient.PatientFName,
                PatientLName = a.Patient.PatientLName

            }));

            return BillDtos;
        }
        /// <summary>
        /// find a particular bill using id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>returns a specific bill based on id</returns>
        // GET: api/BillData/FindBill/5
        [ResponseType(typeof(Bill))]
        [HttpGet]
        public IHttpActionResult FindBill(int id)
        {
            Bill Bill = db.Bills.Find(id);
            BillDto BillDto = new BillDto()
            {
                BillID = Bill.BillID,
                BillDetail =Bill.BillDetail,
                TotalCost = Bill.TotalCost,
                PatientID = Bill.Patient.PatientID,
                PatientFName = Bill.Patient.PatientFName,
                PatientLName = Bill.Patient.PatientLName

            };
            if (Bill == null)
            {
                return NotFound();
            }

            return Ok(BillDto);
        }
        /// <summary>
        /// changes bill records
        /// </summary>
        /// <param name="id"></param>
        /// <param name="bill"></param>
        /// <returns>updated bill</returns>
        // PUT: api/BillData/UpdateBill/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateBill(int id, Bill bill)
        {
            //Debug.WriteLine("I have reached the update bill method!");

            if (!ModelState.IsValid)
            {
                //Debug.WriteLine("Model State is invalid");
                return BadRequest(ModelState);
            }

            if (id != bill.BillID)
            {
                
                return BadRequest();
            }

            db.Entry(bill).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BillExists(id))
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
        /// creates new bill
        /// </summary>
        /// <param name="bill"></param>
        /// <returns></returns>
        // POST: api/BillData/AddBill
        [ResponseType(typeof(Bill))]
        [HttpPost]
        public IHttpActionResult AddBill(Bill bill)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Bills.Add(bill);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = bill.BillID }, bill);
        }
        /// <summary>
        /// Deletes a specific bill based on id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // POST: api/BillData/DeleteBill/5
        [ResponseType(typeof(Bill))]
        [HttpPost]
        public IHttpActionResult DeleteBill(int id)
        {
            Bill bill = db.Bills.Find(id);
            if (bill == null)
            {
                return NotFound();
            }

            db.Bills.Remove(bill);
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

        private bool BillExists(int id)
        {
            return db.Bills.Count(e => e.BillID == id) > 0;
        }
    }
}