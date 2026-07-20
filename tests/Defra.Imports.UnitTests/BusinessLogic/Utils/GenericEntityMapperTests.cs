using Defra.Imports.BusinessLogic.Utils;
using Defra.Imports.Model;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using Xunit;

namespace Defra.Imports.UnitTests.BusinessLogic.Utils
{
    public class GenericEntityMapperTests
    {
        private GenericEntityMapper<defraimp_itahc, defraimp_importapplication> _genericEntityMapper;

        public GenericEntityMapperTests()
        {
            Dictionary<string, string> fieldsToMap = new Dictionary<string, string>();
            fieldsToMap.Add("defraimp_consignorname", "defraimp_importerorganisation");
            fieldsToMap.Add("defraimp_consignoraddressstreet", "defraimp_importeraddress1");
            fieldsToMap.Add("defraimp_consignoraddresspostcode", "defraimp_importeraddresspostcode");
            fieldsToMap.Add("defraimp_consignoraddresscity", "defraimp_importeraddresscity");
            fieldsToMap.Add("defraimp_consignoraddresscountryid", "defraimp_importeraddresscountryid");

            _genericEntityMapper = new GenericEntityMapper<defraimp_itahc, defraimp_importapplication>(fieldsToMap);
        }

        [Fact]
        public void MapAllFields_AnItahcWithValidFieldsAndAnImportRecord_ShouldMapAllFieldsInMappingConfig()
        {
            defraimp_itahc itahcToMapFrom = new defraimp_itahc()
            {
                defraimp_ConsignorName = "Importer Name",
                defraimp_ConsignorAddressStreet = "40 Holborn Viaduct",
                defraimp_ConsignorAddressPostcode = "EC1N 2PB",
                defraimp_ConsignorAddressCity = "London",
                defraimp_ConsignorAddressCountryId = new EntityReference("defra_country", Guid.NewGuid())
            };

            defraimp_importapplication importRecordToMapTo = new defraimp_importapplication();

            importRecordToMapTo = _genericEntityMapper.MapAllFields(itahcToMapFrom, importRecordToMapTo);

            Assert.Equal(itahcToMapFrom.defraimp_ConsignorName, importRecordToMapTo.defraimp_ImporterOrganisation);
            Assert.Equal(itahcToMapFrom.defraimp_ConsignorAddressStreet, importRecordToMapTo.defraimp_ImporterAddress1);
            Assert.Equal(itahcToMapFrom.defraimp_ConsignorAddressPostcode, importRecordToMapTo.defraimp_ImporterAddressPostcode);
            Assert.Equal(itahcToMapFrom.defraimp_ConsignorAddressCity, importRecordToMapTo.defraimp_ImporterAddressCity);
            Assert.Equal(itahcToMapFrom.defraimp_ConsignorAddressCountryId, importRecordToMapTo.defraimp_ImporterAddressCountryID);
        }

        [Fact]
        public void MapAllFields_AnItahcWithValidFieldsAndAnImportRecord_ShouldNotMapFieldNotInFieldsToMap()
        {
            defraimp_itahc itahcToMapFrom = new defraimp_itahc()
            {
                defraimp_ConsignorName = "Importer Name",
                defraimp_ConsignorAddressStreet = "40 Holborn Viaduct",
                defraimp_ConsignorAddressPostcode = "EC1N 2PB",
                defraimp_ConsignorAddressCity = "London",
                defraimp_ConsignorAddressCountryId = new EntityReference("defra_country", Guid.NewGuid())
            };

            string originalImportRecordDestinationName = "Destination Organisation";

            defraimp_importapplication importRecordToMapTo = new defraimp_importapplication()
            {
                defraimp_PlaceofDestinationOrganisation = originalImportRecordDestinationName 
            };

            importRecordToMapTo = _genericEntityMapper.MapAllFields(itahcToMapFrom, importRecordToMapTo);

            Assert.Equal(originalImportRecordDestinationName, importRecordToMapTo.defraimp_PlaceofDestinationOrganisation);
        }

        [Fact]
        public void MapEmptyFields_AnItahcWithValidFieldsAndAnImportRecord_ShouldOnlyPopulateEmptyFields()
        {
            defraimp_itahc itahcToMapFrom = new defraimp_itahc()
            {
                defraimp_ConsignorName = "Importer Name",
                defraimp_ConsignorAddressStreet = "40 Holborn Viaduct",
                defraimp_ConsignorAddressPostcode = "EC1N 2PB",
                defraimp_ConsignorAddressCity = "London",
                defraimp_ConsignorAddressCountryId = new EntityReference("defra_country", Guid.NewGuid())
            };

            string originalImportRecordOrganisation = "Destination Organisation";

            defraimp_importapplication importRecordToMapTo = new defraimp_importapplication()
            {
                defraimp_ImporterOrganisation = originalImportRecordOrganisation
            };

            importRecordToMapTo = _genericEntityMapper.MapEmptyFields(itahcToMapFrom, importRecordToMapTo);

            Assert.Equal(itahcToMapFrom.defraimp_ConsignorAddressStreet, importRecordToMapTo.defraimp_ImporterAddress1);
            Assert.Equal(itahcToMapFrom.defraimp_ConsignorAddressPostcode, importRecordToMapTo.defraimp_ImporterAddressPostcode);
            Assert.Equal(itahcToMapFrom.defraimp_ConsignorAddressCity, importRecordToMapTo.defraimp_ImporterAddressCity);
            Assert.Equal(itahcToMapFrom.defraimp_ConsignorAddressCountryId, importRecordToMapTo.defraimp_ImporterAddressCountryID);

            Assert.Equal(originalImportRecordOrganisation, importRecordToMapTo.defraimp_ImporterOrganisation);
        }

    }
}
