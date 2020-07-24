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
        public void ValidateCommodityIdTypesIsPopulated()
        {
            // Arrange
            string commodityComplementJson = @"{'CommodityComplement':{'CommodityCode':'0101','ComplementID':'126700','SpeciesModel':'10319','Species':{'SpeciesID':'8698562','SpeciesNomination':'Equus cabalus'}}}";
            string identificationOfAnimalsJson = @"{'IdentificationParameterSet':[{'IdentificationParameter':[{'Key':'complement','Data':'126700'},{'Key':'species','Data':'8698562'},{'Key':'identnumber','Data':'A 123456'}]},{'IdentificationParameter':[{'Key':'complement','Data':'126700'},{'Key':'species','Data':'8698562'},{'Key':'identnumber','Data':'B 123456'}]},{'IdentificationParameter':[{'Key':'complement','Data':'126700'},{'Key':'species','Data':'8698562'},{'Key':'identnumber','Data':'C 123456'}]},{'IdentificationParameter':[{'Key':'complement','Data':'126700'},{'Key':'species','Data':'8698562'},{'Key':'identnumber','Data':'D 123456'}]},{'IdentificationParameter':[{'Key':'complement','Data':'126700'},{'Key':'species','Data':'8698562'},{'Key':'identnumber','Data':'E 123123'}]}]}";

            var itahc = new defraimp_itahc()
            {
                defraimp_CommodityComplementsText = commodityComplementJson,
                defraimp_IdentificationOfAnimalsText = identificationOfAnimalsJson,
                defraimp_SpeciesNomination = "Equus cabalus"
            };

            defraimp_itahc preImage = null;

            // Act
            var populateFormattedJSONTextFields = new PopulateFormattedJSONTextFields(itahc, preImage);
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
                defraimp_IdentificationOfAnimalsText = identificationOfAnimalsJson,
                defraimp_SpeciesNomination = "Bos taurus"
            };

            defraimp_itahc preImage = null;

            // Act
            var populateFormattedJSONTextFields = new PopulateFormattedJSONTextFields(itahc, preImage);
            populateFormattedJSONTextFields.FormatIntegrationData();

            // Assert
            Assert.NotNull(itahc.defraimp_CommodityIdTypes);
            Assert.True(itahc.defraimp_CommodityIdTypes.Contains("SpeciesName: Bos taurus; official_ident: UK123456; numpassportemp: ; bovex_state: ;"));
            Assert.True(itahc.defraimp_CommodityIdTypes.Contains("SpeciesName: Bos taurus; official_ident: UK567890; numpassportemp: ; bovex_state: ;"));
            Assert.True(itahc.defraimp_CommodityIdTypes.Contains("SpeciesName: Bos taurus; official_ident: UK012345; numpassportemp: ; bovex_state: ;"));
        }

        [Fact]
        public void CommodityIDTypesShouldBeFormattedCorrectlyForMultipleSpecies()
        {
            // Arrange
            string commodityComplementJson = @"{'CommodityComplement':{'CommodityCode':'01061900','ComplementID':'231547','SpeciesClass':'Carnivora','SpeciesModel':'10912','Species':[{'SpeciesID':'10476331','SpeciesNomination':'Felis catus'},{'SpeciesID':'10476330','SpeciesNomination':'Canis familiaris'}]}}";
            string identificationOfAnimalsJson = @"{'IdentificationParameterSet':[{'IdentificationParameter':[{'Key':'complement','Data':'231547'},{'Key':'species','Data':'10476331'},{'Key':'identsystem','Data':'microchip'},{'Key':'identnumber','Data':'123'},{'Key':'passportnumber','Data':'741'},{'Key':'sexinfo','Data':'female'},{'Key':'age','Data':'6 MONTHS'},{'Key':'quantity','Data':'1'}]},{'IdentificationParameter':[{'Key':'complement','Data':'231547'},{'Key':'species','Data':'10476330'},{'Key':'identsystem','Data':'microchip'},{'Key':'identnumber','Data':'456'},{'Key':'passportnumber','Data':'852'},{'Key':'sexinfo','Data':'female'},{'Key':'age','Data':'6 MONTHS'},{'Key':'quantity','Data':'1'}]},{'IdentificationParameter':[{'Key':'complement','Data':'231547'},{'Key':'species','Data':'10476330'},{'Key':'identsystem','Data':'microchip'},{'Key':'identnumber','Data':'789'},{'Key':'passportnumber','Data':'963'},{'Key':'sexinfo','Data':'female'},{'Key':'age','Data':'6 MONTHS'},{'Key':'quantity','Data':'1'}]}]}";

            var itahc = new defraimp_itahc()
            {
                defraimp_CommodityComplementsText = commodityComplementJson,
                defraimp_IdentificationOfAnimalsText = identificationOfAnimalsJson
            };

            defraimp_itahc preImage = null;

            // Act
            var populateFormattedJSONTextFields = new PopulateFormattedJSONTextFields(itahc, preImage);
            populateFormattedJSONTextFields.FormatIntegrationData();

            // Assert
            Assert.NotNull(itahc.defraimp_CommodityIdTypes);
            Assert.True(itahc.defraimp_CommodityIdTypes.Contains("SpeciesName: Felis catus; microchip: 123; passportnumber: 741; sexinfo: female; age: 6 MONTHS; quantity: 1;"));
            Assert.True(itahc.defraimp_CommodityIdTypes.Contains("SpeciesName: Canis familiaris; microchip: 456; passportnumber: 852; sexinfo: female; age: 6 MONTHS; quantity: 1;"));
            Assert.True(itahc.defraimp_CommodityIdTypes.Contains("SpeciesName: Canis familiaris; microchip: 789; passportnumber: 963; sexinfo: female; age: 6 MONTHS; quantity: 1;"));
        }

        [Fact]
        public void CommodityIDTypesShouldUseSpeciesNameFromTargetWhenNotInParameterSet()
        {
            // Arrange
            string commodityComplementJson = @"{'CommodityComplement':{'CommodityCode':'0103','ComplementID':'244110','SpeciesType':'domestique','SpeciesModel':'11002','Species':{'SpeciesID':'10650140','SpeciesNomination':'Sus scrofa domesticus'}}}";
            string identificationOfAnimalsJson = @"{'IdentificationParameterSet':{'IdentificationParameter':[{'Key':'official_ident','Data':'2344'},{'Key':'age','Data':'2'}]}}";

            var itahc = new defraimp_itahc()
            {
                defraimp_CommodityComplementsText = commodityComplementJson,
                defraimp_IdentificationOfAnimalsText = identificationOfAnimalsJson,
                defraimp_SpeciesNomination = "Sus scrofa domesticus"
            };

            defraimp_itahc preImage = null;

            // Act
            var populateFormattedJSONTextFields = new PopulateFormattedJSONTextFields(itahc, preImage);
            populateFormattedJSONTextFields.FormatIntegrationData();

            // Assert
            Assert.NotNull(itahc.defraimp_CommodityIdTypes);
            Assert.True(itahc.defraimp_CommodityIdTypes.Contains($"SpeciesName: {itahc.defraimp_SpeciesNomination}; official_ident: 2344; age: 2;"));
        }

        [Fact]
        public void CommodityIDTypesShouldUseSpeciesNameFromPreImageWhenNotInTarget()
        {
            // Arrange
            string commodityComplementJson = @"{'CommodityComplement':{'CommodityCode':'0103','ComplementID':'244110','SpeciesType':'domestique','SpeciesModel':'11002','Species':{'SpeciesID':'10650140','SpeciesNomination':'Sus scrofa domesticus'}}}";
            string identificationOfAnimalsJson = @"{'IdentificationParameterSet':{'IdentificationParameter':[{'Key':'official_ident','Data':'2344'},{'Key':'age','Data':'2'}]}}";

            var itahc = new defraimp_itahc()
            {
                defraimp_CommodityComplementsText = commodityComplementJson,
                defraimp_IdentificationOfAnimalsText = identificationOfAnimalsJson,
            };

            defraimp_itahc preImage = new defraimp_itahc()
            {
                defraimp_SpeciesNomination = "Sus scrofa domesticus"
            };

            // Act
            var populateFormattedJSONTextFields = new PopulateFormattedJSONTextFields(itahc, preImage);
            populateFormattedJSONTextFields.FormatIntegrationData();

            // Assert
            Assert.NotNull(itahc.defraimp_CommodityIdTypes);
            Assert.True(itahc.defraimp_CommodityIdTypes.Contains($"SpeciesName: {preImage.defraimp_SpeciesNomination}; official_ident: 2344; age: 2;"));
        }

        [Fact]
        public void ShouldUseCommodityComplementsFromPreImageIfItDoesntExistOnTheTarget()
        {
            // Arrange
            string commodityComplementJson = @"{'CommodityComplement':{'CommodityCode':'01061900','ComplementID':'231547','SpeciesClass':'Carnivora','SpeciesModel':'10912','Species':[{'SpeciesID':'10476331','SpeciesNomination':'Felis catus'},{'SpeciesID':'10476330','SpeciesNomination':'Canis familiaris'}]}}";
            string identificationOfAnimalsJson = @"{'IdentificationParameterSet':[{'IdentificationParameter':[{'Key':'complement','Data':'231547'},{'Key':'species','Data':'10476331'},{'Key':'identsystem','Data':'microchip'},{'Key':'identnumber','Data':'123'},{'Key':'passportnumber','Data':'741'},{'Key':'sexinfo','Data':'female'},{'Key':'age','Data':'6 MONTHS'},{'Key':'quantity','Data':'1'}]},{'IdentificationParameter':[{'Key':'complement','Data':'231547'},{'Key':'species','Data':'10476330'},{'Key':'identsystem','Data':'microchip'},{'Key':'identnumber','Data':'456'},{'Key':'passportnumber','Data':'852'},{'Key':'sexinfo','Data':'female'},{'Key':'age','Data':'6 MONTHS'},{'Key':'quantity','Data':'1'}]},{'IdentificationParameter':[{'Key':'complement','Data':'231547'},{'Key':'species','Data':'10476330'},{'Key':'identsystem','Data':'microchip'},{'Key':'identnumber','Data':'789'},{'Key':'passportnumber','Data':'963'},{'Key':'sexinfo','Data':'female'},{'Key':'age','Data':'6 MONTHS'},{'Key':'quantity','Data':'1'}]}]}";

            var itahc = new defraimp_itahc()
            {
                defraimp_IdentificationOfAnimalsText = identificationOfAnimalsJson
            };

            defraimp_itahc preImage = new defraimp_itahc()
            {
                defraimp_CommodityComplementsText = commodityComplementJson,
            };

            // Act
            var populateFormattedJSONTextFields = new PopulateFormattedJSONTextFields(itahc, preImage);
            populateFormattedJSONTextFields.FormatIntegrationData();

            // Assert
            Assert.NotNull(itahc.defraimp_CommodityIdTypes);
            Assert.True(itahc.defraimp_CommodityIdTypes.Contains("SpeciesName: Felis catus; microchip: 123; passportnumber: 741; sexinfo: female; age: 6 MONTHS; quantity: 1;"));
        }
    }
}
