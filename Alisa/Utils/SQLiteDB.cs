using Alisa.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
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
        public ObservableCollection<HistTEP> histTEP { get; set; }
        HistTEP oneTEP;

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
            String Command = "CREATE TABLE TEP (id INTEGER PRIMARY KEY UNIQUE, DateTime DATETIME, SQLw_Data1 DOUBLE, SQLw_Data2 DOUBLE, SQLw_Data3 DOUBLE, SQLw_Data4 DOUBLE, SQLw_Data5 DOUBLE, SQLw_Data6 DOUBLE, SQLw_Data7 DOUBLE, SQLw_Data8 DOUBLE, SQLw_Data9 DOUBLE, SQLw_Data10 DOUBLE, SQLw_Data11 DOUBLE, SQLw_Data12 DOUBLE, SQLw_Data13 DOUBLE);";
            SQLiteConnection connection = new SQLiteConnection(string.Format("Data Source={0};Version=3;Password={1};", DataBaseName, pass));

            SQLiteCommand sqlitecommand = new SQLiteCommand(Command, connection);
            connection.Open();
            sqlitecommand.ExecuteNonQuery();
            connection.Close();
        }

        public void TEPWrite(LiveTEP liveTEP)
        {
            SQLiteConnection connection = new SQLiteConnection(string.Format("Data Source={0};Password={1};", DataBaseName, pass));
            connection.Open();
            SQLiteCommand command;

            command = new SQLiteCommand("INSERT INTO 'TEP' ('DateTime', 'SQLw_Data1', 'SQLw_Data2', 'SQLw_Data3', 'SQLw_Data4', 'SQLw_Data5', 'SQLw_Data6', 'SQLw_Data7', 'SQLw_Data8', 'SQLw_Data9', 'SQLw_Data10', 'SQLw_Data11', 'SQLw_Data12', 'SQLw_Data13') VALUES ('" + DateTime.Now + "', '" + liveTEP.SQLw_Data1 + "', '" + liveTEP.SQLw_Data2 + "', '" + liveTEP.SQLw_Data3 + "', '" + liveTEP.SQLw_Data4 + "', '" + liveTEP.SQLw_Data5 + "', '" + liveTEP.SQLw_Data6 + "', '" + liveTEP.SQLw_Data7 + "', '" + liveTEP.SQLw_Data8 + "', '" + liveTEP.SQLw_Data9 + "', '" + liveTEP.SQLw_Data10 + "', '" + liveTEP.SQLw_Data11 + "', '" + liveTEP.SQLw_Data12 + "', '" + liveTEP.SQLw_Data13 + "');", connection);
            command.ExecuteNonQuery();

            connection.Close();
        }

        public ObservableCollection<HistTEP> TEPRead()
        {
            histTEP = new ObservableCollection<HistTEP>();

            SQLiteConnection connection = new SQLiteConnection(string.Format("Data Source={0};Password={1};", DataBaseName, pass));
            connection.Open();
            SQLiteCommand command = new SQLiteCommand("SELECT SQLw_Data1, SQLw_Data2 FROM 'TEP';", connection);
            SQLiteDataReader reader = command.ExecuteReader();
            foreach (DbDataRecord record in reader)
            {
                oneTEP = new HistTEP();
                //oneTEP.dateTime = Convert.ToDateTime(record["DateTime"]);
                oneTEP.SQLw_Data1 = Convert.ToDouble(record["SQLw_Data1"]);
                oneTEP.SQLw_Data2 = Convert.ToDouble(record["SQLw_Data2"]);
                //oneTEP.SQLw_Data3 = Convert.ToDouble(record["SQLw_Data3"]);
                //oneTEP.SQLw_Data4 = Convert.ToDouble(record["SQLw_Data4"]);
                //oneTEP.SQLw_Data5 = Convert.ToDouble(record["SQLw_Data5"]);
                //oneTEP.SQLw_Data6 = Convert.ToDouble(record["SQLw_Data6"]);
                //oneTEP.SQLw_Data7 = Convert.ToDouble(record["SQLw_Data7"]);
                //oneTEP.SQLw_Data8 = Convert.ToDouble(record["SQLw_Data8"]);
                //oneTEP.SQLw_Data9 = Convert.ToDouble(record["SQLw_Data9"]);
                //oneTEP.SQLw_Data10 = Convert.ToDouble(record["SQLw_Data10"]);
                //oneTEP.SQLw_Data11 = Convert.ToDouble(record["SQLw_Data11"]);
                //oneTEP.SQLw_Data12 = Convert.ToDouble(record["SQLw_Data12"]);
                //oneTEP.SQLw_Data13 = Convert.ToDouble(record["SQLw_Data13"]);
                histTEP.Add(oneTEP);
            }
            connection.Close();

            return histTEP;
        }





    }
}
