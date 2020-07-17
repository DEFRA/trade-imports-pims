using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Defra.Imports.BusinessLogic.Itahc;
using Defra.Imports.Model;
using Xunit;

namespace Defra.Imports.Tests.Unit.BusinessLogic.Itahc
{
    public class PopulateFormattedJSONTextFieldsTests
    {
        [Fact]
        public void CommodityComplementAndIdentificationFormattingTest()
        {
            var formattedCommodityComplement = "CommodityCode: 0103" + Environment.NewLine
                                             + Environment.NewLine
                                             + "ComplementID: 244110" + Environment.NewLine
                                             + Environment.NewLine
                                             + "SpeciesType: domestique" + Environment.NewLine
                                             + Environment.NewLine
                                             + "SpeciesModel: 11002" + Environment.NewLine
                                             + Environment.NewLine
                                             + "Species:" + Environment.NewLine
                                             + "SpeciesID: 10650140" + Environment.NewLine
                                             + "SpeciesNomination: Sus scrofa domesticus";

            var identificationParameter = "Key: official_ident" + Environment.NewLine
                                        + "Data: 2344" + Environment.NewLine
                                        + Environment.NewLine
                                        + "Key: age" + Environment.NewLine
                                        + "Data: 2" + Environment.NewLine
                                        + Environment.NewLine;


            var itahcFromContext = new defraimp_itahc()
            {
                defraimp_CommodityComplementsText = @"{'CommodityComplement':{'CommodityCode':'0103','ComplementID':'244110','SpeciesType':'domestique','SpeciesModel':'11002','Species':{'SpeciesID':'10650140','SpeciesNomination':'Sus scrofa domesticus'}}}",
                defraimp_IdentificationOfAnimalsText = @"{'IdentificationParameterSet':{'IdentificationParameter':[{'Key':'official_ident','Data':'2344'},{'Key':'age','Data':'2'}]}}"
            };

            var populateFormattedJSONTextFields = new PopulateFormattedJSONTextFields(itahcFromContext);
            populateFormattedJSONTextFields.FormatIntegrationData();

            Assert.NotNull(itahcFromContext.defraimp_FormattedCommodityComplementsText);
            Assert.Equal(formattedCommodityComplement, itahcFromContext.defraimp_FormattedCommodityComplementsText);
            Assert.NotNull(itahcFromContext.defraimp_formattedIdentificationOfAnimalsText);
            Assert.Equal(identificationParameter, itahcFromContext.defraimp_formattedIdentificationOfAnimalsText);
        }

        [Fact]
        public void ValidateParameterSetIfList()
        {
            var identificationParameter = "Key: official_ident" + Environment.NewLine
                                        + "Data: 2344" + Environment.NewLine
                                        + Environment.NewLine
                                        + "Key: age" + Environment.NewLine
                                        + "Data: 2" + Environment.NewLine
                                        + Environment.NewLine
                                        + "----------" + Environment.NewLine
                                        + Environment.NewLine;

            var itahcFromContext = new defraimp_itahc()
            {
                defraimp_IdentificationOfAnimalsText = @"{'IdentificationParameterSet':[{'IdentificationParameter':[{'Key':'official_ident','Data':'2344'},{'Key':'age','Data':'2'}]}]}"
            };

            var populateFormattedJSONTextFields = new PopulateFormattedJSONTextFields(itahcFromContext);
            populateFormattedJSONTextFields.FormatIntegrationData();

            Assert.NotNull(itahcFromContext.defraimp_formattedIdentificationOfAnimalsText);
            Assert.Equal(identificationParameter, itahcFromContext.defraimp_formattedIdentificationOfAnimalsText);
        }

