namespace Defra.Imports.IntegrationTests.Dynamics.Transporter.SampleRecords
{
    using System;
    using Defra.Imports.Model;
    using Defra.Imports.Model.ReferenceData;

    public class TransporterWithData
    {
        public TransporterWithData()
        {
            Guid recordId = Guid.NewGuid();
            this.Transporter = new defraimp_Transporter
            {
                Id = recordId,
                defraimp_Name = $"INT TEST {recordId}",
                defraimp_AddressLine1 = "123 Fake Street",
                defraimp_City = "London",
                defraimp_Postcode = "N9 999",
                defraimp_Country = Countries.UnitedKingdom,
            };
        }

        public defraimp_Transporter Transporter { get; }
    }
}
