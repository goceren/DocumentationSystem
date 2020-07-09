using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DocumentationSystem.WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using DocumentationSystem.Entity;

namespace DocumentationSystem.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<DocSysUser> _userManager;
        
        public HomeController(UserManager<DocSysUser> userManager)
        {
            _userManager = userManager;
        }
        public IActionResult IndexAsync()
        {
            ViewBag.UserCount = _userManager.Users.Where(i => i.isApprovedByAdmin).Count();
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
