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
    public class AppointmentController : Controller
    {

        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static AppointmentController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44342/api/");
        }
        // GET: Appointment/List
        public ActionResult List()
        {
            // to communicate with BillData api controller to access list of Bills
            //curl https://localhost:44342/api/Appointmentdata/listappointments

            string url = "appointmentdata/listappointments";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<AppointmentDto> appointments = response.Content.ReadAsAsync<IEnumerable<AppointmentDto>>().Result;

            return View(appointments);
        }

        // GET: Appointment/Details/5
        public ActionResult Details(int id)
        {
            // to communicate with AppointmentData api controller to access list of Appointment
            //curl https://localhost:44342/api/Appointmentdata/findappointment/{id}

            string url = "appointmentdata/findappointment/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            AppointmentDto selectedappointment = response.Content.ReadAsAsync<AppointmentDto>().Result;


            return View(selectedappointment);
        }
        public ActionResult Error()
        {
            return View();
        }

        // GET: Appointment/New
        public ActionResult New()
        {
            return View();
        }


        // POST: Appointment/Create
        [HttpPost]
        public ActionResult Create(Appointment appointment)
        {
            //curl -H "Content-Type:application/json" -d @Appointment.json https://localhost:44342/api/Appointmentdata/addappointment
            string url = "appointmentdata/addappointment";

            string jsonpayload = jss.Serialize(appointment);

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


        // GET: Appointment/Edit/5
        public ActionResult Edit(int id)
        {
            // to communicate with AppointmentData api controller to access list of Appointment
            //curl https://localhost:44342/api/Appointmentdata/editappointment/{id}

            string url = "appointmentdata/findappointments/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

           AppointmentDto selectedappointment = response.Content.ReadAsAsync<AppointmentDto>().Result;

            return View(selectedappointment);
        }

        // POST: Appointment/Update/5
        [HttpPost]
        public ActionResult Update(int id, Appointment appointment)
        {
            // to communicate with AppointmentData api controller to access list of Appointment
            //curl https://localhost:44342/api/Appointmentdata/updateappointment/{id}
            string url = "appointmentdata/updateappointment/" + id;
            
            string jsonpayload = jss.Serialize(appointment);

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

        // GET: Appointment/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            // to communicate with AppointmentData api controller to access list of Appointment
            //curl https://localhost:44342/api/Appointmentdata/findappointment/{id}

            string url = "appointmentdata/findappointment/" + id;

            HttpResponseMessage response = client.GetAsync(url).Result;

            AppointmentDto selectedappointment = response.Content.ReadAsAsync<AppointmentDto>().Result;

            return View(selectedappointment);
        }

        // POST: Appointment/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "Appointmentdata/deleteappointment/" + id;

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
