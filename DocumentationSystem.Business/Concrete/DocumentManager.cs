using DocumentationSystem.Business.Abstract;
using DocumentationSystem.DataAccess.Abstract;
using DocumentationSystem.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocumentationSystem.Business.Concrete
{
    public class DocumentManager : IDocumentService
    {
        private readonly IDocSysDocumentDAL _docSysDocumentDAL;

        public DocumentManager(IDocSysDocumentDAL docSysDocumentDAL)
        {
            _docSysDocumentDAL = docSysDocumentDAL;
        }

        public void Create(DocSysDocument entity)
        {
            _docSysDocumentDAL.Create(entity);
        }

        public void Delete(DocSysDocument entity)
        {
            _docSysDocumentDAL.Delete(entity);
        }

        public List<DocSysDocument> GetAll()
        {
            return _docSysDocumentDAL.GetAll().ToList();
        }

        public List<DocSysDocument> GetAllWithDepartmentAndUser()
        {
            return _docSysDocumentDAL.GetAllWithDepartmentAndUser().ToList();
        }

        public DocSysDocument GetById(int id)
        {
            return _docSysDocumentDAL.GetById(id);
        }

        public DocSysDocument GetByIdWithDepartmentAndUser(int Id)
        {
            return _docSysDocumentDAL.GetByIdWithDepartmentAndUser(Id);
        }

        public List<DocSysDocument> GetByUserId(string Id)
        {
            return _docSysDocumentDAL.GetByUserId(Id);
        }

        public void Update(DocSysDocument entity)
        {
            _docSysDocumentDAL.Update(entity);
        }
    }
}
