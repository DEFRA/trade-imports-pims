using Defra.Imports.Model;
using System;
using System.Collections.Generic;

namespace Defra.Imports.BusinessLogic.RepoInterfaces
{
    public interface IAnnotationRepository
    {
        List<Annotation> RetrieveNoteByRegardingId(Guid Id);

        void CreateAnnotations(List<Annotation> listOfAnnotations);
    }
}
