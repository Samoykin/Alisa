using Alisa.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alisa.Utils
{
    class CalculateTEP
    {
        LogFile logFile = new LogFile();

        public Single CalculateTEP_1(int i, Single SQLw_Data, ObservableCollection<RuntimeModel> _RtModel)
        { 
            try
            {
                Single Q_5 = _RtModel[i].Value * 60;
                Single Data1 = Q_5 / 600;
                SQLw_Data = SQLw_Data + Data1;
                
            }
            catch (Exception exception)
            {
                String logText = DateTime.Now.ToString() + "|fail|MainViewModel - CalculateTEP_1|" + exception.Message;
                logFile.WriteLog(logText);
            }

            return SQLw_Data;            
        }


        public Single CalculateTEP_2(int i, Single SQLw_Data, ObservableCollection<RuntimeModel> _RtModel)
        {
            try
            {
                Single Q_6 = _RtModel[i].Value * 60;
                Single Data2 = Q_6 / 600;
                SQLw_Data = SQLw_Data + Data2;
            }
            catch (Exception exception)
            {
                String logText = DateTime.Now.ToString() + "|fail|MainViewModel - CalculateTEP_2|" + exception.Message;
                logFile.WriteLog(logText); 
            }
            return SQLw_Data; 
        }

        public Single CalculateTEP_3(int i, Single SQLw_Data, ObservableCollection<RuntimeModel> _RtModel)
        {
            try
            {
                Single Q_4 = _RtModel[i].Value * 60;
                Single Data3 = Q_4 / 600;
                SQLw_Data = SQLw_Data + Data3;
            }
            catch (Exception exception)
            {
                String logText = DateTime.Now.ToString() + "|fail|MainViewModel - CalculateTEP_3|" + exception.Message;
                logFile.WriteLog(logText);                
            }
            return SQLw_Data;
        }

        public Single CalculateTEP_4(int i, Single SQLw_Data, ObservableCollection<RuntimeModel> _RtModel)
        {
            try
            {
                Single Q_3 = _RtModel[i].Value;
                Single Data4 = Q_3 / 600;
                SQLw_Data = SQLw_Data + Data4;
            }
            catch (Exception exception)
            {
                String logText = DateTime.Now.ToString() + "|fail|MainViewModel - CalculateTEP_4|" + exception.Message;
                logFile.WriteLog(logText);
            }
            return SQLw_Data;
        }





    }
}
