namespace Defra.Imports.IntegrationTests.Dynamics.PlaceOfOrigin.SampleData
{
    using Defra.Imports.Model;
    using System;

    public class GoldPlaceOfOrigin
    {
        public GoldPlaceOfOrigin()
        {
            PlaceOfOrigin = new defraimp_placeoforigin
            {
                Id = Guid.NewGuid(),
                defraimp_name = "INT TEST Gold Place of Origin",
                defraimp_TrustLevel = defraimp_trustlevel.Gold,
            };
        }

        public defraimp_placeoforigin PlaceOfOrigin { get; }
    }
}
