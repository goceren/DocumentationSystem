using DocumentationSystem.DataAccess.Abstract;
using DocumentationSystem.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocumentationSystem.DataAccess.Concrete.EFCore
{
    public class EFCoreDocSysDepartmentsDAL : EFCoreGenericRepository<DocSysDepartments, ApplicationDbContext>, IDocSysDepartmentsDAL
    {
        public List<DocSysDepartments> GetAllWithDocumentAndUser()
        {
            using (var context = new ApplicationDbContext())
            {
                return context.Set<DocSysDepartments>().Include(i => i.Documents).Include(i => i.Users).ToList();
            }
        }

        public DocSysDepartments GetByIdWithDocumentAndUser(int id)
        {
            using (var context = new ApplicationDbContext())
            {
                return context.Set<DocSysDepartments>().Where(i => i.DepartmentId == id).Include(i => i.Documents).Include(i => i.Users).FirstOrDefault();
            }
        }
    }
}
