namespace Defra.Imports.IntegrationTests.LogicApps
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Xml;
    using Defra.Imports.IntegrationTests.ServiceBus;
    using Microsoft.PowerPlatform.Dataverse.Client;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Query;

    public class CertificatesLogicAppIntegrationTestsBase : IntegrationTests, IDisposable
    {
        private readonly ServiceBusFixture serviceBus;
        private readonly ServiceClient appUserClient;
        private bool disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="CertificatesLogicAppIntegrationTestsBase"/> class.
        /// </summary>
        protected CertificatesLogicAppIntegrationTestsBase()
        {
            this.serviceBus = this.GetServiceBusFixture(TestConfig.ServiceBus.HealthCertQueue);
            this.appUserClient = this.GetAppUserClient();
        }

        /// <summary>
        /// Sends a message to the health certificate Service Bus queue.
        /// </summary>
        /// <param name="message">The message content to send.</param>
        protected void SendServiceBusMessage(string message)
        {
            this.serviceBus.SendMessage(message);
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases the underlying <see cref="ServiceBusFixture"/>.
        /// </summary>
        /// <param name="disposing">Whether this is being called from <see cref="Dispose()"/>.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

            if (disposing)
            {
                this.serviceBus?.Dispose();
            }

            this.disposed = true;
        }

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
                    this.appUserClient.Delete($"defraimp_{certificateType}", cert.Id);
                }
            }
        }

        protected DataCollection<Entity> GetCertificatesByReferenceNumbers(string certificateEntityName, string certificateReferenceNumber, string[] columnsToRetrieve)
        {
            return this.GetCertificatesByReferenceNumbers(certificateEntityName, new string[] { certificateReferenceNumber }, columnsToRetrieve);
        }

        protected DataCollection<Entity> GetCertificatesByReferenceNumbers(string certificateEntityName, string[] certificateReferenceNumbers, string[] columnsToRetrieve)
        {
            QueryExpression qe = new QueryExpression(certificateEntityName);
            qe.ColumnSet = new ColumnSet(columnsToRetrieve);
            qe.Criteria.AddCondition("defraimp_name", ConditionOperator.In, certificateReferenceNumbers);
            EntityCollection eCollection = this.appUserClient.RetrieveMultiple(qe);
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
