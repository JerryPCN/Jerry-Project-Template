using System.IO;
using System.Net.Http;
using System.Net.Http.Formatting;

namespace JerryPlat.Utils.Helpers
{
    public static class SerializationHelper
    {
        private static JsonMediaTypeFormatter _jsonFormatter = null;

        public static JsonMediaTypeFormatter jsonFormatter
        {
            get
            {
                if (_jsonFormatter == null)
                {
                    _jsonFormatter = new JsonMediaTypeFormatter();
                    _jsonFormatter.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                }
                return _jsonFormatter;
            }
        }

        private static XmlMediaTypeFormatter _xmlFormatter;

        public static XmlMediaTypeFormatter xmlFormatter
        {
            get
            {
                if (_xmlFormatter == null)
                {
                    _xmlFormatter = new XmlMediaTypeFormatter();
                }
                return _xmlFormatter;
            }
        }

        public static string ToJson<T>(T value)
        {
            return Serialize<T, JsonMediaTypeFormatter>(jsonFormatter, value);
        }

        public static string ToXml<T>(T value)
        {
            return Serialize<T, XmlMediaTypeFormatter>(xmlFormatter, value);
        }

        public static T JsonToObject<T>(string str) where T : class
        {
            return Deserialize<T, JsonMediaTypeFormatter>(jsonFormatter, str);
        }

        public static T XmlToObject<T>(string str) where T : class
        {
            return Deserialize<T, XmlMediaTypeFormatter>(xmlFormatter, str);
        }

        public static string Serialize<T, K>(K formatter, T value)
            where K : MediaTypeFormatter
        {
            // Create a dummy HTTP Content.
            Stream stream = new MemoryStream();
            var content = new StreamContent(stream);
            /// Serialize the object.
            formatter.WriteToStreamAsync(typeof(T), value, stream, content, null).Wait();
            // Read the serialized string.
            stream.Position = 0;
            return content.ReadAsStringAsync().Result;
        }

        public static T Deserialize<T, K>(K formatter, string str) where T : class
             where K : MediaTypeFormatter
        {
            // Write the serialized string to a memory stream.
            Stream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(str);
            writer.Flush();
            stream.Position = 0;
            // Deserialize to an object of type T
            return formatter.ReadFromStreamAsync(typeof(T), stream, null, null).Result as T;
        }
    }
}