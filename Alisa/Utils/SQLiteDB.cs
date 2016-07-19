﻿using Alisa.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Data.SQLite;
using System.Globalization;
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
            String connStr = String.Format("Data Source={0};Version=3;Password={1};", DataBaseName, pass);

            using (var connection = new SQLiteConnection(connStr))
            {
                String Command = "CREATE TABLE IF NOT EXISTS TEP (id INTEGER PRIMARY KEY UNIQUE, DateTime DATETIME, SQLw_Data1 STRING, SQLw_Data2 STRING, SQLw_Data3 STRING, SQLw_Data4 STRING, SQLw_Data5 STRING, SQLw_Data6 STRING, SQLw_Data7 STRING, SQLw_Data8 STRING, SQLw_Data9 STRING, SQLw_Data10 STRING, SQLw_Data11 STRING, SQLw_Data12 STRING, SQLw_Data13 STRING);";

                SQLiteCommand sqlitecommand = new SQLiteCommand(Command, connection);
                connection.Open();
                sqlitecommand.ExecuteNonQuery();
                connection.Close();
            }
        }

        public void TEPWrite(LiveTEP liveTEP)
        {
            SQLiteConnection connection = new SQLiteConnection(string.Format("Data Source={0};Password={1};", DataBaseName, pass));
            connection.Open();
            SQLiteCommand command;

            String format_date = "yyyy-MM-dd HH:mm:ss.fff";
            //return value.ToString(format_date);

            command = new SQLiteCommand("INSERT INTO 'TEP' ('DateTime', 'SQLw_Data1', 'SQLw_Data2', 'SQLw_Data3', 'SQLw_Data4', 'SQLw_Data5', 'SQLw_Data6', 'SQLw_Data7', 'SQLw_Data8', 'SQLw_Data9', 'SQLw_Data10', 'SQLw_Data11', 'SQLw_Data12', 'SQLw_Data13') VALUES ('" + DateTime.Now.ToString(format_date) + "', '" + Convert.ToString(liveTEP.SQLw_Data1) + "', '" + Convert.ToString(liveTEP.SQLw_Data2) + "', '" + Convert.ToString(liveTEP.SQLw_Data3) + "', '" + Convert.ToString(liveTEP.SQLw_Data4) + "', '" + Convert.ToString(liveTEP.SQLw_Data5) + "', '" + Convert.ToString(liveTEP.SQLw_Data6) + "', '" + Convert.ToString(liveTEP.SQLw_Data7) + "', '" + Convert.ToString(liveTEP.SQLw_Data8) + "', '" + Convert.ToString(liveTEP.SQLw_Data9) + "', '" + Convert.ToString(liveTEP.SQLw_Data10) + "', '" + Convert.ToString(liveTEP.SQLw_Data11) + "', '" + Convert.ToString(liveTEP.SQLw_Data12) + "', '" + Convert.ToString(liveTEP.SQLw_Data13) + "');", connection);
            //command = new SQLiteCommand("INSERT INTO 'TEP' ('DateTime', 'SQLw_Data1', 'SQLw_Data2', 'SQLw_Data3', 'SQLw_Data4', 'SQLw_Data5', 'SQLw_Data6', 'SQLw_Data7', 'SQLw_Data8', 'SQLw_Data9', 'SQLw_Data10', 'SQLw_Data11', 'SQLw_Data12', 'SQLw_Data13') VALUES ('" + DateTime.Now.ToString(format_date) + "', '" + liveTEP.SQLw_Data1 + "', '" + liveTEP.SQLw_Data2 + "', '" + liveTEP.SQLw_Data3 + "', '" + liveTEP.SQLw_Data4 + "', '" + liveTEP.SQLw_Data5 + "', '" + liveTEP.SQLw_Data6 + "', '" + liveTEP.SQLw_Data7 + "', '" + liveTEP.SQLw_Data8 + "', '" + liveTEP.SQLw_Data9 + "', '" + liveTEP.SQLw_Data10 + "', '" + liveTEP.SQLw_Data11 + "', '" + liveTEP.SQLw_Data12 + "', '" + liveTEP.SQLw_Data13 + "');", connection);
            
            command.ExecuteNonQuery();

            connection.Close();
        }

        public ObservableCollection<HistTEP> TEPRead(DateTime startDate, DateTime endDate)
        {
            String format_date = "yyyy-MM-dd HH:mm:ss.fff";

            histTEP = new ObservableCollection<HistTEP>();

            SQLiteConnection connection = new SQLiteConnection(string.Format("Data Source={0};Password={1};", DataBaseName, pass));
            connection.Open();
            SQLiteCommand command = new SQLiteCommand("SELECT * FROM TEP WHERE DateTime >= '" + startDate.ToString(format_date) + "' and DateTime <= '" + endDate.ToString(format_date) + "'  ;", connection);
            //and DateTime <= '" + endDate.ToString() + "'
            SQLiteDataReader reader = command.ExecuteReader();
            foreach (DbDataRecord record in reader)
            {


                oneTEP = new HistTEP();
                oneTEP.DateTimeTEP = Convert.ToDateTime(record["DateTime"]);


                //Object ddi = record["SQLw_Data1"];
                //String aaa = String.Format("{0:F}", record["SQLw_Data1"]);
                String aaa = record["SQLw_Data1"].ToString();
                    //Object ddi = record["SQLw_Data2"];
                //String aaa = Convert.ToString(record["SQLw_Data1"], CultureInfo.CurrentCulture); //.ToString("G17", CultureInfo.InvariantCulture);
                    oneTEP.SQLw_Data1 = Convert.ToDouble(aaa);
                oneTEP.SQLw_Data2 = Convert.ToDouble(record["SQLw_Data2"]);

                oneTEP.SQLw_Data3 = Convert.ToDouble(record["SQLw_Data3"]);
                oneTEP.SQLw_Data4 = Convert.ToDouble(record["SQLw_Data4"]);
                oneTEP.SQLw_Data5 = Convert.ToDouble(record["SQLw_Data5"]);
                oneTEP.SQLw_Data6 = Convert.ToDouble(record["SQLw_Data6"]);
                oneTEP.SQLw_Data7 = Convert.ToDouble(record["SQLw_Data7"]);
                oneTEP.SQLw_Data8 = Convert.ToDouble(record["SQLw_Data8"]);
                oneTEP.SQLw_Data9 = Convert.ToDouble(record["SQLw_Data9"]);
                oneTEP.SQLw_Data10 = Convert.ToDouble(record["SQLw_Data10"]);
                oneTEP.SQLw_Data11 = Convert.ToDouble(record["SQLw_Data11"]);
                oneTEP.SQLw_Data12 = Convert.ToDouble(record["SQLw_Data12"]);
                oneTEP.SQLw_Data13 = Convert.ToDouble(record["SQLw_Data13"]);
                histTEP.Add(oneTEP);
            }
            connection.Close();

            return histTEP;
        }





    }
}
