using Defra.Imports.BusinessLogic.Itahc.JsonFormatterClassObjects;
using Defra.Imports.BusinessLogic.Itahc.JsonFormatterClassObjects.Formatters;
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
        private IdentificationParameterFormatter identParameterFormatter;

        public PopulateFormattedJSONTextFields(defraimp_itahc _itahcFromContext)
        {
            this.itahcFromContext = _itahcFromContext;
            this.identParameterFormatter = new IdentificationParameterFormatter();

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
            var serializedObject = DeserializeParameterSetObject(json);
            if(serializedObject.IdentificationParameterSet.IdentificationParameter != null)
            {
                this.identParameterFormatter.BuildFormattedAttributes(serializedObject);
            }
            else
            {
                var serializedList = DeserializeParameterSetList(json);
                this.identParameterFormatter.BuildFormattedAttributes(serializedList);
            }

            itahcFromContext.defraimp_formattedIdentificationOfAnimalsText = identParameterFormatter.FinalString;
            itahcFromContext.defraimp_CommodityIdTypes = identParameterFormatter.CommodityIdTypes;
            itahcFromContext.defraimp_PassportNumber = identParameterFormatter.PassportNumber;

            return identParameterFormatter.FinalString;
        }

        private IdentificationParameterSetObject DeserializeParameterSetObject(string json)
        {
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
            return serializedObject;
        }

        private IdentificationParameterSetList DeserializeParameterSetList(string json)
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
            return serializedList;
        }
    }
}
