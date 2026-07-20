namespace Defra.Imports.IntegrationTests.Dynamics.PlaceOfDestination.SampleData
{
    using Defra.Imports.Model;
    using Defra.Imports.Model.ReferenceData;
    using System;

    public class PlaceOfDestinationWithData
    {
        public PlaceOfDestinationWithData()
        {
            Guid recordId = Guid.NewGuid();
            PlaceOfDestination = new defraimp_PlaceOfDestination
            {
                Id = recordId,
                defraimp_Name = $"INT TEST {recordId}",
                defraimp_AddressLine1 = "123 Fake Street",
                defraimp_City = "London",
                defraimp_Postcode = "N9 999",
                defraimp_Country = Countries.UnitedKingdom,
            };
        }

        public defraimp_PlaceOfDestination PlaceOfDestination { get; }

    }
}
