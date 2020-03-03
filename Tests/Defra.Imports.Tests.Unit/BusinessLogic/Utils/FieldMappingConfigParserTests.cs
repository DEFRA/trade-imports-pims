using Defra.Imports.BusinessLogic.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Defra.Imports.Tests.Unit.BusinessLogic.Utils
{
    public class FieldMappingConfigParserTests
    {
        private FieldMappingConfigParser _fieldMappingConfigParser;

        public FieldMappingConfigParserTests()
        {
            _fieldMappingConfigParser = new FieldMappingConfigParser();
        }

        [Fact]
        public void ParserMappingConfig_AnXMLConfigStringWithOneRule_ReturnsADictionaryWithOneItem()
        {
            string xmlString = @"<FieldMappings><FieldMapping mapfrom='address1_city' mapto='address1_city' /></FieldMappings>";
            Dictionary<string, string> mappingDictionary = _fieldMappingConfigParser.ParseMappingConfig(xmlString);
            Assert.True(mappingDictionary.Count == 1);
        }

        [Fact]
        public void ParserMappingConfig_AnXMLConfigStringWithOneRule_ReturnsADictionaryWithTheCorrectKeyValuePair()
        {
            string xmlString = @"<FieldMappings><FieldMapping mapfrom='firstname' mapto='accountname' /></FieldMappings>";
            Dictionary<string, string> mappingDictionary = _fieldMappingConfigParser.ParseMappingConfig(xmlString);

            string predictedKey = "firstname";
            string predictedValue = "accountname";
            Assert.True(mappingDictionary.ContainsKey(predictedKey));
            Assert.Equal(predictedValue, mappingDictionary[predictedKey]);

        }

        [Fact]
        public void ParseMappingConfig_AnXMLConfigStringWithThreeRules_ReturnsADictionaryWithThreeItems()
        {
            string xmlString = @"
                <FieldMappings>
                    <FieldMapping mapfrom='firstname' mapto='accountname' />
                    <FieldMapping mapfrom='address1_city' mapto='address1_city' />
                    <FieldMapping mapfrom='address1_telephone1' mapto='address1_telephone2' />
                </FieldMappings>";

            Dictionary<string, string> mappingDictionary = _fieldMappingConfigParser.ParseMappingConfig(xmlString);
            Assert.True(mappingDictionary.Count == 3);
        }

        [Fact]
        public void ParseMappingConfig_AnXMLConfigStringWithThreeRules_ReturnsADictionaryWithTheCorrectKeyValuePairs()
        {
            string xmlString = @"
                <FieldMappings>
                    <FieldMapping mapfrom='firstname' mapto='accountname' />
                    <FieldMapping mapfrom='address1_city' mapto='address1_city' />
                    <FieldMapping mapfrom='address1_telephone1' mapto='address1_telephone2' />
                </FieldMappings>";

            Dictionary<string, string> mappingDictionary = _fieldMappingConfigParser.ParseMappingConfig(xmlString);
            Assert.Equal("accountname", mappingDictionary["firstname"]);
            Assert.Equal("address1_city", mappingDictionary["address1_city"]);
            Assert.Equal("address1_telephone2", mappingDictionary["address1_telephone1"]);
        }
    }
}
