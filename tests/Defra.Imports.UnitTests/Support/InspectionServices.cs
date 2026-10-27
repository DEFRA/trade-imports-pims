namespace Defra.Imports.UnitTests.Support
{
    using System.Collections.Generic;
    using Defra.Imports.Application.InspectionRequirements;
    using Defra.Imports.Dataverse.Model;
    using Defra.Imports.Dataverse.Queries;
    using Defra.Imports.Infrastructure.Logging;

    // The inspection requirement services wired to one in-memory store, as the composition root wires them.
    internal sealed class InspectionServices
    {
        public InspectionServices(InspectionDataverse dataverse)
        {
            var service = dataverse.Service.Object;
            this.Dataverse = dataverse;
            this.Queries = new InspectionQueries(new ImportsContext(service));
            this.AutonumberCounters = new AutonumberCounters(new AutonumberQueries(new ImportsContext(service)), service);
            this.PlaceOfOriginCounters = new PlaceOfOriginCounters(new PlaceOfOriginQueries(new ImportsContext(service)), service);
            this.Updater = new InspectionRequirementUpdater(service);
            this.Auditor = new ImportRiskCounterAuditor(service);
            this.CounterManagerFactory = new RiskCounterManagerFactory(this.Queries, this.AutonumberCounters, this.PlaceOfOriginCounters, this.Auditor, service, this.Logger);
            this.StrategyFactory = new InspectionStrategyFactory(this.Queries, this.Updater, this.AutonumberCounters, this.PlaceOfOriginCounters);
            this.Determiner = new InspectionRequirementDeterminer(this.Queries, this.PlaceOfOriginCounters, this.CounterManagerFactory, this.StrategyFactory, service, this.Logger);
            this.Changer = new InspectionPlaceOfOriginChanger(this.Queries, this.PlaceOfOriginCounters, this.StrategyFactory, service);
        }

        public InspectionDataverse Dataverse { get; }

        public RecordingLogWriter Logger { get; } = new RecordingLogWriter();

        public InspectionQueries Queries { get; }

        public AutonumberCounters AutonumberCounters { get; }

        public PlaceOfOriginCounters PlaceOfOriginCounters { get; }

        public InspectionRequirementUpdater Updater { get; }

        public ImportRiskCounterAuditor Auditor { get; }

        public RiskCounterManagerFactory CounterManagerFactory { get; }

        public InspectionStrategyFactory StrategyFactory { get; }

        public InspectionRequirementDeterminer Determiner { get; }

        public InspectionPlaceOfOriginChanger Changer { get; }

        internal sealed class RecordingLogWriter : ILogWriter
        {
            public List<string> Messages { get; } = new List<string>();

            public void Log(Severity severity, string tag, string message)
            {
                this.Messages.Add(tag + ": " + severity + ": " + message);
            }
        }
    }
}
