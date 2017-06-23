using Alisa.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Alisa.Utils
{
    class SettingsXML<T>
    {
        String _path;
        private LogFile logFile = new LogFile();
        String logText;

        public SettingsXML(String path)
        {
            _path = path;
        }
        
        public void WriteXml(T data)
        {
            try
            {
                XmlSerializer serializer_obj = new XmlSerializer(typeof(T));

                TextWriter stream = new StreamWriter(_path);
                serializer_obj.Serialize(stream, data);
                stream.Close();

            }
            catch (Exception e)
            {
                logText = DateTime.Now.ToString() + "|fail|SettingsXML - WriteXml|" + e.Message;
                logFile.WriteLog(logText);
            }
        }        

        public T ReadXml(T data)
        {
            try
            {
                XmlSerializer serializer_obj = new XmlSerializer(typeof(T));

                TextReader stream = new StreamReader(_path);
                data = (T)serializer_obj.Deserialize(stream);
                stream.Close();
                
            }
            catch (Exception e)
            {
                logText = DateTime.Now.ToString() + "|fail|SettingsXML - ReadXml|" + e.Message;
                logFile.WriteLog(logText);
            }
            return data;


        }


    }
}
