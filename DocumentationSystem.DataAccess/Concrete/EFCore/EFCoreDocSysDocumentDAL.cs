using DocumentationSystem.DataAccess.Abstract;
using DocumentationSystem.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocumentationSystem.DataAccess.Concrete.EFCore
{
    public class EFCoreDocSysDocumentDAL : EFCoreGenericRepository<DocSysDocument, ApplicationDbContext>, IDocSysDocumentDAL
    {
        public List<DocSysDocument> GetAllWithDepartmentAndUser()
        {
            using (var context = new ApplicationDbContext())
            {
                return context.Set<DocSysDocument>().Include(i => i.Department).Include(i => i.User).ToList();
            }
        }

        public DocSysDocument GetByIdWithDepartmentAndUser(int Id)
        {
            using (var context = new ApplicationDbContext())
            {
                return context.Set<DocSysDocument>().Where(i => i.DocumentId == Id).Include(i => i.Department).Include(i => i.User).FirstOrDefault();
            }
        }

        public List<DocSysDocument> GetByUserId(string Id)
        {
            using (var context = new ApplicationDbContext())
            {
                return context.Set<DocSysDocument>().Where(i => i.UserId == Id).Include(i => i.Department).Include(i => i.User).ToList();
            }
        }
    }
}
