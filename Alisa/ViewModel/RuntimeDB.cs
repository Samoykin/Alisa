using Alisa.Model;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alisa.ViewModel
{
    class RuntimeDB
    {
        List<Single> tagValue = new List<float>();

        public List<Single> DataRead(String tags, DBConnect dbc)
        {
            String connStr = @"server=" + dbc.server + @";uid=" + dbc.login + @";
                        pwd=" + dbc.password + @";database=" + dbc.database + @"";

            SqlConnection conn = new SqlConnection(connStr);

            try
            {
                conn.Open();
            }
            catch (SqlException se)
            {
                return tagValue;
            }

            String command = "";
            //Выборка Аварий

            command = "SELECT DateTime=CONVERT(VARCHAR,DateTime,121), TagName, Value FROM Runtime.dbo.Live WHERE TagName IN (" + tags + ") AND Value is not NULL;";

            //command = "SELECT DateTime=CONVERT(VARCHAR,DateTime,121), TagName, Value=CONVERT(nvarchar(512),Value) FROM Runtime.dbo.History WHERE TagName IN ('KP47_Dek_TMBox_U_PV') AND DateTime >= '" + startDate + "' AND DateTime <= '" + endDate + "' AND Value is not NULL AND wwRetrievalMode='Full' AND wwVersion='Latest' AND wwResolution=0 AND wwTimeZone='UTC';";

            SqlCommand cmd = new SqlCommand(command, conn);

            SqlDataReader reader = cmd.ExecuteReader();

            int i = 0;

            tagValue.Clear();

            foreach (DbDataRecord record in reader)
            {
                tagValue.Add(Convert.ToSingle(record["Value"]));
                i++;

            }

            reader.Close();
            conn.Close();

            return tagValue;
        }
    }
}
