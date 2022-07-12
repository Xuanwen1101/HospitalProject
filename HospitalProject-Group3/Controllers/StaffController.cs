using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Diagnostics;
using HospitalProject_Group3.Models;
using HospitalProject_Group3.Models.ViewModels;
using System.Web.Script.Serialization;

namespace HospitalProject_Group3.Controllers
{
    public class StaffController : Controller
    {

        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static StaffController()
        {
            //client = new HttpClient();
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false,
                //cookies are manually set in RequestHeader
                UseCookies = false
            };

            client = new HttpClient(handler);

            client.BaseAddress = new Uri("https://localhost:44342/api/");
        }


        /// <summary>
        /// Grabs the authentication cookie sent to this controller.
        /// For proper WebAPI authentication, you can send a post request with login credentials to the WebAPI and log the access token from the response. The controller already knows this token, so we're just passing it up the chain.
        /// 
        /// Here is a descriptive article which walks through the process of setting up authorization/authentication directly.
        /// https://docs.microsoft.com/en-us/aspnet/web-api/overview/security/individual-accounts-in-web-api
        /// </summary>
        private void GetApplicationCookie()
        {
            string token = "";

            client.DefaultRequestHeaders.Remove("Cookie");
            if (!User.Identity.IsAuthenticated) return;

            HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies.Get(".AspNet.ApplicationCookie");
            if (cookie != null) token = cookie.Value;

            //collect token as it is submitted to the controller
            //use it to pass along to the WebAPI.
            Debug.WriteLine("Token Submitted is : " + token);
            if (token != "") client.DefaultRequestHeaders.Add("Cookie", ".AspNet.ApplicationCookie=" + token);

            return;
        }

        /// GET: Staff/List
        public ActionResult List()
        {
            //objective: communicate with the data api to retrieve a list of Staffs
            //curl https://localhost:44342/api/StaffData/ListStaffs

            string url = "StaffData/ListStaffs";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<StaffDto> staffs = response.Content.ReadAsAsync<IEnumerable<StaffDto>>().Result;

            return View(staffs);
        }

        // GET: Staff/Details/5
        public ActionResult Details(int id)
        {
            DetailsStaff ViewModel = new DetailsStaff();

            //objective: communicate with the data api to retrieve the selected Staff info
            //curl https://localhost:44342/api/StaffData/FindStaff/{id}

            string url = "StaffData/FindStaff/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            StaffDto selectedStaff = response.Content.ReadAsAsync<StaffDto>().Result;

            ViewModel.SelectedStaff = selectedStaff;

            //show associated Shifts with this Staff
            url = "ShiftData/ListShiftsWorkingForStaff/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<ShiftDto> workingingShifts = response.Content.ReadAsAsync<IEnumerable<ShiftDto>>().Result;

            ViewModel.WorkingingShifts = workingingShifts;

            url = "ShiftData/ListShiftsNotWorkingForStaff/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<ShiftDto> availableShifts = response.Content.ReadAsAsync<IEnumerable<ShiftDto>>().Result;

            ViewModel.AvailableShifts = availableShifts;


            /*
            url = "AppointmentData/ListAppointmentsForStaff/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<AppointmentDto> hadAppointments = response.Content.ReadAsAsync<IEnumerable<AppointmentDto>>().Result;

            ViewModel.HadAppointments = hadAppointments;

            url = "PrescriptionData/ListPrescriptionsForStaff/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<PrescriptionDto> createdPrescriptions = response.Content.ReadAsAsync<IEnumerable<PrescriptionDto>>().Result;

            ViewModel.CreatedPrescriptions = createdPrescriptions;
            */


            return View(ViewModel);

        }


        //POST: Staff/Associate/{staffId}
        [HttpPost]
        /*[Authorize]*/
        public ActionResult Associate(int id, int ShiftID)
        {
            GetApplicationCookie();//get token credentials
            //Debug.WriteLine("Attempting to associate staff :" + id + " with shift " + ShiftID);

            //associate staff with shift
            string url = "StaffData/AssociateStaffWithShift/" + id + "/" + ShiftID;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("Details/" + id);
        }


