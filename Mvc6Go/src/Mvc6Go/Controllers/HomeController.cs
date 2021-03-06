﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using System.Security.Claims;
using Microsoft.AspNet.Authentication.Cookies;
using Microsoft.AspNet.Authorization;
using Mvc6Go.System;
using Mvc6Go.Business;

namespace Mvc6Go.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        { 
            return View();
        }

        public string Login() {
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim("Name", "TestName", ClaimValueTypes.String));
            claims.Add(new Claim(ClaimTypes.Name, "TestName", ClaimValueTypes.String));
            ClaimsIdentity identity = new ClaimsIdentity(claims, "AuthenticationType", "Name", ClaimTypes.Role);
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);
            HttpContext.Authentication.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal); 
            return "OK";
        }

        public T GetRequestService<T>() where T :class
        {
            return this.HttpContext.RequestServices.GetService(typeof(T)) as T;
        }

        [Authorize]
        public IActionResult TestAjax()
        {
            var factory = GetRequestService<DataControllerFactory>();
            //TestDataController testDc = factory.c
            //ViewBag.UserName = testDc.GetUserName(); 

            ViewBag.UserName = factory.UserName;
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
    }
}
