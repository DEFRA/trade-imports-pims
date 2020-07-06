using Defra.Imports.BusinessLogic.RepoInterfaces;
using Defra.Imports.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Defra.Imports.BusinessLogic.Import_Query
{
    public class AddNotesToCloneImportQuery
    {
        #region Private Variables

        private readonly IAnnotationRepository AnnotationRepos;
        private defraimp_importquery ImportQueryFromContext;

        #endregion Private Variables

        #region Constructor

        public AddNotesToCloneImportQuery(IAnnotationRepository _annotationRepos, defraimp_importquery _importQueryFromContext)
        {
            AnnotationRepos = _annotationRepos;
            ImportQueryFromContext = _importQueryFromContext;
        }

        #endregion Constructor

        #region Public Methods

        public void CloneNotes()
        {
            if (ImportQueryFromContext.Contains("defraimp_originalquery"))
            {
                var annotationsToCreate = new List<Annotation>();

                var retrievedAnnotations = AnnotationRepos.RetrieveNoteByRegardingId(ImportQueryFromContext.defraimp_OriginalQuery.Id);

                retrievedAnnotations.ForEach(x => 
                {
                    var noteToCreate = new Annotation()
                    {
                        ObjectId = ImportQueryFromContext.ToEntityReference(),
                        ObjectTypeCode = ImportQueryFromContext.LogicalName,
                        Subject = (x.Contains("subject")) ? x.Subject.Trim() : string.Empty,
                        NoteText = (x.Contains("notetext")) ? x.NoteText : string.Empty
                    };

                    annotationsToCreate.Add(noteToCreate);
                });

                AnnotationRepos.CreateAnnotations(annotationsToCreate);
            }
        }

        #endregion Public Methods
    }
}
