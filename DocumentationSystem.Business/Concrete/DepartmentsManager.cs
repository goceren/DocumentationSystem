using DocumentationSystem.Business.Abstract;
using DocumentationSystem.DataAccess.Abstract;
using DocumentationSystem.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocumentationSystem.Business.Concrete
{
    public class DepartmentsManager : IDepartmentService
    {
        private readonly IDocSysDepartmentsDAL _departmentsDAL;
        public DepartmentsManager(IDocSysDepartmentsDAL departmentsDAL)
        {
            _departmentsDAL = departmentsDAL;
        }
        public void Create(DocSysDepartments entity)
        {
            _departmentsDAL.Create(entity);
        }

        public void Delete(DocSysDepartments entity)
        {
            _departmentsDAL.Delete(entity);

        }

        public List<DocSysDepartments> GetAll()
        {
            return _departmentsDAL.GetAll().ToList();
        }

        public List<DocSysDepartments> GetAllWithDocumentAndUser()
        {
            return _departmentsDAL.GetAllWithDocumentAndUser();
        }

        public DocSysDepartments GetById(int id)
        {
            return _departmentsDAL.GetById(id);
        }

        public DocSysDepartments GetByIdWithDocumentAndUser(int id)
        {
            return _departmentsDAL.GetByIdWithDocumentAndUser(id);
        }

        public void Update(DocSysDepartments entity)
        {
            _departmentsDAL.Update(entity);
        }
    }
}
