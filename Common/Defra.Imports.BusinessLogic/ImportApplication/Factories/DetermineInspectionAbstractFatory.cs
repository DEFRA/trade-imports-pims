using Defra.Imports.BusinessLogic.ImportApplication.DetermineInspection.Strategies;
using System;
using System.Collections.Generic;
using System.Text;

namespace Defra.Imports.BusinessLogic.ImportApplication.Factories
{
    public abstract class DetermineInspectionAbstractFatory
    {
        public abstract AbstractDetermineInspection GetDetermineInspection(string riskLevel);
    }
}
