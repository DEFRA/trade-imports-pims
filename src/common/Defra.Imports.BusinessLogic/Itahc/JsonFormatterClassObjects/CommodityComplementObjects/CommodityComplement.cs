using System.Collections.Generic;

namespace Defra.Imports.BusinessLogic.Itahc.JsonFormatterClassObjects
{
    public class CommodityComplement
    {
        public string CommodityCode { get; set; }
        public string ComplementID { get; set; }
        public string SpeciesType { get; set; }
        public string SpeciesModel { get; set; }
        public List<Species> Species { get; set; }
    }
}