        //Get: Staff/UnAssociate/{id}?ShiftID={shiftID}
        [HttpGet]
        /*[Authorize]*/
        public ActionResult UnAssociate(int id, int ShiftID)
        {
            GetApplicationCookie();//get token credentials
            //Debug.WriteLine("Attempting to unassociate staff :" + id + " with shift: " + ShiftID);

            //unassociate staff with shift
            string url = "StaffData/UnAssociateStaffWithShift/" + id + "/" + ShiftID;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("Details/" + id);
        }


        public ActionResult Error()
        {

            return View();
        }


        // GET: Staff/New
        /*[Authorize]*/
        public ActionResult New()
        {
            string url = "RoleData/ListRoles";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<RoleDto> RoleOptions = response.Content.ReadAsAsync<IEnumerable<RoleDto>>().Result;

            return View(RoleOptions);
        }

        // POST: Staff/Create
        [HttpPost]
        /*[Authorize]*/
        public ActionResult Create(Staff staff)
        {
            GetApplicationCookie();//get token credentials
            //objective: add a new staff into our system using the API
            //curl -H "Content-Type:application/json" -d @staff.json https://localhost:44342/api/StaffData/AddStaff 
            string url = "StaffData/AddStaff";


            string jsonPayload = jss.Serialize(staff);

            Debug.WriteLine("the json payload is :", jsonPayload);

            HttpContent content = new StringContent(jsonPayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Staff/Edit/5
        /*[Authorize]*/
        public ActionResult Edit(int id)
        {
            UpdateStaff ViewModel = new UpdateStaff();

            //get the existing staff information
            //curl https://localhost:44342/api/StaffData/FindStaff/{id}
            string url = "StaffData/FindStaff/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            StaffDto SelectedStaff = response.Content.ReadAsAsync<StaffDto>().Result;
            ViewModel.SelectedStaff = SelectedStaff;

            //all roles to choose from when updating this staff
            //the existing Role Options
            url = "RoleData/ListRoles/";
            response = client.GetAsync(url).Result;
            IEnumerable<RoleDto> roleOptions = response.Content.ReadAsAsync<IEnumerable<RoleDto>>().Result;

            ViewModel.RoleOptions = roleOptions;

            return View(ViewModel);
        }

        // POST: Staff/Update/5
        [HttpPost]
        /*[Authorize]*/
        public ActionResult Update(int id, Staff staff, HttpPostedFileBase StaffPicture)
        {
            GetApplicationCookie();//get token credentials
            //objective: update the selected staff into our system using the API
            //curl -H "Content-Type:application/json" -d @staff.json  https://localhost:44342/api/StaffData/UpdateStaff/{id}
            string url = "StaffData/UpdateStaff/" + id;
            string jsonPayload = jss.Serialize(staff);

            HttpContent content = new StringContent(jsonPayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(content);

            //update request is successful, and we have image data
            if (response.IsSuccessStatusCode && StaffPicture != null)
            {
                //Updating the Staff picture as a separate request
                Debug.WriteLine("Calling Update Image method.");
                //Send over image data for player
                url = "StaffData/UploadStaffPicture/" + id;

                MultipartFormDataContent requestcontent = new MultipartFormDataContent();
                HttpContent imagecontent = new StreamContent(StaffPicture.InputStream);
                requestcontent.Add(imagecontent, "StaffPicture", StaffPicture.FileName);
                response = client.PostAsync(url, requestcontent).Result;

                return RedirectToAction("List");
            }
            else if (response.IsSuccessStatusCode)
            {
                //No image upload, but update still successful
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }

        }

        // GET: Staff/DeleteConfirm/5
        /*[Authorize]*/
        public ActionResult DeleteConfirm(int id)
        {
            // get the existing staff information
            //curl https://localhost:44342/api/StaffData/FindStaff/{id}
            string url = "StaffData/FindStaff/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            StaffDto selectedStaff = response.Content.ReadAsAsync<StaffDto>().Result;

            return View(selectedStaff);

        }

        // POST: Staff/Delete/5
        [HttpPost]
        /*[Authorize]*/
        public ActionResult Delete(int id)
        {
            GetApplicationCookie();//get token credentials
            //objective: delete the selected staff from our system using the API
            //curl -d "" https://localhost:44342/api/StaffData/DeleteStaff/{id}
            string url = "StaffData/DeleteStaff/" + id;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

    }
}
