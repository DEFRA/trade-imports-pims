using Defra.Imports.BusinessLogic.Itahc.JsonFormatterClassObjects;
using Defra.Imports.BusinessLogic.Itahc.JsonFormatterClassObjects.Formatters;
using Defra.Imports.BusinessLogic.Itahc.JsonFormatterClassObjects.IdentificationParameterSetObjects;
using Defra.Imports.Model;
using Microsoft.Xrm.Sdk;
using System;
using System.IO;
using System.Runtime.Serialization.Json;

namespace Defra.Imports.BusinessLogic.Itahc
{
    public class PopulateFormattedJSONTextFields
    {
        private defraimp_itahc _itahcFromContext;
        private defraimp_itahc _itahcPreImage;
        private IdentificationParameterFormatter _identParameterFormatter;

        public PopulateFormattedJSONTextFields(defraimp_itahc itahcFromContext, defraimp_itahc itahcPreImage)
        {
            this._itahcFromContext = itahcFromContext;
            this._itahcPreImage = itahcPreImage;

            string defaultSpeciesName = GetStringFieldFromTargetOrPreImage(_itahcFromContext, _itahcPreImage, "defraimp_speciesnomination");
            this._identParameterFormatter = new IdentificationParameterFormatter(defaultSpeciesName);

        }

        /// <summary>
        /// Method to update the formatted json fields
        /// </summary>
        public void FormatIntegrationData()
        {
            CommodityComplementObject commodityComplements = null;

            string commodityComplementsJson = GetStringFieldFromTargetOrPreImage(_itahcFromContext, _itahcPreImage, "defraimp_commoditycomplementstext");
            string identificationParameterSetJson = GetStringFieldFromTargetOrPreImage(_itahcFromContext, _itahcPreImage, "defraimp_identificationofanimalstext");

            if (!string.IsNullOrEmpty(commodityComplementsJson))
            {
                commodityComplements = ProcessCommodityComplementJson(commodityComplementsJson);
            }

            if (!string.IsNullOrEmpty(identificationParameterSetJson))
            {
                ProcessIdentificationParameterSetJson(identificationParameterSetJson, commodityComplements);
            }
        }

        private string GetStringFieldFromTargetOrPreImage(Entity target, Entity preImage, string fieldName)
        {
            String output = String.Empty;

            if(target != null && target.Contains(fieldName) && !String.IsNullOrEmpty(target.GetAttributeValue<string>(fieldName)))
            {
                output = target.GetAttributeValue<string>(fieldName);
            }
            else if(preImage != null && preImage.Contains(fieldName) && !String.IsNullOrEmpty(preImage.GetAttributeValue<string>(fieldName)))
            {
                output = preImage.GetAttributeValue<string>(fieldName);
            }

            return output;
        }

        /// <summary>
        /// Method to process the JSON string
        /// </summary>
        /// <param name="firstLevel"> First level of the Json </param>
        /// <param name="json"> Json string  </param>
        /// <returns> Returns a string with the formatted json values </returns>
        private CommodityComplementObject ProcessCommodityComplementJson(string json)
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

            return serializedObject;
        }

        private void ProcessIdentificationParameterSetJson(string json, CommodityComplementObject commodityComplements)
        {
            var serializedObject = DeserializeParameterSetObject(json);
            if(serializedObject.IdentificationParameterSet.IdentificationParameter != null)
            {
                this._identParameterFormatter.BuildFormattedAttributes(serializedObject, commodityComplements);
            }
            else
            {
                var serializedList = DeserializeParameterSetList(json);
                this._identParameterFormatter.BuildFormattedAttributes(serializedList, commodityComplements);
            }

            _itahcFromContext.defraimp_CommodityIdTypes = _identParameterFormatter.CommodityIdTypes;
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
