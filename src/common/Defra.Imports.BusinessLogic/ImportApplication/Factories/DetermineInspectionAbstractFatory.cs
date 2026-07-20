using Defra.Imports.BusinessLogic.ImportApplication.DetermineInspection.Strategies;

namespace Defra.Imports.BusinessLogic.ImportApplication.Factories
{
    public abstract class DetermineInspectionAbstractFatory
    {
        public abstract AbstractDetermineInspection GetDetermineInspection(string riskLevel);
    }
}
