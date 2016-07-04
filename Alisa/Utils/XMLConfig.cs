using Alisa.Model;
using System;
using System.Collections.Generic;
using System.Xml;

namespace Alisa.Utils
{
    class XMLConfig
    {
        DBConnect dbc = new DBConnect();

        public DBConnect ReadXmlConf(String path)
        {
            XmlDocument document = new XmlDocument();
            document.Load(path);

            XmlElement xRoot = document.DocumentElement;
            foreach (XmlElement xnode in xRoot)
            {
                foreach (XmlNode childnode in xnode.ChildNodes)
                {
                    if (childnode.Name == "server")
                        dbc.server = childnode.InnerText;
                    if (childnode.Name == "login")
                        dbc.login = childnode.InnerText;
                    if (childnode.Name == "password")
                        dbc.password = childnode.InnerText;
                    if (childnode.Name == "database")
                        dbc.database = childnode.InnerText;
                }
            }
            return dbc;
        }
    }
}
