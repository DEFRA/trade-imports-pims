namespace Defra.Imports.BusinessLogic.Extensions
{
    using System.IO;
    using System.Runtime.Serialization.Json;
    using System.Text;

    /// <summary>
    /// Class to store object extension methods.
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Converts the specified string to an instance of the specified type.
        /// </summary>
        /// <typeparam name="T">Type.</typeparam>
        /// <param name="json">A string representation of the type T.</param>
        /// <returns>Returns an instance of type T.</returns>
        public static T FromJSON<T>(this string json)
        {
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                var serializerSettings = new DataContractJsonSerializerSettings()
                {
                    UseSimpleDictionaryFormat = true,
                };

                var serializer = new DataContractJsonSerializer(typeof(T), serializerSettings);
                var output = (T)serializer.ReadObject(stream);
                return output;
            }
        }
    }
}
