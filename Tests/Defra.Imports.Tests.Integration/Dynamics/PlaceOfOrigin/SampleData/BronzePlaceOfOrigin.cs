namespace Defra.Imports.Tests.Integration.Dynamics.PlaceOfOrigin.SampleData
{
    using System;
    using Defra.Imports.Model;

    public class BronzePlaceOfOrigin
    {
        public BronzePlaceOfOrigin()
        {
            PlaceOfOrigin = new defraimp_placeoforigin
            {
                Id = Guid.NewGuid(),
                defraimp_name = "INT TEST Bronze Place of Origin",
                defraimp_TrustLevel = defraimp_trustlevel.Bronze,
            };
        }

        public defraimp_placeoforigin PlaceOfOrigin { get; }
    }
}
