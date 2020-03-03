using System;
using System.Collections.Generic;
using System.Text;

namespace Defra.Imports.BusinessLogic.Utils
{
    interface IFieldMappingConfigParser
    {
        Dictionary<string, string> ParseMappingConfig(string configContents);

    }
}
