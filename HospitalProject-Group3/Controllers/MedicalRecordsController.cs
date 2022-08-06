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
    public class MedicalRecordsController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static MedicalRecordsController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44342/api/");
        }
        // GET: /List
        public ActionResult List()
        {

            // to communicate with PatientData api controller to access list of Patients
            //curl https://localhost:44342/api/MedicalRecordsData/Listmedicalrecords

            string url = "MedicalRecordsData/Listmedicalrecords";
            HttpResponseMessage response = client.GetAsync(url).Result;

            // Debug.WriteLine("The response is ok");
            // Debug.WriteLine(response.StatusCode);
            Debug.WriteLine("Debug me");
            IEnumerable<MedicalRecordDto> medicalRecords = response.Content.ReadAsAsync<IEnumerable<MedicalRecordDto>>().Result;

            return View(medicalRecords);
        }

        // GET: /Details/5
        public ActionResult Details(int id)
        {

            // to communicate with PatientData api controller to access list of Patient
            //curl https://localhost:44342/api/MedicalRecordsData/FindMedicalRecords/{id}

            string url = "MedicalRecordsData/FindMedicalRecords/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            MedicalRecordDto selectedMedicalRecord = response.Content.ReadAsAsync<MedicalRecordDto>().Result;

            /*DetailsMedicalRecord ViewModel = new DetailsMedicalRecord();

            ViewModel.selectedMedicalRecord = selectedMedicalRecord;

            return View(ViewModel);*/

            // comment code out because it fails the project build
            return View();
        }
        public ActionResult Error()
        {
            return View();
        }


        public ActionResult New()
        {
            return View();
        }

        // comment code out because it fails the project build
        // POST: MedicalRecord/Create
        /*[HttpPost]
        public ActionResult Create(MedicalRecord medicalRecord)
        {

            //curl -H "Content-Type:application/json" -d @patient.json https://localhost:44342/apiMedicalRecordsData/AddMedicalRecords
            string url = "MedicalRecordsData/AddMedicalRecords";

            string jsonpayload = jss.Serialize(MedicalRecord);

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

        }*/

        // Get: /Edit/5
        [HttpPost]
        public ActionResult Edit(int id)
        {
            // TODO: Add update logic here

            string url = "DonorTransplantsData/FindDonorTransplant/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            MedicalRecordDto selectededicalRecord = response.Content.ReadAsAsync<MedicalRecordDto>().Result;


            /*UpdateMedicalRecord ViewModel = new UpdateMedicalRecord();

            return View(ViewModel);*/

            // comment code out because it fails the project build
            return View();

        }

        // comment code out because it fails the project build
        /*// POST: Patient/Update/5
        [HttpPost]
        public ActionResult Update(int id, MedicalRecord medicalRecord)
        {

            // to communicate with PatientData api controller to access list of Patient
            //curl https://localhost:44342/api/Patientdata/updatepatient/{id}
            string url = "MedicalRecordsData/UpdateMedicalRecords/" + id;


            string jsonpayload = jss.Serialize(medicalRecord);

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
        }*/


        // GET: MedicalrecordsData/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "MedicalRecordsData/FindMedicalRecords/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            MedicalRecordDto selectedMedicalRecord = response.Content.ReadAsAsync<MedicalRecordDto>().Result;

            return View(selectedMedicalRecord);
        }



        // POST: MedicalrecordsData/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "MedicalRecordsData/DeleteMedicalRecords/" + id;
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
