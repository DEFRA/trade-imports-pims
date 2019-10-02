namespace Defra.Imports.Repositories
{
    using Defra.Imports.BusinessLogic.RepoInterfaces;
    using Defra.Imports.Model;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Client;
    using System;
    using System.Linq;

    public class ConfigurationParameterRepository : IConfigurationParameterRepository
    {
        #region Private Variables

        private readonly ImportsContext _crmContext;

        #endregion Private Variables

        #region Constructor

        public ConfigurationParameterRepository(ImportsContext crmContext)
        {
            _crmContext = crmContext;
        }

        #endregion Constructor

        #region Public Methods

        /// <summary>
        /// Getting a Value from the config parameter by given key
        /// </summary>
        /// <param name="key"></param>
        /// <returns> Returns string with config value </returns>
        public string GetConfigurationParameterValueByKey(string key)
        {
            var configParameter = _crmContext.defraexp_configurationparameterSet.
            Where(x => x.defraexp_Key == key)
            .Select(x => new defraexp_configurationparameter
            {
                Id = x.Id,
                defraexp_Value = x.defraexp_Value,
            })?.FirstOrDefault();
            return configParameter?.defraexp_Value;
        }

        /// <summary>
        /// Getting a key from a given value
        /// </summary>
        /// <param name="value"></param>
        /// <returns> Returns config record </returns>
        public defraexp_configurationparameter GetConfigurationParameterByName(string name)
        {
            var configParameter = _crmContext.defraexp_configurationparameterSet.
            Where(x => x.defraexp_name == name)
            .Select(x => new defraexp_configurationparameter
            {
                Id = x.Id,
                defraexp_Key = x.defraexp_Key,
                defraexp_Value = x.defraexp_Value,
            }).FirstOrDefault();
            return configParameter;
        }

        #endregion Public Methods
    }
}