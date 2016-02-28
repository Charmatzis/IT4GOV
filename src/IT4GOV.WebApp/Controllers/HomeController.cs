using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Authorization;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using IT4GOV.WebApp.Extensions;

namespace IT4GOV.WebApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
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

        public IActionResult Error()
        {
            return View();
        }




        [Authorize]
        public IActionResult Secure()
        {
            

            return View();
        }

        [Authorize]
        public async Task<IActionResult> CallApi()
        {
            var token = User.FindFirst("access_token").Value;

            var client = new HttpClient();
            client.SetBearerToken(token);

            //WebApi Url
            var response = await client.GetStringAsync(URLS.Api_url+"identity");
            ViewBag.Json = JArray.Parse(response).ToString();

            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.Authentication.SignOutAsync("cookies");
            return Redirect("~/");
        }

       
    }
}
