namespace Defra.Imports.IntegrationTests.Dynamics.ImporterNotification.SampleRecords
{
    using System;
    using Defra.Imports.Model;

    public class EnglandImporterNotification
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EnglandImporterNotification"/> class.
        /// Sample data for an Import Application detined for England.
        /// </summary>
        public EnglandImporterNotification(Guid recordId)
        {
            this.ImporterNotification = new defraimp_ImporterNotification
            {
                Id = recordId,
                defraimp_Name = "INT TEST " + Guid.NewGuid().ToString(),
                defraimp_placeofdestinationaddresspostalzipcode = "PO1 4BJ",
            };
        }

        public defraimp_ImporterNotification ImporterNotification { get; }
    }
}
