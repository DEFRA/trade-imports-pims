using Defra.Imports.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Defra.Imports.BusinessLogic.RepoInterfaces
{
    public interface IPostcodeRegionRepository
    {
        defraimp_postcoderegion FindPostcodeRegionByPostcodePrefix(string postcodePrefix);
    }
}
