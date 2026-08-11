using Defra.Imports.BusinessLogic.Itahc.JsonFormatterClassObjects.IdentificationParameterSetObjects;
using System;
using System.Linq;

namespace Defra.Imports.BusinessLogic.Itahc.JsonFormatterClassObjects.Formatters
{
    public class IdentificationParameterFormatter
    {

        public string CommodityIdTypes { get; private set; }

        private string _defaultSpeciesNomination;

        private string _currenLine;

        public IdentificationParameterFormatter(string defaultSpeciesNomination)
        {
            CommodityIdTypes = String.Empty;
            _defaultSpeciesNomination = defaultSpeciesNomination;
            _currenLine = String.Empty;
        }

        public void BuildFormattedAttributes(IdentificationParameterSetObject identParameterSet, CommodityComplementObject commodityComplements)
        {
            ResetAttributes();
            identParameterSet.IdentificationParameterSet.IdentificationParameter.ForEach(x =>
            {
                _currenLine += ExtractIdentParameterData(x, commodityComplements);
            });

            InjectSpeciesNameIfEmpty();

            CommodityIdTypes += _currenLine;
            _currenLine = String.Empty;
        }

        public void BuildFormattedAttributes(IdentificationParameterSetList identParameterSetList, CommodityComplementObject commodityComplements)
        {
            ResetAttributes();
            identParameterSetList.IdentificationParameterSet.ForEach(x =>
            {
                x.IdentificationParameter.ForEach(y =>
                {
                    _currenLine += ExtractIdentParameterData(y, commodityComplements);
                });

                InjectSpeciesNameIfEmpty();

                CommodityIdTypes += _currenLine;
                CommodityIdTypes += $"{Environment.NewLine}{Environment.NewLine}";
                _currenLine = String.Empty;
            });
        }

        private void ResetAttributes()
        {
            CommodityIdTypes = String.Empty;
            _currenLine = String.Empty;
        }

        private void InjectSpeciesNameIfEmpty()
        {
            // Inject species name if it doesn't exist
            if (!_currenLine.Contains("SpeciesName"))
            {
                _currenLine = $"SpeciesName: {_defaultSpeciesNomination}; {_currenLine}";
            }
        }

        private string ExtractIdentParameterData(IdentificationParameter parameter, CommodityComplementObject commodityComplements)
        {
            string extractedData = String.Empty;

            if (parameter.Key.Trim().Equals("species"))
            {
                string speciesNomination = _defaultSpeciesNomination;

                // If there is more than once species pull it out from commodityComplements
                if(commodityComplements.CommodityComplement.Species.Count >= 1)
                {
                    string speciesIdToSearchFor = parameter.Data.Trim();
                    Species species = commodityComplements.CommodityComplement.Species.FirstOrDefault(e => e.SpeciesID == speciesIdToSearchFor);
                    speciesNomination = species != null && !String.IsNullOrEmpty(species.SpeciesNomination) ? species.SpeciesNomination : _defaultSpeciesNomination;
                }

                extractedData  = $"SpeciesName: {speciesNomination}; ";
            }
            else if (parameter.Key.Trim().Equals("identsystem"))
            {
                extractedData = parameter.Data.Trim() + ": ";
            }
            else if (parameter.Key.Trim().Equals("identnumber"))
            {
                extractedData = parameter.Data.Trim() + "; ";
            }
            else if(parameter.Key.Trim().Equals("complement"))
            {
                // skip this value
            }
            else
            {
                extractedData = $"{parameter.Key.Trim()}: {parameter.Data.Trim()}; ";
            }

            return extractedData;

        }
    }
}
