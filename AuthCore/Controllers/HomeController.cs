﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AuthCore.Models;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft;
using System.Security.Claims;

namespace AuthCore.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Authorize("OnlyForMicrosoft_Name")]
        [Authorize("pname")]
        // [Authorize("pname1")]

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";
            var d=User.FindFirst(_ => _.Type == ClaimsIdentity.DefaultRoleClaimType).Value;
            var g = User.Claims.Where(_ => _.Type == ClaimsIdentity.DefaultRoleClaimType).FirstOrDefault()?.Value;
            return Content(User.Identity.Name + " " + g + " " + d);
            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
