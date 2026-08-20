namespace Defra.Imports.IntegrationTests.LogicApps
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading;
    using Microsoft.Xrm.Sdk;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

  [TestClass]
  [Ignore("These tests are ignored pending review")]
  public class ItahcCertificatesLogicAppIntegrationTests : CertificatesLogicAppIntegrationTestsBase
  {
    [TestMethod]
    [ExcludeFromCodeCoverage]
    public void SendToSBQueue_AValidItahcXMLMessage_ItahcIsCreatedInDynamics()
    {
      // Arrange
      string certificateReferenceNumber = "INTRA.CZ.2019.0019190";
      string itahcXmlMessage = this.GetItahcXml(certificateReferenceNumber);

      // Act
      this.SendServiceBusMessage(itahcXmlMessage);
      Thread.Sleep(150000);

      // Assert
      DataCollection<Entity> retrievedItahcs = this.GetCertificatesByReferenceNumbers("defraimp_itahc", certificateReferenceNumber, Array.Empty<string>());
      Assert.IsTrue(retrievedItahcs.Count > 0);

      // Clear Down
      this.ClearDownCertificateTest("itahc", retrievedItahcs);
    }

    [TestMethod]
    [ExcludeFromCodeCoverage]
    public void SendToSBQueue_ItahcWithIdentificationParameters_ItahcIsCreatedWithIdentificationParameters()
    {
      // Arrange
      string certificateReferenceNumber = "INTRA.CZ.2019.0019190";
      string itahcXmlMessage = this.GetItahcXml(certificateReferenceNumber);

      // Act
      this.SendServiceBusMessage(itahcXmlMessage);
      Thread.Sleep(150000);

      // Assert
      DataCollection<Entity> retrievedItahcs = this.GetCertificatesByReferenceNumbers("defraimp_itahc", certificateReferenceNumber, new string[] { "defraimp_identificationofanimalstext" });
      Assert.IsTrue(retrievedItahcs.Count > 0);
      Assert.IsTrue(retrievedItahcs.First().Attributes.Contains("defraimp_identificationofanimalstext"));

      this.ClearDownCertificateTest("itahc", retrievedItahcs);
    }

    [TestMethod]
    [ExcludeFromCodeCoverage]
    public void SendToSBQueue_ItahcWithCommodityComplements_ItahcIsCreatedWithCommodityComplements()
    {
      // Arrange
      string certificateReferenceNumber = "INTRA.CZ.2019.0019190";
      string itahcXmlMessage = this.GetItahcXml(certificateReferenceNumber);

      // Act
      this.SendServiceBusMessage(itahcXmlMessage);
      Thread.Sleep(150000);

      // Assert
      DataCollection<Entity> retrievedItahcs = this.GetCertificatesByReferenceNumbers("defraimp_itahc", certificateReferenceNumber, new string[] { "defraimp_commoditycomplementstext" });
      Assert.IsTrue(retrievedItahcs.Count > 0);
      Assert.IsTrue(retrievedItahcs.First().Attributes.Contains("defraimp_commoditycomplementstext"));

      this.ClearDownCertificateTest("itahc", retrievedItahcs);
    }

    [TestMethod]
    [ExcludeFromCodeCoverage]
    public void SendToSBQueue_ListOfItahcs_ListOfItahcsAreCreated()
    {
      // Arrange
      List<string> itahcXmlList = this.GetItahcListXml();

      // Act
      itahcXmlList.ForEach(item => this.SendServiceBusMessage(item));
      Thread.Sleep(300000);

      // Assert
      List<string> certificateReferenceNumbers = this.GetCertificateReferenceNumbersFromXml(itahcXmlList);
      DataCollection<Entity> retrievedItahcs = this.GetCertificatesByReferenceNumbers("defraimp_itahc", certificateReferenceNumbers.ToArray(), Array.Empty<string>());
      Assert.IsTrue(retrievedItahcs.Count == 50);

      // Clear Down
      this.ClearDownCertificateTest("itahc", retrievedItahcs);
    }

    private string GetItahcXml(string certificateReferenceNumber)
    {
      return this.GetTestCertificateXml("ITAHC1.xml", certificateReferenceNumber);
    }

    private List<string> GetItahcListXml()
    {
      return this.GetCertificateXmlList("ITAHC_LIST.xml", "ns2:intraTrade");
    }
  }
}