        [Fact]
        public void ValidateCommodityIdTypesIsPopulated()
        {
            // Arrange
            string commodityComplementJson = @"{'CommodityComplement':{'CommodityCode':'0101','ComplementID':'126700','SpeciesModel':'10319','Species':{'SpeciesID':'8698562','SpeciesNomination':'Equus cabalus'}}}";
            string identificationOfAnimalsJson = @"{'IdentificationParameterSet':[{'IdentificationParameter':[{'Key':'complement','Data':'126700'},{'Key':'species','Data':'8698562'},{'Key':'identnumber','Data':'A 123456'}]},{'IdentificationParameter':[{'Key':'complement','Data':'126700'},{'Key':'species','Data':'8698562'},{'Key':'identnumber','Data':'B 123456'}]},{'IdentificationParameter':[{'Key':'complement','Data':'126700'},{'Key':'species','Data':'8698562'},{'Key':'identnumber','Data':'C 123456'}]},{'IdentificationParameter':[{'Key':'complement','Data':'126700'},{'Key':'species','Data':'8698562'},{'Key':'identnumber','Data':'D 123456'}]},{'IdentificationParameter':[{'Key':'complement','Data':'126700'},{'Key':'species','Data':'8698562'},{'Key':'identnumber','Data':'E 123123'}]}]}";

            var itahc = new defraimp_itahc()
            {
                defraimp_CommodityComplementsText = commodityComplementJson,
                defraimp_IdentificationOfAnimalsText = identificationOfAnimalsJson
            };

            // Act
            var populateFormattedJSONTextFields = new PopulateFormattedJSONTextFields(itahc);
            populateFormattedJSONTextFields.FormatIntegrationData();

            // Assert
            Assert.NotNull(itahc.defraimp_CommodityIdTypes);
        }

        [Fact]
        public void CommodityIDTypeShouldBeFormattedCorrectlyForMultipleAnimals()
        {
            // Arrange
            string commodityComplementJson = @"{'CommodityComplement':{'CommodityCode':'0102','ComplementID':'239300','SpeciesType':'domestique','SpeciesModel':'10998','Species':{'SpeciesID':'10537542','SpeciesNomination':'Bos taurus'}}}";
            string identificationOfAnimalsJson = @"{'IdentificationParameterSet':[{'IdentificationParameter':[{'Key':'official_ident','Data':'UK123456'},{'Key':'numpassportemp','Data':''},{'Key':'bovex_state','Data':''}]},{'IdentificationParameter':[{'Key':'official_ident','Data':'UK234567'},{'Key':'numpassportemp','Data':''},{'Key':'bovex_state','Data':''}]},{'IdentificationParameter':[{'Key':'official_ident','Data':'UK345678'},{'Key':'numpassportemp','Data':''},{'Key':'bovex_state','Data':''}]},{'IdentificationParameter':[{'Key':'official_ident','Data':'UK456789'},{'Key':'numpassportemp','Data':''},{'Key':'bovex_state','Data':''}]},{'IdentificationParameter':[{'Key':'official_ident','Data':'UK567890'},{'Key':'numpassportemp','Data':''},{'Key':'bovex_state','Data':''}]},{'IdentificationParameter':[{'Key':'official_ident','Data':'UK678901'},{'Key':'numpassportemp','Data':''},{'Key':'bovex_state','Data':''}]},{'IdentificationParameter':[{'Key':'official_ident','Data':'UK789012'},{'Key':'numpassportemp','Data':''},{'Key':'bovex_state','Data':''}]},{'IdentificationParameter':[{'Key':'official_ident','Data':'UK890123'},{'Key':'numpassportemp','Data':''},{'Key':'bovex_state','Data':''}]},{'IdentificationParameter':[{'Key':'official_ident','Data':'UK901234'},{'Key':'numpassportemp','Data':''},{'Key':'bovex_state','Data':''}]},{'IdentificationParameter':[{'Key':'official_ident','Data':'UK012345'},{'Key':'numpassportemp','Data':''},{'Key':'bovex_state','Data':''}]}]}";

            var itahc = new defraimp_itahc()
            {
                defraimp_CommodityComplementsText = commodityComplementJson,
                defraimp_IdentificationOfAnimalsText = identificationOfAnimalsJson
            };

            // Act
            var populateFormattedJSONTextFields = new PopulateFormattedJSONTextFields(itahc);
            populateFormattedJSONTextFields.FormatIntegrationData();

            // Assert
            Assert.NotNull(itahc.defraimp_CommodityIdTypes);
            Assert.True(itahc.defraimp_CommodityIdTypes.Contains("official_ident: UK123456; numpassportemp: ; bovex_state: ;"));
            Assert.True(itahc.defraimp_CommodityIdTypes.Contains("official_ident: UK567890; numpassportemp: ; bovex_state: ;"));
            Assert.True(itahc.defraimp_CommodityIdTypes.Contains("official_ident: UK012345; numpassportemp: ; bovex_state: ;"));
        }
    }
}
