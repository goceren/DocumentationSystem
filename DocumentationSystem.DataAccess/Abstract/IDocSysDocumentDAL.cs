using DocumentationSystem.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace DocumentationSystem.DataAccess.Abstract
{
    public interface IDocSysDocumentDAL : IRepository<DocSysDocument>
    {
        List<DocSysDocument> GetByUserId(string Id);
        DocSysDocument GetByIdWithDepartmentAndUser(int Id);
        List<DocSysDocument> GetAllWithDepartmentAndUser();
    }
}
