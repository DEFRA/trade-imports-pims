using System;
using System.Collections.Generic;
using System.Text;
using Defra.Imports.BusinessLogic.ImportApplication.DetermineInspectionStrategies;

namespace Defra.Imports.BusinessLogic.ImportApplication.Factories
{
    public class DetermineInspectionFactory : DetermineInspectionAbstractFatory
    {
        public override AbstractDetermineInspection GetDetermineInspection(string riskLevel)
        {
            switch (riskLevel.ToLower())
            {
                case "p1":
                    return null;
                case "p2":
                    return new P2DetermineInspection();
                case "p3":
                    return new P3DetermineInspection();
                default:
                    return new UnknownDetermineInspection();
            }
        }
    }
}
