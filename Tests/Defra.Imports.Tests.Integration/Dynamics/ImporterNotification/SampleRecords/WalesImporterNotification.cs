namespace Defra.Imports.Tests.Integration.Dynamics.ImporterNotification.SampleRecords
{
    using System;
    using Defra.Imports.Model;

    public class WalesImporterNotification
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WalesImporterNotification"/> class.
        /// Sample data for an Import Application detined for Wales.
        /// </summary>
        public WalesImporterNotification(Guid recordId)
        {
            this.ImporterNotification = new defraimp_ImporterNotification
            {
                Id = recordId,
                defraimp_Name = "INT TEST " + Guid.NewGuid().ToString(),
                defraimp_placeofdestinationaddresspostalzipcode = "CF10 2BY",
            };

        }

        public defraimp_ImporterNotification ImporterNotification { get; }
    }
}
