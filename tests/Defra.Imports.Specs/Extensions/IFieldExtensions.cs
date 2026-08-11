namespace Defra.Imports.Specs.Extensions
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Playwright;
    using PowerPlaywright.Framework.Controls.Pcf.Classes;

    /// <summary>
    /// Extensions to the <see cref="IField"/> interface.
    /// </summary>
    public static class IFieldExtensions
    {
        /// <summary>
        /// Recalculates a calculated field.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public static async Task RecalculateAsync(this IField field)
        {
            await field.Container.GetByRole(AriaRole.Button, new LocatorGetByRoleOptions { Name = "Show Recalculate", Exact = true }).ClickAsync();
            await field.Container.GetByRole(AriaRole.Button, new LocatorGetByRoleOptions { Name = "Recalculate", Exact = true }).ClickAsync();
        }

        /// <summary>
        /// Gets the value from the field whose control type is only known at runtime.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="controlType">The control type.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public static async Task<object> GetValueAsync(this IField field, Type controlType)
        {
            // TODO: Refactor Power Playwright to enable getting/setting values when the control type is only known at runtime.
            var controlInstance = field.GetControl(controlType);

            var getValueAsyncMethod = controlType.GetMethods()
                .Concat(controlType.GetInterfaces().SelectMany(i => i.GetMethods()))
                .FirstOrDefault(m => m.Name == "GetValueAsync" && m.GetParameters().Length == 0);

            var getValueTask = (Task)getValueAsyncMethod.Invoke(controlInstance, null);

            await getValueTask;

            var resultProperty = getValueTask.GetType().GetProperty("Result");

            return resultProperty.GetValue(getValueTask);
        }

        /// <summary>
        /// Sets the value for the field whose control type is only known at runtime.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="controlType">The control type.</param>
        /// <param name="value">The value.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public static async Task SetValueAsync(this IField field, Type controlType, object value)
        {
            // TODO: Refactor Power Playwright to enable getting/setting values when the control type is only known at runtime.
            var controlInstance = field.GetControl(controlType);
            var setValueAsyncMethod = controlType.GetMethod("SetValueAsync");
            var parameterType = setValueAsyncMethod.GetParameters()[0].ParameterType;
            var data = ConvertToType(value, parameterType);

            await (Task)setValueAsyncMethod.Invoke(controlInstance, new object[] { data });
        }

        private static object GetControl(this IField field, Type controlType)
        {
            var getControlMethod = field.GetType().GetMethod("GetControl").MakeGenericMethod(controlType);

            return getControlMethod.Invoke(field, null);
        }

        private static object ConvertToType(object rawValue, Type targetType)
        {
            var underlyingType = Nullable.GetUnderlyingType(targetType) ?? targetType;

            if (rawValue == null)
            {
                if (!targetType.IsValueType || Nullable.GetUnderlyingType(targetType) != null)
                {
                    return null;
                }
            }

            var converter = TypeDescriptor.GetConverter(targetType);
            if (converter == null)
            {
                throw new InvalidOperationException($"No type converter found for type {underlyingType.FullName}");
            }

            if (!converter.CanConvertFrom(rawValue.GetType()))
            {
                throw new InvalidOperationException($"Cannot convert from type {rawValue.GetType().FullName} to type {underlyingType.FullName}");
            }

            return converter.ConvertFrom(rawValue);
        }
    }
}