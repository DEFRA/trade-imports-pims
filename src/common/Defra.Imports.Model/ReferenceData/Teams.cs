namespace Defra.Imports.Model.ReferenceData
{
    using Microsoft.Xrm.Sdk;
    using System;

    public static class Teams
    {
        public static readonly EntityReference EnglandTeam = new EntityReference("team",Guid.Parse("a8e19bee-0106-ea11-a811-000d3ab5d511"));
        public static readonly EntityReference ScotlandTeam = new EntityReference("team", Guid.Parse("6bdc7830-0206-ea11-a811-000d3ab5d511"));
        public static readonly EntityReference WalesTeam = new EntityReference("team", Guid.Parse("d2e55f54-0206-ea11-a811-000d3ab5d511"));
        public static readonly EntityReference NONGBTeam = new EntityReference("team", Guid.Parse("c82c940b-b6e3-ea11-a813-000d3ad82cac"));
        public static readonly EntityReference UnknownTeam = new EntityReference("team", Guid.Parse("18785ed4-b5e3-ea11-a813-000d3ad82cac"));
    }
}
