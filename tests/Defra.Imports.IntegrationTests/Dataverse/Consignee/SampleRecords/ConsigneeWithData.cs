namespace Defra.Imports.IntegrationTests.Dataverse.Consignee.SampleRecords
{
    using System;
    using Defra.Imports.Dataverse.Model;
    using Defra.Imports.Application.ReferenceData;

    public class ConsigneeWithData
    {
        public ConsigneeWithData()
        {
            Guid recordId = Guid.NewGuid();
            this.Consignee = new defraimp_Consignee
            {
                Id = recordId,
                defraimp_Name = $"INT TEST {recordId}",
                defraimp_AddressLine1 = "123 Fake Street",
                defraimp_City = "London",
                defraimp_Postcode = "N9 999",
                defraimp_Country = Countries.UnitedKingdom,
            };
        }

        public defraimp_Consignee Consignee { get; }
    }
}
