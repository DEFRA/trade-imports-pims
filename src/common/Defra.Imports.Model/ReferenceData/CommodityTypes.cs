namespace Defra.Imports.Model.ReferenceData
{
    using Microsoft.Xrm.Sdk;
    using System;

    public static class CommodityTypes
    {
        public static readonly EntityReference Dog = new EntityReference("defraexp_commoditytype", Guid.Parse("195545a7-bbd3-e911-a870-000d3ab1dcdd"));
        public static readonly EntityReference Pig = new EntityReference("defraexp_commoditytype", Guid.Parse("465545a7-bbd3-e911-a870-000d3ab1dcdd"));
        public static readonly EntityReference Cattle = new EntityReference("defraexp_commoditytype",Guid.Parse("055545a7-bbd3-e911-a870-000d3ab1dcdd"));
        public static readonly EntityReference Ferret = new EntityReference("defraexp_commoditytype", Guid.Parse("235545a7-bbd3-e911-a870-000d3ab1dcdd"));
    }
}
