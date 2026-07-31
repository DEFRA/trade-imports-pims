namespace Defra.Imports.Specs.Extensions
{
    using System;
    using PowerPlaywright.Framework.Controls.Pcf;
    using PowerPlaywright.Framework.Controls.Pcf.Classes;
    using PowerPlaywright.Framework.Controls.Platform;

    /// <summary>
    /// Extensions to the <see cref="IDataSet"/> interface.
    /// </summary>
    public static class IDataSetExtensions
    {
        /// <summary>
        /// Gets the control from the data set whose control type is only known at runtime.
        /// </summary>
        /// <param name="dataSet">The data set.</param>
        /// <param name="controlType">The control type.</param>
        /// <returns>A <see cref="IReadOnlyGrid"/> representing the control.</returns>
        public static IReadOnlyGrid GetControl(this IDataSet dataSet, Type controlType)
        {
            switch (controlType)
            {
                case Type _ when controlType == typeof(IPowerAppsOneGrid):
                    return dataSet.GetControl<IPowerAppsOneGrid>();
                case Type _ when controlType == typeof(IGridControl):
                    return dataSet.GetControl<IGridControl>();
                case Type _ when controlType == typeof(IReadOnlyGrid):
                    return dataSet.GetControl<IReadOnlyGrid>();
                case Type _ when controlType == typeof(IPcfGridControl):
                    return dataSet.GetControl<IPcfGridControl>();
                default:
                    throw new NotSupportedException($"Control type '{controlType.Name}' is not supported.");
            }
        }
    }
}