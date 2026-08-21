namespace Defra.Imports.IntegrationTests.Dataverse.PlaceOfOrigin.TestCases
{
    using System;
    using Defra.Imports.IntegrationTests.Dataverse.PlaceOfOrigin.Assertions;
    using Defra.Imports.IntegrationTests.Dataverse.PlaceOfOrigin.SampleData;
    using Defra.Imports.IntegrationTests.Dataverse.PlaceOfOrigin.Scenarios;
    using Defra.Imports.Model;
    using MarkTek.Fluent.Testing.RecordGeneration;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class PlaceOfOrigin_OnUpdate_OfLockedToBronze_SetLockedToBronzeData : IntegrationTests
    {
        [TestMethod]
        public void PlaceOfOrigin_Should_Populate_Date_Locked_To_Bronze_When_Record_Is_Locked_To_Bronze()
        {
            var recordService = new RecordService<Guid>(Guid.NewGuid());
            defraimp_placeoforigin placeOfOrigin = new BronzePlaceOfOrigin().PlaceOfOrigin;
            var context = this.GetAppUserContext();

            recordService
                .CreateRecord(new CreatePlaceOfOrigin(context, placeOfOrigin))
                .Delay(2000)
                .ExecuteAction(new SetLockedToBronze(context, placeOfOrigin.Id, true))
                .Delay(5000)
                .AssertAgainst(new PlaceOfOriginValidateLockedToBronze(context, placeOfOrigin.Id, DateTime.Today))
                .Delay(2000)
                .ExecuteAction(new DeletePlaceOfOrigin(context, placeOfOrigin));
        }

        [TestMethod]
        public void PlaceOfOrigin_Should_Populate_Date_Unlocked_From_Bronze_And_Set_Trust_Level_To_Bronze_When_Unlocked_From_Bronze()
        {
            var recordService = new RecordService<Guid>(Guid.NewGuid());
            defraimp_placeoforigin placeOfOrigin = new BronzePlaceOfOrigin().PlaceOfOrigin;
            var context = this.GetAppUserContext();

            recordService
                .CreateRecord(new CreatePlaceOfOrigin(context, placeOfOrigin))
                .Delay(2000)
                .ExecuteAction(new SetLockedToBronze(context, placeOfOrigin.Id, true))
                .Delay(2000)
                .ExecuteAction(new SetLockedToBronze(context, placeOfOrigin.Id, false))
                .Delay(2000)
                .AssertAgainst(new PlaceOfOriginValidateUnlockedFromBronze(context, placeOfOrigin.Id, DateTime.Today, defraimp_trustlevel.Bronze))
                .Delay(2000)
                .ExecuteAction(new DeletePlaceOfOrigin(context, placeOfOrigin));
        }

        [TestMethod]
        public void PlaceOfOrigin_Should_Populate_Date_Unlocked_From_Bronze_And_Set_Trust_Level_To_Gold_When_Unlocked_From_Bronze()
        {
            var recordService = new RecordService<Guid>(Guid.NewGuid());
            defraimp_placeoforigin placeOfOrigin = new GoldPlaceOfOrigin().PlaceOfOrigin;
            var context = this.GetAppUserContext();

            recordService
                .CreateRecord(new CreatePlaceOfOrigin(context, placeOfOrigin))
                .Delay(2000)
                .ExecuteAction(new SetLockedToBronze(context, placeOfOrigin.Id, true))
                .Delay(2000)
                .ExecuteAction(new SetLockedToBronze(context, placeOfOrigin.Id, false))
                .Delay(2000)
                .AssertAgainst(new PlaceOfOriginValidateUnlockedFromBronze(context, placeOfOrigin.Id, DateTime.Today, defraimp_trustlevel.Gold))
                .Delay(2000)
                .ExecuteAction(new DeletePlaceOfOrigin(context, placeOfOrigin));
        }
    }
}
