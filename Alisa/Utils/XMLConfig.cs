using Alisa.Model;
using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;

namespace Alisa.Utils
{
    class XMLConfig
    {
        XMLFields xmlFields = new XMLFields();
        private LogFile logFile = new LogFile();

        public XMLFields ReadXmlConf(String path)
        {
            XDocument xdoc = XDocument.Load(path);
            XElement xelMain = xdoc.Element("head");
            XElement xelCat;

            //кокфигурация подключения к MSSQL
            xelCat = xelMain.Element("DBConnect");
            xmlFields.dbServer = xelCat.Element("dbServer").Value;
            xmlFields.dbName = xelCat.Element("dbName").Value;
            xmlFields.dbLogin = xelCat.Element("dbLogin").Value;
            xmlFields.dbPass = xelCat.Element("dbPass").Value;

            //кокфигурация подключения к SQLite
            xelCat = xelMain.Element("SQLiteDBConnect");
            xmlFields.SQLitePass = xelCat.Element("SQLitePass").Value;
            xmlFields.SQLiteName = xelCat.Element("SQLiteName").Value;

            //конфигурация отправки E-mail
            xelCat = xelMain.Element("MailConf");
            xmlFields.mailSmtpServer = xelCat.Element("mailSmtpServer").Value;
            xmlFields.mailPort = Convert.ToInt16(xelCat.Element("mailPort").Value);
            xmlFields.mailLogin = xelCat.Element("mailLogin").Value;
            xmlFields.mailPass = xelCat.Element("mailPass").Value;
            xmlFields.mailFrom = xelCat.Element("mailFrom").Value;

            foreach(XElement xel2 in xelCat.Elements("mailTo"))
            {
                if (xmlFields.mailTo == "") //null
                {
                    xmlFields.mailTo = xel2.Value;
                }
                else
                {
                    xmlFields.mailTo += ";" + xel2.Value;
                }
            }            

            xmlFields.mailServiceTo = xelCat.Element("mailServiceTo").Value;

            //конфигурация параметров резервирования
            xelCat = xelMain.Element("Reserv");

            var eee = xelCat.Element("Master").Value;

            try
            {
                xmlFields.Master = Convert.ToBoolean(xelCat.Element("Master").Value);
            }
            catch(Exception ex)
            {
                String logText = DateTime.Now.ToString() + "|fail|XMLConfig - ReadXmlConf|" + ex.Message;
                logFile.WriteLog(logText);
            }

            return xmlFields;
        }


        //public XMLFields ReadXmlConf(String path)
        //{
        //    XmlDocument document = new XmlDocument();
        //    document.Load(path);

        //    XmlElement xRoot = document.DocumentElement;
        //    foreach (XmlElement xnode in xRoot)
        //    {
        //        foreach (XmlNode childnode in xnode.ChildNodes)
        //        {
        //            //кокфигурация подключения к MSSQL
        //            if (childnode.Name == "dbServer")
        //                xmlFields.dbServer = childnode.InnerText;
        //            if (childnode.Name == "dbLogin")
        //                xmlFields.dbLogin = childnode.InnerText;
        //            if (childnode.Name == "dbPass")
        //                xmlFields.dbPass = childnode.InnerText;
        //            if (childnode.Name == "dbName")
        //                xmlFields.dbName = childnode.InnerText;
        //            //кокфигурация подключения к SQLite
        //            if (childnode.Name == "SQLitePass")
        //                xmlFields.SQLitePass = childnode.InnerText;
        //            if (childnode.Name == "SQLiteName")
        //                xmlFields.SQLiteName = childnode.InnerText;
        //            //конфигурация отправки E-mail
        //            if (childnode.Name == "mailSmtpServer")
        //                xmlFields.mailSmtpServer = childnode.InnerText;
        //            if (childnode.Name == "mailPort")
        //                xmlFields.mailPort = Convert.ToInt16(childnode.InnerText);
        //            if (childnode.Name == "mailLogin")
        //                xmlFields.mailLogin = childnode.InnerText;
        //            if (childnode.Name == "mailPass")
        //                xmlFields.mailPass = childnode.InnerText;
        //            if (childnode.Name == "mailFrom")
        //                xmlFields.mailFrom = childnode.InnerText;
        //            if (childnode.Name == "mailTo")
        //                if (xmlFields.mailTo == "") //null
        //                {
        //                    xmlFields.mailTo = childnode.InnerText;
        //                }
        //                else
        //                {
        //                    xmlFields.mailTo += ";" + childnode.InnerText;
        //                }                        

        //            if (childnode.Name == "mailServiceTo")
        //                xmlFields.mailServiceTo = childnode.InnerText;
        //        }
        //    }
        //    return xmlFields;
        //}
    }
}
