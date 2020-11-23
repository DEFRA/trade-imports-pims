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
        private Entity target;
        private IPostcodeRegionRepository postcodeRegionRepo;
        private IConfigurationParameterRepository configParameterRepo;

        public PopulateDevolvedOfficeBusinessLogic(Entity target, IPostcodeRegionRepository postcodeRegionRepository, IConfigurationParameterRepository configParameterRepo)
        {
            this.target = target;
            this.postcodeRegionRepo = postcodeRegionRepository;
            this.configParameterRepo = configParameterRepo;
        }

        public void UpdateDevolvedOfficeForTarget(string postcodeFieldName, string devolvedOfficeFieldName)
        {
            if(this.target.Attributes.Contains(postcodeFieldName))
            {
                defraimp_postcoderegion postcodeRegion = null;
                if (this.target[postcodeFieldName] != null && this.target[postcodeFieldName].GetType() == typeof(string))
                {
                    string postcodeVal = this.target.GetAttributeValue<string>(postcodeFieldName);

                    if (postcodeVal != null)
                    {
                        string sanitizedPostcode = postcodeVal.Replace(" ", String.Empty).ToLower();
                        postcodeRegion = this.FindPostcodeRegionForMultiplePrefixes(sanitizedPostcode, 4);
                    }
                }
                this.SetDevolvedOfficeOnTargetEntity(postcodeRegion, devolvedOfficeFieldName);
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
                    postcodeRegion = this.postcodeRegionRepo.FindPostcodeRegionByPostcodePrefix(postcodePrefix);
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
                this.target[devolvedOfficeFieldName] = postcoderegion.defraimp_DevolvedOffice;
            }
            else
            {
                this.SetUnknownDevolvedOffice(devolvedOfficeFieldName);
            }
        }

        private void SetUnknownDevolvedOffice(string devolvedOfficeFieldName)
        {
            string unknownDevolvedOffice = this.configParameterRepo.GetConfigurationParameterValueByKey("defraimp_unknown_devolved_office_id");
            Guid unknownDevolvedOfficeGuid = new Guid(unknownDevolvedOffice);
            EntityReference unknownOfficeRef = new EntityReference("team", unknownDevolvedOfficeGuid);
            this.target[devolvedOfficeFieldName] = unknownOfficeRef;
        }
    }
}
