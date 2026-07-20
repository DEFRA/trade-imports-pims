using Microsoft.Xrm.Sdk;
using System.Collections.Generic;
using System.Linq;

namespace Defra.Imports.BusinessLogic.Utils
{
    public class GenericEntityMapper<TEntityToMapFrom, TEntityToMapTo> : IGenericEntityMapper<TEntityToMapFrom, TEntityToMapTo>
        where TEntityToMapFrom : Entity
        where TEntityToMapTo : Entity
    {
        private Dictionary<string, string> _fieldsToMap;

        public GenericEntityMapper(Dictionary<string, string> fieldsToMap) 
        {
            _fieldsToMap = fieldsToMap;
        }

        public TEntityToMapTo MapAllFields(TEntityToMapFrom entityToMapFrom, TEntityToMapTo entityToMapTo)
        {
            List<string> mapFromKeys = _fieldsToMap.Keys.ToList();
            mapFromKeys.ForEach(mapFromKey =>
            {
                if(entityToMapFrom.Attributes.Contains(mapFromKey))
                {
                    string mapToKey = _fieldsToMap[mapFromKey];
                    if(entityToMapTo.Attributes.Contains(mapToKey))
                    {
                        entityToMapTo.Attributes[mapToKey] = entityToMapFrom[mapFromKey];
                    }
                    else
                    {
                        entityToMapTo.Attributes.Add(new KeyValuePair<string, object>(mapToKey, entityToMapFrom[mapFromKey]));
                    }
                }
            });

            return entityToMapTo.ToEntity<TEntityToMapTo>();

        }

        public TEntityToMapTo MapEmptyFields(TEntityToMapFrom entityToMapFrom, TEntityToMapTo entityToMapTo)
        {
            List<string> mapFromKeys = _fieldsToMap.Keys.ToList();
            mapFromKeys.ForEach(mapFromKey =>
            {
                if (entityToMapFrom.Attributes.Contains(mapFromKey))
                {
                    string mapToKey = _fieldsToMap[mapFromKey];
                    if (!entityToMapTo.Attributes.Contains(mapToKey))
                    {
                        entityToMapTo.Attributes.Add(new KeyValuePair<string, object>(mapToKey, entityToMapFrom[mapFromKey]));
                    }
                }
            });

            return entityToMapTo.ToEntity<TEntityToMapTo>();
        }
    }
}
