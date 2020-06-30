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
            var complementObject = ProcessCommodityComplementJson(notificationFromContext.defraimp_CommodityComplementsText);

            ProcessIdentificationJson(notificationFromContext.defraimp_IdentificationOfAnimalsText, complementObject);
        }

        private List<CommodityComplementObject> ProcessCommodityComplementJson(string json)
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

            if (this.notificationFromContext.Contains("defraimp_commoditycomplementstext") && !string.IsNullOrEmpty(notificationFromContext.defraimp_CommodityComplementsText))
            {
                var finalString = string.Empty;

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
                notificationFromContext.defraimp_CommoditySpeciesName = serializedObject.FirstOrDefault().speciesName ?? string.Empty;
            }

            return serializedObject;
        }

        private List<IdentificationOfAnimals> ProcessIdentificationJson(string json, List<CommodityComplementObject> commodityComplementObject)
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
            var speciesName = string.Empty;

            if (this.notificationFromContext.Contains("defraimp_identificationofanimalstext") && !string.IsNullOrEmpty(notificationFromContext.defraimp_IdentificationOfAnimalsText))
            {
                serializedObject.ForEach(x =>
                {
                    if (x.speciesID != null)
                    {
                        speciesName = commodityComplementObject.Where(complement => complement.speciesID.Trim() == x.speciesID.Trim()).Select(complement => complement.speciesNomination).FirstOrDefault();
                    }

                    finalString += "ComplementID: " + (x.complementID.ToString() ?? string.Empty) + System.Environment.NewLine
                                 + "SpeciesID: " + (x.speciesID ?? string.Empty) + System.Environment.NewLine
                                 + System.Environment.NewLine;

                    x.keyDataPair.ForEach(y =>
                    {
                        finalString += y.key + ": " + y.data + System.Environment.NewLine
                                     + System.Environment.NewLine;

                        if (y.key.Equals("imp_number_animal"))
                        {
                            notificationFromContext.defraimp_commoditiesnumberofanimals = Convert.ToInt32(y.data.Trim());
                        }
                    });

                    finalString += "Identifiers:" + System.Environment.NewLine;
                    x.identifiers.ForEach(z =>
                    {
                        if (!string.IsNullOrEmpty(speciesName))
                            commodityIdTypes = "SpeciesName: " + speciesName + "; ";
                        if (z.data.microchip != null)
                            commodityIdTypes += "Microchip: " + (z.data.microchip ?? string.Empty) + "; ";
                        if (z.data.passport != null)
                            commodityIdTypes += "Passport: " + (z.data.passport ?? string.Empty) + "; ";
                        if (z.data.leg_ring != null)
                            commodityIdTypes += "leg_ring: " + (z.data.leg_ring ?? string.Empty) + "; ";
                        if (z.data.tattoo != null)
                            commodityIdTypes += "tattoo: " + (z.data.tattoo ?? string.Empty) + "; ";

                        commodityIdTypes += System.Environment.NewLine + System.Environment.NewLine;

                        finalString += commodityIdTypes;
                    });

                    finalString += "-----------------" + System.Environment.NewLine
                                 + System.Environment.NewLine;
                });

                notificationFromContext.defraimp_FormattedIdentificationofAnimalsText = finalString;
                notificationFromContext.defraimp_CommodityId = serializedObject.FirstOrDefault()?.complementID.ToString() ?? string.Empty;
                notificationFromContext.defraimp_CommoditySpeciesId = serializedObject.FirstOrDefault()?.speciesID ?? string.Empty;
                notificationFromContext.defraimp_CommodityIDTypes = commodityIdTypes;
            }

            return serializedObject;
        }
    }
}
