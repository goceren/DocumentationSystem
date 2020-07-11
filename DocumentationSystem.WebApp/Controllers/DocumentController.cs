using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DocumentationSystem.Business.Abstract;
using DocumentationSystem.Entity;
using DocumentationSystem.WebApp.Extensions;
using DocumentationSystem.WebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DocumentationSystem.WebApp.Controllers
{
    public class DocumentController : Controller
    {
        private readonly IDocumentService _documentService;
        private readonly UserManager<DocSysUser> _userManager;
        private readonly IDepartmentService _departmentService;

        public DocumentController(IDocumentService documentService, UserManager<DocSysUser> userManager, IDepartmentService departmentService)
        {
            _documentService = documentService;
            _userManager = userManager;
            _departmentService = departmentService;
        }

        [Route("/user/documentations")]
        public async Task<IActionResult> UserDocumentAsync()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var document = _documentService.GetByUserId(user.Id);
            return View(document);
        }

        [Route("/user/documentations/create")]
        public async Task<IActionResult> CreateDocumentAsync()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewBag.userId = user.Id;
            ViewBag.departments = new SelectList(_departmentService.GetAll().Where(i => i.DepartmentIsDeleted == false && i.DepartmentIsActive).ToList(), "DepartmentId", "DepartmentName");
            return View(new DocSysDocument());
        }

        [HttpPost, Route("/user/documentations/create")]
        public async Task<IActionResult> CreateDocumentAsync(DocSysDocument entity, IFormFile file)
        {
            if (User.IsInRole("user"))
            {
                entity.DocumentOpentoEveryone = false;
                entity.DocumentIsApproved = false;
            }

            if (file != null && file.ContentType.Contains("image"))
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img\\documents", file.FileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                entity.DocumentImage = file.FileName;
            }

            if (ModelState.IsValid)
            {
                entity.DocumentCreatedDate = DateTime.Now;
                entity.DocumentUpdatedTime = DateTime.Now;
                _documentService.Create(entity);
                TempData.Put("message", new ResultMessage()
                {
                    Title = "Bildirim",
                    Message = "Dökümantasyon başarıyla oluşturuldu. Yetkili onayından sonra yayınlanabilir.",
                    Css = "success"
                });
                return RedirectToAction("UserDocument");
            }
            TempData.Put("message", new ResultMessage()
            {
                Title = "Bildirim",
                Message = "Dökümantasyon oluşturulamadı. ",
                Css = "danger"
            });
            return View(entity);
        }

        [Route("/user/documentations/edit/{id?}")]
        public IActionResult EditDocument(int id)
        {
            ViewBag.departments = new SelectList(_departmentService.GetAll().Where(i => i.DepartmentIsDeleted == false && i.DepartmentIsActive).ToList(), "DepartmentId", "DepartmentName");
            return View(_documentService.GetById(id));
        }

        [HttpPost, Route("/user/documentations/edit/{id?}")]
        public async Task<IActionResult> EditDocumentAsync(DocSysDocument entity, IFormFile file)
        {
            ViewBag.departments = new SelectList(_departmentService.GetAll().Where(i => i.DepartmentIsDeleted == false && i.DepartmentIsActive).ToList(), "DepartmentId", "DepartmentName");
            var documentation = _documentService.GetById(entity.DocumentId);
            if (User.IsInRole("user"))
            {
                entity.DocumentIsApproved = documentation.DocumentIsApproved;
                entity.DocumentOpentoEveryone = documentation.DocumentOpentoEveryone;
            }
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

                if (ModelState.IsValid)
                {
                    entity.DocumentCreatedDate = documentation.DocumentCreatedDate;
                    entity.DocumentUpdatedTime = DateTime.Now;
                    _documentService.Update(entity);
                    TempData.Put("message", new ResultMessage()
                    {
                        Title = "Bildirim",
                        Message = "Dökümantasyon başarıyla güncellendi.",
                        Css = "success"
                    });
                    return RedirectToAction("UserDocument");
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

        [Route("/user/documentations/delete/{id?}")]
        public IActionResult DeleteDocument(int id)
        {
            var document = _documentService.GetById(id);
            if (document != null)
            {
                document.DocumentIsDeleted = true;
                _documentService.Update(document);
                TempData.Put("message", new ResultMessage()
                {
                    Title = "Bildirim",
                    Message = "Dökümantasyon Silindi",
                    Css = "success"
                });
                return RedirectToAction("UserDocument");
            }
            TempData.Put("message", new ResultMessage()
            {
                Title = "Bildirim",
                Message = "Dökümantasyon Silindi",
                Css = "danger"
            });
            return RedirectToAction("UserDocument");
        }

        [Route("/user/documentations/undelete/{id?}")]
        public IActionResult UnDeleteDocument(int id)
        {
            var document = _documentService.GetById(id);
            if (document != null)
            {
                document.DocumentIsDeleted = false;
                _documentService.Update(document);
                TempData.Put("message", new ResultMessage()
                {
                    Title = "Bildirim",
                    Message = "Dökümantasyon Geri Alma İşlemi Başarılı",
                    Css = "success"
                });
                return RedirectToAction("UserDocument");
            }
            TempData.Put("message", new ResultMessage()
            {
                Title = "Bildirim",
                Message = "Dökümantasyon Geri Alma İşlemi Başarısız",
                Css = "danger"
            });
            return RedirectToAction("UserDocument");
        }
    }
}
