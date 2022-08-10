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
    public class StaffDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns all Staffs in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all Staffs in the database.
        /// </returns>
        /// <example>
        /// GET: api/StaffData/ListStaffs
        /// </example>
        [HttpGet]
        [ResponseType(typeof(StaffDto))]
        public IHttpActionResult ListStaffs()
        {
            List<Staff> Staffs = db.Staffs.ToList();
            List<StaffDto> StaffDtos = new List<StaffDto>();

            Staffs.ForEach(s => StaffDtos.Add(new StaffDto()
            {
                StaffID = s.StaffID,
                StaffFName = s.StaffFName,
                StaffLName = s.StaffLName,
                StaffBio = s.StaffBio,
                PhoneNumber = s.PhoneNumber,
                Email = s.Email,
                HireDate = s.HireDate,
                StaffHasPhoto = s.StaffHasPhoto,
                PicExtension = s.PicExtension,
                RoleID = s.Role.RoleID,
                RoleType = s.Role.RoleType
            }));


            return Ok(StaffDtos);

        }


        /// <summary>
        /// Gathers information about all Staffs related to the selected Role ID
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all Staffs in the database matched with the selected Role ID
        /// </returns>
        /// <param name="id">Role ID.</param>
        /// <example>
        /// GET: api/StaffData/ListStaffsForRole/3
        /// </example>
        [HttpGet]
        [ResponseType(typeof(StaffDto))]
        public IHttpActionResult ListStaffsForRole(int id)
        {
            List<Staff> Staffs = db.Staffs.Where(m => m.RoleID == id).ToList();
            List<StaffDto> StaffDtos = new List<StaffDto>();

            Staffs.ForEach(s => StaffDtos.Add(new StaffDto()
            {
                StaffID = s.StaffID,
                StaffFName = s.StaffFName,
                StaffLName = s.StaffLName,
                StaffBio = s.StaffBio,
                PhoneNumber = s.PhoneNumber,
                Email = s.Email,
                HireDate = s.HireDate,
                StaffHasPhoto = s.StaffHasPhoto,
                PicExtension = s.PicExtension,
                RoleID = s.Role.RoleID,
                RoleType = s.Role.RoleType
            }));

            return Ok(StaffDtos);
        }


        /// <summary>
        /// Gathers information about Staffs related to the selected Shift ID
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all Staffs in the database matched to the selected Shift ID
        /// </returns>
        /// <param name="id">Shift ID.</param>
        /// <example>
        /// GET: api/StaffData/ListStaffsForShift/2
        /// </example>
        [HttpGet]
        [ResponseType(typeof(StaffDto))]
        public IHttpActionResult ListStaffsForShift(int id)
        {
            //all Staffs that have Shifts which match with our ID
            List<Staff> Staffs = db.Staffs.Where(
                m => m.Shifts.Any(
                    c => c.ShiftID == id
                )).ToList();
            List<StaffDto> StaffDtos = new List<StaffDto>();

            Staffs.ForEach(s => StaffDtos.Add(new StaffDto()
            {
                StaffID = s.StaffID,
                StaffFName = s.StaffFName,
                StaffLName = s.StaffLName,
                StaffBio = s.StaffBio,
                PhoneNumber = s.PhoneNumber,
                Email = s.Email,
                HireDate = s.HireDate,
                StaffHasPhoto = s.StaffHasPhoto,
                PicExtension = s.PicExtension,
                RoleID = s.Role.RoleID,
                RoleType = s.Role.RoleType
            }));

            return Ok(StaffDtos);
        }


        /// <summary>
        /// Associates a particular shift with a particular staff
        /// </summary>
        /// <param name="staffId">The staff ID primary key</param>
        /// <param name="shiftId">The shift ID primary key</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST api/StaffData/AssociateStaffWithShift/2/1
        /// </example>
        [HttpPost]
        [Route("api/StaffData/AssociateStaffWithShift/{staffId}/{shiftId}")]
        /*[Authorize]*/
        public IHttpActionResult AssociateStaffWithShift(int staffId, int shiftId)
        {

            Staff SelectedStaff = db.Staffs.Include(m => m.Shifts).Where(m => m.StaffID == staffId).FirstOrDefault();
            Shift SelectedShift = db.Shifts.Find(shiftId);

            if (SelectedStaff == null || SelectedShift == null)
            {
                return NotFound();
            }


            SelectedStaff.Shifts.Add(SelectedShift);
            db.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Removes an association between a particular shift and a particular staff
        /// </summary>
        /// <param name="staffId">The staff ID primary key</param>
        /// <param name="shiftId">The shift ID primary key</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST api/StaffData/AssociateStaffWithShift/2/1
        /// </example>
        [HttpPost]
        [Route("api/StaffData/UnAssociateStaffWithShift/{staffId}/{shiftId}")]
        /*[Authorize]*/
        public IHttpActionResult UnAssociateStaffWithShift(int staffId, int shiftId)
        {

            Staff SelectedStaff = db.Staffs.Include(m => m.Shifts).Where(m => m.StaffID == staffId).FirstOrDefault();
            Shift SelectedShift = db.Shifts.Find(shiftId);

            if (SelectedStaff == null || SelectedShift == null)
            {
                return NotFound();
            }


            SelectedStaff.Shifts.Remove(SelectedShift);
            db.SaveChanges();

            return Ok();
        }


        /// <summary>
        /// Returns the staff info for the given staff id.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: The staff in the system matching up to the staff ID primary key
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <param name="id">The primary key of the staff</param>
        /// <example>
        /// GET: api/StaffData/FindStaff/5
        /// </example>
        [ResponseType(typeof(StaffDto))]
        [HttpGet]
        public IHttpActionResult FindStaff(int id)
        {
            Staff Staff = db.Staffs.Find(id);
            StaffDto StaffDto = new StaffDto()
            {
                StaffID = Staff.StaffID,
                StaffFName = Staff.StaffFName,
                StaffLName = Staff.StaffLName,
                StaffBio = Staff.StaffBio,
                PhoneNumber = Staff.PhoneNumber,
                Email = Staff.Email,
                HireDate = Staff.HireDate,
                StaffHasPhoto = Staff.StaffHasPhoto,
                PicExtension = Staff.PicExtension,
                RoleID = Staff.Role.RoleID,
                RoleType = Staff.Role.RoleType
            };
            if (Staff == null)
            {
                return NotFound();
            }

            return Ok(StaffDto);
        }


        /// <summary>
        /// Updates the selected staff in the system with POST Data input
        /// </summary>
        /// <param name="id">Represents the Staff ID primary key</param>
        /// <param name="staff">JSON FORM DATA of an staff</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// PUT: api/StaffData/UpdateStaff/5
        /// FORM DATA: Staff JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        [Authorize]
        public IHttpActionResult UpdateStaff(int id, Staff staff)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != staff.StaffID)
            {
                return BadRequest();
            }

            db.Entry(staff).State = EntityState.Modified;
            // Picture update is handled by another method
            db.Entry(staff).Property(m => m.StaffHasPhoto).IsModified = false;
            db.Entry(staff).Property(m => m.PicExtension).IsModified = false;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StaffExists(id))
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
        /// Receives staff picture data, uploads it to the webserver and updates the staff's HasPic option
        /// </summary>
        /// <param name="id">the staff id</param>
        /// <returns>status code 200 if successful.</returns>
        /// <example>
        /// curl -F StaffPicture=@file.jpg "https://localhost:44342/api/StaffData/UploadStaffPicture/5"
        /// POST: api/StaffData/UploadStaffPicture/5
        /// HEADER: enctype=multipart/form-data
        /// FORM-DATA: image
        /// </example>
        /// https://stackoverflow.com/questions/28369529/how-to-set-up-a-web-api-controller-for-multipart-form-data

        [HttpPost]
        [Authorize]
        public IHttpActionResult UploadStaffPicture(int id)
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
                    var staffPicture = HttpContext.Current.Request.Files[0];
                    //Check if the file is empty
                    if (staffPicture.ContentLength > 0)
                    {
                        //establish valid file types (can be changed to other file extensions if desired!)
                        var valtypes = new[] { "jpeg", "jpg", "png", "gif" };
                        var extension = Path.GetExtension(staffPicture.FileName).Substring(1);
                        //Check the extension of the file
                        if (valtypes.Contains(extension))
                        {
                            try
                            {
                                //file name is the id of the image
                                string fn = id + "." + extension;

                                //get a direct file path to ~/Content/Images/Staffs/{id}.{extension}
                                string path = Path.Combine(HttpContext.Current.Server.MapPath("~/Content/Images/Staffs/"), fn);

                                //save the file
                                staffPicture.SaveAs(path);

                                //if these are all successful then we can set these fields
                                haspic = true;
                                picextension = extension;

                                //Update the staff haspic and picextension fields in the database
                                Staff SelectedStaff = db.Staffs.Find(id);
                                SelectedStaff.StaffHasPhoto = haspic;
                                SelectedStaff.PicExtension = extension;
                                db.Entry(SelectedStaff).State = EntityState.Modified;

                                db.SaveChanges();

                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine("Staff Image was not saved successfully.");
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


        /// <summary>
        /// Adds a Staff to the system
        /// </summary>
        /// <param name="staff">JSON FORM DATA of a Staff</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: Staff ID, Staff Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/StaffData/AddStaff
        /// FORM DATA: Staff JSON Object
        /// </example>
        [ResponseType(typeof(Staff))]
        [HttpPost]
        [Authorize]
        public IHttpActionResult AddStaff(Staff staff)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Staffs.Add(staff);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = staff.StaffID }, staff);
        }


        /// <summary>
        /// Deletes the selected Staff from the system by it's ID.
        /// </summary>
        /// <param name="id">The primary key of the Staff</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// DELETE: api/StaffData/DeleteStaff/5
        /// FORM DATA: (empty)
        /// </example>
        [ResponseType(typeof(Staff))]
        [HttpPost]
        [Authorize]
        public IHttpActionResult DeleteStaff(int id)
        {
            Staff staff = db.Staffs.Find(id);
            if (staff == null)
            {
                return NotFound();
            }

            if (staff.StaffHasPhoto && staff.PicExtension != "")
            {
                //also delete image from path
                string path = HttpContext.Current.Server.MapPath("~/Content/Images/Staffs/" + id + "." + staff.PicExtension);
                if (System.IO.File.Exists(path))
                {
                    Debug.WriteLine("File exists... preparing to delete!");
                    System.IO.File.Delete(path);
                }
            }

            db.Staffs.Remove(staff);
            db.SaveChanges();

            return Ok(staff);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool StaffExists(int id)
        {
            return db.Staffs.Count(e => e.StaffID == id) > 0;
        }
    }
}