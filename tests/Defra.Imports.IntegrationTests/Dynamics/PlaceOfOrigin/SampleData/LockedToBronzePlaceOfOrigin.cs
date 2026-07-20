namespace Defra.Imports.IntegrationTests.Dynamics.PlaceOfOrigin.SampleData
{
    using Defra.Imports.Model;
    using System;

    public class LockedToBronzePlaceOfOrigin
    {
        public LockedToBronzePlaceOfOrigin()
        {
            PlaceOfOrigin = new defraimp_placeoforigin
            {
                Id = Guid.NewGuid(),
                defraimp_name = "INT TEST Bronze Place of Origin",
                defraimp_TrustLevel = defraimp_trustlevel.Bronze,
                defraimp_LocktoBronze = true,
                defraimp_DateLockedtoBronze = DateTime.Today,
            };
        }

        public defraimp_placeoforigin PlaceOfOrigin { get; }
    }
}
