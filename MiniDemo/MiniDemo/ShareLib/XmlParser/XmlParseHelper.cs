using System.IO;
using System.Xml.Serialization;

namespace ShareLib.XmlParser
{
    public class XmlParseHelper
    {
        /// <summary>
        /// Deserializes the XML document contained by the specified Stream.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        public static T Deserialize<T>(string path) where T : new()
        {
            T obj;
            var xmlSerializer = new XmlSerializer(typeof(T));
            using (var reader = new StreamReader(path))
            {
                obj = (T) xmlSerializer.Deserialize(reader);
            }
            return obj;
        }

        /// <summary>
        /// Serializes the specified Object and writes the XML document to a file 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sourse"></param>
        /// <param name="path"></param>
        public static void Serialize<T>(T sourse, string path) where T : new() //Where T: new()--Constrians T's constructor has no arguments
        {
            var xmlSerializer = new XmlSerializer(typeof (T));
            using (var writer = new StreamWriter(path))
            {
                xmlSerializer.Serialize(writer, sourse);
            }
        }
    }
}
