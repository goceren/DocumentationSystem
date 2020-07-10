using DocumentationSystem.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace DocumentationSystem.Business.Abstract
{
    public interface IDocumentService : IRepositoryService<DocSysDocument>
    {
        List<DocSysDocument> GetByUserId(string Id);
        DocSysDocument GetByIdWithDepartmentAndUser(int Id);
        List<DocSysDocument> GetAllWithDepartmentAndUser();
    }
}
