namespace Defra.Imports.IntegrationTests.Dataverse.PlaceOfOrigin.SampleData
{
    using System;
    using Defra.Imports.Model;

    public class BronzePlaceOfOrigin
    {
        public BronzePlaceOfOrigin()
        {
            Guid recordId = Guid.NewGuid();
            this.PlaceOfOrigin = new defraimp_placeoforigin
            {
                Id = Guid.NewGuid(),
                defraimp_name = $"INT TEST Bronze Place of Origin {recordId}",
                defraimp_TrustLevel = defraimp_trustlevel.Bronze,
            };
        }

        public defraimp_placeoforigin PlaceOfOrigin { get; }
    }
}
