using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocumentationSystem.Entity;
using DocumentationSystem.WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DocumentationSystem.WebApp.Controllers
{
    [Authorize(Roles = "admin")]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            return RedirectToAction("Role");
        }

        [Route("/admin/roles")]
        public IActionResult Role()
        {
            var roles = new List<GetRoleModel>();
            foreach (var item in _roleManager.Roles)
            {
                var role = new GetRoleModel()
                {
                    Id = item.Id,
                    Name = item.Name
                };
                roles.Add(role);
            }
            return View(roles);
        }
    }
}
