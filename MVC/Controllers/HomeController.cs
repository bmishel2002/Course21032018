using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using Newtonsoft.Json;

namespace MVC.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            List<UserModel> users = new List<UserModel>();

            using (var httpClinet = new HttpClient())
            {
                try
                {
                    HttpResponseMessage res = httpClinet.GetAsync("http://localhost:5000/api/users").Result;
                    res.EnsureSuccessStatusCode();
                    users = JsonConvert.DeserializeObject<List<UserModel>>(res.Content.ReadAsStringAsync().Result);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }


            }
            
            return View(users);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }
        
    }
}
