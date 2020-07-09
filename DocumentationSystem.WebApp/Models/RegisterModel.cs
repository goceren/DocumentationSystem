using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentationSystem.WebApp.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Boş olamaz")]
        public string NameSurname { get; set; }

        [Required(ErrorMessage = "Boş olamaz")]
        public string Phone { get; set; }

        public string ProfilePhoto { get; set; }
        public bool isApprovedByAdmin { get; set; }

        [Required(ErrorMessage = "Boş olamaz")]
        public int DepartmentId { get; set; }

        [Required(ErrorMessage = "Boş olamaz")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Boş olamaz")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Email olmalıdır.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Boş olamaz")]
        [DataType(DataType.Password, ErrorMessage = "Şifre yeterince güçlü değil")]
        [Compare("Password", ErrorMessage = "Şifreler Uyuşmuyor")]
        public string RePassword { get; set; }
    }
}
