using Defra.Imports.Model;
using Defra.Imports.Workflows.ImportApplication;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Defra.Imports.UnitTests.Workflows.ImportApplication
{
    public class AutomatedRiskLevelAssignmentTests : WorkflowActivityTests<AutomatedRiskLevelAssignment>
    {
        private Guid _targetId;
        private Guid _countryOfOriginId;
        private Guid _commodityTypeId;
        private Dictionary<string, object> _inputs;
        private defraimp_importapplication _stubbedImportApplication;
        private defraimp_importcountrycommodityrisklevel _stubbedCountryCommodityRiskLevel;
        private EntityCollection _stubbedCountryCommodityRiskLevelCol;

        public AutomatedRiskLevelAssignmentTests()
        {
            InitialiseTest();
        }

        [Fact]
        public void Execute_AnyImportApplication_DoesNotSetTheManualImportCheckDecisionToBlank()
        {
            // Act
            this.WorkflowInvoker.Invoke(_inputs);

            // Assert
            string manualCheckAttributeName = "defraimp_manualpostimportcheckdecision";
            OrgSvcMock.Verify(
                o => o.Update(
                    It.Is<defraimp_importapplication>(
                        e => !e.Attributes.Contains(manualCheckAttributeName))
                )
            );
        }

        [Fact]
        public void Execute_CountryCommodityThatHasRiskLevel_SetsTheRiskLevelOnTheImportRecord()
        {
            // Arrange
            Guid riskLevelId = Guid.NewGuid();
            _stubbedCountryCommodityRiskLevel.defraimp_importrisklevelid = new EntityReference(defraimp_importrisklevel.EntityLogicalName, riskLevelId);

            // Act
            this.WorkflowInvoker.Invoke(_inputs);

            // Assert
            OrgSvcMock.Verify(
                o => o.Update(
                    It.Is<defraimp_importapplication>(
                        e => e.defraimp_importrisklevelid.Id.Equals(riskLevelId) && 
                        e.defraimp_ImportRiskLevelStatus.Equals(defraimp_importapplication_defraimp_importrisklevelstatus.AutomaticallyRiskAssessed))
                )
            );

        }

        [Fact]
        public void Execute_CountryCommodityThatHasNoRiskLevel_UpdatesTheImportApplicationWithCorrectRiskLevelStatus()
        {
            // Act
            this.WorkflowInvoker.Invoke(_inputs);

            // Assert
            VerifyUpdateStatus(defraimp_importapplication_defraimp_importrisklevelstatus.UnabletoAutomaticallyRiskAssessNoCorrespondingRiskLevel);
        }

        [Fact]
        public void Execute_WithoutCountryCommodityRiskLevel_UpdatesTheImportApplicationWithCorrectRiskLevelStatus()
        {
            // Arrange
            // Remove the entities from the retrieved collection
            _stubbedCountryCommodityRiskLevelCol.Entities.Clear();

            // Act
            this.WorkflowInvoker.Invoke(_inputs);

            // Assert
            VerifyUpdateStatus(defraimp_importapplication_defraimp_importrisklevelstatus.UnabletoAutomaticallyRiskAssessNoCorrespondingRiskLevel);
        }

        [Fact]
        public void Execute_WithoutCountry_UpdatesTheImportApplicationWithCorrectRiskLevelStatus()
        {
            _stubbedImportApplication.defraimp_CountryofOriginId = null;
            _stubbedImportApplication.Attributes.Remove("defraimp_countryoforiginid");

            this.WorkflowInvoker.Invoke(_inputs);

            VerifyUpdateStatus(defraimp_importapplication_defraimp_importrisklevelstatus.UnabletoAutomaticallyRiskAssessMissingData);
        }

        [Fact]
        public void Execute_WithoutCommodity_UpdatesTheImportApplicationWithCorrectRiskLevelStatus()
        {
            _stubbedImportApplication.defraimp_CommodityTypeId = null;
            _stubbedImportApplication.Attributes.Remove("defraimp_commoditytypeid");

            this.WorkflowInvoker.Invoke(_inputs);

            VerifyUpdateStatus(defraimp_importapplication_defraimp_importrisklevelstatus.UnabletoAutomaticallyRiskAssessMissingData);
        }


        private void VerifyUpdateStatus(defraimp_importapplication_defraimp_importrisklevelstatus riskLevelStatus)
        {
            // Assert
            OrgSvcMock.Verify(
                o => o.Update(
                    It.Is<defraimp_importapplication>(
                        e => e.defraimp_importrisklevelid == null &&
                        e.defraimp_ImportRiskLevelStatus.Equals(riskLevelStatus))
                )
            );
        }

        private void InitialiseTest()
        {
            _targetId = Guid.NewGuid();
            _countryOfOriginId = Guid.NewGuid();
            _commodityTypeId = Guid.NewGuid();

            _inputs = new Dictionary<string, object>()
            {
                { "ImportApplication", new EntityReference(defraimp_importapplication.EntityLogicalName, _targetId) }
            };

            // Mock the retrieval of an import application
            _stubbedImportApplication = new defraimp_importapplication()
            {
                Id = _targetId,
                defraimp_importapplicationId = _targetId,
                defraimp_CountryofOriginId = new EntityReference("defra_country", _countryOfOriginId),
                defraimp_CommodityTypeId = new EntityReference("defraexp_commoditytype", _commodityTypeId)
            };
            OrgSvcMock
                .Setup(o => o.Retrieve(defraimp_importapplication.EntityLogicalName, _targetId, It.IsAny<ColumnSet>()))
                .Returns(_stubbedImportApplication);

            // Mock the retrieval of the risk level
            _stubbedCountryCommodityRiskLevel = new defraimp_importcountrycommodityrisklevel()
            {
                defraimp_countryid = new EntityReference("defra_country", _countryOfOriginId),
                defraimp_commoditytypeid = new EntityReference("defraexp_commoditytype", _commodityTypeId)
            };

            _stubbedCountryCommodityRiskLevelCol = new EntityCollection();
            _stubbedCountryCommodityRiskLevelCol.Entities.Add(_stubbedCountryCommodityRiskLevel);

            OrgSvcMock.Setup(
                o => o.RetrieveMultiple(
                    It.Is<QueryExpression>(q =>
                        q.EntityName.Equals(defraimp_importcountrycommodityrisklevel.EntityLogicalName) &&
                        q.Criteria.Conditions[0].AttributeName.Equals("defraimp_countryid") && q.Criteria.Conditions[0].Values[0].Equals(_countryOfOriginId) &&
                        q.Criteria.Conditions[1].AttributeName.Equals("defraimp_commoditytypeid") && q.Criteria.Conditions[1].Values[0].Equals(_commodityTypeId))
                )
            ).Returns(_stubbedCountryCommodityRiskLevelCol);
        }
    }
}
