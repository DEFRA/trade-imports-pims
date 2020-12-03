using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Defra.Imports.Tests.Integration.LogicApps
{
    public class DocomCertificatesLogicAppIntegrationTests : CertificatesLogicAppIntegrationTestsBase
    {
        // Change this to null to run these tests
        const string skip = "Skip Logic App Tests";

        [Fact(Skip = skip)]
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
            DataCollection<Entity> retrievedDocoms = GetCertificatesByReferenceNumbers("defraimp_docom", certificateReferenceNumber, Array.Empty<string>());
            Assert.True(retrievedDocoms.Count > 0);

            // Clear Down
            ClearDownCertificateTest("docom", retrievedDocoms);

        }

        [Fact(Skip = skip)]
        [ExcludeFromCodeCoverage]
        public void SendToSBQueue_ListOfDocoms_ListOfDocomsAreCreated()
        {
            List<string> docomXmlList = GetDocomListXml();

            docomXmlList.ForEach(item => this.SendServiceBusMessage(item));
            Thread.Sleep(300000);

            // Assert
            List<string> certificateReferenceNumbers = GetCertificateReferenceNumbersFromXml(docomXmlList);
            DataCollection<Entity> retrievedDocoms = GetCertificatesByReferenceNumbers("defraimp_docom", certificateReferenceNumbers.ToArray(), Array.Empty<string>());
            Assert.True(retrievedDocoms.Count == 50);

            // Clear Down
            ClearDownCertificateTest("docom", retrievedDocoms);
        }

        private string GetDocomXml(string certificateReferenceNumber)
        {
            return GetTestCertificateXml("DOCOM1.xml", certificateReferenceNumber);
        }

        private List<string> GetDocomListXml()
        {
            return GetCertificateXmlList("DOCOM_LIST.xml", "ns2:doCom");
        }
    }
}
