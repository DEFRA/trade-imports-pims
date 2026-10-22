namespace Defra.Imports.UnitTests.Support
{
    using System;
    using Defra.Imports.Dataverse.Model;
    using Microsoft.Xrm.Sdk;

    // Describes the economic operator table that each type of watch list refers to. The generated names differ in
    // case between the tables, so the generated constants are used rather than assuming they match.
    internal static class EconomicOperatorTestData
    {
        public static string LogicalName(defraimp_watchtype watchType)
        {
            switch (watchType)
            {
                case defraimp_watchtype.PlaceofOrigin:
                    return defraimp_placeoforigin.EntityLogicalName;
                case defraimp_watchtype.PlaceofDestination:
                    return defraimp_PlaceOfDestination.EntityLogicalName;
                case defraimp_watchtype.Consignee:
                    return defraimp_Consignee.EntityLogicalName;
                case defraimp_watchtype.Transporter:
                    return defraimp_Transporter.EntityLogicalName;
                case defraimp_watchtype.Veterinarian:
                    return defraimp_Veterinarian.EntityLogicalName;
                default:
                    throw new ArgumentOutOfRangeException(nameof(watchType));
            }
        }

        public static string NameAttribute(defraimp_watchtype watchType)
        {
            switch (watchType)
            {
                case defraimp_watchtype.PlaceofOrigin:
                    return defraimp_placeoforigin.Fields.defraimp_name;
                case defraimp_watchtype.PlaceofDestination:
                    return defraimp_PlaceOfDestination.Fields.defraimp_Name;
                case defraimp_watchtype.Consignee:
                    return defraimp_Consignee.Fields.defraimp_Name;
                case defraimp_watchtype.Transporter:
                    return defraimp_Transporter.Fields.defraimp_Name;
                case defraimp_watchtype.Veterinarian:
                    return defraimp_Veterinarian.Fields.defraimp_Name;
                default:
                    throw new ArgumentOutOfRangeException(nameof(watchType));
            }
        }

        public static string IdAttribute(defraimp_watchtype watchType)
        {
            switch (watchType)
            {
                case defraimp_watchtype.PlaceofOrigin:
                    return defraimp_placeoforigin.Fields.defraimp_placeoforiginId;
                case defraimp_watchtype.PlaceofDestination:
                    return defraimp_PlaceOfDestination.Fields.defraimp_PlaceOfDestinationId;
                case defraimp_watchtype.Consignee:
                    return defraimp_Consignee.Fields.defraimp_ConsigneeId;
                case defraimp_watchtype.Transporter:
                    return defraimp_Transporter.Fields.defraimp_TransporterId;
                case defraimp_watchtype.Veterinarian:
                    return defraimp_Veterinarian.Fields.defraimp_VeterinarianId;
                default:
                    throw new ArgumentOutOfRangeException(nameof(watchType));
            }
        }

        // Query results must be early-bound, as the sandbox returns them.
        public static Entity Create(defraimp_watchtype watchType, Guid id)
        {
            switch (watchType)
            {
                case defraimp_watchtype.PlaceofOrigin:
                    return new defraimp_placeoforigin { Id = id };
                case defraimp_watchtype.PlaceofDestination:
                    return new defraimp_PlaceOfDestination { Id = id };
                case defraimp_watchtype.Consignee:
                    return new defraimp_Consignee { Id = id };
                case defraimp_watchtype.Transporter:
                    return new defraimp_Transporter { Id = id };
                case defraimp_watchtype.Veterinarian:
                    return new defraimp_Veterinarian { Id = id };
                default:
                    throw new ArgumentOutOfRangeException(nameof(watchType));
            }
        }
    }
}
