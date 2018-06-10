namespace Alisa.Utils
{
    using System.IO;
    using System.Xml.Serialization;

    /// <summary>Параметры XML.</summary>
    /// <typeparam name="T">Тип.</typeparam>
    public class SettingsXml<T>
    {
        private readonly string path;

        /// <summary>Initializes a new instance of the <see cref="SettingsXml{T}" /> class.</summary>
        /// <param name="path">Путь к файлу.</param>
        public SettingsXml(string path)
        {
            this.path = path;
        }

        /// <summary>Записать в XML.</summary>
        /// <param name="data">Данные.</param>
        public void WriteXml(T data)
        {
            var serializerObj = new XmlSerializer(typeof(T));

            TextWriter stream = new StreamWriter(this.path);
            serializerObj.Serialize(stream, data);
            stream.Close();
        }

        /// <summary>Прочитать из XML.</summary>
        /// <param name="data">Данные.</param>
        /// <returns>Набор значений.</returns>
        public T ReadXml(T data)
        {
            var serializerObj = new XmlSerializer(typeof(T));

            TextReader stream = new StreamReader(this.path);
            data = (T)serializerObj.Deserialize(stream);
            stream.Close();                

            return data;
        }
    }
}