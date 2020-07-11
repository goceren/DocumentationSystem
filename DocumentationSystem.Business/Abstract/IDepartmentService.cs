using DocumentationSystem.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace DocumentationSystem.Business.Abstract
{
    public interface IDepartmentService : IRepositoryService<DocSysDepartments>
    {
        List<DocSysDepartments> GetAllWithDocumentAndUser();
        DocSysDepartments GetByIdWithDocumentAndUser(int id);

    }
}
