using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocumentationSystem.Business.Abstract;
using DocumentationSystem.Entity;
using DocumentationSystem.WebApp.Extensions;
using DocumentationSystem.WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;

namespace DocumentationSystem.WebApp.Controllers
{
    [Authorize(Roles = "admin, staff")]
    public class UserController : Controller
    {
        private readonly UserManager<DocSysUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IDepartmentService _departmentService;

        public UserController(UserManager<DocSysUser> userManager, RoleManager<IdentityRole> roleManager, IDepartmentService departmentService)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _departmentService = departmentService;
        }

        [Route("/admin/users")]
        public IActionResult Index()
        {
            var users = new List<UserModel>();
            foreach (var item in _userManager.Users)
            {
                var user = new UserModel()
                {
                    ID = item.Id,
                    DepartmentId = item.DepartmentId,
                    Department = _departmentService.GetById(item.DepartmentId),
                    Email = item.Email,
                    isDeleted = item.isDeleted,
                    NameSurname = item.NameSurname,
                    isApprovedByAdmin = item.isApprovedByAdmin,
                    Phone = item.PhoneNumber,
                    ProfilePhoto = item.ProfilePhoto,
                    RoleId = _userManager.GetRolesAsync(item).Result.FirstOrDefault()
                };
                users.Add(user);
            }
            return View(users);
        }

        [Route("/admin/user/edit/{email?}")]
        public async Task<IActionResult> EditUserAsync(string email)
        {
            var roles = _roleManager.Roles;
            ViewBag.roles = new SelectList(roles, "Name", "Name");
            ViewBag.departments = new SelectList(_departmentService.GetAll().Where(i => i.DepartmentIsDeleted == false && i.DepartmentIsActive).ToList(), "DepartmentId", "DepartmentName");
            var user = await _userManager.FindByEmailAsync(email);
            var userRoles = await _userManager.GetRolesAsync(user);
            var model = new UserModel()
            {
                ID = user.Id,
                isApprovedByAdmin = user.isApprovedByAdmin,
                Email = user.Email,
                isDeleted = user.isDeleted,
                Phone = user.PhoneNumber,
                NameSurname = user.NameSurname,
                RoleId = userRoles.FirstOrDefault(),
                DepartmentId = user.DepartmentId,
            };
            return View(model);
        }

