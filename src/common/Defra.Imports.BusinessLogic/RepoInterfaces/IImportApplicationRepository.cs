namespace Defra.Imports.BusinessLogic.RepoInterfaces
{
    using Defra.Imports.Model;
    using Microsoft.Xrm.Sdk.Query;
    using System;

    interface IImportApplicationRepository
    {
        defraimp_importapplication GetImportApplicationWithID(Guid id, ColumnSet columnSet);
    }
}
