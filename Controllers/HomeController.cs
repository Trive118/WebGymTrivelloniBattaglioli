using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebGymTrivelloniBattaglioli.Models;

namespace WebApplicationPrimoMVCInserimentUjtente.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View("prova");
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult UtenteView()
        {
            return View();
        }

       /**
        [HttpPost]
        public ActionResult UtenteView(UtenteModel u)
        {
            if(ModelState.IsValid)
                return View("Index");
            return null;
        }**/
    }
}