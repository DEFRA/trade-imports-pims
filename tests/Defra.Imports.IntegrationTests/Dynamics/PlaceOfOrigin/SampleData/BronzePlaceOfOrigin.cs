namespace Defra.Imports.IntegrationTests.Dynamics.PlaceOfOrigin.SampleData
{
    using Defra.Imports.Model;
    using System;

    public class BronzePlaceOfOrigin
    {
        public BronzePlaceOfOrigin()
        {
            Guid recordId = Guid.NewGuid();
            PlaceOfOrigin = new defraimp_placeoforigin
            {
                Id = Guid.NewGuid(),
                defraimp_name = $"INT TEST Bronze Place of Origin {recordId}",
                defraimp_TrustLevel = defraimp_trustlevel.Bronze,
            };
        }

        public defraimp_placeoforigin PlaceOfOrigin { get; }
    }
}
