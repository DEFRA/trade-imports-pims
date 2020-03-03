using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Text;

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
