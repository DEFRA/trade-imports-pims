namespace Defra.Imports.BusinessLogic.Utils
{
    using Microsoft.Xrm.Sdk;
    using System.Collections.Generic;

    public interface IFetchTemplateParser
    {
        string Parse(string fetchXmlTemplate, EntityReference primaryEntity, IDictionary<string, object> additionalValues = null);
    }
}
