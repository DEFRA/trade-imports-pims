namespace Defra.Imports.IntegrationTests.Dynamics.ImporterNotification.SampleRecords
{
    using System;
    using Defra.Imports.Model;

    public class UnknownImporterNotification
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NonGBImporterNotification"/> class.
        /// Sample data for an Import Application detined for England.
        /// </summary>
        public UnknownImporterNotification(Guid recordId)
        {
            this.ImporterNotification = new defraimp_ImporterNotification
            {
                Id = recordId,
                defraimp_Name = "INT TEST " + Guid.NewGuid().ToString(),
            };
        }

        public defraimp_ImporterNotification ImporterNotification { get; }
    }
}
