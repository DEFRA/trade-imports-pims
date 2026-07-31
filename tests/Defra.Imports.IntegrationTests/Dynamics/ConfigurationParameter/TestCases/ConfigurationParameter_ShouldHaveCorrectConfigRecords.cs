namespace Defra.Imports.IntegrationTests.Dynamics.ConfigurationParameter.TestCases
{
    using System;
    using Defra.Imports.IntegrationTests.Dynamics.ConfigurationParameter.Assertions;
    using Defra.Imports.Model.ReferenceData;
    using MarkTek.Fluent.Testing.RecordGeneration;
    using Xunit;

    public class ConfigurationParameter_ShouldHaveCorrectConfigRecords : TestCasesBase
    {
        [Fact]
        public void ConfigurationParameters_Should_Have_Record_For_Ipaffs_Url()
        {
            var recordService = new RecordService<Guid>(ConfigurationParameters.ipaffsUrl.Id);
            recordService
                .AssertAgainst(new ConfigurationParameterValidateValues(this.context, ConfigurationParameters.IpaffsUrlKey));
        }

        [Fact]
        public void ConfigurationParameters_Should_Have_Record_For_Traces_Enabled()
        {
            var recordService = new RecordService<Guid>(ConfigurationParameters.tracesEnabled.Id);
            recordService.AssertAgainst(new ConfigurationParameterValidateValues(this.context, ConfigurationParameters.TracesEnabledKey));
        }

        [Fact]
        public void ConfigurationParameters_Should_Have_Record_For_Unknown_Devolved_Office()
        {
            var recordService = new RecordService<Guid>(ConfigurationParameters.unknownDevolvedOfficeId.Id);
            recordService
                .AssertAgainst(new ConfigurationParameterValidateValues(this.context, ConfigurationParameters.UnknownDevolvedOfficeKey, Teams.UnknownTeam.Id.ToString()));
        }
    }
}
