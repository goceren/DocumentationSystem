using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DocumentationSystem.Business.Abstract;
using DocumentationSystem.Entity;
using DocumentationSystem.WebApp.Extensions;
using DocumentationSystem.WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DocumentationSystem.WebApp.Controllers
{
    [Authorize(Roles = "admin, staff")]
    public class ConfirmDocumentController : Controller
    {
        private readonly IDocumentService _documentService;
        private readonly UserManager<DocSysUser> _userManager;
        private readonly IDepartmentService _departmentService;

        public ConfirmDocumentController(IDocumentService documentService, UserManager<DocSysUser> userManager, IDepartmentService departmentService)
        {
            _documentService = documentService;
            _userManager = userManager;
            _departmentService = departmentService;
        }

        [Route("/documents/waiting")]
        public IActionResult Index()
        {
            var documents = _documentService.GetAllWithDepartmentAndUser().Where(i => i.DocumentIsApproved == false && i.DocumentIsActive == true && i.DocumentIsDeleted == false).ToList();
            return View(documents);
        }

        [Route("/documents/confirm/{id?}")]
        public IActionResult ConfirmDocument(int id)
        {

            var document = _documentService.GetByIdWithDepartmentAndUser(id);
            if (document != null)
            {
                document.DocumentIsApproved = true;
                _documentService.Update(document);
                TempData.Put("message", new ResultMessage()
                {
                    Title = "Dökümantasyon Onayı",
                    Message = "Dökümantasyon başarıyla onaylandı. Düzenlemek için panel menüsünden Dökümantasyonları görüntüleyebilirsiniz.",
                    Css = "success"
                });
                return RedirectToAction("Index");
            }
            TempData.Put("message", new ResultMessage()
            {
                Title = "Kullanıcı Onayı",
                Message = "Kullanıcı onaylama işlemi başarısız",
                Css = "danger"
            });
            return RedirectToAction("Index");
        }

        [Route("/documentations/edit/{id?}")]
        public IActionResult EditDocument(int id)
        {
            ViewBag.departments = new SelectList(_departmentService.GetAll(), "DepartmentId", "DepartmentName");
            return View(_documentService.GetById(id));
        }

        [HttpPost, Route("documentations/edit/{id?}")]
        public async Task<IActionResult> EditDocumentAsync(DocSysDocument entity, IFormFile file)
        {
            var documentation = _documentService.GetById(entity.DocumentId);
            if (documentation != null)
            {
                if (file != null && file.ContentType.Contains("image"))
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img\\documents", file.FileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    entity.DocumentImage = file.FileName;
                }
                else
                {
                    entity.DocumentImage = documentation.DocumentImage;
                }

                if (ModelState.IsValid)
                {
                    entity.DocumentCreatedDate = documentation.DocumentCreatedDate;
                    entity.DocumentUpdatedTime = documentation.DocumentUpdatedTime;
                    _documentService.Update(entity);
                    TempData.Put("message", new ResultMessage()
                    {
                        Title = "Bildirim",
                        Message = "Dökümantasyon başarıyla güncellendi.",
                        Css = "success"
                    });
                    return RedirectToAction("Index");
                }
            }

            TempData.Put("message", new ResultMessage()
            {
                Title = "Bildirim",
                Message = "Dökümantasyon güncellenemedi. ",
                Css = "danger"
            });
            return View(entity);
        }
    }
        
}
