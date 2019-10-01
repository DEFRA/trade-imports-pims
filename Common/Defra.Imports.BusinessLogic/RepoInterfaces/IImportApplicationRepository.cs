namespace Defra.Imports.BusinessLogic.RepoInterfaces
{
    using System;
    using Microsoft.Xrm.Sdk.Query;
    using Defra.Imports.Model;

    interface IImportApplicationRepository
    {
        defraimp_importapplication GetImportApplicationWithID(Guid id, ColumnSet columnSet);
    }
}
