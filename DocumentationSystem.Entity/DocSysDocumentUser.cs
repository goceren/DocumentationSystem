using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DocumentationSystem.Entity
{
    public class DocSysDocumentUser
    {
        [Required]
        [MaxLength(128)]
        public string UserId { get; set; }
        public DocSysUser User { get; set; }

        [Required]
        public int DocumentId { get; set; }
        public DocSysDocument Document { get; set; }
    }
}
