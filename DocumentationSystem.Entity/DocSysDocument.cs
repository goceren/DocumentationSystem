using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DocumentationSystem.Entity
{
    public class DocSysDocument
    {
        [Key]
        public int DocumentId { get; set; }
        public string DocumentTitle { get; set; }
        public string DocumentDescription { get; set; }
        public DateTime DocumentCreatedDate { get; set; }
        public DateTime DocumentUpdatedTime { get; set; }
        public string DocumentImage { get; set; }
        public bool DocumentIsActive { get; set; }
        public bool DocumentIsDeleted { get; set; }
        public bool DocumentOpentoEveryone { get; set; }
        public bool DocumentIsApproved { get; set; }
        public int DepartmentId { get; set; }

        public DocSysDepartments Department { get; set; }

        public string UserId { get; set; }
        public DocSysUser User { get; set; }

    }
}
