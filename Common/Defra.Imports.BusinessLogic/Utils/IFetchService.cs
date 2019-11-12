using System;
using System.Collections.Generic;
using System.Text;

namespace Defra.Imports.BusinessLogic.Utils
{
    using System.Collections.Generic;
    using Microsoft.Xrm.Sdk;

    public interface IFetchService
    {
        IEnumerable<Entity> FetchByTemplate(string templateFetchXml, EntityReference primaryEntity, Dictionary<string, object> additionalValues = null);

        IEnumerable<TEntity> FetchByTemplate<TEntity>(string templateFetchXml, EntityReference primaryEntity, Dictionary<string, object> additionalValues = null)
            where TEntity : Entity;
    }
}
