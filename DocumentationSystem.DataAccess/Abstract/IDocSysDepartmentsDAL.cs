using DocumentationSystem.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace DocumentationSystem.DataAccess.Abstract
{
    public interface IDocSysDepartmentsDAL : IRepository<DocSysDepartments>
    {
        List<DocSysDepartments> GetAllWithDocumentAndUser();
        DocSysDepartments GetByIdWithDocumentAndUser(int id);

    }
}
