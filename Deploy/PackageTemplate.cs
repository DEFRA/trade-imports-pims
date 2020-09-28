namespace Defra.Imports.Deployment
{
    using System.ComponentModel.Composition;
    using Capgemini.PowerApps.Deployment;
    using Microsoft.Xrm.Tooling.PackageDeployment.CrmPackageExtentionBase;

    /// <summary>
    /// Import package starter frame.
    /// </summary>
    [Export(typeof(IImportExtensions))]
    public class PackageTemplate : CapgeminiPackageTemplate
    {
        /// <inheritdoc/>
        public override string GetImportPackageDataFolderName => "PkgFolder";

        /// <inheritdoc/>
        public override string GetImportPackageDescriptionText => "Imports";

        /// <inheritdoc/>
        public override string GetLongNameOfImport => "Imports";

        /// <inheritdoc/>
        public override string GetNameOfImport(bool plural) => "Imports";
    }
}
