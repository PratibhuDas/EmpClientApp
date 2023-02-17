using EmpClientApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;
namespace EmpClientApp.Controllers
{
    public class HomeController : Controller
    {
        HttpClient client = new HttpClient();
        public IActionResult Index()
        {
            List<Employee> employees = new List<Employee>();
            var response = client.GetAsync("http://localhost:5062/api/Employee");
            response.Wait();
            var test=response.Result;
            if(test.IsSuccessStatusCode)
            {
                var e=test.Content.ReadAsAsync<List<Employee>>();
                e.Wait();
                employees=e.Result;

            }
            return View(employees);
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();  
        }
        [HttpPost]
        public ActionResult Create(Employee employee)
        {
            var response = client.PostAsJsonAsync<Employee>("http://localhost:5062/api/Employee", employee);
            response.Wait();
            var test=response.Result;
            if(test.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View();
        }
        [Route("Home/Index/{id}")]
        [HttpGet]
        public Employee Index(int id)
        {
            var response = client.GetAsync("http://localhost:5062/api/Employee/" + id);
            response.Wait();
            var test = response.Result;
            if (test.IsSuccessStatusCode)
            {
                var result = test.Content.ReadAsAsync<Employee>();
                result.Wait();
                Employee finalResult = result.Result;
                return finalResult;
            }
            return null;
        }
        public ActionResult Edit(int id) 
        {
           Employee result=Index(id);
            if(result.Id==id)
            {
                return View(result);
            }
            return RedirectToAction("Not Found");
        }
        [HttpPost]
        public ActionResult Edit(Employee employee) 
        {
            var response = client.PutAsJsonAsync("http://localhost:5062/api/Employee/" + employee.Id, employee);
            response.Wait();
            var test=response.Result;
            if(test.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Not Found");
        }
        [HttpGet]
        public ActionResult Details(int id) 
        {
            Employee response=Index(id);
            if(response.Id==id)
            {
                return View(response);
            }
            return RedirectToAction("Not Found");

        }
        
        public ActionResult Delete(int id)
        {
            var response = client.DeleteAsync("http://localhost:5062/api/Employee/" + id);
            response.Wait();
            var test=response.Result;
            if(test.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Not Found");
        }
        [HttpGet]
        public ActionResult NotFound()
        {
            return View();
        }


    }
}
