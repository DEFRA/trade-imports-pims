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
        /// Converts the specified type to a string representation of the object.
        /// </summary>
        /// <typeparam name="T">Type.</typeparam>
        /// <param name="obj">An instance of type T.</param>
        /// <returns>Returns a string representation of the type T.</returns>
        public static string ToJSON<T>(this T obj)
            where T : class
        {
            using (var stream = new MemoryStream())
            {
                var serializer = new DataContractJsonSerializer(typeof(T));
                serializer.WriteObject(stream, obj);
                stream.Position = 0;

                using (var reader = new StreamReader(stream, Encoding.UTF8))
                {
                    return reader.ReadToEnd();
                }
            }
        }

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
