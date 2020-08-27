using Defra.Imports.BusinessLogic.Itahc;
using Defra.Imports.BusinessLogic.RepoInterfaces;
using Defra.Imports.Model;
using Microsoft.Xrm.Sdk;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Defra.Imports.Tests.Unit.BusinessLogic.Itahc
{
    public class PopulateDevolvedOfficeBusinessLogicTests
    {
        private Entity _target;
        private Mock<IPostcodeRegionRepository> _mockPostcodeRegionRepo;
        private Mock<IConfigurationParameterRepository> _mockConfigurationParameterRepo;
        private PopulateDevolvedOfficeBusinessLogic _PopulateDevolvedOfficeBusinessLogic;
        private string _postcodeFieldName;
        private string _devolvedOfficeFieldName;

        public PopulateDevolvedOfficeBusinessLogicTests()
        {
            _target = new Entity();
            _mockPostcodeRegionRepo = new Mock<IPostcodeRegionRepository>();
            _mockConfigurationParameterRepo = new Mock<IConfigurationParameterRepository>();
            _PopulateDevolvedOfficeBusinessLogic = new PopulateDevolvedOfficeBusinessLogic(_target, _mockPostcodeRegionRepo.Object, _mockConfigurationParameterRepo.Object);
            _postcodeFieldName = "defraimp_placeofdestinationaddresspostcode";
            _devolvedOfficeFieldName = "defraimp_devolvedoffice";
        }

        [Fact]
        public void UpdateDevolvedOfficeForTarget_UnknownPostcode_SetsTheTargetDevolvedOfficeToUnknown()
        {
            // Arrange
            _target.Attributes.Add(_postcodeFieldName, "FAKEPOSTCODE");
            Guid unknownDevolvedOfficeId = Guid.NewGuid();
            _mockConfigurationParameterRepo.Setup(r => r.GetConfigurationParameterValueByKey("defraimp_unknown_devolved_office_id")).Returns(unknownDevolvedOfficeId.ToString());

            // Act
            _PopulateDevolvedOfficeBusinessLogic.UpdateDevolvedOfficeForTarget(_postcodeFieldName, _devolvedOfficeFieldName);

            // Assert
            Assert.Equal(unknownDevolvedOfficeId, ((EntityReference)_target[_devolvedOfficeFieldName]).Id);
        }

        [Fact]
        public void UpdateDevolvedOfficeForTarget_ValidPostcode_CallsFindPostcodeByPostcodePrefixWith4Substrings()
        {
            // Arrange
            _target.Attributes.Add(_postcodeFieldName, "N4 3AG");

            defraimp_postcoderegion stubbedPostcodeRegion = new defraimp_postcoderegion()
            {
                defraimp_DevolvedOffice = new EntityReference("team", Guid.NewGuid()),
                defraimp_postcodeprefix = "N",
            };

            _mockPostcodeRegionRepo.Setup(r => r.FindPostcodeRegionByPostcodePrefix("n")).Returns(stubbedPostcodeRegion);

            // Act
            _PopulateDevolvedOfficeBusinessLogic.UpdateDevolvedOfficeForTarget(_postcodeFieldName, _devolvedOfficeFieldName);

            // Assert
            _mockPostcodeRegionRepo.Verify(r => r.FindPostcodeRegionByPostcodePrefix(It.IsAny<string>()), Times.Exactly(4));
            _mockPostcodeRegionRepo.Verify(r => r.FindPostcodeRegionByPostcodePrefix("n43a"), Times.Once);
            _mockPostcodeRegionRepo.Verify(r => r.FindPostcodeRegionByPostcodePrefix("n43"), Times.Once);
            _mockPostcodeRegionRepo.Verify(r => r.FindPostcodeRegionByPostcodePrefix("n4"), Times.Once);
            _mockPostcodeRegionRepo.Verify(r => r.FindPostcodeRegionByPostcodePrefix("n"), Times.Once);
        }

        [Fact]
        public void UpdateDevolvedOfficeForTarget_PostcodeWithMatchingDevolvedOfficeOneCharacter_SetsTheDevolvedOfficeToTheMatchingOne()
        {
            // Arrange
            _target.Attributes.Add(_postcodeFieldName, "G1 1BX");

            defraimp_postcoderegion stubbedPostcodeRegion = new defraimp_postcoderegion()
            {
                defraimp_DevolvedOffice = new EntityReference("team", Guid.NewGuid()),
                defraimp_postcodeprefix = "G",
            };

            _mockPostcodeRegionRepo.Setup(r => r.FindPostcodeRegionByPostcodePrefix("g")).Returns(stubbedPostcodeRegion);

            // Act
            _PopulateDevolvedOfficeBusinessLogic.UpdateDevolvedOfficeForTarget(_postcodeFieldName, _devolvedOfficeFieldName);

            // Assert
            Assert.Equal(stubbedPostcodeRegion.defraimp_DevolvedOffice.Id, ((EntityReference)_target[_devolvedOfficeFieldName]).Id);
        }

        [Fact]
        public void UpdateDevolvedOfficeForTarget_PostcodeWithMatchingDevolvedOfficeFourCharacters_SetsTheDevolvedOfficeToTheMatchingOne()
        {
            // Arrange
            _target.Attributes.Add(_postcodeFieldName, "SY22 5AA");

            defraimp_postcoderegion stubbedPostcodeRegion = new defraimp_postcoderegion()
            {
                defraimp_DevolvedOffice = new EntityReference("team", Guid.NewGuid()),
                defraimp_postcodeprefix = "SY22",
            };

            _mockPostcodeRegionRepo.Setup(r => r.FindPostcodeRegionByPostcodePrefix("sy22")).Returns(stubbedPostcodeRegion);

            // Act
            _PopulateDevolvedOfficeBusinessLogic.UpdateDevolvedOfficeForTarget(_postcodeFieldName, _devolvedOfficeFieldName);

            // Assert
            Assert.Equal(stubbedPostcodeRegion.defraimp_DevolvedOffice.Id, ((EntityReference)_target[_devolvedOfficeFieldName]).Id);
        }

        [Fact]
        public void UpdateDevolvedOfficeForTarget_PostcodeThatMatchesMoreThanOne_SetsTheDevolvedOfficeOfThePostcodeWithMoreMatchingCharacters()
        {
            // Arrange
            _target.Attributes.Add(_postcodeFieldName, "GU1 1AF");

            defraimp_postcoderegion stubbedPostcodeRegion = new defraimp_postcoderegion()
            {
                defraimp_DevolvedOffice = new EntityReference("team", Guid.NewGuid()),
                defraimp_postcodeprefix = "G",
            };

            defraimp_postcoderegion expectedStubbedPostcodeRegion = new defraimp_postcoderegion()
            {
                defraimp_DevolvedOffice = new EntityReference("team", Guid.NewGuid()),
                defraimp_postcodeprefix = "GU",
            };

            _mockPostcodeRegionRepo.Setup(r => r.FindPostcodeRegionByPostcodePrefix("g")).Returns(stubbedPostcodeRegion);
            _mockPostcodeRegionRepo.Setup(r => r.FindPostcodeRegionByPostcodePrefix("gu")).Returns(expectedStubbedPostcodeRegion);

            // Act
            _PopulateDevolvedOfficeBusinessLogic.UpdateDevolvedOfficeForTarget(_postcodeFieldName, _devolvedOfficeFieldName);

            // Assert
            Assert.Equal(expectedStubbedPostcodeRegion.defraimp_DevolvedOffice.Id, ((EntityReference)_target[_devolvedOfficeFieldName]).Id);
        }

        [Fact]
        public void UpdateDevolvedOfficeForTarget_PostcodeWithSameNumCharacters_SetsTheDevolvedOfficeCorrectly()
        {
            // Arrange
            _target.Attributes.Add(_postcodeFieldName, "SY25");

            defraimp_postcoderegion stubbedPostcodeRegion = new defraimp_postcoderegion()
            {
                defraimp_DevolvedOffice = new EntityReference("team", Guid.NewGuid()),
                defraimp_postcodeprefix = "SY25",
            };

            _mockPostcodeRegionRepo.Setup(r => r.FindPostcodeRegionByPostcodePrefix("sy25")).Returns(stubbedPostcodeRegion);

            // Act
            _PopulateDevolvedOfficeBusinessLogic.UpdateDevolvedOfficeForTarget(_postcodeFieldName, _devolvedOfficeFieldName);

            // Assert
            Assert.Equal(stubbedPostcodeRegion.defraimp_DevolvedOffice.Id, ((EntityReference)_target[_devolvedOfficeFieldName]).Id);
        }

        [Fact]
        public void UpdateDevolvedOfficeForTarget_TargetWithoutPostcodeField_DoesNotRunLogic()
        {
            // Arrange

            // Act
            _PopulateDevolvedOfficeBusinessLogic.UpdateDevolvedOfficeForTarget(_postcodeFieldName, _devolvedOfficeFieldName);

            // Assert
            _mockPostcodeRegionRepo.Verify(r => r.FindPostcodeRegionByPostcodePrefix(It.IsAny<string>()), Times.Never);
        }

    }
}
