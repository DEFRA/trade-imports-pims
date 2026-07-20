using Microsoft.Xrm.Sdk;

namespace Defra.Imports.BusinessLogic.Utils
{
    public interface IGenericEntityMapper<TEntityTypeToMapFrom, TEntityTypeToMapTo> 
        where TEntityTypeToMapFrom : Entity
        where TEntityTypeToMapTo : Entity
    {
        TEntityTypeToMapTo MapAllFields(TEntityTypeToMapFrom entityToMapFrom, TEntityTypeToMapTo entityToMapTo);

        TEntityTypeToMapTo MapEmptyFields(TEntityTypeToMapFrom entityToMapFrom, TEntityTypeToMapTo entityToMapTo);
    }
}
