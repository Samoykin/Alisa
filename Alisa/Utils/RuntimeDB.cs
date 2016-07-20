using Alisa.Model;
using Alisa.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alisa.ViewModel
{
    class RuntimeDB
    {
        private LogFile logFile = new LogFile();        
        private ObservableCollection<RuntimeModel> _RtModel = new ObservableCollection<RuntimeModel>();

        public ObservableCollection<RuntimeModel> DataRead(String tags, XMLFields xmlFields)
        {
            String connStr = @"server=" + xmlFields.dbServer + @";uid=" + xmlFields.dbLogin + @";
                        pwd=" + xmlFields.dbPass + @";database=" + xmlFields.dbName + @"";

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {

                    conn.Open();

                    String command = "";

                    command = "SELECT DateTime=CONVERT(VARCHAR,DateTime,121), TagName, Value FROM Runtime.dbo.LiveTemp WHERE TagName IN (" + tags + ") AND Value is not NULL;";
                    //command = "SELECT DateTime=CONVERT(VARCHAR,DateTime,121), TagName, Value=CONVERT(nvarchar(512),Value) FROM Runtime.dbo.History WHERE TagName IN ('KP47_Dek_TMBox_U_PV') AND DateTime >= '" + startDate + "' AND DateTime <= '" + endDate + "' AND Value is not NULL AND wwRetrievalMode='Full' AND wwVersion='Latest' AND wwResolution=0 AND wwTimeZone='UTC';";

                    SqlCommand cmd = new SqlCommand(command, conn);

                    SqlDataReader reader = cmd.ExecuteReader();

                    int i = 0;

                    foreach (DbDataRecord record in reader)
                    {
                        RuntimeModel RtModel = new RuntimeModel();

                        Single aa = Convert.ToSingle(record["Value"]);
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
            catch (SqlException se)
            {
                String logText = DateTime.Now.ToString() + "|fail|RuntimeDB - DataRead|" + se.Message;
                logFile.WriteLog(logText);
            }
            return _RtModel;

        }
    }
}
