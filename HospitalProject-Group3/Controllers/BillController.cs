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
    
    public class BillController : Controller
    {

        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static BillController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44342/api/");
        }
        // GET: Bill/List
        public ActionResult List()
        {
            // to communicate with BillData api controller to access list of Bills
            //curl https://localhost:44342/api/Billdata/listbills
            
            string url = "billdata/listbills";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<BillDto> bills = response.Content.ReadAsAsync<IEnumerable<BillDto>>().Result;


            return View(bills);
        }

        // GET: Bill/Details/5
        public ActionResult Details(int id)
        {
            // to communicate with BillData api controller to access list of Bill
            //curl https://localhost:44342/api/Billdata/findbill/{id}

            string url = "billdata/findbill/"+id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            BillDto selectedbill = response.Content.ReadAsAsync<BillDto>().Result;


            return View(selectedbill);
        }

        public ActionResult Error()
        {
            return View();
        }

        // GET: Bill/New
        public ActionResult New()
        {
            string url = "PatientData/ListPatients";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<PatientDto> PatientOptions = response.Content.ReadAsAsync<IEnumerable<PatientDto>>().Result;

            return View(PatientOptions);
            /*return View();*/
        }

        // POST: Bill/Create
        [HttpPost]
        public ActionResult Create(Bill bill)
        {

            //curl -H "Content-Type:application/json" -d @bill.json https://localhost:44342/api/Billdata/addbill
            string url = "billdata/addbill";

            string jsonpayload = jss.Serialize(bill);

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

        // GET: Bill/Edit/5
        public ActionResult Edit(int id)
        {
            // to communicate with BillData api controller to access list of Bill
            //curl https://localhost:44342/api/Billdata/editbill/{id}

            string url = "billdata/findbill/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
           
            BillDto selectedbill = response.Content.ReadAsAsync<BillDto>().Result;


            return View(selectedbill);
        }

        // POST: Bill/Update/5
        [HttpPost]
        public ActionResult Update(int id, Bill bill)
        {
            // to communicate with BillData api controller to access list of Bill
            //curl https://localhost:44342/api/Billdata/updatebill/{id}
            string url = "billdata/updatebill/" + id; 


            string jsonpayload = jss.Serialize(bill);

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
        

        // GET: Bill/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            //to communicate with BillData  api controller to access one Bill
            //curl https://localhost:44342/api/Billdata/findbill/{id}

             string url = "billdata/findbill/" + id;
             HttpResponseMessage response = client.GetAsync(url).Result;
       
             BillDto selectedBill = response.Content.ReadAsAsync<BillDto>().Result;
       
            return View(selectedBill);
        }

        // POST: Bill/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "billdata/deletebill/" + id;

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
