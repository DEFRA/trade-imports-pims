using Defra.Imports.Model;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Text;

namespace Defra.Imports.BusinessLogic.RepoInterfaces
{
    public interface IAnnotationRepository
    {
        List<Annotation> RetrieveNoteByRegardingId(Guid Id);

        void CreateAnnotations(List<Annotation> listOfAnnotations);
    }
}
