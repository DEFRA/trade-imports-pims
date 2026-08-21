namespace Defra.Imports.IntegrationTests.Dataverse.ImporterNotification.SampleRecords
{
    using System;
    using Defra.Imports.Model;

    public class ScotlandImporterNotification
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScotlandImporterNotification"/> class.
        /// Sample data for an Import Application detined for Scotland.
        /// </summary>
        public ScotlandImporterNotification(Guid recordId)
        {
            this.ImporterNotification = new defraimp_ImporterNotification
            {
                Id = recordId,
                defraimp_Name = "INT TEST " + Guid.NewGuid().ToString(),
                defraimp_placeofdestinationaddresspostalzipcode = "EH4 2EB",
            };

        }

        public defraimp_ImporterNotification ImporterNotification { get; }
    }
}
