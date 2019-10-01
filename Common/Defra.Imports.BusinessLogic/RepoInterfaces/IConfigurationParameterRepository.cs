namespace Defra.Imports.BusinessLogic.RepoInterfaces
{
    using Defra.Imports.Model;
    using System;

    public interface IConfigurationParameterRepository
    {
        /// <summary>
        /// Getting a Value from the config parameter by given key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string GetConfigurationParameterValueByKey(string key);

        /// <summary>
        /// Getting a key from a given value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        defraexp_configurationparameter GetConfigurationParameterByName(string name);

    }
}