using Microsoft.ServiceBus.Messaging;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Xunit;

namespace Defra.Imports.Tests.Integration.LogicApps
{
    public class CertificatesLogicAppIntegrationTests : IntegrationTests
    {

        [Fact]
        [ExcludeFromCodeCoverage]
        public void SendToSBQueue_AValidItahcXMLMessage_ItahcIsCreatedInDynamics()
        {
            // Arrange
            string certificateReferenceNumber = "INTRA.CZ.2019.0019190";
            string itahcXmlMessage = GetItahcXml(certificateReferenceNumber);

            // Act
            this.SendServiceBusMessage(itahcXmlMessage);
            Thread.Sleep(150000);

            // Assert
            DataCollection<Entity> retrievedItahcs = GetCertificateByReferenceNumber("defraimp_itahc", certificateReferenceNumber, Array.Empty<string>());
            Assert.True(retrievedItahcs.Count > 0);

            // Clear Down
            ClearDownCertificateTest("itahc", retrievedItahcs);
        }

        [Fact]
        [ExcludeFromCodeCoverage]
        public void SendToSBQueue_ItahcWithIdentificationParameters_ItahcIsCreatedWithIdentificationParameters()
        {
            // Arrange
            string certificateReferenceNumber = "INTRA.CZ.2019.0019190";
            string itahcXmlMessage = GetItahcXml(certificateReferenceNumber);

            // Act
            this.SendServiceBusMessage(itahcXmlMessage);
            Thread.Sleep(150000);

            // Assert
            DataCollection<Entity> retrievedItahcs = GetCertificateByReferenceNumber("defraimp_itahc", certificateReferenceNumber, new string[] { "defraimp_identificationofanimalstext" });
            Assert.True(retrievedItahcs.Count > 0);
            Assert.True(retrievedItahcs.First().Attributes.Contains("defraimp_identificationofanimalstext"));

            ClearDownCertificateTest("itahc", retrievedItahcs);
        }

        [Fact]
        [ExcludeFromCodeCoverage]
        public void SendToSBQueue_ItahcWithCommodityComplements_ItahcIsCreatedWithCommodityComplements()
        {
            // Arrange
            string certificateReferenceNumber = "INTRA.CZ.2019.0019190";
            string itahcXmlMessage = GetItahcXml(certificateReferenceNumber);

            // Act
            this.SendServiceBusMessage(itahcXmlMessage);
            Thread.Sleep(150000);

            // Assert
            DataCollection<Entity> retrievedItahcs = GetCertificateByReferenceNumber("defraimp_itahc", certificateReferenceNumber, new string[] { "defraimp_commoditycomplementstext" });
            Assert.True(retrievedItahcs.Count > 0);
            Assert.True(retrievedItahcs.First().Attributes.Contains("defraimp_commoditycomplementstext"));

            ClearDownCertificateTest("itahc", retrievedItahcs);
        }

        [Fact(Skip = "This is skipped as we don't want to flood our system with records")]
        [ExcludeFromCodeCoverage]
        public void SendToSBQueue_ListOfItahcs_ListOfItahcsAreCreated()
        {
            List<string> itahcXmlList = GetItahcListXml();

            itahcXmlList.ForEach(item => this.SendServiceBusMessage(item));
        }

        [Fact]
        [ExcludeFromCodeCoverage]
        public void SendToSBQueue_AValidDocomXMLMessage_DOCOMIsCreatedInDynamics()
        {
            // Arrange
            string certificateReferenceNumber = "DOCOM.CZ.2016.0001882";
            string docomXmlMessage = GetDocomXml(certificateReferenceNumber);

            // Act
            this.SendServiceBusMessage(docomXmlMessage);
            Thread.Sleep(150000);

            // Assert
            DataCollection<Entity> retrievedDocoms = GetCertificateByReferenceNumber("defraimp_docom", certificateReferenceNumber, Array.Empty<string>());
            Assert.True(retrievedDocoms.Count > 0);

            // Clear Down
            ClearDownCertificateTest("docom", retrievedDocoms);

        }


        [Fact(Skip = "This is skipped as we don't want to flood our system with records")]
        [ExcludeFromCodeCoverage]
        public void SendToSBQueue_ListOfDocoms_ListOfDocomsAreCreated()
        {
            List<string> docomXmlList = GetDocomListXml();

            docomXmlList.ForEach(item => this.SendServiceBusMessage(item));
        }

        private void ClearDownCertificateTest(string certificateType, DataCollection<Entity> certificates)
        {
            if (certificates.Count > 0)
            {
                Entity foundEntity = certificates.First();

                // Delete the linked import records
                DataCollection<Entity> retrievedImportRecords = GetImportRecordByCertificate(certificateType, foundEntity.Id);
                foreach (Entity importRecord in retrievedImportRecords)
                {
                    _orgSvc.Delete(importRecord.LogicalName, importRecord.Id);
                }

                // Delete the Itahc
                _orgSvc.Delete($"defraimp_{certificateType}", foundEntity.Id);
            }
        }

        private DataCollection<Entity> GetCertificateByReferenceNumber(string certificateEntityName, string certificateReferenceNumber, string[] columnsToRetrieve)
        {
            QueryExpression qe = new QueryExpression(certificateEntityName);
            qe.ColumnSet = new ColumnSet(columnsToRetrieve);
            qe.Criteria.AddCondition("defraimp_name", ConditionOperator.Equal, certificateReferenceNumber);
            EntityCollection eCollection = _orgSvc.RetrieveMultiple(qe);
            return eCollection.Entities;
        }

        private DataCollection<Entity> GetImportRecordByCertificate(string certificateType, Guid certificateId)
        {
            QueryExpression qe = new QueryExpression("defraimp_importapplication");
            qe.Criteria.AddCondition($"defraimp_primary{certificateType}id", ConditionOperator.Equal, certificateId);
            EntityCollection eCollection = _orgSvc.RetrieveMultiple(qe);
            return eCollection.Entities;
        }

        private List<string> GetItahcListXml()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load($"{Directory.GetCurrentDirectory()}\\TestData\\ITAHC_LIST.xml");
            XmlNodeList certNodes = doc.GetElementsByTagName("ns2:intraTrade");

            List<string> outputCertList = new List<string>();
            using(StringWriter stringWriter = new StringWriter())
            {
                using(XmlTextWriter xmlWriter = new XmlTextWriter(stringWriter))
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

        private List<string> GetDocomListXml()
        {
            return GetCertificateXmlList("DOCOM_LIST.xml", "ns2:doCom");
        }

        private List<string> GetCertificateXmlList(string xmlDocumentName, string certificateTagName)
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

        private string GetItahcXml(string certificateReferenceNumber)
        {
            return GetTestCertificateXml("ITAHC1.xml", certificateReferenceNumber);
        }


        private string GetDocomXml(string certificateReferenceNumber)
        {
            return GetTestCertificateXml("DOCOM1.xml", certificateReferenceNumber);
        }

        private string GetTestCertificateXml(string filename, string certificateReferenceNumber)
        {
            string CertificateXml = File.ReadAllText($"{Directory.GetCurrentDirectory()}\\TestData\\{filename}");
            return String.Format(CertificateXml, certificateReferenceNumber);
        }

    }
}
