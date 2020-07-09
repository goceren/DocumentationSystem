using DocumentationSystem.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentationSystem.WebApp.Models
{
    public class UserModel
    {
        [Required]
        public string ID { get; set; }
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string Phone { get; set; }
        [Required]
        public string RoleId { get; set; }
        public string NameSurname { get; set; }
        public int DepartmentId { get; set; }
        public bool isApprovedByAdmin { get; set; }
        public bool isDeleted { get; set; }
        public string ProfilePhoto { get; set; }
        public DocSysDepartments Department { get; set; }
        public List<DocSysDocumentUser> DocumentUsers { get; set; }
    }
}
