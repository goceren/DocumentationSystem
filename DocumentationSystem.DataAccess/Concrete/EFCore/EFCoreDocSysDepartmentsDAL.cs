using DocumentationSystem.DataAccess.Abstract;
using DocumentationSystem.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace DocumentationSystem.DataAccess.Concrete.EFCore
{
    public class EFCoreDocSysDepartmentsDAL : EFCoreGenericRepository<DocSysDepartments, ApplicationDbContext>, IDocSysDepartmentsDAL
    {
    }
}
