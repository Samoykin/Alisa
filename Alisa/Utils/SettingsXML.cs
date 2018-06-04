namespace Alisa.Utils
{
    using System;
    using System.IO;
    using System.Xml.Serialization;
    using NLog;

    /// <summary>Параметры XML.</summary>
    /// <typeparam name="T">Тип.</typeparam>
    public class SettingsXML<T>
    {
        private Logger logger = LogManager.GetCurrentClassLogger();
        private string path;        
        private string logText;

        /// <summary>Initializes a new instance of the <see cref="SettingsXML{T}" /> class.</summary>
        /// <param name="path">Путь к файлу.</param>
        public SettingsXML(string path)
        {
            this.path = path;
        }

        /// <summary>Записать в XML.</summary>
        /// <param name="data">Данные.</param>
        public void WriteXml(T data)
        {
            try
            {
                var serializer_obj = new XmlSerializer(typeof(T));

                TextWriter stream = new StreamWriter(this.path);
                serializer_obj.Serialize(stream, data);
                stream.Close();
            }
            catch (Exception ex)
            {
                this.logger.Error(ex.Message);
            }
        }

        /// <summary>Прочитать из XML.</summary>
        /// <param name="data">Данные.</param>
        /// <returns>Набор значений.</returns>
        public T ReadXml(T data)
        {
            try
            {
                var serializer_obj = new XmlSerializer(typeof(T));

                TextReader stream = new StreamReader(this.path);
                data = (T)serializer_obj.Deserialize(stream);
                stream.Close();                
            }
            catch (Exception ex)
            {
                this.logger.Error(ex.Message);
            }

            return data;
        }
    }
}