namespace Defra.Imports.Tests.Integration.Dynamics.ImporterNotification.SampleRecords
{
    using System;
    using Defra.Imports.Model;
    using Defra.Imports.Model.ReferenceData;

    public class ImporterNotificationWithData
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="ImporterNotificationWithData"/> class.
        /// Sample data for an Import Application with example data.
        /// </summary>
        public ImporterNotificationWithData(Guid recordId)
        {
            this.ImporterNotification = new defraimp_ImporterNotification
            {
                Id = recordId,
                defraimp_Name = "INT TEST " + Guid.NewGuid().ToString(),
                defraimp_Version = 1,
                defraimp_submissiondate = DateTime.Now,
                defraimp_submittedbydisplayname = "Test User",
                defraimp_lastupdated = DateTime.Now,
                defraimp_lastupdatedbydisplayname = "Test User",
                defraimp_type = defraimp_importernotificationtype.IMP,
                defraimp_commoditiesanimalscertifiedas = defraimp_animalcertifiedas.Pets,
                defraimp_CountryofOriginId = Countries.Germany,
                defraimp_ArrivalDate = DateTime.Now.AddDays(2),

                defraimp_CommodityId = "106400",
                defraimp_CommoditySpeciesId = "22392",
                defraimp_CommodityDescription = "Dogs",

                defraimp_personresponsiblecompanyname = "Test User",
                defraimp_personresponsibleemail = "testuser@example.com",
                defraimp_personresponsiblephone = "07111111111",
                defraimp_personresponsibleaddress = "33 THE APPROACH, LEEDS, LS15 4AN",
                defraimp_PersonResponsibleCountryId = Countries.UnitedKingdom,

                defraimp_importercompanyname = "Test User",
                defraimp_importeraddressemail = "testuser@exampleimporter.com",
                defraimp_importeraddresstelephone = "07111111111",
                defraimp_importerstatus = "nonapproved",
                defraimp_importertype = "importer",
                defraimp_importeraddressaddressline1 = "33 The Approach",
                defraimp_importeraddresscity = "Leeds",
                defraimp_importeraddresspostalzipcode = "LS15 4AN",
                defraimp_ImporterAddressCountryid = Countries.UnitedKingdom,

                defraimp_consignorcompanyname = "Boris' Farm",
                defraimp_consignoraddressemail = "boris@example.com",
                defraimp_consignoraddresstelephone = "07222222222",
                defraimp_consignorstatus = "nonapproved",
                defraimp_consignortype = "exporter",
                defraimp_consignoraddressaddressline1 = "The street",
                defraimp_consignoraddresscity = "Berlin",
                defraimp_consignoraddresspostalzipcode = "834834",
                defraimp_ConsignorAddressCountryid = Countries.Germany,

                defraimp_placeofdestinationcompanyname = "Testers Dogs",
                defraimp_placeofdestinationaddressemail = "testersdogs@example.com",
                defraimp_placeofdestinationaddresstelephone = "07333333333",
                defraimp_placeofdestinationstatus = "nonapproved",
                defraimp_placeofdestinationtype = "destination",
                defraimp_placeofdestinationaddressaddressline1 = "7 Mill Lane",
                defraimp_placeofdestinationaddresscity = "Leeds",
                defraimp_placeofdestinationaddresspostalzipcode = "LS13 4AN",
                defraimp_PlaceofDestinationCountryid = Countries.UnitedKingdom,
                defraimp_isplaceofdestinationthepermanentaddress = true,

                defraimp_transportercompanyname = "N/A",
                defraimp_transporterapprovalnumber = "UK/LEEDS/T1/00087744?",
                defraimp_transporterstatus = "approved",
                defraimp_transportertype = "commercial transporter",
                defraimp_transporteraddressaddressline1 = "NEWHOLME",
                defraimp_transporteraddressaddressline2 = "EAST BROW ROAD",
                defraimp_transporteraddressaddressline3 = "NEWTON-ON-RAWCLIFFE",
                defraimp_transporteraddresscity = "PICKERING",
                defraimp_transporteraddresspostalzipcode = "YO18 8JS",
                defraimp_TransporterAddressCountryid = Countries.UnitedKingdom,
                defraimp_portofentry = "Dover",
            };
        }

        public defraimp_ImporterNotification ImporterNotification { get; }
    }
}
