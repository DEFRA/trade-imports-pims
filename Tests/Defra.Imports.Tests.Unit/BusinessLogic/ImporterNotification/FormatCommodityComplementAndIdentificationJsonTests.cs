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
                                        + "SpeciesNumber: 1; Microchip: jfsjhd28745837; Passport: 83453889; leg_ring: ; tattoo: " + Environment.NewLine
                                        + Environment.NewLine
                                        + "SpeciesNumber: 2; Microchip: jgkjdg738567389; Passport: 836783; leg_ring: ; tattoo: " + Environment.NewLine
                                        + Environment.NewLine
                                        + "-----------------" + Environment.NewLine
                                        + Environment.NewLine;

            var notificationFromContext = new defraimp_ImporterNotification()
            {
                defraimp_CommodityComplementsText = @"[{'commodityID':'01061900','commodityDescription':'Dogs','complementID':106400,'complementName':'Canis familiaris','speciesID':'22392','speciesName':'Canis familiaris','speciesType':'2','speciesClassName':'Carnivora','speciesClass':'106400','speciesNomination':'Canis familiaris','speciesCommonName':'Dogs'}]",
                defraimp_IdentificationOfAnimalsText = @"[{'complementID':106400,'speciesID':'22392','keyDataPair':[{'key':'imp_number_animal','data':'2'}],'identifiers':[{'speciesNumber':1,'data':{'microchip':'jfsjhd28745837','passport':'83453889'}},{'speciesNumber':2,'data':{'microchip':'jgkjdg738567389','passport':'836783'}}]}]"
            };

            var populateFormattedJSONTextFields = new PopulateJSONTextFields(notificationFromContext);
            populateFormattedJSONTextFields.FormatIntegrationData();

            Assert.NotNull(notificationFromContext.defraimp_FormattedCommodityComplementsText);
            Assert.Equal(commodityComplement, notificationFromContext.defraimp_FormattedCommodityComplementsText);
            Assert.NotNull(notificationFromContext.defraimp_FormattedIdentificationofAnimalsText);
            Assert.Equal(identificationOfAnimals, notificationFromContext.defraimp_FormattedIdentificationofAnimalsText);
        }
    }
}
