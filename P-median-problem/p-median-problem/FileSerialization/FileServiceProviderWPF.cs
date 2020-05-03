using GraphX.Common.Models;
using System.Collections.Generic;
using System.IO;
using YAXLib;

namespace p_median_problem.FileSerialization
{
    /// <summary>
    /// Вспомогательный инструмент для сериализации и десериализации файлов.
    /// </summary>
    public static class FileServiceProviderWPF
    {
        /// <summary>
        /// Сериализует список данных в файл.
        /// </summary>
        /// <param name="filename">Имя файла.</param>
        /// <param name="modelsList">Спсиок данных.</param>
        public static void SerializeDataToFile(string filename, List<GraphSerializationData> modelsList)
        {
            using (FileStream stream = File.Open(filename, FileMode.Create, FileAccess.Write, FileShare.Read))
            {
                SerializeDataToStream(stream, modelsList);
            }
        }

        /// <summary>
        /// Десериализует список данных из файла.
        /// </summary>
        /// <param name="filename">Имя файла.</param>
        /// <returns>Десериализованный список данных.</returns>
        public static List<GraphSerializationData> DeserializeDataFromFile(string filename)
        {
            using (FileStream stream = File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                return DeserializeDataFromStream(stream);
            }
        }

        /// <summary>
        /// Сериализует список данных в поток.
        /// </summary>
        /// <param name="stream">Поток.</param>
        /// <param name="modelsList">Список данных.</param>
		public static void SerializeDataToStream(Stream stream, List<GraphSerializationData> modelsList)
        {
            var serializer = new YAXSerializer(typeof(List<GraphSerializationData>));
            using (var textWriter = new StreamWriter(stream))
            {
                serializer.Serialize(modelsList, textWriter);
                textWriter.Flush();
            }
        }

        /// <summary>
        /// Десериализует список данных из потока.
        /// </summary>
        /// <param name="stream">Поток.</param>
        /// <returns>Десериализованный список данных.</returns>
		public static List<GraphSerializationData> DeserializeDataFromStream(Stream stream)
        {
            var deserializer = new YAXSerializer(typeof(List<GraphSerializationData>));
            using (var textReader = new StreamReader(stream))
            {
                return (List<GraphSerializationData>)deserializer.Deserialize(textReader);
            }
        }
    }
}
