namespace Defra.Imports.UnitTests.Support
{
    using System;
    using Defra.Imports.Dataverse.Model;
    using Microsoft.Xrm.Sdk;

    // Test data for the watch list example described in architecture/adr/0001-plugin-workflow-di-scaffold.md.
    internal static class WatchListTestData
    {
        public const string OwnerName = "Olivia Owner";
        public const string OwnerEmail = "olivia.owner@example.test";
        public const string TransporterName = "Acme Transport";
        public const string UnassignedName = "Unassigned transporter watch list (Olivia Owner)";
        public const string TransporterWatchListName = "Acme Transport watch list (Olivia Owner)";
        public const string OwnerComments = "Owned by Olivia Owner (olivia.owner@example.test).";

        // Values left as null are absent from the entity, as they are on a Create Target that omits them.
        public static defraimp_WatchList CreateWatchList(
            Guid id,
            Guid? ownerId,
            Guid? transporterId = null,
            string name = null,
            string comments = null)
        {
            var watchList = new defraimp_WatchList();
            if (id != Guid.Empty)
            {
                watchList.Id = id;
            }

            if (ownerId.HasValue)
            {
                watchList.OwnerId = new EntityReference(SystemUser.EntityLogicalName, ownerId.Value);
            }

            if (transporterId.HasValue)
            {
                watchList.defraimp_TransporterId = new EntityReference(defraimp_Transporter.EntityLogicalName, transporterId.Value);
            }

            if (name != null)
            {
                watchList.defraimp_Name = name;
            }

            if (comments != null)
            {
                watchList.defraimp_Comments = comments;
            }

            return watchList;
        }

        // The early-bound LINQ provider needs typed records, as the platform returns when the assembly carries the
        // generated ProxyTypesAssemblyAttribute.
        public static Entity CreateOwner(Guid ownerId)
        {
            var owner = new SystemUser { Id = ownerId };
            owner["fullname"] = OwnerName;
            owner["internalemailaddress"] = OwnerEmail;
            return owner;
        }

        public static Entity CreateTransporter(Guid transporterId)
        {
            return new defraimp_Transporter { Id = transporterId, defraimp_Name = TransporterName };
        }
    }
}
