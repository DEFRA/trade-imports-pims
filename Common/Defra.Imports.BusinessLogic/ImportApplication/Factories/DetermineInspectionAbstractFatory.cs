using Defra.Imports.BusinessLogic.ImportApplication.DetermineInspectionStrategies;
using System;
using System.Collections.Generic;
using System.Text;

namespace Defra.Imports.BusinessLogic.ImportApplication.Factories
{
    public abstract class DetermineInspectionAbstractFatory
    {
        public abstract IDetermineInspection GetDetermineInspection(string riskLevel);
    }
}
