using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Alisa.Model
{
    

        [Serializable]
        [XmlRootAttribute("Settings")]
        public class RootElement
        {
            public MSSQL MSSQL { get; set; }
            public SQLite SQLite { get; set; }
            public Mail Mail { get; set; }
            public Reserv Reserv { get; set; }
        }

        [Serializable]
        public class MSSQL
        {
            [XmlAttribute]
            public String Server { get; set; }
            [XmlAttribute]
            public String DBName { get; set; }
            [XmlAttribute]
            public String Login { get; set; }
            [XmlAttribute]
            public String Pass { get; set; }
        }

        [Serializable]
        public class SQLite
        {
            [XmlAttribute]
            public String DBName { get; set; }
            [XmlAttribute]
            public String Pass { get; set; }
        }

        [Serializable]
        public class Mail
        {
            [XmlAttribute]
            public String SmtpServer { get; set; }
            [XmlAttribute]
            public Int16 Port { get; set; }
            [XmlAttribute]
            public String Login { get; set; }
            [XmlAttribute]
            public String Pass { get; set; }
            [XmlAttribute]
            public String From { get; set; }
            [XmlElement]
            public List<String> To { get; set; }
            [XmlAttribute]
            public String ServiceTo { get; set; }
        }

        [Serializable]
        public class Reserv
        {
            [XmlAttribute]
            public Boolean Master { get; set; }
        }







}
