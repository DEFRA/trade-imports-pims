using System.Collections.Generic;
using System.Xml;

namespace Defra.Imports.BusinessLogic.Utils
{
    public class FieldMappingConfigParser : IFieldMappingConfigParser
    {
        /*
         * Contents of the configContents xml text should be in the following format
         * 
         * <FielMappings>
         *  <FieldMapping mapfrom="defraimp_name" mapto="defraimp_title" />
         *  <FieldMapping mapfrom="defraimp_organisation" mapto="defraimp_account" />
         * </FieldMappings>
         * 
         */
        public Dictionary<string, string> ParseMappingConfig(string configContents)
        {

            // TODO add code that reads a schema and fails if it doesn't match the schema =========

            Dictionary<string, string> outputMap = new Dictionary<string, string>();

            // parse the contents of the xml
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(configContents);
            XmlNodeList fieldMappingNodes = xmlDoc.GetElementsByTagName("FieldMapping");

            foreach (XmlNode fieldMappingNode in fieldMappingNodes)
            {

                if(fieldMappingNode.Attributes != null)
                {
                    string mapFromField = fieldMappingNode.Attributes["mapfrom"].Value;
                    string mapToField = fieldMappingNode.Attributes["mapto"].Value;

                    if(mapFromField != null && mapToField != null)
                    {
                        outputMap.Add(mapFromField, mapToField);
                    }
                }
            }

            return outputMap;

        }
    }
}
