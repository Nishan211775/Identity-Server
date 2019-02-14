using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MvcApplication.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using AuthenticationOptions = IdentityServer4.Configuration.AuthenticationOptions;

namespace MvcApplication.Controllers
{
    public class HomeController : Controller
    {
        public async Task<IActionResult> Index()
        {
            Dictionary<string, string> data = new Dictionary<string, string>
            {
                {"Access Token",await HttpContext.GetTokenAsync("access_token") },
                {"Claims",JsonConvert.SerializeObject((from c in User.Claims select new { c.Type, c.Value }).ToList()) }
            };
            return View(data);
        }
        [Authorize]
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

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> Inventoy()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var response = string.Empty;
            try
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
               response = await client.GetStringAsync("http://localhost:59552/api/values");
                //response = JArray.Parse(content).ToString();
            }
            catch (Exception ex)
            {
                response = ex.Message;
            }

            ViewBag.Json = response;


            return View("json");
        }
        public async Task<IActionResult> Banking()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var response = string.Empty;
            try
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                response = await client.GetStringAsync("http://localhost:59553/api/values");
                //response = JArray.Parse(content).ToString();
            }
            catch (Exception ex)
            {
                response = ex.Message;
            }

            ViewBag.Json = response;
            return View("json");
        }

        public IActionResult Logout()
        {
            return SignOut(new AuthenticationProperties
            {
                RedirectUri = "Home/Index"
            }, "Cookies", "oidc");
        }
    }
}
