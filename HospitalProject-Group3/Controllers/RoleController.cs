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
    public class RoleController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static RoleController()
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


        /// GET: Role/List
        public ActionResult List()
        {
            //objective: communicate with the data api to retrieve a list of Roles
            //curl https://localhost:44342/api/RoleData/ListRoles

            string url = "RoleData/ListRoles";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<RoleDto> staffs = response.Content.ReadAsAsync<IEnumerable<RoleDto>>().Result;

            return View(staffs);
        }


        // GET: Role/Details/5
        public ActionResult Details(int id)
        {
            DetailsRole ViewModel = new DetailsRole();

            string url = "RoleData/FindRole/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            RoleDto SelectedRole = response.Content.ReadAsAsync<RoleDto>().Result;

            ViewModel.SelectedRole = SelectedRole;


            url = "StaffData/ListStaffsForRole/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<StaffDto> workedStaffs = response.Content.ReadAsAsync<IEnumerable<StaffDto>>().Result;

            ViewModel.WorkedStaffs = workedStaffs;


            return View(ViewModel);
        }


        public ActionResult Error()
        {

            return View();
        }


        // GET: Role/New
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

        // POST: Role/Create
        [HttpPost]
        /*[Authorize]*/
        public ActionResult Create(Role role)
        {
            GetApplicationCookie();//get token credentials
            //objective: add a new role into our system using the API
            //curl -H "Content-Type:application/json" -d @role.json https://localhost:44342/api/RoleData/AddRole 
            string url = "RoleData/AddRole";


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


        // GET: Role/Edit/5
        /*[Authorize]*/
        public ActionResult Edit(int id)
        {
            UpdateRole ViewModel = new UpdateRole();

            //get the existing role information
            //curl https://localhost:44349/api/RoleData/FindRole/{id}
            string url = "RoleData/FindRole/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            RoleDto SelectedRole = response.Content.ReadAsAsync<RoleDto>().Result;
            ViewModel.SelectedRole = SelectedRole;

            /*
            url = "DepartmentData/ListDepartments/";
            response = client.GetAsync(url).Result;
            IEnumerable<DepartmentDto> departmentOptions = response.Content.ReadAsAsync<IEnumerable<DepartmentDto>>().Result;

            ViewModel.DepartmentOptions = departmentOptions;
            */

            return View(ViewModel);
        }


        // POST: Role/Update/5
        [HttpPost]
        /*[Authorize]*/
        public ActionResult Update(int id, Role role)
        {
            GetApplicationCookie();//get token credentials

            string url = "RoleData/UpdateRole/" + id;
            string jsonPayload = jss.Serialize(role);

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

        // GET: Role/DeleteConfirm/5
        /*[Authorize]*/
        public ActionResult DeleteConfirm(int id)
        {
            // get the existing role information
            //curl https://localhost:44342/api/RoleData/FindRole/{id}
            string url = "RoleData/FindRole/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            RoleDto selectedRole = response.Content.ReadAsAsync<RoleDto>().Result;

            return View(selectedRole);

        }

        // POST: Role/Delete/5
        [HttpPost]
        /*[Authorize]*/
        public ActionResult Delete(int id)
        {
            GetApplicationCookie();//get token credentials
            //objective: delete the selected role from our system using the API
            //curl -d "" https://localhost:44342/api/RoleData/DeleteRole/{id}
            string url = "RoleData/DeleteRole/" + id;
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
