namespace Defra.Imports.Tests.Integration.Dynamics.ImporterNotification.SampleRecords
{
    using System;
    using Defra.Imports.Model;

    public class NonGBImporterNotification
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NonGBImporterNotification"/> class.
        /// Sample data for an Import Application detined for England.
        /// </summary>
        public NonGBImporterNotification(Guid recordId)
        {
            this.ImporterNotification = new defraimp_ImporterNotification
            {
                Id = recordId,
                defraimp_Name = "INT TEST " + Guid.NewGuid().ToString(),
                defraimp_placeofdestinationaddresspostalzipcode = "BT1 2NB",
            };
        }

        public defraimp_ImporterNotification ImporterNotification { get; }
    }
}
