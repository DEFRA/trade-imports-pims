namespace Defra.Imports.Model.ReferenceData
{
    using System;
    using Microsoft.Xrm.Sdk;

    public static class Countries
    {
        public static readonly EntityReference France = new EntityReference("defra_country", Guid.Parse("be9bb7ed-b2d3-e911-a861-000d3ab1dad7"));
        public static readonly EntityReference Germany = new EntityReference("defra_country", Guid.Parse("ce9bb7ed-b2d3-e911-a861-000d3ab1dad7"));
        public static readonly EntityReference Romania = new EntityReference("defra_country",Guid.Parse("9c9cb7ed-b2d3-e911-a861-000d3ab1dad7"));
        public static readonly EntityReference RepublicOfIreland = new EntityReference("defra_country", Guid.Parse("fe9bb7ed-b2d3-e911-a861-000d3ab1dad7"));
    }
}
