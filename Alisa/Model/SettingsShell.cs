namespace Alisa.Model
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;

    /// <summary>Оболочка.</summary>
    public class SettingsShell
    {
        /// <summary>Корневой элемент.</summary>
        [Serializable]
        [XmlRootAttribute("Settings")]
        public class RootElement
        {
            /// <summary>БД MSSQL.</summary>
            public MSSQL MSSQL { get; set; }

            /// <summary>БД SQLite.</summary>
            public SQLite SQLite { get; set; }

            /// <summary>Почта.</summary>
            public Mail Mail { get; set; }

            /// <summary>Резервирование.</summary>
            public Reserv Reserv { get; set; }
        }

        /// <summary>Параметры MSSQL.</summary>
        [Serializable]
        public class MSSQL
        {
            /// <summary>Имя сервера.</summary>
            [XmlAttribute]
            public string Server { get; set; }

            /// <summary>Название БД.</summary>
            [XmlAttribute]
            public string DBName { get; set; }

            /// <summary>Логин.</summary>
            [XmlAttribute]
            public string Login { get; set; }

            /// <summary>Пароль.</summary>
            [XmlAttribute]
            public string Pass { get; set; }
        }

        /// <summary>Параметры SQLite.</summary>
        [Serializable]
        public class SQLite
        {
            /// <summary>Название БД.</summary>
            [XmlAttribute]
            public string DBName { get; set; }

            /// <summary>Пароль.</summary>
            [XmlAttribute]
            public string Pass { get; set; }
        }

        /// <summary>Параметры почты.</summary>
        [Serializable]
        public class Mail
        {
            /// <summary>Сервер SMTP.</summary>
            [XmlAttribute]
            public string SmtpServer { get; set; }

            /// <summary>Порт.</summary>
            [XmlAttribute]
            public short Port { get; set; }

            /// <summary>Логин.</summary>
            [XmlAttribute]
            public string Login { get; set; }

            /// <summary>Пароль.</summary>
            [XmlAttribute]
            public string Pass { get; set; }

            /// <summary>Отправитель.</summary>
            [XmlAttribute]
            public string From { get; set; }

            /// <summary>Получатель.</summary>
            [XmlElement]
            public List<string> To { get; set; }

            /// <summary>Копия.</summary>
            [XmlAttribute]
            public string ServiceTo { get; set; }
        }

        /// <summary>Резервирование.</summary>
        [Serializable]
        public class Reserv
        {
            /// <summary>Основной.</summary>
            [XmlAttribute]
            public bool Master { get; set; }
        }
    }
}