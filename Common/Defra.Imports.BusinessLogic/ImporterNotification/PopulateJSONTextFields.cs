using Defra.Imports.BusinessLogic.ImporterNotification.JsonFormatterClassObjects.CommodityComplementObjects;
using Defra.Imports.BusinessLogic.ImporterNotification.JsonFormatterClassObjects.IdentificationOfAnimalsObjects;
using Defra.Imports.Model;
using Microsoft.Xrm.Sdk;
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
        private const string NumberOfAnimalKeyDataPairKey = "imp_number_animal";
        private const string QuantityKeyDataPairKey = "quantity";
        private const string WeightKeyDataPairKey = "imp_weight";

        private defraimp_ImporterNotification notificationFromContext;
        private defraimp_ImporterNotification notificationPreImage;

        public PopulateJSONTextFields(defraimp_ImporterNotification _notificationFromContext, defraimp_ImporterNotification _notificationPreImage)
        {
            this.notificationFromContext = _notificationFromContext;
            this.notificationPreImage = _notificationPreImage;
        }

        /// <summary>
        /// Method to update the formatted json fields
        /// </summary>
        public void FormatIntegrationData()
        {
            var commodityJson = GetValueFromFields(notificationPreImage, notificationFromContext, "defraimp_commoditycomplementstext");
            var identJson = GetValueFromFields(notificationPreImage, notificationFromContext, "defraimp_identificationofanimalstext");

            var complementObject = ProcessCommodityComplementJson(commodityJson);

            ProcessIdentificationJson(identJson, complementObject);
        }

        private List<CommodityComplementObject> ProcessCommodityComplementJson(string commodityJson)
        {
            var serializedObject = DeserializeCommodityComplementsObject(commodityJson);
            var finalString = string.Empty;

            if (this.notificationFromContext.Contains("defraimp_commoditycomplementstext") && !string.IsNullOrEmpty(commodityJson))
            {
                serializedObject.ForEach(x =>
                {
                    finalString += "CommodityID: " + (x.commodityID ?? string.Empty) + System.Environment.NewLine
                                    + "Commodity Description: " + (x.commodityDescription ?? string.Empty) + System.Environment.NewLine
                                    + "Complement ID: " + (x.complementID.ToString() ?? string.Empty) + System.Environment.NewLine
                                    + "Complement Name: " + (x.complementName ?? string.Empty) + System.Environment.NewLine
                                    + "SpeciesID: " + (x.speciesID ?? string.Empty) + System.Environment.NewLine
                                    + "Species Name: " + (x.speciesName ?? string.Empty) + System.Environment.NewLine
                                    + "Species Type: " + (x.speciesType ?? string.Empty) + System.Environment.NewLine
                                    + "Species Class Name: " + (x.speciesClassName ?? string.Empty) + System.Environment.NewLine
                                    + "Species Class: " + (x.speciesClass ?? string.Empty) + System.Environment.NewLine
                                    + "Species Nomination: " + (x.speciesNomination ?? string.Empty) + System.Environment.NewLine
                                    + "Species Common Name: " + (x.speciesCommonName ?? string.Empty) + System.Environment.NewLine
                                    + System.Environment.NewLine + "-------------" + System.Environment.NewLine;
                });

                notificationFromContext.defraimp_FormattedCommodityComplementsText = finalString;
            }

            return serializedObject;
        }

        private List<IdentificationOfAnimals> ProcessIdentificationJson(string json, List<CommodityComplementObject> commodityComplementObject)
        {
            if (!string.IsNullOrEmpty(json))
            {
                var serializedObject = DeserializeIdentificationOfAnimalsList(json);

                var finalString = string.Empty;
                var commodityIdTypes = string.Empty;
                var speciesName = string.Empty;

                serializedObject.ForEach(x =>
                {
                    if (x.speciesID != null && commodityComplementObject != null && commodityComplementObject.Any())
                    {
                        speciesName = commodityComplementObject.Where(complement => complement.speciesID.Trim() == x.speciesID.Trim()).Select(complement => complement.speciesNomination).FirstOrDefault();
                    }

                    finalString += "ComplementID: " + (x.complementID.ToString() ?? string.Empty) + System.Environment.NewLine
                                 + "SpeciesID: " + (x.speciesID ?? string.Empty) + System.Environment.NewLine
                                 + System.Environment.NewLine;

                    if(x.keyDataPair != null)
                    {
                        x.keyDataPair.ForEach(y =>
                        {
                            finalString += y.key + ": " + y.data + System.Environment.NewLine
                                         + System.Environment.NewLine;

                            if (y.key.Equals(NumberOfAnimalKeyDataPairKey, StringComparison.OrdinalIgnoreCase))
                            {
                                this.notificationFromContext.defraimp_commoditiesnumberofanimals = Convert.ToInt32(y.data.Trim());
                            }
                            else if (y.key.Equals(QuantityKeyDataPairKey, StringComparison.OrdinalIgnoreCase))
                            {
                                this.notificationFromContext.defraimp_commoditiesnumberofanimals = Convert.ToInt32(y.data.Trim());
                            }
                            else if (y.key.Equals(WeightKeyDataPairKey, StringComparison.OrdinalIgnoreCase))
                            {
                                this.notificationFromContext.defraimp_Weight = y.data.Trim();
                            }
                        });
                    }

                    finalString += "Identifiers:" + System.Environment.NewLine;
                    if(x.identifiers != null)
                    {
                        x.identifiers.ForEach(z =>
                        {
                            commodityIdTypes = AddIdentifierToCommodityIdTypes(commodityIdTypes, speciesName, z);
                            //finalString += commodityIdTypes;
                        });
                    }

                    finalString += commodityIdTypes;
                    finalString += "-----------------" + System.Environment.NewLine
                                 + System.Environment.NewLine;
                });

                notificationFromContext.defraimp_FormattedIdentificationofAnimalsText = finalString;
                notificationFromContext.defraimp_CommodityIDTypes = commodityIdTypes;

                return serializedObject;
            }

            return null;
        }

        private string AddIdentifierToCommodityIdTypes(string commodityIdTypes, string speciesName, Identifiers z)
        {
            if (!string.IsNullOrEmpty(speciesName))
                commodityIdTypes += $"SpeciesName: {speciesName}; ";
			if (z.data.horse_name != null)
				commodityIdTypes += $"HorseName: {(z.data.horse_name ?? string.Empty)}; ";
			if (z.data.microchip != null)
                commodityIdTypes += $"Microchip: {(z.data.microchip ?? string.Empty)}; ";
            if (z.data.passport != null)
                commodityIdTypes += $"Passport: {(z.data.passport ?? string.Empty)}; ";
            if (z.data.leg_ring != null)
                commodityIdTypes += $"leg_ring: {(z.data.leg_ring ?? string.Empty)}; ";
            if (z.data.tattoo != null)
                commodityIdTypes += $"tattoo: {(z.data.tattoo ?? string.Empty)}; ";

            commodityIdTypes += System.Environment.NewLine + System.Environment.NewLine;

            return commodityIdTypes;
        }

        private List<CommodityComplementObject> DeserializeCommodityComplementsObject(string commodityJson)
        {
            var serializedObject = new List<CommodityComplementObject>();

            if (!string.IsNullOrEmpty(commodityJson))
            {
                using (MemoryStream DeSerializememoryStream = new MemoryStream())
                {
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(List<CommodityComplementObject>));

                    StreamWriter writer = new StreamWriter(DeSerializememoryStream);
                    writer.Write(commodityJson.Replace("'", "\""));
                    writer.Flush();

                    DeSerializememoryStream.Position = 0;
                    serializedObject = (List<CommodityComplementObject>)serializer.ReadObject(DeSerializememoryStream);
                }
            }

            return serializedObject;
        }

        private List<IdentificationOfAnimals> DeserializeIdentificationOfAnimalsList(string json)
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

            return serializedObject;
        }

        private string GetValueFromFields(Entity preImage, Entity target, string value)
        {
            var imageValue = (preImage != null) ? preImage.GetAttributeValue<string>(value) : string.Empty;
            var validValue = (target.Contains(value)) ? target.GetAttributeValue<string>(value) : imageValue;

            return validValue;
        }
    }
}
