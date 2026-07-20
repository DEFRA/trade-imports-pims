using System;
using System.ComponentModel.Composition;
using Capgemini.PowerApps.PackageDeployerTemplate;
using Capgemini.PowerApps.PackageDeployerTemplate.Config;
using Microsoft.Xrm.Tooling.PackageDeployment.CrmPackageExtentionBase;

namespace Defra.Imports.Deployment
{
    /// <summary>
    /// Import package starter frame.
    /// </summary>
    [Export(typeof(IImportExtensions))]
    public class PackageImportExtension : PackageTemplateBase
    {
        private bool? forceSameVersionUpdate;
        private bool? importSeedData;

        #region Metadata

        /// <summary>
        /// Folder name where package assets are located in the final output package zip.
        /// </summary>
        public override string GetImportPackageDataFolderName => "PkgAssets";

        /// <summary>
        /// Name of the Import Package to Use
        /// </summary>
        /// <param name="plural">if true, return plural version</param>
        public override string GetNameOfImport(bool plural) => "Defra.Imports.Deployment";

        /// <summary>
        /// Long name of the Import Package.
        /// </summary>
        public override string GetLongNameOfImport => "Defra.Imports.Deployment";

        /// <summary>
        /// Description of the package, used in the package selection UI
        /// </summary>
        public override string GetImportPackageDescriptionText => "Defra.Imports.Deployment";

        #endregion

        /// <summary>
        /// Gets a value indicating whether solutions should import even when solution versions match.
        /// </summary>
        protected bool ForceSameVersionUpdate
        {
            get
            {
                if (!this.forceSameVersionUpdate.HasValue)
                {
                    this.forceSameVersionUpdate = this.GetSetting<bool>(nameof(this.ForceSameVersionUpdate));
                }

                return this.forceSameVersionUpdate.HasValue && this.forceSameVersionUpdate.Value;
            }
        }

        protected bool ImportSeedData
        {
            get
            {
                if (!this.importSeedData.HasValue)
                {
                    this.importSeedData = this.GetSetting<bool>(nameof(this.ImportSeedData));
                }

                return this.importSeedData.HasValue && this.importSeedData.Value;
            }
        }

        public override bool AfterPrimaryImport()
        {
            var baseResult = base.AfterPrimaryImport();

            if (this.ImportSeedData)
            {
                this.DataImporterService.Import(
                    new DataImportConfig[] {
                        new DataImportConfig
                        {
                            DataFolderPath = "data/seed/extract",
                            ImportConfigPath = "data/seed/import.json",
                        }
                    },
                    this.PackageFolderPath);
            }

            return baseResult;
        }

        /// <inheritdoc/>
        public override UserRequestedImportAction OverrideSolutionImportDecision(string solutionUniqueName, Version organizationVersion, Version packageSolutionVersion, Version inboundSolutionVersion, Version deployedSolutionVersion, ImportAction systemSelectedImportAction)
        {
            var decision = base.OverrideSolutionImportDecision(solutionUniqueName, organizationVersion, packageSolutionVersion, inboundSolutionVersion, deployedSolutionVersion, systemSelectedImportAction);

            if (systemSelectedImportAction == ImportAction.SkipSameVersion && this.ForceSameVersionUpdate && decision != UserRequestedImportAction.ForceUpdate)
            {
                decision = UserRequestedImportAction.ForceUpdate;
            }
            else if (systemSelectedImportAction == ImportAction.Import && deployedSolutionVersion.DetermineDifference(inboundSolutionVersion).Major == 0)
            {
                decision = UserRequestedImportAction.ForceUpdate;
            }

            return decision;
        }
    }
}
