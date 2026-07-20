namespace Defra.Imports.BusinessLogic.RepoInterfaces
{
    using Defra.Imports.Model;

    interface IImportRiskLevelRepository
    {
        defraimp_importrisklevel GetRiskLevelByName(string name);
    }
}
