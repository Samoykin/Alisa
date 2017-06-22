using Alisa.Model;
using Alisa.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alisa.ViewModel
{
    class RuntimeDB
    {
        private LogFile logFile = new LogFile();

        String logText;
        String connStr;
        XMLFields _xmlFields;

        public RuntimeDB(XMLFields xmlFields)
        {
            connStr = @"server=" + xmlFields.dbServer + @";uid=" + xmlFields.dbLogin + @";
                        pwd=" + xmlFields.dbPass + @";database=" + xmlFields.dbName + @"";
            _xmlFields = xmlFields;
        }

        public ObservableCollection<RuntimeModel> DataReadTest(String tags, ObservableCollection<RuntimeModel> _RtModel)
        {
            _RtModel.Clear();
            List<String> tagList = new List<string>();

            using (StreamReader sr = new StreamReader("TagList.txt", System.Text.Encoding.Default))
            {
                String s = "";
                while ((s = sr.ReadLine()) != null)
                {
                    tagList.Add(s);
                }
            }

            var addresses = tags.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            Random rnd = new Random();

            foreach(var adr in tagList)
            {
                RuntimeModel RtModel = new RuntimeModel();
                String name = adr.Replace("'","");
                RtModel.TagName = adr;
                RtModel.Value = rnd.Next(0,20);

                _RtModel.Add(RtModel);
            }

            return _RtModel;
        }
        public ObservableCollection<RuntimeModel> DataRead(String tags, ObservableCollection<RuntimeModel> _RtModel)
        {
            _RtModel.Clear();
            
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();

                    String query = "SELECT DateTime=CONVERT(VARCHAR,DateTime,121), TagName, Value FROM Runtime.dbo.Live WHERE TagName IN (" + tags + ")";
                    
                    SqlCommand cmd = new SqlCommand(query, conn);

                    SqlDataReader reader = cmd.ExecuteReader();

                    int i = 0;

                    foreach (DbDataRecord record in reader)
                    {
                        RuntimeModel RtModel = new RuntimeModel();
                        String tempStr = Convert.ToString(record["Value"]);
                        if (String.IsNullOrEmpty(tempStr))
                            tempStr = "0";
                        Single aa = Convert.ToSingle(tempStr);
                        String aa1 = record["TagName"].ToString();

                        RtModel.TagName = aa1;
                        RtModel.Value = aa;

                        _RtModel.Add(RtModel);
                        i++;

                    }

                    reader.Close();
                    conn.Close();
                }
            }
            catch (SqlException ex)
            {
                logText = DateTime.Now.ToString() + "|fail|RuntimeDB - DataRead|" + ex.Message;
                logFile.WriteLog(logText);
            }
            return _RtModel;

        }

        public Boolean DataReadLastReport(Decimal hour)
        {
            String connStr = @"server=" + _xmlFields.dbServer + @";uid=" + _xmlFields.dbLogin + @";
                        pwd=" + _xmlFields.dbPass + @";database=AlarmSuite";
            Boolean lastReport = true;
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();

                    String query = "SELECT TOP 1 DATA FROM dbo.Report ORDER BY DATA DESC";
                    
                    SqlCommand cmd = new SqlCommand(query, conn);

                    SqlDataReader reader = cmd.ExecuteReader();

                    foreach (DbDataRecord record in reader)
                    {
                        String sss = record["DATA"].ToString();
                        Decimal hourT = Convert.ToDecimal(sss.Substring(9, 2));

                        if (hourT != hour)
                            lastReport = false;
                    }
                    reader.Close();
                    conn.Close();
                }
            }
            catch (SqlException ex)
            {
                logText = DateTime.Now.ToString() + "|fail|RuntimeDB - DataRead|" + ex.Message;
                logFile.WriteLog(logText);                
            }

            return lastReport;

        }

        public void DataWrite(LiveTEP liveTEP, XMLFields xmlFields)
        {
            String connStr = @"server=" + xmlFields.dbServer + @";uid=" + xmlFields.dbLogin + @";
                        pwd=" + xmlFields.dbPass + @";database=AlarmSuite";
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {                    
                    String format_date = "yyyyMMdd HH:mm:ss";

                    String query = "INSERT INTO dbo.Report (DATA, Data1, Data2, Data3, Data4, Data5, Data6, Data7, Data8, Data9, Data10, Data11, Data12, Data13) VALUES (@Data, @Data1, @Data2, @Data3, @Data4, @Data5, @Data6, @Data7, @Data8, @Data9, @Data10, @Data11, @Data12, @Data13)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
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

                logText = DateTime.Now.ToString() + "|event|RuntimeDB - DataWrite|Записан 2-х часовой отчет TEP";
                logFile.WriteLog(logText);
            }
            catch (SqlException ex)
            {
                logText = DateTime.Now.ToString() + "|fail|RuntimeDB - DataWrite|" + ex.Message;
                logFile.WriteLog(logText);
            }
        }

        //Состояние связи с MSSQL
        public Boolean CheckMSSQLConn()
        {
            SqlConnection conn = new SqlConnection(connStr);

            try
            {
                conn.Open();
                return true;
            }
            catch (SqlException ex)
            {
                logText = DateTime.Now.ToString() + "|fail|RuntimeDB - CheckMSSQLConn|" + ex.Message;
                logFile.WriteLog(logText);
                return false;                
            }
            finally
            {
                conn.Dispose();
            }

        }

    }
}
