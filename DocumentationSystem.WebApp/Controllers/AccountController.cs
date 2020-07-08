using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using DocumentationSystem.Business.Abstract;
using DocumentationSystem.Entity;
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

        public IActionResult Index()
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
            ViewBag.departments = new SelectList(_departmentService.GetAll(), "DepartmentId", "DepartmentName");
            return View(new RegisterModel());
        }

        [HttpPost, Route("/account/register")]
        public async Task<IActionResult> Register(RegisterModel registerModel)
        {
            ViewBag.departments = new SelectList(_departmentService.GetAll(), "DepartmentId", "DepartmentName");
            if (!ModelState.IsValid)
            {
                return View(registerModel);
            }
            var user = new DocSysUser()
            { 
                Email = registerModel.Email,
                UserName = registerModel.Email,
                NameSurname = registerModel.NameSurname,
                DepartmentId = registerModel.DepartmentId,
                ProfilePhoto = "",
                isApprovedByAdmin = false,
            };
            var result = await _userManager.CreateAsync(user, registerModel.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "user");
                return RedirectToAction("login", "account");
            }
            ModelState.AddModelError("", result.Errors.FirstOrDefault().Description);
            return View(registerModel);
        }
    }
}
