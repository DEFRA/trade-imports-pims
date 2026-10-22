namespace Defra.Imports.UnitTests.Dataverse.Extensions
{
    using System;
    using System.Linq;
    using Defra.Imports.Dataverse.Extensions;
    using Defra.Imports.Dataverse.Model;
    using Microsoft.Xrm.Sdk;
    using Xunit;

    public sealed class ParameterSetKeyDataPairExtensionsTests
    {
        [Fact]
        public void ToApplyCommodityComplementKeyDataContext_NullKeyDataPair_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ((defraimp_parametersetkeydatapair)null).ToApplyCommodityComplementKeyDataContext());
        }

        [Fact]
        public void ToApplyCommodityComplementKeyDataContext_KeyDataPair_ProjectsTheKeyTheValueAndTheReferences()
        {
            var importerNotificationId = Guid.NewGuid();
            var parameterSetId = Guid.NewGuid();
            var keyDataPair = new defraimp_parametersetkeydatapair
            {
                defraimp_name = "number_animal",
                defraimp_data = "12",
                defraimp_ImporterNotificationId = new EntityReference(defraimp_ImporterNotification.EntityLogicalName, importerNotificationId),
                defraimp_CommodityComplementParameterSet = new EntityReference(defraimp_commoditycomplementparameterset.EntityLogicalName, parameterSetId) { Name = "42" },
            };

            var context = keyDataPair.ToApplyCommodityComplementKeyDataContext();

            Assert.Equal("number_animal", context.Key);
            Assert.Equal("12", context.Data);
            Assert.Equal(importerNotificationId, context.ImporterNotificationId);
            Assert.Equal(parameterSetId, context.CommodityComplementParameterSetId);
            Assert.Equal("42", context.CommodityComplementParameterSetName);
        }

        [Fact]
        public void ToApplyCommodityComplementKeyDataContext_KeyDataPairWithoutTheAttributesSupplied_ProjectsNulls()
        {
            var context = new defraimp_parametersetkeydatapair { Id = Guid.NewGuid() }.ToApplyCommodityComplementKeyDataContext();

            Assert.Null(context.Key);
            Assert.Null(context.Data);
            Assert.Null(context.ImporterNotificationId);
            Assert.Null(context.CommodityComplementParameterSetId);
            Assert.Null(context.CommodityComplementParameterSetName);
        }

        [Fact]
        public void ToApplyCommodityComplementKeyDataContext_ParameterSetReferenceWithoutAName_ProjectsANullName()
        {
            var parameterSetId = Guid.NewGuid();
            var keyDataPair = new defraimp_parametersetkeydatapair
            {
                defraimp_CommodityComplementParameterSet = new EntityReference(defraimp_commoditycomplementparameterset.EntityLogicalName, parameterSetId),
            };

            var context = keyDataPair.ToApplyCommodityComplementKeyDataContext();

            Assert.Equal(parameterSetId, context.CommodityComplementParameterSetId);
            Assert.Null(context.CommodityComplementParameterSetName);
        }

        [Fact]
        public void ToApplyCommodityComplementKeyDataContext_KeyDataPair_DoesNotModifyTheKeyDataPair()
        {
            var keyDataPair = new defraimp_parametersetkeydatapair { defraimp_name = "number_animal", defraimp_data = "12" };
            var attributes = keyDataPair.Attributes.ToDictionary(pair => pair.Key, pair => pair.Value);

            keyDataPair.ToApplyCommodityComplementKeyDataContext();

            Assert.Equal(attributes, keyDataPair.Attributes.ToDictionary(pair => pair.Key, pair => pair.Value));
        }
    }
}
