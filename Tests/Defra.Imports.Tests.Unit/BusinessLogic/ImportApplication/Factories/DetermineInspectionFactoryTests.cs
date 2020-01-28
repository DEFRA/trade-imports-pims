using Defra.Imports.BusinessLogic.ImportApplication.DetermineInspection.Strategies;
using Defra.Imports.BusinessLogic.ImportApplication.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Defra.Imports.Tests.Unit.BusinessLogic.ImportApplication.Factories
{
    public class DetermineInspectionFactoryTests
    {
        private DetermineInspectionFactory _determineInspectionFactory;

        public DetermineInspectionFactoryTests()
        {
            _determineInspectionFactory = new DetermineInspectionFactory();
        }

        [Theory]
        [InlineData("p1", typeof(P1DetermineInspection))]
        [InlineData("p2", typeof(P2DetermineInspection))]
        [InlineData("p3", typeof(P3DetermineInspection))]
        public void GetDetermineInspection_AValidPriorityName_CorrectInspectionType(string priorityName, Type determineInspectionType)
        {
            AssertInspectionType(priorityName, determineInspectionType);
        }

        [Fact]
        public void GetDetermineInspection_AValidPriorityNameUppercase_CorrectInspectionType()
        {
            AssertInspectionType("P1", typeof(P1DetermineInspection));
        }

        [Fact]
        public void GetDetermineInspection_UnkownPriorityName_UnkownDetermineInspectionType()
        {
            AssertInspectionType("Unkown Priority", typeof(UnknownDetermineInspection));
        }

        private void AssertInspectionType(string getInspectionInput, Type typeOfInspection)
        {
            AbstractDetermineInspection determineInspection = _determineInspectionFactory.GetDetermineInspection(getInspectionInput);

            Assert.Equal(typeOfInspection, determineInspection.GetType());
        }
    }
}
