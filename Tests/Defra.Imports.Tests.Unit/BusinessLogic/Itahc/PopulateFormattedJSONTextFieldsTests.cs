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
    }
}
