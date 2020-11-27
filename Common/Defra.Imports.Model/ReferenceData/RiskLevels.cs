namespace Defra.Imports.Model.ReferenceData
{
    using System;
    using Microsoft.Xrm.Sdk;

    public static class RiskLevels
    {
        public static readonly EntityReference NA= new EntityReference("defraimp_importrisklevel", Guid.Parse("10c87fb3-5d31-ea11-a810-000d3ab5d037"));
        public static readonly EntityReference P1 = new EntityReference("defraimp_importrisklevel", Guid.Parse("6d5b1d98-b2d3-e911-a84e-000d3ab0d281"));
        public static readonly EntityReference P2 = new EntityReference("defraimp_importrisklevel", Guid.Parse("a4c3149e-b2d3-e911-a84e-000d3ab0d281"));
        public static readonly EntityReference P3 = new EntityReference("defraimp_importrisklevel", Guid.Parse("48c4149e-b2d3-e911-a84e-000d3ab0d281"));
        public static readonly EntityReference TB = new EntityReference("defraimp_importrisklevel", Guid.Parse("615f6e73-9e0b-ea11-a811-000d3ab35da5"));
    }
}
