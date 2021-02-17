using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AspNetCoreIdentity.Models;
using Microsoft.AspNetCore.Authorization;
using AspNetCoreIdentity.Extension;

namespace AspNetCoreIdentity.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult Privacy()
        {
            return View();
        }

        [Authorize(Roles = "Admin, Manager")]
        public IActionResult Secret()
        {
            return View();
        }

        //Claim
        [Authorize(Policy = "AuthorizedDelete")]
        public IActionResult SecretClaim()
        {
            return View();
        }

        //Claim
        [Authorize(Policy = "Write")]
        public IActionResult SecretClaimWrite()
        {
            return View();
        }

        //Curdston Claims Authentication
        [ClaimsAuthorizeAttribute("Product", "Read")]
        public IActionResult SecretClaimCustomWrite()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
