using DocumentationSystem.Business.Abstract;
using DocumentationSystem.Entity;
using DocumentationSystem.WebApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentationSystem.WebApp.ViewComponents
{
    public class CountsViewComponent : ViewComponent
    {
        private readonly UserManager<DocSysUser> _userManager;
        private readonly IDocumentService _documentService;

        public CountsViewComponent(UserManager<DocSysUser> userManager, IDocumentService documentService)
        {
            _userManager = userManager;
            _documentService = documentService;
        }
        public IViewComponentResult Invoke()
        {
            CountsModel countsModel = new CountsModel()
            {
                DocumentCounts = _documentService.GetAll().Where(i => i.DocumentIsActive && i.DocumentIsApproved == false && i.DocumentIsDeleted == false).Count(),
                UserCounts = _userManager.Users.Where(i => i.isApprovedByAdmin == false && i.isDeleted == false).Count()
            };
            return View(countsModel);
        }
    }
}
