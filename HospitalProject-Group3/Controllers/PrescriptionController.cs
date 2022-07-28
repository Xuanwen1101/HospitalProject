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
    public class PrescriptionController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static PrescriptionController()
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


        /// GET: Prescription/List
        public ActionResult List()
        {
            //objective: communicate with the data api to retrieve a list of Prescriptions
            //curl https://localhost:44342/api/PrescriptionData/ListPrescriptions

            string url = "PrescriptionData/ListPrescriptions";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<PrescriptionDto> staffs = response.Content.ReadAsAsync<IEnumerable<PrescriptionDto>>().Result;

            return View(staffs);
        }


        // GET: Prescription/Details/5
        public ActionResult Details(int id)
        {
            DetailsPrescription ViewModel = new DetailsPrescription();

            string url = "PrescriptionData/FindPrescription/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            PrescriptionDto SelectedPrescription = response.Content.ReadAsAsync<PrescriptionDto>().Result;

            ViewModel.SelectedPrescription = SelectedPrescription;


            url = "StaffData/ListStaffsForPrescription/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<StaffDto> workedStaffs = response.Content.ReadAsAsync<IEnumerable<StaffDto>>().Result;

            ViewModel.WorkedStaffs = workedStaffs;


            return View(ViewModel);
        }


        public ActionResult Error()
        {

            return View();
        }


        // GET: Prescription/New
        /*[Authorize]*/
        public ActionResult New()
        {
            /*
            string url = "DepartmentData/ListDepartments";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<DepartmentDto> DepartmentOptions = response.Content.ReadAsAsync<IEnumerable<DepartmentDto>>().Result;
            return View(DepartmentOptions);
            */

            return View();
        }


        // POST: Prescription/Create
        [HttpPost]
        /*[Authorize]*/
        public ActionResult Create(Prescription prescription)
        {
            GetApplicationCookie();//get token credentials
            //objective: add a new prescription into our system using the API
            //curl -H "Content-Type:application/json" -d @prescription.json https://localhost:44342/api/PrescriptionData/AddPrescription 
            string url = "PrescriptionData/AddPrescription";


            string jsonPayload = jss.Serialize(prescription);

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


        // GET: Prescription/Edit/5
        /*[Authorize]*/
        public ActionResult Edit(int id)
        {
            UpdatePrescription ViewModel = new UpdatePrescription();

            //get the existing prescription information
            //curl https://localhost:44349/api/PrescriptionData/FindPrescription/{id}
            string url = "PrescriptionData/FindPrescription/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            PrescriptionDto SelectedPrescription = response.Content.ReadAsAsync<PrescriptionDto>().Result;
            ViewModel.SelectedPrescription = SelectedPrescription;

            /*
            url = "DepartmentData/ListDepartments/";
            response = client.GetAsync(url).Result;
            IEnumerable<DepartmentDto> departmentOptions = response.Content.ReadAsAsync<IEnumerable<DepartmentDto>>().Result;
            ViewModel.DepartmentOptions = departmentOptions;
            */

            return View(ViewModel);
        }


        // POST: Prescription/Update/5
        [HttpPost]
        /*[Authorize]*/
        public ActionResult Update(int id, Prescription prescription)
        {
            GetApplicationCookie();//get token credentials

            string url = "PrescriptionData/UpdatePrescription/" + id;
            string jsonPayload = jss.Serialize(prescription);

            HttpContent content = new StringContent(jsonPayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Prescription/DeleteConfirm/5
        /*[Authorize]*/
        public ActionResult DeleteConfirm(int id)
        {
            // get the existing prescription information
            //curl https://localhost:44342/api/PrescriptionData/FindPrescription/{id}
            string url = "PrescriptionData/FindPrescription/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            PrescriptionDto selectedPrescription = response.Content.ReadAsAsync<PrescriptionDto>().Result;

            return View(selectedPrescription);

        }

        // POST: Prescription/Delete/5
        [HttpPost]
        /*[Authorize]*/
        public ActionResult Delete(int id)
        {
            GetApplicationCookie();//get token credentials
            //objective: delete the selected prescription from our system using the API
            //curl -d "" https://localhost:44342/api/PrescriptionData/DeletePrescription/{id}
            string url = "PrescriptionData/DeletePrescription/" + id;
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