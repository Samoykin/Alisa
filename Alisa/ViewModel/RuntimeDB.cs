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
        float[] val = { 0, 0, 0, 0 };
        float[] val2 = { 0, 0, 0, 0 };
        String tags = "'K4_Qg','K5_Qg','OK_AI1102','OK_AI1105'";

        public float[] DataRead(String tags2)
        {
            String connStr = @"server=192.168.1.20;uid=sa;
                        pwd=sa;database=Runtime";

            SqlConnection conn = new SqlConnection(connStr);

            try
            {
                conn.Open();
            }
            catch (SqlException se)
            {
                //hd.tagName.Add("");
                return val;
            }

            String command = "";
            //Выборка Аварий

            command = "SELECT DateTime=CONVERT(VARCHAR,DateTime,121), TagName, Value FROM Runtime.dbo.Live WHERE TagName IN (" + tags + ") AND Value is not NULL;";

            //command = "SELECT DateTime=CONVERT(VARCHAR,DateTime,121), TagName, Value=CONVERT(nvarchar(512),Value) FROM Runtime.dbo.History WHERE TagName IN ('KP47_Dek_TMBox_U_PV') AND DateTime >= '" + startDate + "' AND DateTime <= '" + endDate + "' AND Value is not NULL AND wwRetrievalMode='Full' AND wwVersion='Latest' AND wwResolution=0 AND wwTimeZone='UTC';";

            SqlCommand cmd = new SqlCommand(command, conn);

            SqlDataReader reader = cmd.ExecuteReader();

            int i = 0;

            foreach (DbDataRecord record in reader)
            {

                val[i] = Convert.ToSingle(record["Value"]);
                i++;

            }

            reader.Close();
            conn.Close();

            return val;
        }
    }
}
