namespace Defra.Imports.BusinessLogic.Utils
{
    using System.Collections.Generic;
    using Microsoft.Xrm.Sdk;

    public interface IFetchTemplateParser
    {
        string Parse(string fetchXmlTemplate, EntityReference primaryEntity, IDictionary<string, object> additionalValues = null);
    }
}
