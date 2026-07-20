namespace Defra.Imports.IntegrationTests.Dynamics.Consignee.SampleRecords
{
    using Defra.Imports.Model;
    using Defra.Imports.Model.ReferenceData;
    using System;

    public class ConsigneeWithData
    {
        public ConsigneeWithData()
        {
            Guid recordId = Guid.NewGuid();
            Consignee = new defraimp_Consignee
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
