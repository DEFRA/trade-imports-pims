namespace Defra.Imports.IntegrationTests.Dynamics.PlaceOfOrigin.TestCases
{
    using System;
    using Defra.Imports.IntegrationTests.Dynamics.PlaceOfOrigin.Assertions;
    using Defra.Imports.IntegrationTests.Dynamics.PlaceOfOrigin.SampleData;
    using Defra.Imports.IntegrationTests.Dynamics.PlaceOfOrigin.Scenarios;
    using Defra.Imports.Model;
    using MarkTek.Fluent.Testing.RecordGeneration;
    using Xunit;

    public class PlaceOfOrigin_OnUpdate_OfLockedToBronze_SetLockedToBronzeData : TestCasesBase
    {
        [Fact]
        public void PlaceOfOrigin_Should_Populate_Date_Locked_To_Bronze_When_Record_Is_Locked_To_Bronze()
        {
            var recordService = new RecordService<Guid>(Guid.NewGuid());
            defraimp_placeoforigin placeOfOrigin = new BronzePlaceOfOrigin().PlaceOfOrigin;

            recordService
                .CreateRecord(new CreatePlaceOfOrigin(this.context, placeOfOrigin))
                .Delay(2000)
                .ExecuteAction(new SetLockedToBronze(this.context, placeOfOrigin.Id, true))
                .Delay(5000)
                .AssertAgainst(new PlaceOfOriginValidateLockedToBronze(this.context, placeOfOrigin.Id, DateTime.Today))
                .Delay(2000)
                .ExecuteAction(new DeletePlaceOfOrigin(this.context, placeOfOrigin));
        }

        [Fact]
        public void PlaceOfOrigin_Should_Populate_Date_Unlocked_From_Bronze_And_Set_Trust_Level_To_Bronze_When_Unlocked_From_Bronze()
        {
            var recordService = new RecordService<Guid>(Guid.NewGuid());
            defraimp_placeoforigin placeOfOrigin = new BronzePlaceOfOrigin().PlaceOfOrigin;

            recordService
                .CreateRecord(new CreatePlaceOfOrigin(this.context, placeOfOrigin))
                .Delay(2000)
                .ExecuteAction(new SetLockedToBronze(this.context, placeOfOrigin.Id, true))
                .Delay(2000)
                .ExecuteAction(new SetLockedToBronze(this.context, placeOfOrigin.Id, false))
                .Delay(2000)
                .AssertAgainst(new PlaceOfOriginValidateUnlockedFromBronze(this.context, placeOfOrigin.Id, DateTime.Today, defraimp_trustlevel.Bronze))
                .Delay(2000)
                .ExecuteAction(new DeletePlaceOfOrigin(this.context, placeOfOrigin));
        }

        [Fact]
        public void PlaceOfOrigin_Should_Populate_Date_Unlocked_From_Bronze_And_Set_Trust_Level_To_Gold_When_Unlocked_From_Bronze()
        {
            var recordService = new RecordService<Guid>(Guid.NewGuid());
            defraimp_placeoforigin placeOfOrigin = new GoldPlaceOfOrigin().PlaceOfOrigin;

            recordService
                .CreateRecord(new CreatePlaceOfOrigin(this.context, placeOfOrigin))
                .Delay(2000)
                .ExecuteAction(new SetLockedToBronze(this.context, placeOfOrigin.Id, true))
                .Delay(2000)
                .ExecuteAction(new SetLockedToBronze(this.context, placeOfOrigin.Id, false))
                .Delay(2000)
                .AssertAgainst(new PlaceOfOriginValidateUnlockedFromBronze(this.context, placeOfOrigin.Id, DateTime.Today, defraimp_trustlevel.Gold))
                .Delay(2000)
                .ExecuteAction(new DeletePlaceOfOrigin(this.context, placeOfOrigin));
        }
    }
}
