namespace Defra.Imports.Deployment
{
    using System;

    /// <summary>
    /// Static class containing <see cref="Version"/> extension methods.
    /// </summary>
    public static class VersionExtensions
    {
        /// <summary>
        /// Determines the difference between two versions.
        /// </summary>
        /// <param name="source">The source <see cref="Version"/> to compare.</param>
        /// <param name="target">The target <see cref="Version"/> to compare.</param>
        /// <returns><see cref="Version"/> containing the difference.</returns>
        public static Version DetermineDifference(this Version source, Version target)
        {
            int majorDifference = Math.Max(target.Major - source.Major, 0);
            int minorDifference = Math.Max(target.Minor - source.Minor, 0);
            int buildDifference = Math.Max(target.Build - source.Build, 0);
            int revisionDifference = Math.Max(target.Revision - source.Revision, 0);
            return new Version(majorDifference, minorDifference, buildDifference, revisionDifference);
        }
    }
}
