namespace Alisa.Utils
{
    using System;
    using System.Collections.ObjectModel;
    using System.Data.Common;
    using System.Data.SQLite;
    using System.IO;
    using Model;
    using NLog;
    using static Model.Shell;

    /// <summary>База данных SQLite.</summary>
    public class SQLiteDB
    {
        private Logger logger = LogManager.GetCurrentClassLogger();
        private string dataBaseName;
        private string pass;        
        private HistTEP oneTEP;

        /// <summary>Initializes a new instance of the <see cref="SQLiteDB" /> class.</summary>
        /// <param name="sqlite">Модель подключения к БД.</param>
        public SQLiteDB(SQLite sqlite)
        {
            this.dataBaseName = sqlite.DBName;
            this.pass = sqlite.Pass;
        }

        private ObservableCollection<HistTEP> HistTEP { get; set; }

        /// <summary>Создать БД.</summary>
        public void CreateBase()
        {
            if (!File.Exists(this.dataBaseName))
            {
                SQLiteConnection.CreateFile(this.dataBaseName);
                var сonnection = new SQLiteConnection("Data Source=DBTels.sqlite;Version=3;");
                сonnection.SetPassword(this.pass);

                this.logger.Info("Создана БД DBTEP");
            }
        }

        /// <summary>Создать таблицу ТЭП.</summary>
        public void TEPCreateTable()
        {  
            try
            {
                using (var connection = new SQLiteConnection(this.Connstring()))
                {
                    connection.Open();
                    var command = new SQLiteCommand("CREATE TABLE IF NOT EXISTS TEP (id INTEGER PRIMARY KEY UNIQUE, DateTime DATETIME, SQLw_Data1 string, SQLw_Data2 string, SQLw_Data3 string, SQLw_Data4 string, SQLw_Data5 string, SQLw_Data6 string, SQLw_Data7 string, SQLw_Data8 string, SQLw_Data9 string, SQLw_Data10 string, SQLw_Data11 string, SQLw_Data12 string, SQLw_Data13 string);", connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                this.logger.Error(ex.Message);
            }
        }

        /// <summary>Записать ТЭП.</summary>
        /// <param name="liveTEP">Текущие значения.</param>
        public void TEPWrite(LiveTEP liveTEP)
        {
            try
            {
                using (var connection = new SQLiteConnection(this.Connstring()))
                {
                    connection.Open();
                    var format_date = "yyyy-MM-dd HH:mm:ss.fff";
                    var command = new SQLiteCommand("INSERT INTO 'TEP' ('DateTime', 'SQLw_Data1', 'SQLw_Data2', 'SQLw_Data3', 'SQLw_Data4', 'SQLw_Data5', 'SQLw_Data6', 'SQLw_Data7', 'SQLw_Data8', 'SQLw_Data9', 'SQLw_Data10', 'SQLw_Data11', 'SQLw_Data12', 'SQLw_Data13') VALUES ('" + DateTime.Now.ToString(format_date) + "', '" + Convert.ToString(liveTEP.SQLw_Data1) + "', '" + Convert.ToString(liveTEP.SQLw_Data2) + "', '" + Convert.ToString(liveTEP.SQLw_Data3) + "', '" + Convert.ToString(liveTEP.SQLw_Data4) + "', '" + Convert.ToString(liveTEP.SQLw_Data5) + "', '" + Convert.ToString(liveTEP.SQLw_Data6) + "', '" + Convert.ToString(liveTEP.SQLw_Data7) + "', '" + Convert.ToString(liveTEP.SQLw_Data8) + "', '" + Convert.ToString(liveTEP.SQLw_Data9) + "', '" + Convert.ToString(liveTEP.SQLw_Data10) + "', '" + Convert.ToString(liveTEP.SQLw_Data11) + "', '" + Convert.ToString(liveTEP.SQLw_Data12) + "', '" + Convert.ToString(liveTEP.SQLw_Data13) + "');", connection);
                    
                    command.ExecuteNonQuery();
                    connection.Close();
                }

                this.logger.Info("Записан 2-х часовой отчет TEP");
            }
            catch (Exception ex)
            {
                this.logger.Error(ex.Message);
            }
        }

        /// <summary>Прочитать отчет ТЭП.</summary>
        /// <param name="startDate">Начальная дата.</param>
        /// <param name="endDate">Конечная дата.</param>
        /// <returns>Отчет ТЭП.</returns>
        public ObservableCollection<HistTEP> TEPRead(DateTime startDate, DateTime endDate)
        {
            var format_date = "yyyy-MM-dd HH:mm:ss.fff";
            var format_date_small = "yyyy-MM-dd HH:mm";

            this.HistTEP = new ObservableCollection<HistTEP>();

            try
            {
                using (var connection = new SQLiteConnection(this.Connstring()))
                {
                    connection.Open();
                    var command = new SQLiteCommand("SELECT * FROM TEP WHERE DateTime >= '" + startDate.ToString(format_date) + "' and DateTime <= '" + endDate.ToString(format_date) + "'  ;", connection);

                    var reader = command.ExecuteReader();
                    foreach (DbDataRecord record in reader)
                    {
                        this.oneTEP = new HistTEP();
                        this.oneTEP.DateTimeTEP = Convert.ToDateTime(record["DateTime"]);
                        this.oneTEP.SQLw_Data1 = Convert.ToDouble(record["SQLw_Data1"]);
                        this.oneTEP.SQLw_Data2 = Convert.ToDouble(record["SQLw_Data2"]);
                        this.oneTEP.SQLw_Data3 = Convert.ToDouble(record["SQLw_Data3"]);
                        this.oneTEP.SQLw_Data4 = Convert.ToDouble(record["SQLw_Data4"]);
                        this.oneTEP.SQLw_Data5 = Convert.ToDouble(record["SQLw_Data5"]);
                        this.oneTEP.SQLw_Data6 = Convert.ToDouble(record["SQLw_Data6"]);
                        this.oneTEP.SQLw_Data7 = Convert.ToDouble(record["SQLw_Data7"]);
                        this.oneTEP.SQLw_Data8 = Convert.ToDouble(record["SQLw_Data8"]);
                        this.oneTEP.SQLw_Data9 = Convert.ToDouble(record["SQLw_Data9"]);
                        this.oneTEP.SQLw_Data10 = Convert.ToDouble(record["SQLw_Data10"]);
                        this.oneTEP.SQLw_Data11 = Convert.ToDouble(record["SQLw_Data11"]);
                        this.oneTEP.SQLw_Data12 = Convert.ToDouble(record["SQLw_Data12"]);
                        this.oneTEP.SQLw_Data13 = Convert.ToDouble(record["SQLw_Data13"]);
                        this.HistTEP.Add(this.oneTEP);
                    }

                    connection.Close(); 
                }

                this.logger.Info("Выбран отчет TEP за период с " + startDate.ToString(format_date_small) + " по " + endDate.ToString(format_date_small));
            }            
            catch (Exception ex)
            {
                this.logger.Error(ex.Message);
            }

            return this.HistTEP;
        }

        private string Connstring()
        {
            return string.Format("Data Source={0};Version=3;Password={1};", this.dataBaseName, this.pass);
        }
    }
}