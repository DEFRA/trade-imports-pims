using Defra.Imports.BusinessLogic.RepoInterfaces;
using Defra.Imports.Model;
using Defra.Imports.Repositories;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Text;

namespace Defra.Imports.BusinessLogic.Itahc
{
    public class PopulateDevolvedOfficeBusinessLogic
    {
        private Entity _target;
        private IPostcodeRegionRepository _postcodeRegionRepo;
        private IConfigurationParameterRepository _configParameterRepo;

        public PopulateDevolvedOfficeBusinessLogic(Entity target, IPostcodeRegionRepository postcodeRegionRepository, IConfigurationParameterRepository configParameterRepo)
        {
            this._target = target;
            this._postcodeRegionRepo = postcodeRegionRepository;
            this._configParameterRepo = configParameterRepo;
        }

        public void UpdateDevolvedOfficeForTarget(string postcodeFieldName, string devolvedOfficeFieldName)
        {
           if(_target.Attributes.Contains(postcodeFieldName) && _target[postcodeFieldName].GetType() == typeof(string))
            {
                string sanitizedPostcode = _target.GetAttributeValue<string>(postcodeFieldName).Replace(" ", String.Empty).ToLower();
                defraimp_postcoderegion postcodeRegion = FindPostcodeRegionForMultiplePrefixes(sanitizedPostcode, 4);
                SetDevolvedOfficeOnTargetEntity(postcodeRegion, devolvedOfficeFieldName);
            }
        }

        private defraimp_postcoderegion FindPostcodeRegionForMultiplePrefixes(string sanitizedPostcode, int maximumPrefixLength)
        {
            defraimp_postcoderegion postcodeRegion = null;

            for (int i = maximumPrefixLength; i >= 1; i--)
            {
                if (sanitizedPostcode.Length >= i && postcodeRegion == null)
                {
                    string postcodePrefix = sanitizedPostcode.Substring(0, i);
                    postcodeRegion = _postcodeRegionRepo.FindPostcodeRegionByPostcodePrefix(postcodePrefix);
                    if (postcodeRegion != null)
                    {
                        break;
                    }
                }
            }

            return postcodeRegion;

        }

        private void SetDevolvedOfficeOnTargetEntity(defraimp_postcoderegion postcoderegion, string devolvedOfficeFieldName)
        {
            if(postcoderegion != null)
            {
                _target[devolvedOfficeFieldName] = postcoderegion.defraimp_DevolvedOffice;
            }
            else
            {
                SetUnknownDevolvedOffice(devolvedOfficeFieldName);
            }
        }

        private void SetUnknownDevolvedOffice(string devolvedOfficeFieldName)
        {
            string unknownDevolvedOffice = _configParameterRepo.GetConfigurationParameterValueByKey("defraimp_unknown_devolved_office_id");
            Guid unknownDevolvedOfficeGuid = new Guid(unknownDevolvedOffice);
            EntityReference unknownOfficeRef = new EntityReference("team", unknownDevolvedOfficeGuid);
            _target[devolvedOfficeFieldName] = unknownOfficeRef;
        }
    }
}
