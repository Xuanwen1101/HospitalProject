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
    public class DepartmentController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static DepartmentController()
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


        /// GET: Department/List
        public ActionResult List()
        {
            //objective: communicate with the data api to retrieve a list of Departments
            //curl https://localhost:44342/api/DepartmentData/ListDepartments

            string url = "DepartmentData/ListDepartments";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<DepartmentDto> departmentOptions = response.Content.ReadAsAsync<IEnumerable<DepartmentDto>>().Result;

            ViewModel.DepartmentOptions = departmentOptions;

            return View(ViewModel);
        }


        // GET: Department/Details/5
        public ActionResult Details(int id)
        {
            DetailsDepartment ViewModel = new DetailsDepartment();

            string url = "DepartmentData/FindDepartment/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            DepartmentDto SelectedDepartment = response.Content.ReadAsAsync<DepartmentDto>().Result;

            ViewModel.SelectedDepartment = SelectedDepartment;


            url = "StaffData/ListStaffsForDepartment/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<StaffDto> deptStaff = response.Content.ReadAsAsync<IEnumerable<StaffDto>>().Result;

            ViewModel.DeptStaff = deptStaff;


            return View(ViewModel);
        }


        public ActionResult Error()
        {

            return View();
        }


        // GET: Department/New
        /*[Authorize]*/
        public ActionResult New()
        {

            return View();
        }

        // POST: Department/Create
        [HttpPost]
        /*[Authorize]*/
        public ActionResult Create(Department department)
        {
            GetApplicationCookie();//get token credentials
            //objective: add a new Department into our system using the API
            //curl -H "Content-Type:application/json" -d @department.json https://localhost:44342/api/DepartmentData/AddDepartment 
            string url = "DepartmentData/AddDepartment";


            string jsonPayload = jss.Serialize(role);

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


        // GET: Department/Edit/5
        /*[Authorize]*/
        public ActionResult Edit(int id)
        {
            UpdateDepartment ViewModel = new UpdateDepartment();

            //get the existing department information
            //curl https://localhost:44349/api/DepartmentData/FindDepartment/{id}
            string url = "DepartmentData/FindDepartment/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            DepartmentDto SelectedDepartment = response.Content.ReadAsAsync<DepartmentDto>().Result;
            ViewModel.SelectedDepartment = SelectedDepartment;

            return View(ViewModel);
        }


        // POST: Department/Update/5
        [HttpPost]
        /*[Authorize]*/
        public ActionResult Update(int id, Department department)
        {
            GetApplicationCookie();//get token credentials

            string url = "DepartmentData/UpdateDepartment/" + id;
            string jsonPayload = jss.Serialize(department);

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

        // GET: Department/DeleteConfirm/5
        /*[Authorize]*/
        public ActionResult DeleteConfirm(int id)
        {
            // get the existing department information
            //curl https://localhost:44342/api/DepartmentData/FindDepartment/{id}
            string url = "DepartmentData/FindDepartment/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            DepartmentDto selectedDepartment = response.Content.ReadAsAsync<DepartmentDto>().Result;

            return View(selectedDepartment);

        }

        // POST: Department/Delete/5
        [HttpPost]
        /*[Authorize]*/
        public ActionResult Delete(int id)
        {
            GetApplicationCookie();//get token credentials
            //objective: delete the selected department from our system using the API
            //curl -d "" https://localhost:44342/api/DepartmentData/DeleteDepartment/{id}
            string url = "DepartmentData/DeleteDepartment/" + id;
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
