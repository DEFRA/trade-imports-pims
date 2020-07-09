using Defra.Imports.BusinessLogic.RepoInterfaces;
using Defra.Imports.Model;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Defra.Imports.Repositories.Annotations
{
    public class AnnotationsRepository : IAnnotationRepository
    {
        #region Private Variables

        private readonly ImportsContext CrmContext;

        #endregion Private Variables

        #region Constructor

        public AnnotationsRepository (ImportsContext _crmContext)
        {
            CrmContext = _crmContext;
        }

        #endregion Constructor

        #region Retrieve Methods

        public List<Annotation> RetrieveNoteByRegardingId(Guid Id)
        {
            var retrievedAnnotations = CrmContext.AnnotationSet.Where(x => x.ObjectId.Id == Id)
                                      .Select(x => new Annotation
                                      {
                                          Id = x.Id,
                                          Subject = x.Subject,
                                          MimeType = x.MimeType,
                                          NoteText = x.NoteText,
                                          FileName = x.FileName
                                      }).ToList();

            return retrievedAnnotations;
        }

        #endregion Retrieve Methods

        #region Create Methods

        public void CreateAnnotations(List<Annotation> listOfAnnotations)
        {
            if (listOfAnnotations.Any())
            {
                listOfAnnotations.ForEach(x =>
                {
                    CrmContext.AddObject(x);
                });

                CrmContext.SaveChanges();
            }
        }

        #endregion Create Methods
    }
}
