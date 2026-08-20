namespace Defra.Imports.IntegrationTests.Dataverse.Autonumber.TestCases
{
    using System;
    using Defra.Imports.BusinessLogic.ImportApplication;
    using Defra.Imports.IntegrationTests.Dataverse;
    using Defra.Imports.IntegrationTests.Dataverse.Autonumber.Assertions;
    using Defra.Imports.Model.ReferenceData;
    using MarkTek.Fluent.Testing.RecordGeneration;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    [DoNotParallelize]
    public class AutoNumber_ShouldHaveCorrectAutoNumberRecords : IntegrationTests
    {
        [TestMethod]
        public void AutoNumbers_Should_Have_Record_For_P1_Counter()
        {
            var recordService = new RecordService<Guid>(Autonumbers.p1RecordCount.Id);
            recordService
                .AssertAgainst(new AutonumberRecordValidateValues(this.GetAppUserContext(), ImportApplicationConstants.P1_COUNTER_NAME));
        }

        [TestMethod]
        public void AutoNumbers_Should_Have_Record_For_P1_Quota_Counter()
        {
            var recordService = new RecordService<Guid>(Autonumbers.p1QuotaCount.Id);
            recordService
                .AssertAgainst(new AutonumberRecordValidateValues(this.GetAppUserContext(), ImportApplicationConstants.P1_QUOTA_COUNTER_NAME));
        }

        [TestMethod]
        public void AutoNumbers_Should_Have_Record_For_P2_Counter()
        {
            var recordService = new RecordService<Guid>(Autonumbers.p2RecordCount.Id);
            recordService
                .AssertAgainst(new AutonumberRecordValidateValues(this.GetAppUserContext(), ImportApplicationConstants.P2_COUNTER_NAME));
        }

        [TestMethod]
        public void AutoNumbers_Should_Have_Record_For_P2_Quota_Counter()
        {
            var recordService = new RecordService<Guid>(Autonumbers.p2QuotaCount.Id);
            recordService
                .AssertAgainst(new AutonumberRecordValidateValues(this.GetAppUserContext(), ImportApplicationConstants.P2_QUOTA_COUNTER_NAME));
        }

        [TestMethod]
        public void AutoNumbers_Should_Have_Record_For_P3_Counter()
        {
            var recordService = new RecordService<Guid>(Autonumbers.p3RecordCount.Id);
            recordService
                .AssertAgainst(new AutonumberRecordValidateValues(this.GetAppUserContext(), ImportApplicationConstants.P3_COUNTER_NAME));
        }

        [TestMethod]
        public void AutoNumbers_Should_Have_Record_For_P3_Quota_Counter()
        {
            var recordService = new RecordService<Guid>(Autonumbers.p3QuotaCount.Id);
            recordService
                .AssertAgainst(new AutonumberRecordValidateValues(this.GetAppUserContext(), ImportApplicationConstants.P3_QUOTA_COUNTER_NAME));
        }

        [TestMethod]
        public void AutoNumbers_Should_Have_Record_For_Import_Application_Counter()
        {
            var recordService = new RecordService<Guid>(Autonumbers.importApplicationCount.Id);
            recordService
                .AssertAgainst(new AutonumberRecordValidateValues(this.GetAppUserContext(), ImportApplicationConstants.IMPORT_APPLICATION_COUNTER_NAME));
        }

    }
}

