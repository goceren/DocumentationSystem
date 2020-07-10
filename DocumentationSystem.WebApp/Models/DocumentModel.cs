using DocumentationSystem.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentationSystem.WebApp.Models
{
    public class DocumentModel
    {
        public int DocumentId { get; set; }
        public string DocumentTitle { get; set; }
        public string DocumentDescription { get; set; }
        public DateTime DocumentCreatedDate { get; set; }
        public DateTime DocumentUpdatedTime { get; set; }
        public string DocumentImage { get; set; }
        public bool DocumentIsActive { get; set; }
        public bool DocumentIsDeleted { get; set; }
        public bool DocumentOpentoEveryone { get; set; }
        public int DepartmentId { get; set; }

        public DocSysDepartments Department { get; set; }

        public string UserId { get; set; }
        public DocSysUser User { get; set; }
    }
}
