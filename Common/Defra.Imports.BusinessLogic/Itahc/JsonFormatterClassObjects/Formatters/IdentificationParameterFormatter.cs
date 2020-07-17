using Defra.Imports.BusinessLogic.Itahc.JsonFormatterClassObjects.IdentificationParameterSetObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Defra.Imports.BusinessLogic.Itahc.JsonFormatterClassObjects.Formatters
{
    public class IdentificationParameterFormatter
    {

        public string FinalString { get; private set; }

        public string CommodityIdTypes { get; private set; }

        public string PassportNumber { get; private set; }

        public IdentificationParameterFormatter()
        {
            FinalString = String.Empty;
            CommodityIdTypes = String.Empty;
            PassportNumber = String.Empty;
        }

        public void BuildFormattedAttributes(IdentificationParameterSetObject identParameterSet)
        {
            ResetAttributes();
            identParameterSet.IdentificationParameterSet.IdentificationParameter.ForEach(x =>
            {
                ExtractIdentParameterData(x);
            });
        }

        public void BuildFormattedAttributes(IdentificationParameterSetList identParameterSetList)
        {
            ResetAttributes();
            identParameterSetList.IdentificationParameterSet.ForEach(x =>
            {
                x.IdentificationParameter.ForEach(y =>
                {
                    ExtractIdentParameterData(y);
                });

                FinalString += $"----------{Environment.NewLine}{Environment.NewLine}";
                CommodityIdTypes += $"{Environment.NewLine}{Environment.NewLine}";
                PassportNumber += $"----------{Environment.NewLine}{Environment.NewLine}";
            });
        }

        private void ResetAttributes()
        {
            FinalString = String.Empty;
            CommodityIdTypes = String.Empty;
            PassportNumber = String.Empty;
        }

        private void ExtractIdentParameterData(IdentificationParameter parameter)
        {
            FinalString += "Key: " + parameter.Key + System.Environment.NewLine
                 + "Data: " + parameter.Data + System.Environment.NewLine
                 + System.Environment.NewLine;

            if (parameter.Key.Trim().Equals("species"))
            {
                CommodityIdTypes += "SpeciesID: " + parameter.Data.Trim() + "; ";
            }
            else if (parameter.Key.Trim().Equals("identsystem"))
            {
                CommodityIdTypes += parameter.Data.Trim() + ": ";
            }
            else if (parameter.Key.Trim().Equals("identnumber"))
            {
                CommodityIdTypes += parameter.Data.Trim() + "; ";
            }
            else if (parameter.Key.Trim().Equals("passportnumber"))
            {
                CommodityIdTypes += parameter.Key.Trim() + ": " + parameter.Data.Trim() + "; ";
                PassportNumber += parameter.Data.Trim() + Environment.NewLine;
            }
            else
            {
                CommodityIdTypes += $"{parameter.Key.Trim()}: {parameter.Data.Trim()}; ";
            }
        }
    }
}
