﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DocumentationSystem.Entity
{
    public class DocSysDepartments
    {
        [Key]
        public int DepartmentId { get; set; }
        [Required]

        public string DepartmentName { get; set; }
        [Required]

        public string DeparmentDetail { get; set; }

        public bool DepartmentIsActive { get; set; }
        [Required]

        public string DepartmentPhone { get; set; }
        public DateTime DepartmentCreatedDate { get; set; }
        public bool DepartmentIsDeleted { get; set; }
        public List<DocSysDocument> Documents { get; set; }
        public List<DocSysUser> Users { get; set; }

    }
}
