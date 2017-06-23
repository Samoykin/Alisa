using System;
using System.Collections.Generic;

namespace Alisa.Model
{
    class XMLFields
    {
        public String dbServer { get; set; }
        public String dbLogin { get; set; }
        public String dbPass { get; set; }
        public String dbName { get; set; }
        public String SQLitePass { get; set; }
        public String SQLiteName { get; set; }
        public String mailSmtpServer { get; set; }
        public Int16 mailPort { get; set; }
        public String mailLogin { get; set; }
        public String mailPass { get; set; }
        public String mailFrom { get; set; }
        public String mailTo;
        public String mailServiceTo { get; set; }
        public Boolean Master { get; set; }
    }
}
