using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace DocumentationSystem.Entity
{
    public class DocSysUser : IdentityUser
    {
        public string NameSurname { get; set; }
        public int DepartmentId { get; set; }
        public bool isApprovedByAdmin { get; set; }
        public bool isDeleted { get; set; }
        public string ProfilePhoto { get; set; }
        public DocSysDepartments Department { get; set; }
        public List<DocSysDocumentUser> DocumentUsers { get; set; }
    }
}
