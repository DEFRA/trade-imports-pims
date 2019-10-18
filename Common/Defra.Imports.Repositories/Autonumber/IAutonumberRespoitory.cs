
namespace Defra.Imports.BusinessLogic.RepoInterfaces
{
    using System;
    using Defra.Imports.Model;

    public interface IAutonumberRepository
{

        defraimp_autonumber GetAutonumberWithKey(string key);

        int GetAutonumberValue(string key);

        void IncrementAutonumber(string key);

        void IncrementAutonumber(string key, int amountToIncrementBy);

        void DecrementAutonumber(string key);

        void SetAutonumberValue(string key, int value);
    }
}
