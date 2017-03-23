using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Apps.Web.Areas.BusFlee.Controllers
{
    public class HomeController : Controller
    {
        // GET: BusFlee/Home
        public ActionResult Index()
        {
            return View();
        }

        // GET: BusFlee/Main
        public ActionResult Main()
        {
            return View();
        }
    }
}