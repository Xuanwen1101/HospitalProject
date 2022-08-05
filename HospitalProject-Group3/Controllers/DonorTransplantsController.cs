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
    public class DonorTransplantsController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static DonorTransplantsController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44342/api/");
        }
        // GET: DonorTransplant/List
        public ActionResult List()
        {

            // to communicate with PatientData api controller to access list of Patients
            //curl https://localhost:44342/api/DonorTransplantsData/ListDonors

            string url = "DonorTransplantsData/ListDonors";
            HttpResponseMessage response = client.GetAsync(url).Result;

            // Debug.WriteLine("The response is ok");
            // Debug.WriteLine(response.StatusCode);
            Debug.WriteLine("Debug me");
            IEnumerable<DonorTransplantDto> DonorTransplant = response.Content.ReadAsAsync<IEnumerable<DonorTransplantDto>>().Result;

            return View(DonorTransplant);
        }

        // GET: DonorTransplant/Details/5
        public ActionResult Details(int id)
        {

            // to communicate with PatientData api controller to access list of Patient
            //curl https://localhost:44342/api/DonorTransplantsData/FindDonorTransplant/{id}

            string url = "DonorTransplantsData/FindDonorTransplant/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            DonorTransplantDto selectedDonorTransplant = response.Content.ReadAsAsync<DonorTransplantDto>().Result;

            return View(selectedDonorTransplant);
        }
        public ActionResult Error()
        {
            return View();
        }

        // GET: DonorTransplant/New
        public ActionResult New()
        {
            return View();
        }

        // POST: DonorTransplant/Create
        [HttpPost]
        public ActionResult Create(DonorTransplant DonorTransplant)
        {

            //curl -H "Content-Type:application/json" -d @patient.json https://localhost:44342/api/DonorTransplantsData/AddDonorTransplant
            string url = "DonorTransplantsData/AddDonorTransplant";

            string jsonpayload = jss.Serialize(DonorTransplant);

            Debug.WriteLine(jsonpayload);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error"); ;
            }

        }

        // Get: DonorTransplant/Edit/5
        [HttpPost]
        public ActionResult Edit(int id)
        {
            // TODO: Add update logic here

            string url = "DonorTransplantsData/FindDonorTransplant/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            DonorTransplantDto selectedDonorTransplant = response.Content.ReadAsAsync<DonorTransplantDto>().Result;




            return View(selectedDonorTransplant);

        }
        // POST: Patient/Update/5
        [HttpPost]
        public ActionResult Update(int id, DonorTransplant DonorTransplant)
        {

            // to communicate with PatientData api controller to access list of Patient
            //curl https://localhost:44342/api/Patientdata/updatepatient/{id}
            string url = "DonorTransplantsData/updateDonorTransplant/" + id;


            string jsonpayload = jss.Serialize(DonorTransplant);

            // Debug.WriteLine(jsonpayload);
            HttpContent content = new StringContent(jsonpayload);
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


        // GET: DonorTransplant/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "DonorTransplantsData/FindDonorTransplant/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            DonorTransplantDto selectedDonorTransplant = response.Content.ReadAsAsync<DonorTransplantDto>().Result;

            return View(selectedDonorTransplant);
        }



        // POST: DonorTransplant/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "DonorTransplantsData/DeleteDonorTransplant/" + id;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                // TODO: Add delete logic here

                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }

        }
    }
}
