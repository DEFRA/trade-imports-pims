using Defra.Imports.BusinessLogic.Itahc.JsonFormatterClassObjects;
using Defra.Imports.BusinessLogic.Itahc.JsonFormatterClassObjects.IdentificationParameterSetObjects;
using Defra.Imports.Model;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Defra.Imports.BusinessLogic.Itahc
{
    public class PopulateFormattedJSONTextFields
    {
        private defraimp_itahc itahcFromContext;

        public PopulateFormattedJSONTextFields(defraimp_itahc _itahcFromContext)
        {
            this.itahcFromContext = _itahcFromContext;
        }

        /// <summary>
        /// Method to update the formatted json fields
        /// </summary>
        public void FormatIntegrationData()
        {
            if (this.itahcFromContext.Contains("defraimp_commoditycomplementstext") && !string.IsNullOrEmpty(itahcFromContext.defraimp_CommodityComplementsText))
            {
                ProcessCommodityComplementJson(this.itahcFromContext.defraimp_CommodityComplementsText);
            }

            if (this.itahcFromContext.Contains("defraimp_identificationofanimalstext") && !string.IsNullOrEmpty(itahcFromContext.defraimp_IdentificationOfAnimalsText))
            {
                ProcessIdentificationParameterSetJson(this.itahcFromContext.defraimp_IdentificationOfAnimalsText);
            }
        }

        /// <summary>
        /// Method to process the JSON string
        /// </summary>
        /// <param name="firstLevel"> First level of the Json </param>
        /// <param name="json"> Json string  </param>
        /// <returns> Returns a string with the formatted json values </returns>
        private string ProcessCommodityComplementJson(string json)
        {
            var serializedObject = new CommodityComplementObject();

            using (MemoryStream DeserializeMemoryStream = new MemoryStream())
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(CommodityComplementObject));

                StreamWriter writer = new StreamWriter(DeserializeMemoryStream);
                writer.Write(json.Replace("'", "\""));
                writer.Flush();

                DeserializeMemoryStream.Position = 0;
                serializedObject = (CommodityComplementObject)serializer.ReadObject(DeserializeMemoryStream);
            }

            var finalString = "CommodityCode: " + (serializedObject.CommodityComplement.CommodityCode ?? string.Empty) + System.Environment.NewLine
                            + System.Environment.NewLine
                            + "ComplementID: " + (serializedObject.CommodityComplement.ComplementID ?? string.Empty) + System.Environment.NewLine
                            + System.Environment.NewLine
                            + "SpeciesType: " + (serializedObject.CommodityComplement.SpeciesType ?? string.Empty) + System.Environment.NewLine
                            + System.Environment.NewLine
                            + "SpeciesModel: " + (serializedObject.CommodityComplement.SpeciesModel ?? string.Empty) + System.Environment.NewLine
                            + System.Environment.NewLine
                            + "Species:" + System.Environment.NewLine
                            + "SpeciesID: " + (serializedObject.CommodityComplement.Species?.SpeciesID ?? string.Empty) + System.Environment.NewLine
                            + "SpeciesNomination: " + (serializedObject.CommodityComplement.Species?.SpeciesNomination ?? string.Empty);

            itahcFromContext.defraimp_FormattedCommodityComplementsText = finalString;
            itahcFromContext.defraimp_CommodityCode = serializedObject.CommodityComplement.CommodityCode ?? string.Empty;
            itahcFromContext.defraimp_SpeciesId = serializedObject.CommodityComplement.Species?.SpeciesID ?? string.Empty;
            itahcFromContext.defraimp_SpeciesNomination = serializedObject.CommodityComplement.Species?.SpeciesNomination ?? string.Empty;
            itahcFromContext.defraimp_ComplementId = serializedObject.CommodityComplement.ComplementID ?? string.Empty;

            return finalString;
        }

        private string ProcessIdentificationParameterSetJson(string json)
        {
            var finalString = string.Empty;
            var commodityIdTypes = string.Empty;
            var passportNumber = string.Empty;
            
            var serializedObject = new IdentificationParameterSetObject();

            using (MemoryStream DeserializeMemoryStream = new MemoryStream())
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(IdentificationParameterSetObject));

                StreamWriter writer = new StreamWriter(DeserializeMemoryStream);
                writer.Write(json.Replace("'", "\""));
                writer.Flush();

                DeserializeMemoryStream.Position = 0;
                serializedObject = (IdentificationParameterSetObject)serializer.ReadObject(DeserializeMemoryStream);
            }

            if(serializedObject.IdentificationParameterSet.IdentificationParameter != null)
            {
                serializedObject.IdentificationParameterSet.IdentificationParameter.ForEach(x =>
                {
                    finalString += "Key: " + x.Key + System.Environment.NewLine
                                    + "Data: " + x.Data + System.Environment.NewLine
                                    + System.Environment.NewLine;

                    if (x.Key.Trim().Equals("identsystem"))
                    {
                        commodityIdTypes += x.Data.Trim() + ": ";
                    }
                    else if (x.Key.Trim().Equals("identnumber"))
                    {
                        commodityIdTypes += x.Data.Trim() + "; ";
                    }

                    if (x.Key.Trim().Equals("passportnumber"))
                    {
                        commodityIdTypes += x.Key.Trim() + ": " + x.Data.Trim() + Environment.NewLine;
                        passportNumber += x.Data.Trim() + Environment.NewLine;
                    }
                });
            }
            else
            {
                var serializedList = new IdentificationParameterSetList();

                using (MemoryStream DeserializeMemoryStream = new MemoryStream())
                {
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(IdentificationParameterSetList));

                    StreamWriter writer = new StreamWriter(DeserializeMemoryStream);
                    writer.Write(json.Replace("'", "\""));
                    writer.Flush();

                    DeserializeMemoryStream.Position = 0;
                    serializedList = (IdentificationParameterSetList)serializer.ReadObject(DeserializeMemoryStream);
                }

                serializedList.IdentificationParameterSet.ForEach(x =>
                {
                    x.IdentificationParameter.ForEach(y =>
                    {
                        finalString += "Key: " + y.Key + System.Environment.NewLine
                                 + "Data: " + y.Data + System.Environment.NewLine
                                 + System.Environment.NewLine;

                        commodityIdTypes +=
                        if (y.Key.Trim().Equals("identsystem"))
                        {
                            commodityIdTypes += y.Data.Trim() + ": ";
                        }
                        else if (y.Key.Trim().Equals("identnumber"))
                        {
                            commodityIdTypes += y.Data.Trim() "; ";
                        }

                        if (y.Key.Trim().Equals("passportnumber"))
                        {
                            commodityIdTypes += y.Key.Trim() + ": " + y.Data.Trim() + Environment.NewLine;
                            passportNumber += y.Data.Trim() + Environment.NewLine;
                        }
                    });

                    finalString += "----------" + Environment.NewLine
                                 + Environment.NewLine;

                    commodityIdTypes += "----------" + Environment.NewLine
                                 + Environment.NewLine;

                    passportNumber += "----------" + Environment.NewLine
                                 + Environment.NewLine;
                });
            }

            itahcFromContext.defraimp_formattedIdentificationOfAnimalsText = finalString;
            itahcFromContext.defraimp_CommodityIdTypes = commodityIdTypes;
            itahcFromContext.defraimp_PassportNumber = passportNumber;

            return finalString;
        }
    }
}
