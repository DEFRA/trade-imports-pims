using System.Collections.Generic;

namespace Defra.Imports.BusinessLogic.Utils
{
    interface IFieldMappingConfigParser
    {
        Dictionary<string, string> ParseMappingConfig(string configContents);

    }
}
