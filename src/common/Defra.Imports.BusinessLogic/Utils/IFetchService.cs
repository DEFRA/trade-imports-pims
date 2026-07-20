namespace Defra.Imports.BusinessLogic.Utils
{
    using Microsoft.Xrm.Sdk;
    using System.Collections.Generic;

    public interface IFetchService
    {
        IEnumerable<Entity> FetchByTemplate(string templateFetchXml, EntityReference primaryEntity, Dictionary<string, object> additionalValues = null);

        IEnumerable<TEntity> FetchByTemplate<TEntity>(string templateFetchXml, EntityReference primaryEntity, Dictionary<string, object> additionalValues = null)
            where TEntity : Entity;
    }
}
