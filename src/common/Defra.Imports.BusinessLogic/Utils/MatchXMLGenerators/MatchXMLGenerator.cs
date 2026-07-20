using Defra.Imports.Model;
using Microsoft.Xrm.Sdk;
using System.Collections.Generic;
using System.Xml;

namespace Defra.Imports.BusinessLogic.Utils
{
    class MatchXmlGenerator
    {
        private IRepositoryFactory _repositoryFactory;
        private string _mappingXml;
        private defraimp_matchrecord _matchRecord;
        private EntityReference _mappingXmlRef;

        //DO WE NEED BOTH AT THE SAME TIME? WE WILL CALL THIS TWICE. MAKE MORE GENERIC?

        public MatchXmlGenerator(IRepositoryFactory repositoryFactory, defraimp_matchrecord matchRecord, EntityReference mappingXmlRef)
        {
            _repositoryFactory = repositoryFactory;
            _mappingXmlRef = mappingXmlRef;
            _matchRecord = matchRecord;

            _mappingXml = GetContentOfXml(_mappingXmlRef);
        }

        private string GetContentOfXml(EntityReference webResource)
        {
            IXmlRetriever webResourceXMLRetriever = new WebResourceXmlRetriever(_repositoryFactory, webResource);
            string outputXml = webResourceXMLRetriever.GetXml();
            return outputXml;
        }

        public string GenerateITAHCMatchXml()
        {
            List<string> itahcFieldsToMap = new List<string>();

            itahcFieldsToMap.AddRange(GetITAHCNodeValues());

            XmlDocument itahcXmlDocument = new XmlDocument();

            XmlNode element = itahcXmlDocument.CreateNode(XmlNodeType.Element, "FieldMappings", "FieldMappings");

            foreach (string field in itahcFieldsToMap)
            {
                XmlDocument xmlLine = new XmlDocument();
                xmlLine.LoadXml(field);
                XmlNode node = xmlLine.DocumentElement;
                XmlNode importNode = itahcXmlDocument.ImportNode(node, true);
                element.AppendChild(importNode);
            }

            itahcXmlDocument.AppendChild(element);

            return itahcXmlDocument.OuterXml;
        }

        public string GenerateImporterNotificationMatchXML()
        {
            List<string> importerNotificationFieldsToMap = new List<string>();

            importerNotificationFieldsToMap.AddRange(GetImporterNotificationNodeValues());

            XmlDocument importerNotificationXmlDocument = new XmlDocument();

            XmlNode element = importerNotificationXmlDocument.CreateNode(XmlNodeType.Element, "FieldMappings", "FieldMappings");

            foreach (string field in importerNotificationFieldsToMap)
            {
                XmlDocument xmlLine = new XmlDocument();
                xmlLine.LoadXml(field);
                XmlNode node = xmlLine.DocumentElement;
                XmlNode importNode = importerNotificationXmlDocument.ImportNode(node, true);
                element.AppendChild(importNode);
            }

            importerNotificationXmlDocument.AppendChild(element);

            return importerNotificationXmlDocument.OuterXml;
        }

        private List<string> GetITAHCNodeValues()
        {
            List<string> fieldsToMap = new List<string>();

            //Parse the required ITAHC fields
            fieldsToMap.AddRange(ParseMappingConfig("Generic", _mappingXml));

            //False is Importer Notification, true is ITAHC
            if (_matchRecord.defraimp_CopyConsignorFrom == true)
            {
                fieldsToMap.AddRange(ParseMappingConfig("Consignor", _mappingXml));
            }

            //False is Importer Notification, true is ITAHC
            if (_matchRecord.defraimp_CopyConsigneeFrom == true)
            {
                fieldsToMap.AddRange(ParseMappingConfig("Consignee", _mappingXml));
            }

            //False is Importer Notification, true is ITAHC
            if (_matchRecord.defraimp_CopyPlaceofDestinationFrom == true)
            {
                fieldsToMap.AddRange(ParseMappingConfig("PlaceOfDestination", _mappingXml));
            }

            //False is Importer Notification, true is ITAHC
            if (_matchRecord.defraimp_CopyPlaceofOriginFrom == true)
            {
                fieldsToMap.AddRange(ParseMappingConfig("PlaceOfOrigin", _mappingXml));
            }

            if (_matchRecord.defraimp_CopyTransporterFrom == true)
            {
                fieldsToMap.AddRange(ParseMappingConfig("Transporter", _mappingXml));
            }

            //False is Importer Notification, true is ITAHC
            if (_matchRecord.defraimp_CopyCommodityFrom == true)
            {
                fieldsToMap.AddRange(ParseMappingConfig("Commodity", _mappingXml));
            }

            return fieldsToMap;
        }

        private List<string> GetImporterNotificationNodeValues()
        {
            List<string> fieldsToMap = new List<string>();

            //Parse the required Notification fields
            fieldsToMap.AddRange(ParseMappingConfig("Generic", _mappingXml));

            //False is Importer Notification, true is ITAHC
            if (_matchRecord.defraimp_CopyConsignorFrom == false)
            {
                fieldsToMap.AddRange(ParseMappingConfig("Consignor", _mappingXml));
            }

            //False is Importer Notification, true is ITAHC
            if (_matchRecord.defraimp_CopyConsigneeFrom == false)
            {
                fieldsToMap.AddRange(ParseMappingConfig("Consignee", _mappingXml));
            }

            //False is Importer Notification, true is ITAHC
            if (_matchRecord.defraimp_CopyPlaceofDestinationFrom == false)
            {
                fieldsToMap.AddRange(ParseMappingConfig("PlaceOfDestination", _mappingXml));
            }

            //False is Importer Notification, true is ITAHC
            if (_matchRecord.defraimp_CopyPlaceofOriginFrom == false)
            {
                fieldsToMap.AddRange(ParseMappingConfig("PlaceOfOrigin", _mappingXml));
            }

            //False is Importer Notification, true is ITAHC
            if (_matchRecord.defraimp_CopyTransporterFrom == false)
            {
                fieldsToMap.AddRange(ParseMappingConfig("Transporter", _mappingXml));
            }

            //False is Importer Notification, true is ITAHC
            if (_matchRecord.defraimp_CopyCommodityFrom == false)
            {
                fieldsToMap.AddRange(ParseMappingConfig("Commodity", _mappingXml));
            }

            return fieldsToMap;
        }

        public List<string> ParseMappingConfig(string nodeName, string xmlContents)
        {
            // TODO add code that reads a schema and fails if it doesn't match the schema =========

            List<string> outputMap = new List<string>();

            // parse the contents of the xml
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlContents);
            XmlNode node = xmlDoc.SelectSingleNode("/FieldMappings/" + nodeName);
            XmlNodeList fieldMappingNodes = node.ChildNodes;
            foreach (XmlNode fieldMappingNode in fieldMappingNodes)
            {
                if (fieldMappingNode.Attributes != null)
                {
                    outputMap.Add(fieldMappingNode.OuterXml);
                }
            }

            return outputMap;
        }
    }
}
