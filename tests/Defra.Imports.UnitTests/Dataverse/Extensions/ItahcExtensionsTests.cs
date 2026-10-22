namespace Defra.Imports.UnitTests.Dataverse.Extensions
{
    using System;
    using System.Linq;
    using Defra.Imports.Dataverse.Extensions;
    using Defra.Imports.Dataverse.Model;
    using Defra.Imports.Dataverse.Projections;
    using Xunit;

    public sealed class ItahcExtensionsTests
    {
        [Fact]
        public void ToCreateWatchFlagsContext_NullItahc_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ((defraimp_itahc)null).ToCreateWatchFlagsContext());
        }

        [Fact]
        public void ToCreateWatchFlagsContext_Itahc_ProjectsTheReferenceNameAndEachOperatorName()
        {
            var itahc = new defraimp_itahc
            {
                Id = Guid.NewGuid(),
                defraimp_name = "ITAHC",
                defraimp_PlaceOfOriginHarvestName = "origin",
                defraimp_PlaceOfDestinationName = "destination",
                defraimp_ConsigneeName = "consignee",
                defraimp_TransporterName = "transporter",
                defraimp_OVName = "veterinarian",
            };

            var context = itahc.ToCreateWatchFlagsContext();

            Assert.Equal(itahc.Id, context.Itahc.Id);
            Assert.Equal(defraimp_itahc.EntityLogicalName, context.Itahc.LogicalName);
            Assert.Equal("ITAHC", context.Name);
            Assert.Equal("origin", context.PlaceOfOrigin);
            Assert.Equal("destination", context.PlaceOfDestination);
            Assert.Equal("consignee", context.Consignee);
            Assert.Equal("transporter", context.Transporter);
            Assert.Equal("veterinarian", context.Veterinarian);
        }

        [Fact]
        public void ToCreateWatchFlagsContext_ItahcWithoutTheAttributesSupplied_ProjectsNulls()
        {
            var context = new defraimp_itahc { Id = Guid.NewGuid() }.ToCreateWatchFlagsContext();

            Assert.Null(context.Name);
            Assert.Null(context.PlaceOfOrigin);
            Assert.Null(context.PlaceOfDestination);
            Assert.Null(context.Consignee);
            Assert.Null(context.Transporter);
            Assert.Null(context.Veterinarian);
        }

        [Fact]
        public void ToCreateWatchFlagsContext_ItahcWithSomeAttributesSupplied_ProjectsTheRestAsNull()
        {
            var context = new defraimp_itahc { Id = Guid.NewGuid(), defraimp_ConsigneeName = "consignee" }.ToCreateWatchFlagsContext();

            Assert.Equal("consignee", context.Consignee);
            Assert.Null(context.Name);
            Assert.Null(context.Veterinarian);
        }

        [Fact]
        public void ToCreateWatchFlagsContext_Itahc_DoesNotModifyTheItahc()
        {
            var itahc = new defraimp_itahc { Id = Guid.NewGuid(), defraimp_name = "ITAHC", defraimp_ConsigneeName = "consignee" };
            var attributes = itahc.Attributes.ToDictionary(pair => pair.Key, pair => pair.Value);

            itahc.ToCreateWatchFlagsContext();

            Assert.Equal(attributes, itahc.Attributes.ToDictionary(pair => pair.Key, pair => pair.Value));
        }

        [Fact]
        public void CreateItahcWatchFlagsContext_NullReference_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new CreateItahcWatchFlagsContext(null, "name", null, null, null, null, null));
        }
    }
}
