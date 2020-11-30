namespace Defra.Imports.Model.ReferenceData
{
    using System;
    using Microsoft.Xrm.Sdk;

    public static class ConfigurationParameters
    {
        public const string IpaffsUrlKey = "defraimp_ipaffs_url";
        public const string TracesEnabledKey = "defraimp_traces_enabled";
        public const string UnknownDevolvedOfficeKey = "defraimp_unknown_devolved_office_id";

        public static EntityReference ipaffsUrl = new EntityReference(defraexp_configurationparameter.EntityLogicalName, new Guid("68604929-79e0-ea11-a813-000d3ad82cac"));
        public static EntityReference tracesEnabled = new EntityReference(defraexp_configurationparameter.EntityLogicalName, new Guid("2bb103d9-b629-eb11-a813-000d3ad82cac"));
        public static EntityReference unknownDevolvedOfficeId = new EntityReference(defraexp_configurationparameter.EntityLogicalName, new Guid("cc3cd258-42e8-ea11-a817-000d3ad82cac"));
    }
}
