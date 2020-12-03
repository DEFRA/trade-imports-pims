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
  public class ItahcCertificatesLogicAppIntegrationTests : CertificatesLogicAppIntegrationTestsBase
  {
    // Change this to null to run these tests
    const string skip = "Skip Logic App Tests";

    [Fact(Skip = skip)]
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
      DataCollection<Entity> retrievedItahcs = GetCertificatesByReferenceNumbers("defraimp_itahc", certificateReferenceNumber, Array.Empty<string>());
      Assert.True(retrievedItahcs.Count > 0);

      // Clear Down
      ClearDownCertificateTest("itahc", retrievedItahcs);
    }

    [Fact(Skip = skip)]
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
      DataCollection<Entity> retrievedItahcs = GetCertificatesByReferenceNumbers("defraimp_itahc", certificateReferenceNumber, new string[] { "defraimp_identificationofanimalstext" });
      Assert.True(retrievedItahcs.Count > 0);
      Assert.True(retrievedItahcs.First().Attributes.Contains("defraimp_identificationofanimalstext"));

      ClearDownCertificateTest("itahc", retrievedItahcs);
    }

    [Fact(Skip = skip)]
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
      DataCollection<Entity> retrievedItahcs = GetCertificatesByReferenceNumbers("defraimp_itahc", certificateReferenceNumber, new string[] { "defraimp_commoditycomplementstext" });
      Assert.True(retrievedItahcs.Count > 0);
      Assert.True(retrievedItahcs.First().Attributes.Contains("defraimp_commoditycomplementstext"));

      ClearDownCertificateTest("itahc", retrievedItahcs);
    }

    [Fact(Skip = skip)]
    [ExcludeFromCodeCoverage]
    public void SendToSBQueue_ListOfItahcs_ListOfItahcsAreCreated()
    {
      // Arrange
      List<string> itahcXmlList = GetItahcListXml();

      // Act
      itahcXmlList.ForEach(item => this.SendServiceBusMessage(item));
      Thread.Sleep(300000);

      // Assert
      List<string> certificateReferenceNumbers = GetCertificateReferenceNumbersFromXml(itahcXmlList);
      DataCollection<Entity> retrievedItahcs = GetCertificatesByReferenceNumbers("defraimp_itahc", certificateReferenceNumbers.ToArray(), Array.Empty<string>());
      Assert.True(retrievedItahcs.Count == 50);

      // Clear Down
      ClearDownCertificateTest("itahc", retrievedItahcs);
    }

    private string GetItahcXml(string certificateReferenceNumber)
    {
      return GetTestCertificateXml("ITAHC1.xml", certificateReferenceNumber);
    }

    private List<string> GetItahcListXml()
    {
      return GetCertificateXmlList("ITAHC_LIST.xml", "ns2:intraTrade");
    }
  }
}
