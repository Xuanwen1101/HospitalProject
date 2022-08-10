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
    public class PatientController : Controller
    {
            private static readonly HttpClient client;
            private JavaScriptSerializer jss = new JavaScriptSerializer();

            static PatientController()
            {
                client = new HttpClient();
                client.BaseAddress = new Uri("https://localhost:44342/api/");
            }
            // GET: Patient/List
            public ActionResult List()
            {

            // to communicate with PatientData api controller to access list of Patients
            //curl https://localhost:44342/api/Patientdata/listPatients

            string url = "patientdata/listpatients";
            HttpResponseMessage response = client.GetAsync(url).Result;

               // Debug.WriteLine("The response is ok");
               // Debug.WriteLine(response.StatusCode);
                
            IEnumerable<PatientDto> patients = response.Content.ReadAsAsync<IEnumerable<PatientDto>>().Result;

                return View(patients);
            }

        // GET: Patient/Details/5
        public ActionResult Details(int id)
        {
            
            // to communicate with PatientData api controller to access list of Patient
            //curl https://localhost:44342/api/Patientdata/findpatient/{id}

            string url = "patientdata/findpatient/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            PatientDto selectedpatient = response.Content.ReadAsAsync<PatientDto>().Result;

            DetailsPatient ViewModel = new DetailsPatient();

            ViewModel.SelectedPatient = selectedpatient;

            return View(ViewModel);
        }
        public ActionResult Error()
        {
            return View();
        }

        // GET: Patient/New
        public ActionResult New()
        {
            string url = "InsuranceData/ListInsurances";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<InsuranceDto> InsuranceOptions = response.Content.ReadAsAsync<IEnumerable<InsuranceDto>>().Result;

            return View(InsuranceOptions);
        }

        // POST: Patient/Create
        [HttpPost]
        public ActionResult Create(Patient patient)
        {
            //GetApplicationCookie();//get token credentials
            //curl -H "Content-Type:application/json" -d @patient.json https://localhost:44342/api/Patientdata/addpatient
            string url = "patientdata/Addpatient";

            string jsonpayload = jss.Serialize(patient);

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

        // GET: Patient/Edit/5
        public ActionResult Edit(int id)
        {
           
            // to communicate with PatientData api controller to access list of Patient
            //curl https://localhost:44342/api/Patientdata/editpatient/{id}

            string url = "patientdata/findpatient/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            PatientDto selectedpatient = response.Content.ReadAsAsync<PatientDto>().Result;

            UpdatePatient ViewModel = new UpdatePatient();

            ViewModel.SelectedPatient = selectedpatient;

            return View(ViewModel);
        }

        // POST: Patient/Update/5
        [HttpPost]
        public ActionResult Update(int id, Patient patient, HttpPostedFileBase PatientPicture)
        {

            // GetApplicationCookie();//get token credentials
            // to communicate with PatientData api controller to access list of Patient
            //curl https://localhost:44342/api/Patientdata/updatepatient/{id}

            string url = "PatientData/UpdatePatient/" + id;
            string jsonPayload = jss.Serialize(patient);

            HttpContent content = new StringContent(jsonPayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(content);

            //update request is successful, and we have image data
            if (response.IsSuccessStatusCode && PatientPicture != null)
            {
                //Updating the Patient picture as a separate request
                Debug.WriteLine("Calling Update Image method.");
                //Send over image data for player
                url = "PatientData/UploadPatientPicture/" + id;

                MultipartFormDataContent requestcontent = new MultipartFormDataContent();
                HttpContent imagecontent = new StreamContent(PatientPicture.InputStream);
                requestcontent.Add(imagecontent, "PatientPicture", PatientPicture.FileName);
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
            
            /*string url = "patientdata/updatepatient/" + id;


                string jsonpayload = jss.Serialize(patient);

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
                }*/
        }

        // GET: Patient/Delete/5
        
        public ActionResult DeleteConfirm(int id)
        {

            //to communicate with PatientData  api controller to access one Patient
            //curl https://localhost:44342/api/Patientdata/findpatient/{id}

            string url = "patientdata/findpatient/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            PatientDto selectedpatient = response.Content.ReadAsAsync<PatientDto>().Result;

            return View(selectedpatient);
        }

        // POST: Patient/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "patientdata/deletepatient/" + id;

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
