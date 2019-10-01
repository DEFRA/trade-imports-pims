namespace Defra.Imports.BusinessLogic.RepoInterfaces
{
    using System.Text;
    using Defra.Imports.Model;

    interface IImportRiskLevelRepository
    {
        defraimp_importrisklevel GetRiskLevelByName(string name);
    }
}
