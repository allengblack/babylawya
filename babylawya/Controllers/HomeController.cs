using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using babylawya.Models;

namespace babylawya.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult PersonalAndFamily()
        {
            ViewData["Message"] = "Personal and Family.";

            return View();
        }

        public IActionResult PropertyAndFinance()
        {
            ViewData["Message"] = "Property and Finance.";

            return View();
        }

        public IActionResult RunningBusiness()
        {
            ViewData["Message"] = "Running your Business.";

            return View();
        }

        public IActionResult HowItWorks()
        {
            ViewData["Message"] = "How it Works.";

            return View();
        }

        public IActionResult AskLawyer()
        {
            ViewData["Message"] = "How it Works.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
