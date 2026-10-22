namespace Defra.Imports.BusinessLogic.RepoInterfaces
{
    using Defra.Imports.Dataverse.Model;

    interface IImportRiskLevelRepository
    {
        defraimp_importrisklevel GetRiskLevelByName(string name);
    }
}
