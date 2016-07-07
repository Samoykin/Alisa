using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alisa.Utils
{
    class SQLiteDB
    {
        public String DataBaseName = "DBTEP.sqlite";
        //String pass = "Xt,ehfirf3";
        String pass = "";

        public void CreateBase()
        {
            if (!File.Exists(DataBaseName))
            {
                SQLiteConnection.CreateFile(DataBaseName);
                SQLiteConnection m_dbConnection = new SQLiteConnection("Data Source=DBTels.sqlite;Version=3;");
                m_dbConnection.SetPassword(pass);
            }
        }

        public void TEPCreateTable()
        {
            String Command = "CREATE TABLE TEP (id INTEGER PRIMARY KEY UNIQUE, DateTime VARCHAR, SQLw_Data1 VARCHAR, SQLw_Data2 VARCHAR, SQLw_Data3 VARCHAR, SQLw_Data4 VARCHAR, SQLw_Data5 VARCHAR, SQLw_Data6 VARCHAR, SQLw_Data7 VARCHAR, SQLw_Data8 VARCHAR, SQLw_Data9 VARCHAR, SQLw_Data10 VARCHAR, SQLw_Data11 VARCHAR, SQLw_Data12 VARCHAR, SQLw_Data13 VARCHAR);";
            SQLiteConnection connection = new SQLiteConnection(string.Format("Data Source={0};Version=3;Password={1};", DataBaseName, pass));

            SQLiteCommand sqlitecommand = new SQLiteCommand(Command, connection);
            connection.Open();
            sqlitecommand.ExecuteNonQuery();
            connection.Close();
        }


    }
}
