using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Diagnostics;
using HospitalProject_Group3.Models;
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
            }/// <summary>
            /// This is to list patients in the database
            /// </summary>
           
            /// <returns>all</returns>
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
        /// <summary>
        /// gets patient by it's id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>patient with id number</returns>
        // GET: Patient/Details/5
        public ActionResult Details(int id)
        {

            // to communicate with PatientData api controller to access list of Patient
            //curl https://localhost:44342/api/Patientdata/findpatient/{id}

            string url = "patientdata/findpatient/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            PatientDto selectedpatient = response.Content.ReadAsAsync<PatientDto>().Result;

            return View(selectedpatient);
        }
        public ActionResult Error()
        {
            return View();
        }
        /// <summary>
        /// creating new patient
        /// </summary>
        /// <returns></returns>
        // GET: Patient/New
        public ActionResult New()
        {
            return View();
        }
        /// <summary>
        /// creating a new patient
        /// </summary>
        /// <param name="patient"></param>
        /// <returns>sends new patient info to database</returns>
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
        /// <summary>
        /// makes changes to patient info based on provided id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>changed patient info</returns>
        // GET: Patient/Edit/5
        public ActionResult Edit(int id)
        {
           
            // to communicate with PatientData api controller to access list of Patient
            //curl https://localhost:44342/api/Patientdata/editpatient/{id}

            string url = "patientdata/findpatient/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            PatientDto selectedpatient = response.Content.ReadAsAsync<PatientDto>().Result;
            

            return View(selectedpatient);
        }
        /// <summary>
        /// changes old patient record to new patient info
        /// </summary>
        /// <param name="id"></param>
        /// <param name="patient"></param>
        /// <returns></returns>
        // POST: Patient/Update/5
        [HttpPost]
        public ActionResult Update(int id, Patient patient)
        {
            
                // to communicate with PatientData api controller to access list of Patient
                //curl https://localhost:44342/api/Patientdata/updatepatient/{id}
                string url = "patientdata/updatepatient/" + id;


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
                }
        }
        /// <summary>
        /// confirm if one ones to delete patient with id number from database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
        /// <summary>
        /// deleting patient with id from database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

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
