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
    public class ShiftController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static ShiftController()
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


        /// GET: Shift/List
        public ActionResult List()
        {
            //objective: communicate with the data api to retrieve a list of Shifts
            //curl https://localhost:44342/api/ShiftData/ListShifts

            string url = "ShiftData/ListShifts";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<ShiftDto> staffs = response.Content.ReadAsAsync<IEnumerable<ShiftDto>>().Result;

            return View(staffs);
        }


        // GET: Shift/Details/5
        public ActionResult Details(int id)
        {
            DetailsShift ViewModel = new DetailsShift();

            string url = "ShiftData/FindShift/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            ShiftDto SelectedShift = response.Content.ReadAsAsync<ShiftDto>().Result;

            ViewModel.SelectedShift = SelectedShift;


            url = "StaffData/ListStaffsForShift/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<StaffDto> workedStaffs = response.Content.ReadAsAsync<IEnumerable<StaffDto>>().Result;

            ViewModel.WorkedStaffs = workedStaffs;


            return View(ViewModel);
        }


        public ActionResult Error()
        {

            return View();
        }


        // GET: Shift/New
        /*[Authorize]*/
        public ActionResult New()
        {
            return View();
        }

        // POST: Shift/Create
        [HttpPost]
        [Authorize]
        public ActionResult Create(Shift shift)
        {
            GetApplicationCookie();//get token credentials

            string url = "ShiftData/AddShift";
            string jsonPayload = jss.Serialize(shift);

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


        // GET: Shift/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            UpdateShift ViewModel = new UpdateShift();

            //get the existing shift information
            //curl https://localhost:44342/api/ShiftData/FindShift/{id}
            string url = "ShiftData/FindShift/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            ShiftDto SelectedShift = response.Content.ReadAsAsync<ShiftDto>().Result;
            ViewModel.SelectedShift = SelectedShift;


            return View(ViewModel);
        }

        // POST: Shift/Update/5
        [HttpPost]
        [Authorize]
        public ActionResult Update(int id, Shift shift)
        {
            GetApplicationCookie();//get token credentials

            string url = "ShiftData/UpdateShift/" + id;
            string jsonPayload = jss.Serialize(shift);

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

        // GET: Shift/DeleteConfirm/5
        [Authorize]
        public ActionResult DeleteConfirm(int id)
        {
            // get the existing shift information
            //curl https://localhost:44342/api/ShiftData/FindShift/{id}
            string url = "ShiftData/FindShift/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            ShiftDto selectedShift = response.Content.ReadAsAsync<ShiftDto>().Result;

            return View(selectedShift);

        }

        // POST: Shift/Delete/5
        [HttpPost]
        [Authorize]
        public ActionResult Delete(int id)
        {
            GetApplicationCookie();//get token credentials
            //objective: delete the selected shift from our system using the API
            //curl -d "" https://localhost:44342/api/ShiftData/DeleteShift/{id}
            string url = "ShiftData/DeleteShift/" + id;
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
