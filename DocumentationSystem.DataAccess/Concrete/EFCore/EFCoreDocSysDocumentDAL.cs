using DocumentationSystem.DataAccess.Abstract;
using DocumentationSystem.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace DocumentationSystem.DataAccess.Concrete.EFCore
{
    public class EFCoreDocSysDocumentDAL : EFCoreGenericRepository<DocSysDocument, ApplicationDbContext>, IDocSysDocumentDAL
    {
    }
}
