using Defra.Imports.BusinessLogic.ImporterNotification.JsonFormatterClassObjects.CommodityComplementObjects;
using Defra.Imports.BusinessLogic.ImporterNotification.JsonFormatterClassObjects.IdentificationOfAnimalsObjects;
using Defra.Imports.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Defra.Imports.BusinessLogic.ImporterNotification
{
    public class PopulateJSONTextFields
    {
        private defraimp_ImporterNotification notificationFromContext;

        public PopulateJSONTextFields(defraimp_ImporterNotification _notificationFromContext)
        {
            this.notificationFromContext = _notificationFromContext;
        }

        /// <summary>
        /// Method to update the formatted json fields
        /// </summary>
        public void FormatIntegrationData()
        {
            if (this.notificationFromContext.Contains("defraimp_identificationofanimalstext") && !string.IsNullOrEmpty(notificationFromContext.defraimp_IdentificationOfAnimalsText))
            {
                ProcessIdentificationJson(notificationFromContext.defraimp_IdentificationOfAnimalsText);
            }

            if(this.notificationFromContext.Contains("defraimp_commoditycomplementstext") && !string.IsNullOrEmpty(notificationFromContext.defraimp_CommodityComplementsText))
            {
                ProcessCommodityComplementJson(notificationFromContext.defraimp_CommodityComplementsText);
            }
        }

        private string ProcessCommodityComplementJson(string json)
        {
            var serializedObject = new List<CommodityComplementObject>();

            using (MemoryStream DeSerializememoryStream = new MemoryStream())
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(List<CommodityComplementObject>));

                StreamWriter writer = new StreamWriter(DeSerializememoryStream);
                writer.Write(json.Replace("'", "\""));
                writer.Flush();

                DeSerializememoryStream.Position = 0;
                serializedObject = (List<CommodityComplementObject>)serializer.ReadObject(DeSerializememoryStream);
            }

            var finalString = string.Empty;

            serializedObject.ForEach(x =>
            {
                finalString += "CommodityID: " + x.commodityID + System.Environment.NewLine
                             + "Commodity Description: " + x.commodityDescription + System.Environment.NewLine
                             + "Complement ID: " + x.complementID.ToString() + System.Environment.NewLine
                             + "Complement Name: " + x.complementName + System.Environment.NewLine
                             + "SpeciesID: " + x.speciesID + System.Environment.NewLine
                             + "Species Name: " + x.speciesName + System.Environment.NewLine
                             + "Species Type: " + x.speciesType + System.Environment.NewLine
                             + "Species Class Name: " + x.speciesClassName + System.Environment.NewLine
                             + "Species Class: " + x.speciesClass + System.Environment.NewLine
                             + "Species Nomination: " + x.speciesNomination + System.Environment.NewLine
                             + "Species Common Name: " + x.speciesCommonName + System.Environment.NewLine
                             + System.Environment.NewLine + "-------------" + System.Environment.NewLine;
            });

            notificationFromContext.defraimp_FormattedCommodityComplementsText = finalString;
            notificationFromContext.defraimp_CommoditySpeciesName = serializedObject.FirstOrDefault().speciesName;

            return finalString;
        }

        private string ProcessIdentificationJson(string json)
        {
            var serializedObject = new List<IdentificationOfAnimals>();

            using (MemoryStream DeSerializeMemoryStream = new MemoryStream())
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(List<IdentificationOfAnimals>));

                StreamWriter writer = new StreamWriter(DeSerializeMemoryStream);
                writer.Write(json.Replace("'", "\""));
                writer.Flush();

                DeSerializeMemoryStream.Position = 0;
                serializedObject = (List<IdentificationOfAnimals>)serializer.ReadObject(DeSerializeMemoryStream);
            }

            var finalString = string.Empty;
            var commodityIdTypes = string.Empty;

            serializedObject.ForEach(x =>
            {
                finalString += "ComplementID: " + x.complementID.ToString() + System.Environment.NewLine
                             + "SpeciesID: " + x.speciesID + System.Environment.NewLine
                             + System.Environment.NewLine;

                x.keyDataPair.ForEach(y =>
                {
                    finalString += y.key + ": " + y.data + System.Environment.NewLine
                                 + System.Environment.NewLine;
                });

                finalString += "Identifiers:" + System.Environment.NewLine;
                x.identifiers.ForEach(z =>
                {
                    commodityIdTypes = "SpeciesNumber: " + z.speciesNumber
                                 + "; Microchip: " + z.data.microchip
                                 + "; Passport: " + z.data.passport
                                 + "; leg_ring: " + z.data.leg_ring
                                 + "; tattoo: " + z.data.tattoo + System.Environment.NewLine
                                 + System.Environment.NewLine;

                    finalString += commodityIdTypes;
                });

                finalString += "-----------------" + System.Environment.NewLine
                             + System.Environment.NewLine;
            });

            notificationFromContext.defraimp_FormattedIdentificationofAnimalsText = finalString;
            notificationFromContext.defraimp_CommodityId = serializedObject.FirstOrDefault().complementID.ToString();
            notificationFromContext.defraimp_CommoditySpeciesId = serializedObject.FirstOrDefault().speciesID;
            notificationFromContext.defraimp_CommodityIDTypes = commodityIdTypes;

            return finalString;
        }
    }
}
