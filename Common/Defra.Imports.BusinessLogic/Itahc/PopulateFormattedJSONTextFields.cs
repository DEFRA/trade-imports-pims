using Defra.Imports.BusinessLogic.Itahc.JsonFormatterClassObjects;
using Defra.Imports.BusinessLogic.Itahc.JsonFormatterClassObjects.IdentificationParameterSetObjects;
using Defra.Imports.Model;
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
            if (this.itahcFromContext.Contains("defraimp_commoditycomplementstext"))
            {
                var commodityComplemets = ProcessCommodityComplementJson(this.itahcFromContext.defraimp_CommodityComplementsText);
                this.itahcFromContext.defraimp_FormattedCommodityComplementsText = commodityComplemets;
            }
            else if (this.itahcFromContext.Contains("defraimp_identificationofanimalstext"))
            {
                var identificationParameterSet = ProcessIdentificationParameterSetJson(this.itahcFromContext.defraimp_IdentificationOfAnimalsText);
                this.itahcFromContext.defraimp_formattedIdentificationOfAnimalsText = identificationParameterSet;
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

            var finalString = "CommodityCode: " + serializedObject.CommodityComplement.CommodityCode + System.Environment.NewLine
                            + System.Environment.NewLine
                            + "ComplementID: " + serializedObject.CommodityComplement.ComplementID + System.Environment.NewLine
                            + System.Environment.NewLine
                            + "SpeciesType: " + serializedObject.CommodityComplement.SpeciesType + System.Environment.NewLine
                            + System.Environment.NewLine
                            + "SpeciesModel: " + serializedObject.CommodityComplement.SpeciesModel + System.Environment.NewLine
                            + System.Environment.NewLine
                            + "Species: " + System.Environment.NewLine
                            + "SpeciesID: " + serializedObject.CommodityComplement.Species.SpeciesID + System.Environment.NewLine
                            + "SpeciesNomination: " + serializedObject.CommodityComplement.Species.SpeciesNomination;

            return finalString;
        }

        private string ProcessIdentificationParameterSetJson(string json)
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

            var finalString = string.Empty;

            serializedObject.IdentificationParameterSet.IdentificationParameter.ForEach(x =>
            {
                finalString += "Key: " + x.Key + System.Environment.NewLine
                             + "Data: " + x.Data + System.Environment.NewLine
                             + System.Environment.NewLine;
            });

            return finalString;
        }
    }
}
