using Alisa.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Alisa.Utils
{
    class ConfigXML
    {
        String _path;
        ConfigMSSQL confMSSQL = new ConfigMSSQL();

        public ConfigXML(String path)
        {
            _path = path;
        }

        public void ReadConf()
        {
            //XDocument xdoc = XDocument.Load(_path);
            //confMSSQL.Server = xdoc.Element("head").Element("dbServer").Value;
            //confMSSQL.DBName = xdoc.Element("head").Element("dbName").Value;
            //confMSSQL.Login = xdoc.Element("head").Element("dbLogin").Value;
            //confMSSQL.Pass = xdoc.Element("head").Element("dbPass").Value;

        }
    }
}
