namespace Defra.Imports.Tests.Integration.Dynamics.PlaceOfOrigin.SampleData
{
    using System;
    using Defra.Imports.Model;

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
