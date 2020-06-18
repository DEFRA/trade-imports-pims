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
        public void CommodityComplementFormattingTest()
        {
            var itahcFromContext = new defraimp_itahc()
            {
                defraimp_CommodityComplementsText = @"{'CommodityComplement':{'CommodityCode':'0103','ComplementID':'244110','SpeciesType':'domestique','SpeciesModel':'11002','Species':{'SpeciesID':'10650140','SpeciesNomination':'Sus scrofa domesticus'}}}",
                defraimp_IdentificationOfAnimalsText = @"{'IdentificationParameterSet':{'IdentificationParameter':[{'Key':'official_ident','Data':'2344'},{'Key':'age','Data':'2'}]}}"
            };

            var populateFormattedJSONTextFields = new PopulateFormattedJSONTextFields(itahcFromContext);
            populateFormattedJSONTextFields.FormatIntegrationData();

            Assert.NotNull(itahcFromContext.defraimp_FormattedCommodityComplementsText);
            Assert.NotNull(itahcFromContext.defraimp_formattedIdentificationOfAnimalsText);
        }
    }
}
