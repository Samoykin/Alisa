﻿namespace Alisa.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data.Common;
    using System.Data.SqlClient;
    using System.IO;

    using Model;
    using Utils;
    using static Model.Shell;

    /// <summary>База данных MSSQL.</summary>
    public class RuntimeDB
    {
        private LogFile logFile = new LogFile();
        private string logText;
        private string connStr;
        private MSSQL mssql;

        /// <summary>Initializes a new instance of the <see cref="RuntimeDB" /> class.</summary>
        /// <param name="mssql">Модель подключения к БД.</param>
        public RuntimeDB(MSSQL mssql)
        {
            this.connStr = @"server=" + mssql.Server + @";uid=" + mssql.Login + @";
                        pwd=" + mssql.Pass + @";database=" + mssql.DBName;
            this.mssql = mssql;
        }

        /// <summary>Прочитать данные из тестовой БД.</summary>
        /// <param name="tags">Набор тегов.</param>
        /// <param name="runtimeModels">Модель данных.</param>
        /// <returns>Набор значений.</returns>
        public ObservableCollection<RuntimeModel> DataReadTest(string tags, ObservableCollection<RuntimeModel> runtimeModels)
        {
            var tagList = new List<string>();
            var addresses = tags.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            var rnd = new Random();

            runtimeModels.Clear();
            
            using (var sr = new StreamReader("TagList.txt", System.Text.Encoding.Default))
            {
                var s = string.Empty;
                while ((s = sr.ReadLine()) != null)
                {
                    tagList.Add(s);
                }
            }            

            foreach (var adr in tagList)
            {
                var runtimeModel = new RuntimeModel();
                var name = adr.Replace("'", string.Empty);
                runtimeModel.TagName = adr;
                runtimeModel.Value = rnd.Next(0, 20);

                runtimeModels.Add(runtimeModel);
            }

            return runtimeModels;
        }

        /// <summary>Прочитать данные из БД.</summary>
        /// <param name="tags">Набор тегов.</param>
        /// <param name="runtimeModels">Модель данных.</param>
        /// <returns>Набор значений.</returns>
        public ObservableCollection<RuntimeModel> DataRead(string tags, ObservableCollection<RuntimeModel> runtimeModels)
        {
            runtimeModels.Clear();
            
            try
            {
                using (var conn = new SqlConnection(this.connStr))
                {
                    conn.Open();

                    var query = "SELECT DateTime=CONVERT(VARCHAR,DateTime,121), TagName, Value FROM Runtime.dbo.Live WHERE TagName IN (" + tags + ")";
                    
                    var cmd = new SqlCommand(query, conn);
                    var reader = cmd.ExecuteReader();
                    var i = 0;

                    foreach (DbDataRecord record in reader)
                    {
                        var runtimeModel = new RuntimeModel();
                        var tempStr = Convert.ToString(record["Value"]);
                        if (string.IsNullOrEmpty(tempStr))
                        {
                            tempStr = "0";
                        }

                        var aa = Convert.ToSingle(tempStr);
                        string aa1 = record["TagName"].ToString();

                        runtimeModel.TagName = aa1;
                        runtimeModel.Value = aa;

                        runtimeModels.Add(runtimeModel);
                        i++;
                    }

                    reader.Close();
                    conn.Close();
                }
            }
            catch (SqlException ex)
            {
                this.logText = DateTime.Now.ToString() + "|fail|RuntimeDB - DataRead|" + ex.Message;
                this.logFile.WriteLog(this.logText);
            }

            return runtimeModels;
        }

        /// <summary>Прочитать последний отчет.</summary>
        /// <param name="hour">Час.</param>
        /// <returns>Состояние.</returns>
        public bool DataReadLastReport(decimal hour)
        {
            var connStr = @"server=" + this.mssql.Server + @";uid=" + this.mssql.Login + @";
                        pwd=" + this.mssql.Pass + @";database=AlarmSuite";
            var lastReport = true;

            try
            {
                using (var conn = new SqlConnection(connStr))
                {
                    conn.Open();

                    var query = "SELECT TOP 1 DATA FROM dbo.Report ORDER BY DATA DESC";
                    
                    var cmd = new SqlCommand(query, conn);

                    var reader = cmd.ExecuteReader();

                    foreach (DbDataRecord record in reader)
                    {
                        var sss = record["DATA"].ToString();
                        var hourT = Convert.ToDecimal(sss.Substring(9, 2));

                        if (hourT != hour)
                        {
                            lastReport = false;
                        }
                    }

                    reader.Close();
                    conn.Close();
                }
            }
            catch (SqlException ex)
            {
                this.logText = DateTime.Now.ToString() + "|fail|RuntimeDB - DataRead|" + ex.Message;
                this.logFile.WriteLog(this.logText);                
            }

            return lastReport;
        }

        /// <summary>Записать отчет.</summary>
        /// <param name="liveTEP">Текущие данные.</param>
        public void DataWrite(LiveTEP liveTEP)
        {
            var connStr = @"server=" + this.mssql.Server + @";uid=" + this.mssql.Login + @";
                        pwd=" + this.mssql.Pass + @";database=AlarmSuite";
            try
            {
                using (var conn = new SqlConnection(connStr))
                {
                    var format_date = "yyyyMMdd HH:mm:ss";

                    var query = "INSERT INTO dbo.Report (DATA, Data1, Data2, Data3, Data4, Data5, Data6, Data7, Data8, Data9, Data10, Data11, Data12, Data13) VALUES (@Data, @Data1, @Data2, @Data3, @Data4, @Data5, @Data6, @Data7, @Data8, @Data9, @Data10, @Data11, @Data12, @Data13)";

                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Data", DateTime.Now.ToString(format_date));
                        cmd.Parameters.AddWithValue("@Data1", Math.Round(liveTEP.SQLw_Data1, 7).ToString());
                        cmd.Parameters.AddWithValue("@Data2", Math.Round(liveTEP.SQLw_Data2, 7).ToString());
                        cmd.Parameters.AddWithValue("@Data3", Math.Round(liveTEP.SQLw_Data3, 7).ToString());
                        cmd.Parameters.AddWithValue("@Data4", Math.Round(liveTEP.SQLw_Data4, 7).ToString());
                        cmd.Parameters.AddWithValue("@Data5", Math.Round(liveTEP.SQLw_Data5, 7).ToString());
                        cmd.Parameters.AddWithValue("@Data6", Math.Round(liveTEP.SQLw_Data6, 7).ToString());
                        cmd.Parameters.AddWithValue("@Data7", Math.Round(liveTEP.SQLw_Data7, 7).ToString());
                        cmd.Parameters.AddWithValue("@Data8", Math.Round(liveTEP.SQLw_Data8, 7).ToString());
                        cmd.Parameters.AddWithValue("@Data9", Math.Round(liveTEP.SQLw_Data9, 7).ToString());
                        cmd.Parameters.AddWithValue("@Data10", Math.Round(liveTEP.SQLw_Data10, 7).ToString());
                        cmd.Parameters.AddWithValue("@Data11", Math.Round(liveTEP.SQLw_Data11, 7).ToString());
                        cmd.Parameters.AddWithValue("@Data12", Math.Round(liveTEP.SQLw_Data12, 7).ToString());
                        cmd.Parameters.AddWithValue("@Data13", Math.Round(liveTEP.SQLw_Data13, 7).ToString());

                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }

                    conn.Close();
                }

                this.logText = DateTime.Now.ToString() + "|event|RuntimeDB - DataWrite|Записан 2-х часовой отчет TEP";
                this.logFile.WriteLog(this.logText);
            }
            catch (SqlException ex)
            {
                this.logText = DateTime.Now.ToString() + "|fail|RuntimeDB - DataWrite|" + ex.Message;
                this.logFile.WriteLog(this.logText);
            }
        }

        /// <summary>Проверить связь.</summary>
        /// <returns>Состояние связи с MSSQL.</returns>
        public bool CheckMSSQLConn()
        {
            var conn = new SqlConnection(this.connStr);

            try
            {
                conn.Open();
                return true;
            }
            catch (SqlException ex)
            {
                this.logText = DateTime.Now.ToString() + "|fail|RuntimeDB - CheckMSSQLConn|" + ex.Message;
                this.logFile.WriteLog(this.logText);
                return false;                
            }
            finally
            {
                conn.Dispose();
            }
        }
    }
}