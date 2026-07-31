namespace Defra.Imports.Specs.Extensions
{
    using System;
    using Reqnroll;

    /// <summary>
    /// Extension methods for the <see cref="ScenarioContext"/> class.
    /// </summary>
    public static class ScenarioContextExtensions
    {
        /// <summary>
        /// Determines whether or not the scenario context contains a key for a given type.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="context">The context.</param>
        /// <returns>True if it contains the key, false if it doesn't contain the key.</returns>
        public static bool ContainsKey<T>(this ScenarioContext context)
        {
            return context.ContainsKey(typeof(T).FullName);
        }

        /// <summary>
        /// Adds or updates a value to the context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>False if added, true if updated.</returns>
        public static bool AddOrUpdate(this ScenarioContext context, string key, object value)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException($"'{nameof(key)}' cannot be null or empty.", nameof(key));
            }

            if (context.ContainsKey(key))
            {
                context[key] = value;
                return true;
            }
            else
            {
                context.Add(key, value);
                return false;
            }
        }
    }
}