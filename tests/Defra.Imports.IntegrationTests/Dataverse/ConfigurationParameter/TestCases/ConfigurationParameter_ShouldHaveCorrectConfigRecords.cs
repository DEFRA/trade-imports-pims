namespace Defra.Imports.IntegrationTests.Dataverse.ConfigurationParameter.TestCases
{
    using System;
    using Defra.Imports.IntegrationTests.Dataverse.ConfigurationParameter.Assertions;
    using Defra.Imports.Model.ReferenceData;
    using MarkTek.Fluent.Testing.RecordGeneration;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ConfigurationParameter_ShouldHaveCorrectConfigRecords : IntegrationTests
    {
        [TestMethod]
        public void ConfigurationParameters_Should_Have_Record_For_Ipaffs_Url()
        {
            var recordService = new RecordService<Guid>(ConfigurationParameters.ipaffsUrl.Id);
            recordService
                .AssertAgainst(new ConfigurationParameterValidateValues(this.GetAppUserContext(), ConfigurationParameters.IpaffsUrlKey));
        }

        [TestMethod]
        public void ConfigurationParameters_Should_Have_Record_For_Traces_Enabled()
        {
            var recordService = new RecordService<Guid>(ConfigurationParameters.tracesEnabled.Id);
            recordService.AssertAgainst(new ConfigurationParameterValidateValues(this.GetAppUserContext(), ConfigurationParameters.TracesEnabledKey));
        }

        [TestMethod]
        public void ConfigurationParameters_Should_Have_Record_For_Unknown_Devolved_Office()
        {
            var recordService = new RecordService<Guid>(ConfigurationParameters.unknownDevolvedOfficeId.Id);
            recordService
                .AssertAgainst(new ConfigurationParameterValidateValues(this.GetAppUserContext(), ConfigurationParameters.UnknownDevolvedOfficeKey, Teams.UnknownTeam.Id.ToString()));
        }
    }
}

