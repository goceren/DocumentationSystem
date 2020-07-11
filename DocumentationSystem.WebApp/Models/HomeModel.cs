using DocumentationSystem.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentationSystem.WebApp.Models
{
    public class HomeModel
    {
        public List<DocSysDocument> Documents { get; set; }
        public List<DocSysDepartments> Departments { get; set; }
    }
}
