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
    public class MedicationController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static MedicationController()
        {
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

        /// GET: Medication/List
        public ActionResult List()
        {
            //objective: communicate with the data api to retrieve a list of Shifts
            //curl https://localhost:44342/api/MedicationData/ListMedications

            string url = "MedicationData/ListMedications";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<MedicationDto> medications = response.Content.ReadAsAsync<IEnumerable<MedicationDto>>().Result;

            return View(medications);
        }

        // GET: Medication/Details/5
        public ActionResult Details(int id)
        {
            DetailsMedication ViewModel = new DetailsMedication();

            string url = "MedicationData/FindMedication/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            MedicationDto SelectedMedication = response.Content.ReadAsAsync<MedicationDto>().Result;

            ViewModel.SelectedMedication = SelectedMedication;

            //show associated Prescription with this Medication
            //url = "PrescriptionData/ListPrescriptionsforMedication/" + id;
            // response = client.GetAsync(url).Result;
            //IEnumerable<PrescriptionDto> listPrescription = response.Content.ReadAsAsync<IEnumerable<PrescriptionDto>>().Result;
            //ViewModel.ListPrescription = listPrescription;

            return View(ViewModel);
        }


        // GET: Medication/New
        /*[Authorize]*/
        public ActionResult New()
        {
            return View();
        }

        // POST: Medication/Create
        [HttpPost]
        /*[Authorize]*/
        public ActionResult Create(Medication medication)
        {
            GetApplicationCookie();//get token credentials
            //objective: add a new medication into our system using the API
            //curl -H "Content-Type:application/json" -d @medication.json https://localhost:44342/api/MedicationData/AddMedication
            string url = "MedicationData/AddMedication";

            string jsonPayload = jss.Serialize(medication);

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

        // GET: Medication/Edit/5
        /*[Authorize]*/
        public ActionResult Edit(int id)
        {
            UpdateMedication ViewModel = new UpdateMedication();

            //get the existing Medication information
            //curl https://localhost:44342/api/MedicationData/FindMedication/{id}
            string url = "MedicationData/FindMedication/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            MedicationDto SelectedMedication = response.Content.ReadAsAsync<MedicationDto>().Result;
            ViewModel.SelectedMedication = SelectedMedication;


            return View(ViewModel);
        }

        // POST: Medication/Update/5
        [HttpPost]
        /*[Authorize]*/
        public ActionResult Update(int id, Medication medication)
        {
            GetApplicationCookie();//get token credentials
            //objective: update the selected Medication into our system using the API
            //curl -H "Content-Type:application/json" -d @Medication.json  https://localhost:44342/api/MedicationData/UpdateMedication/{id}
            string url = "MedicationData/UpdateMedication/" + id;
            string jsonPayload = jss.Serialize(medication);

            HttpContent content = new StringContent(jsonPayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            //update request is successful, and we have image data
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }

        }

        // GET: Medication/DeleteConfirm/5
        /*[Authorize]*/
        public ActionResult DeleteConfirm(int id)
        {
            // get the existing Medication information
            //curl https://localhost:44342/api/MedicationData/FindMedication/{id}
            string url = "MedicationData/FindMedication" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            MedicationDto selectedMedication = response.Content.ReadAsAsync<MedicationDto>().Result;
            return View(selectedMedication);

        }

        // POST: Medication/Delete/5
        [HttpPost]
        /*[Authorize]*/
        public ActionResult Delete(int id)
        {
            GetApplicationCookie();//get token credentials
            //objective: delete the selected Medication from our system using the API
            //curl -d "" https://localhost:44342/api/MedicationData/DeleteMedication/{id}
            string url = "MedicationData/DeleteMedication/" + id;
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