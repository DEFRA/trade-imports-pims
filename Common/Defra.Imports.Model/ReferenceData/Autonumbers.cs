namespace Defra.Imports.Model.ReferenceData
{
    using Microsoft.Xrm.Sdk;
    using System;

    public static class Autonumbers
    {
        public static EntityReference p1RecordCount = new EntityReference(defraimp_autonumber.EntityLogicalName, Guid.Parse("d1d9b969-a8e6-e911-a865-000d3ab0da57"));
        public static EntityReference p1QuotaCount = new EntityReference(defraimp_autonumber.EntityLogicalName, Guid.Parse("869324fb-ee62-ea11-a811-000d3ab5d511"));
        public static EntityReference p2RecordCount = new EntityReference(defraimp_autonumber.EntityLogicalName, Guid.Parse("bfaca733-a8e6-e911-a865-000d3ab0da57"));
        public static EntityReference p2QuotaCount = new EntityReference(defraimp_autonumber.EntityLogicalName, Guid.Parse("60b3a56e-0ce9-e911-a865-000d3ab0da57"));
        public static EntityReference p3RecordCount = new EntityReference(defraimp_autonumber.EntityLogicalName, Guid.Parse("aa5dc44d-a7e6-e911-a865-000d3ab0da57"));
        public static EntityReference p3QuotaCount = new EntityReference(defraimp_autonumber.EntityLogicalName, Guid.Parse("aa9b2101-ef62-ea11-a811-000d3ab5d511"));
        public static EntityReference importApplicationCount = new EntityReference(defraimp_autonumber.EntityLogicalName, Guid.Parse("59400776-90db-e911-a85e-000d3ab0d65d"));

    }
}
