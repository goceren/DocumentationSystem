using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using DocumentationSystem.Business.Abstract;
using DocumentationSystem.Entity;
using DocumentationSystem.WebApp.Extensions;
using DocumentationSystem.WebApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Internal;

namespace DocumentationSystem.WebApp.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class AccountController : Controller
    {
        private readonly UserManager<DocSysUser> _userManager;
        private readonly SignInManager<DocSysUser> _signinManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IDepartmentService _departmentService;

        public AccountController(UserManager<DocSysUser> userManager, SignInManager<DocSysUser> signinManager, IDepartmentService departmentService, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signinManager = signinManager;
            _roleManager = roleManager;
            _departmentService = departmentService;
        }

        public async Task<IActionResult> IndexAsync()
        {
            return RedirectToAction("Login");
        }
        
        [Route("/account/login")]
        public IActionResult Login(string returnUrl = null)
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("/home");
            }
            return View(new LoginModel()
            {
                returnUrl = returnUrl
            });
        }

        [HttpPost, Route("/account/login")]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            if (!ModelState.IsValid)
            {
                return View(loginModel);
            }
            var user = await _userManager.FindByEmailAsync(loginModel.Email);
            if (user == null)
            {
                ModelState.AddModelError("", "Bu email ile daha önce hesap oluşturulmamış.");
                return View(loginModel);
            }
            if (user.isDeleted)
            {
                ModelState.AddModelError("", "Kullanıcı silinmiş lütfen yetkiliden yardım isteyiniz.");
                return View(loginModel);
            }
            if (!user.isApprovedByAdmin)
            {
                ModelState.AddModelError("", "Kullanıcı henüz onaylanmamış lütfen yetkili ile iletişime geçiniz.");
                return View(loginModel);
            }
            var result = await _signinManager.PasswordSignInAsync(user, loginModel.Password, false, true);
            if (result.Succeeded)
            {
                return Redirect(loginModel.returnUrl ?? "~/");
            }
            ModelState.AddModelError("", "Email yada şifre yanlış");
            return View(loginModel);
        }

        [Route("/account/register")]
        public IActionResult Register()
        {
            ViewBag.departments = new SelectList(_departmentService.GetAll().Where(i => i.DepartmentIsDeleted == false && i.DepartmentIsActive).ToList(), "DepartmentId", "DepartmentName");
            return View(new RegisterModel());
        }

        [HttpPost, Route("/account/register")]
        public async Task<IActionResult> Register(RegisterModel registerModel)
        {
            ViewBag.departments = new SelectList(_departmentService.GetAll().Where(i => i.DepartmentIsDeleted == false && i.DepartmentIsActive).ToList(), "DepartmentId", "DepartmentName");
            if (!ModelState.IsValid)
            {
                return View(registerModel);
            }
            if (registerModel.DepartmentId == 0)
            {
                ModelState.AddModelError("", "Departman Seçiniz.");

                return View(registerModel);
            }

            var user = new DocSysUser()
            { 
                Email = registerModel.Email,
                UserName = registerModel.Email,
                NameSurname = registerModel.NameSurname,
                DepartmentId = registerModel.DepartmentId,
                ProfilePhoto = "",
                PhoneNumber = registerModel.Phone,
                isApprovedByAdmin = false,
                isDeleted = false
            };
            var result = await _userManager.CreateAsync(user, registerModel.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "user");
                TempData.Put("message", new ResultMessage()
                {
                    Title = "Kayıt Başarılı",
                    Message = "Kullanıcı kaydınız başarılıdır. Lütfen sisteme giriş yapmak için admin onayını bekleyiniz.",
                    Css = "warning"
                });

                return RedirectToAction("login", "account");
            }
            ModelState.AddModelError("", result.Errors.FirstOrDefault().Description);
            return View(registerModel);
        }

        public async Task<IActionResult> Logout()
        {
            await _signinManager.SignOutAsync();
            TempData.Put("message", new ResultMessage()
            {
                Title = "Oturum Kapatıldı",
                Message = "Hesabınızdan güvenli bir şekilde çıkış yapıldı.",
                Css = "warning"
            });
            return RedirectToAction("login", "account");
        }

        [Route("/account/accessdenied")]
        public IActionResult AccessDenied()
        {
            TempData.Put("message", new ResultMessage()
            {
                Title = "İzinsiz Erişim",
                Message = "Bu sayfaya giriş izniniz yok.",
                Css = "danger"
            });
            return View();
        }

        [Route("/user/changepassword")]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost, Route("/user/changepassword")]
        public async Task<IActionResult> ChangePasswordAsync(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    TempData.Put("message", new ResultMessage()
                    {
                        Title = "Şifre Değiştir",
                        Message = "Lütfen önce giriş yapınız.",
                        Css = "danger"
                    });
                    return RedirectToAction("Login");
                }

                var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

                if (result.Succeeded)
                {
                    await _signinManager.RefreshSignInAsync(user);
                    TempData.Put("message", new ResultMessage()
                    {
                        Title = "Şifremi Değiştir",
                        Message = "Şifreniz başarı ile değiştirildi.",
                        Css = "success"
                    });
                    return RedirectToAction("Login", "Account");
                }
                TempData.Put("message", new ResultMessage()
                {
                    Title = "Şifremi Değiştir",
                    Message = "Şifrenizi yanlış girdiniz...",
                    Css = "danger"
                });
                return View();
            }
            return View(model);
        }
    }
}
