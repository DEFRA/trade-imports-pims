using Defra.Imports.Model;

namespace Defra.Imports.BusinessLogic.RepoInterfaces
{
    public interface IPostcodeRegionRepository
    {
        defraimp_postcoderegion FindPostcodeRegionByPostcodePrefix(string postcodePrefix);
    }
}
