namespace Defra.Imports.Tests.Integration.Dynamics.PlaceOfOrigin.SampleData
{
    using System;
    using Defra.Imports.Model;

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
