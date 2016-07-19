using Alisa.Model;
using System;
using System.Collections.Generic;
using System.Xml;

namespace Alisa.Utils
{
    class XMLConfig
    {
        XMLFields xmlFields = new XMLFields();

        public XMLFields ReadXmlConf(String path)
        {
            XmlDocument document = new XmlDocument();
            document.Load(path);

            XmlElement xRoot = document.DocumentElement;
            foreach (XmlElement xnode in xRoot)
            {
                foreach (XmlNode childnode in xnode.ChildNodes)
                {
                    //кокфигурация подключения к MSSQL
                    if (childnode.Name == "dbServer")
                        xmlFields.dbServer = childnode.InnerText;
                    if (childnode.Name == "dbLogin")
                        xmlFields.dbLogin = childnode.InnerText;
                    if (childnode.Name == "dbPass")
                        xmlFields.dbPass = childnode.InnerText;
                    if (childnode.Name == "dbName")
                        xmlFields.dbName = childnode.InnerText;
                    //конфигурация отправки E-mail
                    if (childnode.Name == "mailSmtpServer")
                        xmlFields.mailSmtpServer = childnode.InnerText;
                    if (childnode.Name == "mailPort")
                        xmlFields.mailPort = childnode.InnerText;
                    if (childnode.Name == "mailPort")
                        xmlFields.mailPort = childnode.InnerText;
                    if (childnode.Name == "mailLogin")
                        xmlFields.mailLogin = childnode.InnerText;
                    if (childnode.Name == "mailPass")
                        xmlFields.mailPass = childnode.InnerText;
                    if (childnode.Name == "mailFrom")
                        xmlFields.mailFrom = childnode.InnerText;
                    if (childnode.Name == "mailTo")
                        xmlFields.mailTo = childnode.InnerText;
                }
            }
            return xmlFields;
        }
    }
}
