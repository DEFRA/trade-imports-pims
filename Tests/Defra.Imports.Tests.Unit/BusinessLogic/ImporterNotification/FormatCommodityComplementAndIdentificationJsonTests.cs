using Defra.Imports.BusinessLogic.ImporterNotification;
using Defra.Imports.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Defra.Imports.Tests.Unit.BusinessLogic.ImporterNotification
{
    public class FormatCommodityComplementAndIdentificationJsonTests
    {
        [Fact]
        public void CommodityComplementAndIdentificationFormattingTest()
        {
            var commodityComplement = "CommodityID: 01061900" + Environment.NewLine
                                     + "Commodity Description: Dogs" + Environment.NewLine
                                     + "Complement ID: 106400" + Environment.NewLine
                                     + "Complement Name: Canis familiaris" + Environment.NewLine
                                     + "SpeciesID: 22392" + Environment.NewLine
                                     + "Species Name: Canis familiaris" + Environment.NewLine
                                     + "Species Type: 2" + Environment.NewLine
                                     + "Species Class Name: Carnivora" + Environment.NewLine
                                     + "Species Class: 106400" + Environment.NewLine
                                     + "Species Nomination: Canis familiaris" + Environment.NewLine
                                     + "Species Common Name: Dogs" + Environment.NewLine
                                     + Environment.NewLine + "-------------" + Environment.NewLine;

            var identificationOfAnimals = "ComplementID: 106400" + Environment.NewLine
                                        + "SpeciesID: 22392" + Environment.NewLine
                                        + Environment.NewLine
                                        + "imp_number_animal: 2" + Environment.NewLine
                                        + Environment.NewLine
                                        + "Identifiers:" + Environment.NewLine
                                        + "SpeciesName: Canis familiaris; Microchip: jfsjhd28745837; Passport: 83453889; " + Environment.NewLine
                                        + Environment.NewLine
                                        + "SpeciesName: Canis familiaris; Microchip: jgkjdg738567389; Passport: 836783; " + Environment.NewLine
                                        + Environment.NewLine
                                        + "-----------------" + Environment.NewLine
                                        + Environment.NewLine;

            defraimp_ImporterNotification notificationImage = null;

            var notificationFromContext = new defraimp_ImporterNotification()
            {
                defraimp_CommodityComplementsText = @"[{'commodityID':'01061900','commodityDescription':'Dogs','complementID':106400,'complementName':'Canis familiaris','speciesID':'22392','speciesName':'Canis familiaris','speciesType':'2','speciesClassName':'Carnivora','speciesClass':'106400','speciesNomination':'Canis familiaris','speciesCommonName':'Dogs'}]",
                defraimp_IdentificationOfAnimalsText = @"[{'complementID':106400,'speciesID':'22392','keyDataPair':[{'key':'imp_number_animal','data':'2'}],'identifiers':[{'speciesNumber':1,'data':{'microchip':'jfsjhd28745837','passport':'83453889'}},{'speciesNumber':2,'data':{'microchip':'jgkjdg738567389','passport':'836783'}}]}]"
            };

            var populateFormattedJSONTextFields = new PopulateJSONTextFields(notificationFromContext, notificationImage);
            populateFormattedJSONTextFields.FormatIntegrationData();

            Assert.NotNull(notificationFromContext.defraimp_FormattedCommodityComplementsText);
            Assert.Equal(commodityComplement, notificationFromContext.defraimp_FormattedCommodityComplementsText);
            Assert.NotNull(notificationFromContext.defraimp_FormattedIdentificationofAnimalsText);
            Assert.Equal(identificationOfAnimals, notificationFromContext.defraimp_FormattedIdentificationofAnimalsText);
            Assert.Equal(2, notificationFromContext.defraimp_commoditiesnumberofanimals);
        }

        [Fact]
        public void NoValueNumberOfAnimalsTest()
        {
            var notificationFromContext = new defraimp_ImporterNotification()
            {
                defraimp_IdentificationOfAnimalsText = @"[{'complementID':106400,'speciesID':'22392','keyDataPair':[{'key':'dummy','data':'2'}],'identifiers':[{'speciesNumber':1,'data':{'microchip':'jfsjhd28745837','passport':'83453889'}},{'speciesNumber':2,'data':{'microchip':'jgkjdg738567389','passport':'836783'}}]}]"
            };

            Assert.Null(notificationFromContext.defraimp_commoditiesnumberofanimals);
        }

        [Fact]
        public void NoIdentifiersInIdentificationOfAnimalsTextShouldntFail()
        {
            defraimp_ImporterNotification notificationImage = null;

            var notificationFromContext = new defraimp_ImporterNotification()
            {
                defraimp_CommodityComplementsText = @"[{'commodityID':'05040000','commodityDescription':'Guts, bladders and stomachs of animals (other than fish), whole and pieces thereof, fresh, chilled, frozen, salted, in brine, dried or smoked','complementID':6960,'complementName':'Equus spp.','speciesID':'13050','speciesName':'Equus spp.','speciesTypeName':'Casing','speciesType':'11','speciesClass':'6960','speciesNomination':'Equus spp.'}]",
                defraimp_IdentificationOfAnimalsText = @"[{'complementID':6960,'speciesID':'13050','keyDataPair':[{'key':'quantity','data':'1'}]}]",
            };

            var populateFormattedJSONTextFields = new PopulateJSONTextFields(notificationFromContext, notificationImage);
            populateFormattedJSONTextFields.FormatIntegrationData();

            Assert.Equal(String.Empty, notificationFromContext.defraimp_CommodityIDTypes);
            Assert.NotNull(notificationFromContext.defraimp_FormattedIdentificationofAnimalsText);
        }

        [Fact]
        public void IDTypesShouldFormatCorrectlyWithMultipleAnimals()
        {
            defraimp_ImporterNotification notificationImage = null;

            var notificationFromContext = new defraimp_ImporterNotification()
            {
                defraimp_CommodityComplementsText = @"[{'commodityID':'01061900','commodityDescription':'Dogs','complementID':106400,'complementName':'Canis familiaris','speciesID':'22392','speciesName':'Canis familiaris','speciesType':'2','speciesClassName':'Carnivora','speciesClass':'106400','speciesNomination':'Canis familiaris','speciesCommonName':'Dogs'}]",
                defraimp_IdentificationOfAnimalsText = @"[{'complementID':106400,'speciesID':'22392','keyDataPair':[{'key':'imp_number_animal','data':'2'}],'identifiers':[{'speciesNumber':1,'data':{'microchip':'jsd857439','passport':'285798345'}},{'speciesNumber':2,'data':{'microchip':'jsdfj84275394','passport':'34534'}}]}]",
            };

            var populateFormattedJSONTextFields = new PopulateJSONTextFields(notificationFromContext, notificationImage);
            populateFormattedJSONTextFields.FormatIntegrationData();

            Assert.True(notificationFromContext.defraimp_CommodityIDTypes.Contains("SpeciesName: Canis familiaris; Microchip: jsd857439; Passport: 285798345;"));
            Assert.True(notificationFromContext.defraimp_CommodityIDTypes.Contains("SpeciesName: Canis familiaris; Microchip: jsdfj84275394; Passport: 34534;"));
            Assert.NotNull(notificationFromContext.defraimp_FormattedIdentificationofAnimalsText);
        }

        [Fact]
        public void QuantityShouldGetPopulatedFromQuantityKeyDataPair()
        {
            defraimp_ImporterNotification notificationImage = null;

            var notificationFromContext = new defraimp_ImporterNotification()
            {
                defraimp_CommodityComplementsText = @"[{'commodityID':'01069000','commodityDescription':'Animals other than mammals, birds, reptiles and insects.','complementID':183873,'complementName':'Lithobates (Rana) catesbeianus','speciesID':'60639','speciesName':'Lithobates (Rana) catesbeianus','speciesType':'2','speciesClassName':'Amphibia','speciesClass':'183873','speciesNomination':'Lithobates (Rana) catesbeianus'}]",
                defraimp_IdentificationOfAnimalsText = @"[{'complementID':183873,'speciesID':'60639','keyDataPair':[{'key':'quantity','data':'1'}]}]",
            };

            var populateFormattedJSONTextFields = new PopulateJSONTextFields(notificationFromContext, notificationImage);
            populateFormattedJSONTextFields.FormatIntegrationData();

            Assert.NotNull(notificationFromContext.defraimp_commoditiesnumberofanimals);
        }

        [Fact]
        public void WeightShouldGetPopulatedFromWeightKeyDataPair()
        {
            defraimp_ImporterNotification notificationImage = null;

            var notificationFromContext = new defraimp_ImporterNotification()
            {
                defraimp_CommodityComplementsText = @"[{'commodityID':'01069000','commodityDescription':'Animals other than mammals, birds, reptiles and insects.','complementID':183873,'complementName':'Lithobates (Rana) catesbeianus','speciesID':'60639','speciesName':'Lithobates (Rana) catesbeianus','speciesType':'2','speciesClassName':'Amphibia','speciesClass':'183873','speciesNomination':'Lithobates (Rana) catesbeianus'}]",
                defraimp_IdentificationOfAnimalsText = @"[{'complementID':183873,'speciesID':'60639','keyDataPair':[{'key':'IMP_Weight','data':'1'}]}]",
            };

            var populateFormattedJSONTextFields = new PopulateJSONTextFields(notificationFromContext, notificationImage);
            populateFormattedJSONTextFields.FormatIntegrationData();

            Assert.True(notificationFromContext.Attributes.Contains("defraimp_weight"));
            Assert.Equal("1", notificationFromContext["defraimp_weight"]);
        }

        [Fact]
        public void MicrochipAndPassportNumberShouldGetPopulatedInIDTypes()
        {
            defraimp_ImporterNotification notificationImage = null;

            var notificationFromContext = new defraimp_ImporterNotification()
            {
                defraimp_CommodityComplementsText = @"[{'commodityID':'01061900','commodityDescription':'Other','complementID':106400,'complementName':'Canis familiaris','speciesID':'42935','speciesName':'Canis familiaris','speciesType':'2','speciesClassName':'Carnivora','speciesClass':'106400','speciesNomination':'Canis familiaris'}]",
                defraimp_IdentificationOfAnimalsText = @"[{'complementID':106400,'speciesID':'42935','keyDataPair':[{'key':'imp_number_animal','data':'1'}],'identifiers':[{'speciesNumber':1,'data':{'microchip':'900079000696650','passport':'HU202276920'}}]}]"
            };

            var populateFormattedJSONTextFields = new PopulateJSONTextFields(notificationFromContext, notificationImage);
            populateFormattedJSONTextFields.FormatIntegrationData();

            Assert.True(notificationFromContext.defraimp_CommodityIDTypes.Contains("Microchip: 900079000696650;"));
            Assert.True(notificationFromContext.defraimp_CommodityIDTypes.Contains("Passport: HU202276920;"));
        }
    }
}
