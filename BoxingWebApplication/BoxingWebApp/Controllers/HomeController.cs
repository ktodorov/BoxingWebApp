using BoxingWebApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BoxingWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWebClientService webClient;

        public HomeController(IWebClientService webClient)
        {
            this.webClient = webClient;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Chat()
        {
            return View();
        }
    }
}