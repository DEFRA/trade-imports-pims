using Defra.Imports.BusinessLogic.ImporterNotification;
using Defra.Imports.Model;
using Defra.Imports.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Defra.Imports.Tests.Unit.BusinessLogic.ImporterNotification
{

    public class PopulateImpTypeBusinessLogicTests
    {
        private defraimp_ImporterNotification target;
        private Mock<ICrmRepository<defraimp_imptype1>> mockImpTypeRepo;
        private PopulateImpTypeBusinessLogic businessLogic;

        public PopulateImpTypeBusinessLogicTests()
        {
            this.target = new defraimp_ImporterNotification();
            this.mockImpTypeRepo = new Mock<ICrmRepository<defraimp_imptype1>>();
            this.businessLogic = new PopulateImpTypeBusinessLogic(this.target, this.mockImpTypeRepo.Object);
        }

        [Fact]
        public void RunLogic_TargetWithValidImpCode_PopulatesImpTypeLookupWithFoundReference()
        {
            // Arrange
            string impCode = "test";
            this.target.defraimp_ImpType = impCode;

            defraimp_imptype1 stubbedImpType = this.CreateImpType();

            this.mockImpTypeRepo
                .Setup(x => x.Find(It.IsAny<Expression<Func<defraimp_imptype1, bool>>>(), It.IsAny<Expression<Func<defraimp_imptype1, defraimp_imptype1>>>()))
                .Returns(new List<defraimp_imptype1>() { stubbedImpType }.AsQueryable());

            // Act
            this.businessLogic.RunLogic();

            // Assert
            Assert.NotNull(this.target.defraimp_imptypeid);
            Assert.Equal(stubbedImpType.Id, this.target.defraimp_imptypeid.Id);
        }

        [Fact]
        public void RunLogic_TargetWithNoImpCode_DoesNotPopulateImpTypeLookup()
        {
            // Arrange
            this.target.defraimp_ImpType = null;

            // Act
            this.businessLogic.RunLogic();

            // Assert
            Assert.Null(this.target.defraimp_imptypeid);
        }

        [Fact]
        public void RunLogic_TargetWithNotFoundImpCode_DoesNotPopulateImpTypeLookup()
        {
            // Arrange
            string impCode = "test";
            this.target.defraimp_ImpType = impCode;

            this.mockImpTypeRepo
                .Setup(x => x.Find(It.IsAny<Expression<Func<defraimp_imptype1, bool>>>(), It.IsAny<Expression<Func<defraimp_imptype1, defraimp_imptype1>>>()))
                .Returns(new List<defraimp_imptype1>().AsQueryable());

            // Act
            this.businessLogic.RunLogic();

            // Assert
            Assert.Null(this.target.defraimp_imptypeid);
        }

        private defraimp_imptype1 CreateImpType()
        {
            Guid impId = Guid.NewGuid();
            defraimp_imptype1 stubbedImpType = new defraimp_imptype1()
            {
                Id = impId,
                defraimp_imptypeId = impId,
            };
            return stubbedImpType;
        }
    }
}
