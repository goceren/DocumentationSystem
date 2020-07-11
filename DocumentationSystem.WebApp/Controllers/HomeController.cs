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
using DocumentationSystem.Business.Abstract;
using Microsoft.EntityFrameworkCore;

namespace DocumentationSystem.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDocumentService _documentService;
        private readonly UserManager<DocSysUser> _userManager;
        private readonly IDepartmentService _departmentService;

        public HomeController(IDocumentService documentService, UserManager<DocSysUser> userManager, IDepartmentService departmentService)
        {
            _documentService = documentService;
            _userManager = userManager;
            _departmentService = departmentService;
        }
        public async Task<IActionResult> IndexAsync(string id, string search, string userId)
        {
            ViewBag.search = search;
            var document = _documentService.GetAllWithDepartmentAndUser().Where(i => i.DocumentIsActive && i.DocumentIsApproved && i.DocumentIsDeleted == false).OrderByDescending(d => d.DocumentUpdatedTime).ToList();
            var department = _departmentService.GetAllWithDocumentAndUser().Where(i => i.DepartmentIsActive == true && i.DepartmentIsDeleted == false).ToList();
            HomeModel model = new HomeModel();
            model.Departments = department;

            if (!string.IsNullOrEmpty(userId))
            {
                model.Documents = _documentService.GetAllWithDepartmentAndUser().Where(i => i.DocumentIsActive && i.DocumentIsApproved && i.DocumentIsDeleted == false && i.UserId == userId).OrderByDescending(d => d.DocumentUpdatedTime).ToList();
                return View(model);

            }

            if (!string.IsNullOrEmpty(search))
            {
                model.Documents = _documentService.GetAllWithDepartmentAndUser().Where(i => i.DocumentIsActive && i.DocumentIsApproved && i.DocumentIsDeleted == false && EF.Functions.Like(i.DocumentTitle, "%" + search + "%") || EF.Functions.Like(i.DocumentDescription, "%" + search + "%")).OrderByDescending(d => d.DocumentUpdatedTime).ToList();
                return View(model);

            }
            if (!string.IsNullOrEmpty(id))
            {
                int aranan = int.Parse(id);
                model.Documents = _documentService.GetAllWithDepartmentAndUser().Where(i => i.DocumentIsActive && i.DocumentIsApproved && i.DocumentIsDeleted == false && i.DepartmentId == aranan).OrderByDescending(d => d.DocumentUpdatedTime).ToList();
                ViewBag.selected = aranan;
                return View(model);

            }
            model.Documents = document;
            return View(model);
        }

        public IActionResult Detail(int id)
        {
            var document = _documentService.GetByIdWithDepartmentAndUser(id);
            if (document != null)
            {
                return View(document);
            }
            return View();
        }
    }
}
