﻿using System;
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
    public class InsuranceController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static InsuranceController()
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

        /// GET: Insurance/List
        public ActionResult List()
        {
            //objective: communicate with the data api to retrieve a list of Shifts
            //curl https://localhost:44342/api/InsuranceData/ListInsurances

            string url = "InsuranceData/ListInsurances";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<InsuranceDto> Insurances = response.Content.ReadAsAsync<IEnumerable<InsuranceDto>>().Result;

            return View(Insurances);
        }

        // GET: Insurance/Details/5
        public ActionResult Details(int id)
        {
            DetailsInsurance ViewModel = new DetailsInsurance();

            string url = "InsuranceData/FindInsurance/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            InsuranceDto SelectedInsurance = response.Content.ReadAsAsync<InsuranceDto>().Result;

            ViewModel.SelectedInsurance = SelectedInsurance;
            return View(ViewModel);
        }


        // GET: Insurance/New
        /*[Authorize]*/
        public ActionResult New()
        {
            return View();
        }

        // POST: Insurance/Create
        [HttpPost]
        /*[Authorize]*/
        public ActionResult Create(Insurance insurance)
        {
            GetApplicationCookie();//get token credentials
            //objective: add a new Insurance into our system using the API
            //curl -H "Content-Type:application/json" -d @Insurance.json https://localhost:44342/api/MedicationData/AddMedication
            string url = "InsuranceData/AddInsurance";

            string jsonPayload = jss.Serialize(insurance);

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

        // GET: Insurance/Edit/5
        /*[Authorize]*/
        public ActionResult Edit(int id)
        {
            UpdateInsurance ViewModel = new UpdateInsurance();

            //get the existing Insurance information
            //curl https://localhost:44342/api/InsuranceData/FindInsurance/{id}
            string url = "InsuranceData/FindInsurance/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            InsuranceDto SelectedInsurance = response.Content.ReadAsAsync<InsuranceDto>().Result;
            ViewModel.SelectedInsurance = SelectedInsurance;

            return View(ViewModel);
        }

        // POST: Insurance/Update/5
        [HttpPost]
        /*[Authorize]*/
        public ActionResult Update(int id, Insurance insurance)
        {
            GetApplicationCookie();//get token credentials
            //objective: update the selected Insurance into our system using the API
            //curl -H "Content-Type:application/json" -d @Insurance.json  https://localhost:44342/api/InsuranceData/UpdateInsurance/{id}
            string url = "InsuranceData/UpdateInsurance/" + id;
            string jsonPayload = jss.Serialize(insurance);

            HttpContent content = new StringContent(jsonPayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(content);

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

        // GET: Insurance/DeleteConfirm/5
        /*[Authorize]*/
        public ActionResult DeleteConfirm(int id)
        {
            // get the existing role information
            //curl https://localhost:44342/api/InsuranceData/FindInsurance/{id}
            string url = "InsuranceData/FindInsurance/" + id;

            HttpResponseMessage response = client.GetAsync(url).Result;
            InsuranceDto selectedInsurance = response.Content.ReadAsAsync<InsuranceDto>().Result;

            return View(selectedInsurance);

        }

        // POST: Insurance/Delete/5
        [HttpPost]
        /*[Authorize]*/
        public ActionResult Delete(int id)
        {
            GetApplicationCookie();//get token credentials
            //objective: delete the selected Insurance from our system using the API
            //curl -d "" https://localhost:44342/api/InsuranceData/DeleteInsurance/{id}
            string url = "InsuranceData/DeleteInsurance/" + id;

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