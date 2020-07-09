using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocumentationSystem.Business.Abstract;
using DocumentationSystem.Entity;
using DocumentationSystem.WebApp.Extensions;
using DocumentationSystem.WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DocumentationSystem.WebApp.Controllers
{
    [Authorize(Roles = "admin")]
    public class DepartmentController : Controller
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }
        public IActionResult Index()
        {
            return RedirectToAction("Departments");
        }
        [Route("/admin/departments")]
        public IActionResult Departments()
        {
            var list = new List<DepartmentModel>();
            foreach (var item in _departmentService.GetAll())
            {
                var model = new DepartmentModel()
                {
                    DeparmentDetail = item.DeparmentDetail,
                    DepartmentIsActive = item.DepartmentIsActive,
                    DepartmentName = item.DepartmentName,
                    DepartmentCreatedDate = item.DepartmentCreatedDate,
                    DepartmentId = item.DepartmentId,
                    DepartmentIsDeleted = item.DepartmentIsDeleted,
                    DepartmentPhone = item.DepartmentPhone,
                };
                list.Add(model);
            }
            return View(list);
        }

        [Route("/admin/department/create")]
        public IActionResult CreateDepartment()
        {
            return View(new DepartmentModel());
        }
        [HttpPost, Route("/admin/department/create")]
        public IActionResult CreateDepartment(DocSysDepartments entity)
        {
            if (ModelState.IsValid)
            {
                entity.DepartmentCreatedDate = DateTime.Now;
                _departmentService.Create(entity);
                TempData.Put("message", new ResultMessage()
                {
                    Title = "Bildirim",
                    Message = "Departman başarıyla oluşturuldu.",
                    Css = "success"
                });
                return RedirectToAction("Index");
            }
            return View(entity);
        }

        [Route("/admin/department/edit/{id?}")]
        public IActionResult EditDepartment(int id)
        {
            var department = _departmentService.GetById(id);
            if (department != null)
            {
                return View(department);
            }
            return RedirectToAction("Index");
        }

        [HttpPost, Route("/admin/department/edit/{id?}")]
        public IActionResult EditDepartment(DocSysDepartments model)
        {
            if (ModelState.IsValid)
            {
                DocSysDepartments department = _departmentService.GetById(model.DepartmentId);
                if (department == null)
                {
                    TempData.Put("message", new ResultMessage()
                    {
                        Title = "Departman Güncelle",
                        Message = "Departman Bulunamadı",
                        Css = "danger"
                    });
                    return RedirectToAction("Index");
                }
                department.DeparmentDetail = model.DeparmentDetail;
                department.DepartmentIsActive = model.DepartmentIsActive;
                department.DepartmentName = model.DepartmentName;
                department.DepartmentPhone = model.DepartmentPhone;

                _departmentService.Update(department);
                TempData.Put("message", new ResultMessage()
                {
                    Title = "Departman Güncelle",
                    Message = "Departman Güncelleme Başarılı",
                    Css = "success"
                });
                return View(department);
            }
            TempData.Put("message", new ResultMessage()
            {
                Title = "Departman Güncelle",
                Message = "Departman Güncelleme Başarısız",
                Css = "danger"
            });
            return View(model);
        }

        [Route("/admin/department/delete/{id?}")]
        public IActionResult DeleteDepartment(int id)
        {

            var department = _departmentService.GetById(id);
            if (department != null)
            {
                department.DepartmentIsDeleted = true;
                _departmentService.Update(department);
                TempData.Put("message", new ResultMessage()
                {
                    Title = "Departman Sil",
                    Message = "Departman Silindi",
                    Css = "success"
                });
                return RedirectToAction("Index");

            }
            TempData.Put("message", new ResultMessage()
            {
                Title = "Departman Sil",
                Message = "Departman silme işlemi başarısız",
                Css = "danger"
            });
            return RedirectToAction("Index");
        }

        [Route("/admin/department/undelete/{id?}")]
        public IActionResult UnDeleteDepartment(int id)
        {

            var department = _departmentService.GetById(id);
            if (department != null)
            {
                department.DepartmentIsDeleted = false;
                _departmentService.Update(department);
                TempData.Put("message", new ResultMessage()
                    {
                        Title = "Departman Geri Al",
                        Message = "Departman geri alındı",
                        Css = "success"
                    });
                    return RedirectToAction("Index");
            }
            TempData.Put("message", new ResultMessage()
            {
                Title = "Departman Geri Al",
                Message = "Departman geri alma işlemi başarısız",
                Css = "danger"
            });
            return RedirectToAction("Index");
        }
    }
}