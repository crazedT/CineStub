using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CineStub.Service;

namespace CineStub.Web.Controllers
{
    public class HomeController : Controller
    {

        public HomeController()
        {
        }

        [RequireHttps]
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }
    }
}