        [HttpPost, Route("/admin/user/edit/{email?}")]
        public async Task<IActionResult> EditUserAsync(UserModel model)
        {
            var roles = _roleManager.Roles;
            ViewBag.roles = new SelectList(roles, "Name", "Name");
            ViewBag.departments = new SelectList(_departmentService.GetAll().Where(i => i.DepartmentIsDeleted == false && i.DepartmentIsActive).ToList(), "DepartmentId", "DepartmentName");
            DocSysUser user = await _userManager.FindByEmailAsync(model.Email);

            if (ModelState.IsValid)
            {
                if (user == null)
                {
                    TempData.Put("message", new ResultMessage()
                    {
                        Title = "Kullanıcı Güncelle",
                        Message = "Kullanıcı Bulunamadı",
                        Css = "danger"
                    });
                    return RedirectToAction("Index");
                }
                user.Email = model.Email;
                user.isDeleted = model.isDeleted;
                user.isApprovedByAdmin = model.isApprovedByAdmin;
                user.PhoneNumber = model.Phone;
                user.NameSurname = model.NameSurname;
                user.DepartmentId = model.DepartmentId;

                if (!(await _userManager.IsInRoleAsync(user, model.RoleId)))
                {

                    var userrole = await _userManager.GetRolesAsync(user);
                    RoleModel models = new RoleModel()
                    {
                        RoleName = userrole.First()
                    };

                    if (await _userManager.IsInRoleAsync(user, models.RoleName))
                    {
                        await _userManager.RemoveFromRoleAsync(user, models.RoleName);
                    }
                    await _userManager.AddToRoleAsync(user, model.RoleId);
                }
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    TempData.Put("message", new ResultMessage()
                    {
                        Title = "Kullanıcı Güncelle",
                        Message = "Kullanıcı Güncelleme Başarılı",
                        Css = "success"
                    });
                    return RedirectToAction("Index");
                }
            }
            TempData.Put("message", new ResultMessage()
            {
                Title = "Kullanıcı Güncelle",
                Message = "Kullanıcı Güncelleme Başarısız",
                Css = "danger"
            });
            return View(model);
        }

        [Route("/admin/user/delete/{email?}")]
        public async Task<IActionResult> DeleteUserAsync(string email)
        {
            
            var user = await _userManager.FindByEmailAsync(email);
            var role = await _userManager.GetRolesAsync(user);
            if (role.FirstOrDefault() != "admin")
            {
                TempData.Put("message", new ResultMessage()
                {
                    Title = "Kullanıcı Sil",
                    Message = "Silmek istediğiniz kullanıcı admin olduğu için yanlızca adminler tarafından silinebilir.",
                    Css = "danger"
                });
                return RedirectToAction("Index");
            }
            if (user != null)
            {
                user.isDeleted = true;
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    TempData.Put("message", new ResultMessage()
                    {
                        Title = "Kullanıcı Sil",
                        Message = "Kullanıcı Silindi",
                        Css = "success"
                    });
                    return RedirectToAction("Index");
                }
            }
            TempData.Put("message", new ResultMessage()
            {
                Title = "Kullanıcı Sil",
                Message = "Kullanıcı silme işlemi başarısız",
                Css = "danger"
            });
            return RedirectToAction("Index");
        }

        [Route("/admin/user/undelete/{email?}")]
        public async Task<IActionResult> UnDeleteUserAsync(string email)
        {

            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                user.isDeleted = false;
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    TempData.Put("message", new ResultMessage()
                    {
                        Title = "Kullanıcı Geri Alma",
                        Message = "Kullanıcı geri alma işlemi başarılı.",
                        Css = "success"
                    });
                    return RedirectToAction("Index");
                }
            }
            TempData.Put("message", new ResultMessage()
            {
                Title = "Kullanıcı Geri Al",
                Message = "Kullanıcı geri alma işlemi başarısız.",
                Css = "danger"
            });
            return RedirectToAction("Index");
        }

        [Route("/admin/users/waiting")]
        public IActionResult WaitForConfirmation()
        {
            var users = new List<UserModel>();
            foreach (var item in _userManager.Users.Where(i => i.isApprovedByAdmin == false && i.isDeleted == false).ToList())
            {
                var user = new UserModel()
                {
                    ID = item.Id,
                    DepartmentId = item.DepartmentId,
                    Department = _departmentService.GetById(item.DepartmentId),
                    Email = item.Email,
                    isDeleted = item.isDeleted,
                    NameSurname = item.NameSurname,
                    isApprovedByAdmin = item.isApprovedByAdmin,
                    Phone = item.PhoneNumber,
                    ProfilePhoto = item.ProfilePhoto,
                    RoleId = _userManager.GetRolesAsync(item).Result.FirstOrDefault()
                };
                users.Add(user);
            }
            return View(users);
        }

        [Route("/admin/user/confirm/{email?}")]
        public async Task<IActionResult> ConfirmUser(string email)
        {

            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                user.isApprovedByAdmin = true;
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    TempData.Put("message", new ResultMessage()
                    {
                        Title = "Kullanıcı Onayı",
                        Message = "Kullanıcı başarıyla onaylandı. Düzenlemek için panel menüsünden kullanıcıları görüntüleyebilirsiniz.",
                        Css = "success"
                    });
                    return RedirectToAction("WaitForConfirmation");
                }
            }
            TempData.Put("message", new ResultMessage()
            {
                Title = "Kullanıcı Onayı",
                Message = "Kullanıcı onaylama işlemi başarısız",
                Css = "danger"
            });
            return RedirectToAction("Index");
        }
    }
}
