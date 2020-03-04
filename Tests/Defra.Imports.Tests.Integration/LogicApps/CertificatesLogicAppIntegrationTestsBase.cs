using Microsoft.ServiceBus.Messaging;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Xunit;

namespace Defra.Imports.Tests.Integration.LogicApps
{
    public class CertificatesLogicAppIntegrationTestsBase : IntegrationTests
    {

        protected List<string> GetCertificateReferenceNumbersFromXml(List<string> certificateXmlStringList)
        {
            List<string> certificateReferenceNumbers = new List<string>();
            Regex referenceNumberRegEx = new Regex("<CertificateReferenceNumber>(.+)</CertificateReferenceNumber>");
            foreach (string itahcXml in certificateXmlStringList)
            {
                var matches = referenceNumberRegEx.Matches(itahcXml);
                foreach (Match match in matches)
                {
                    certificateReferenceNumbers.Add(match.Groups[1].Value);
                }
            }

            return certificateReferenceNumbers;
        }

        protected void ClearDownCertificateTest(string certificateType, DataCollection<Entity> certificates)
        {
            if (certificates.Count > 0)
            {
                foreach (Entity cert in certificates)
                {
                    // Delete the Certificate
                    _orgSvc.Delete($"defraimp_{certificateType}", cert.Id);
                }
            }
        }

        protected DataCollection<Entity> GetCertificatesByReferenceNumbers(string certificateEntityName, string certificateReferenceNumber, string[] columnsToRetrieve)
        {
            return GetCertificatesByReferenceNumbers(certificateEntityName, new string[] { certificateReferenceNumber }, columnsToRetrieve);
        }

        protected DataCollection<Entity> GetCertificatesByReferenceNumbers(string certificateEntityName, string[] certificateReferenceNumbers, string[] columnsToRetrieve)
        {
            QueryExpression qe = new QueryExpression(certificateEntityName);
            qe.ColumnSet = new ColumnSet(columnsToRetrieve);
            qe.Criteria.AddCondition("defraimp_name", ConditionOperator.In, certificateReferenceNumbers);
            EntityCollection eCollection = _orgSvc.RetrieveMultiple(qe);
            return eCollection.Entities;
        }

        protected List<string> GetCertificateXmlList(string xmlDocumentName, string certificateTagName)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load($"{Directory.GetCurrentDirectory()}\\TestData\\{xmlDocumentName}");
            XmlNodeList certNodes = doc.GetElementsByTagName(certificateTagName);

            List<string> outputCertList = new List<string>();
            using (StringWriter stringWriter = new StringWriter())
            {
                using (XmlTextWriter xmlWriter = new XmlTextWriter(stringWriter))
                {
                    xmlWriter.Formatting = Formatting.Indented;
                    xmlWriter.Indentation = 2;
                    foreach (XmlNode node in certNodes)
                    {
                        // Clear the string writer
                        StringBuilder sb = stringWriter.GetStringBuilder();
                        sb.Remove(0, sb.Length);

                        // Write node and add to list
                        node.WriteTo(xmlWriter);
                        outputCertList.Add(stringWriter.ToString());
                    }
                }
            }
            return outputCertList;
        }

        protected string GetTestCertificateXml(string filename, string certificateReferenceNumber)
        {
            string CertificateXml = File.ReadAllText($"{Directory.GetCurrentDirectory()}\\TestData\\{filename}");
            return String.Format(CertificateXml, certificateReferenceNumber);
        }

    }
}
