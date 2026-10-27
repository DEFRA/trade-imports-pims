namespace Defra.Imports.IntegrationTests.Dataverse.Autonumber.TestCases
{
    using System;
    using Defra.Imports.Application.Constants;
    using Defra.Imports.IntegrationTests.Dataverse;
    using Defra.Imports.IntegrationTests.Dataverse.Autonumber.Assertions;
    using Defra.Imports.Application.ReferenceData;
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
                .AssertAgainst(new AutonumberRecordValidateValues(this.GetAppUserContext(), InspectionConstants.P1CounterName));
        }

        [TestMethod]
        public void AutoNumbers_Should_Have_Record_For_P1_Quota_Counter()
        {
            var recordService = new RecordService<Guid>(Autonumbers.p1QuotaCount.Id);
            recordService
                .AssertAgainst(new AutonumberRecordValidateValues(this.GetAppUserContext(), InspectionConstants.P1QuotaCounterName));
        }

        [TestMethod]
        public void AutoNumbers_Should_Have_Record_For_P2_Counter()
        {
            var recordService = new RecordService<Guid>(Autonumbers.p2RecordCount.Id);
            recordService
                .AssertAgainst(new AutonumberRecordValidateValues(this.GetAppUserContext(), InspectionConstants.P2CounterName));
        }

        [TestMethod]
        public void AutoNumbers_Should_Have_Record_For_P2_Quota_Counter()
        {
            var recordService = new RecordService<Guid>(Autonumbers.p2QuotaCount.Id);
            recordService
                .AssertAgainst(new AutonumberRecordValidateValues(this.GetAppUserContext(), InspectionConstants.P2QuotaCounterName));
        }

        [TestMethod]
        public void AutoNumbers_Should_Have_Record_For_P3_Counter()
        {
            var recordService = new RecordService<Guid>(Autonumbers.p3RecordCount.Id);
            recordService
                .AssertAgainst(new AutonumberRecordValidateValues(this.GetAppUserContext(), InspectionConstants.P3CounterName));
        }

        [TestMethod]
        public void AutoNumbers_Should_Have_Record_For_P3_Quota_Counter()
        {
            var recordService = new RecordService<Guid>(Autonumbers.p3QuotaCount.Id);
            recordService
                .AssertAgainst(new AutonumberRecordValidateValues(this.GetAppUserContext(), InspectionConstants.P3QuotaCounterName));
        }

        [TestMethod]
        public void AutoNumbers_Should_Have_Record_For_Import_Application_Counter()
        {
            var recordService = new RecordService<Guid>(Autonumbers.importApplicationCount.Id);
            recordService
                .AssertAgainst(new AutonumberRecordValidateValues(this.GetAppUserContext(), InspectionConstants.ImportApplicationCounterName));
        }

    }
}

