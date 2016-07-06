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

        public LiveTEP Calculate(LiveTEP liveTEP, ObservableCollection<RuntimeModel> _RtModel, ObservableCollection<CoeffModel> _coeffModel)
        {
            try
            {
                Int32 indx;
                Int32 indx2, indx3, indx4, indx5, indx6;
                //К1 - Fгаза [м3] 1-FI501
                indx = IndexCalc("K4_Qg", _RtModel);
                Single Q_5 = _RtModel[indx].Value * 60;
                Single Data1 = Q_5 / 600;
                liveTEP.SQLw_Data1 = liveTEP.SQLw_Data1 + Data1;

                //К2 - Fгаза [м3] 2-FI501
                indx = IndexCalc("K5_Qg", _RtModel);
                Single Q_6 = _RtModel[indx].Value * 60;
                Single Data2 = Q_6 / 600;
                liveTEP.SQLw_Data2 = liveTEP.SQLw_Data2 + Data2;

                //К3 - Fгаза [м3] 3-FI501
                indx = IndexCalc("K1_V10040", _RtModel);
                Single Q_4 = _RtModel[indx].Value * 60;
                Single Data3 = Q_4 / 600;
                liveTEP.SQLw_Data3 = liveTEP.SQLw_Data3 + Data3;

                //К3 - Fводы [нм3] 3-FI502
                indx = IndexCalc("K1_Fsv", _RtModel);
                Single Q_3 = _RtModel[indx].Value;
                Single Data4 = Q_3 / 600;
                liveTEP.SQLw_Data4 = liveTEP.SQLw_Data4 + Data4;

                //-----------------------------------------------------------------------------
                //Fпара от котлов [т/ч] FI502
                indx = IndexCalc("OK_AI1004", _RtModel);
                indx2 = IndexCalc2("Pbar", _coeffModel);
                Single FI502_P = _RtModel[indx].Value + _coeffModel[indx2].Value;
                //{ Плотность насыщенного водяного пара в кг/м3 }
                Single FI502_Ro = (Single)((Math.Pow(1.6585 * 10, -7) * Math.Pow(FI502_P, 5)) +
                    (Math.Pow(-1.2976 * 10, -5) * Math.Pow(FI502_P, 4)) +
                    (Math.Pow(4.1981 * 10, -4) * Math.Pow(FI502_P, 3)) +
                    (Math.Pow(-6.9431 * 10, -3) * Math.Pow(FI502_P, 2)) +
                    (0.5382 * FI502_P) + 0.0567);
                //{ Коэф. расширения }
                indx = IndexCalc2("FI502_d", _coeffModel);
                indx2 = IndexCalc2("FI502_Dt", _coeffModel);
                Single FI502_Betta = _coeffModel[indx].Value / _coeffModel[indx2].Value;

                indx = IndexCalc("OK_AI1102", _RtModel);
                indx2 = IndexCalc2("FI502_Ksi", _coeffModel);
                Single FI502_Eps = (Single)(1 - (0.41 + 0.35 * Math.Pow(FI502_Betta, 4)) * _RtModel[indx].Value * 100 / (10000 * FI502_P * _coeffModel[indx2].Value));

                //{ Массовый расход т/ч (основная формула) }
                indx = IndexCalc2("FI502_Alfa", _coeffModel);
                indx2 = IndexCalc2("FI502_d20", _coeffModel);
                indx3 = IndexCalc2("FI502_K0", _coeffModel);
                indx4 = IndexCalc2("FI502_Kp", _coeffModel);
                indx5 = IndexCalc2("FI502_Ksh", _coeffModel);
                indx6 = IndexCalc("OK_AI1102", _RtModel);

                Single FI502_Qm = (Single)(0.012522 * _coeffModel[indx].Value * FI502_Eps * Math.Pow(_coeffModel[indx2].Value, 2) * Math.Pow(_coeffModel[indx3].Value, 2) *
                    _coeffModel[indx4].Value * _coeffModel[indx5].Value * Math.Sqrt(_RtModel[indx].Value * 100 * FI502_Ro) / 1000);
                                
                Single Data5 = FI502_Qm / 600;
                liveTEP.SQLw_Data5 = liveTEP.SQLw_Data5 + Data5;
                //-----------------------------------------------------------------------------

            }
            catch (Exception exception)
            {
                String logText = DateTime.Now.ToString() + "|fail|MainViewModel - Calculate|" + exception.Message;
                logFile.WriteLog(logText);
            }

            return liveTEP;
        }

        private Int32 IndexCalc(String name, ObservableCollection<RuntimeModel> _RtModel)
        {
            Int32 indx;
            indx = _RtModel.IndexOf(_RtModel.Where(X => X.TagName == name).FirstOrDefault());
            return indx;
        }

        private Int32 IndexCalc2(String name, ObservableCollection<CoeffModel> _coeffModel)
        {
            Int32 indx;
            indx = _coeffModel.IndexOf(_coeffModel.Where(X => X.TagName == name).FirstOrDefault());
            return indx;
        }


        //public Single CalculateTEP_1(int i, Single SQLw_Data, ObservableCollection<RuntimeModel> _RtModel)
        //{ 
        //    try
        //    {
        //        Single Q_5 = _RtModel[i].Value * 60;
        //        Single Data1 = Q_5 / 600;
        //        SQLw_Data = SQLw_Data + Data1;
                
        //    }
        //    catch (Exception exception)
        //    {
        //        String logText = DateTime.Now.ToString() + "|fail|MainViewModel - CalculateTEP_1|" + exception.Message;
        //        logFile.WriteLog(logText);
        //    }

        //    return SQLw_Data;            
        //}


        //public Single CalculateTEP_2(int i, Single SQLw_Data, ObservableCollection<RuntimeModel> _RtModel)
        //{
        //    try
        //    {
        //        Single Q_6 = _RtModel[i].Value * 60;
        //        Single Data2 = Q_6 / 600;
        //        SQLw_Data = SQLw_Data + Data2;
        //    }
        //    catch (Exception exception)
        //    {
        //        String logText = DateTime.Now.ToString() + "|fail|MainViewModel - CalculateTEP_2|" + exception.Message;
        //        logFile.WriteLog(logText); 
        //    }
        //    return SQLw_Data; 
        //}

        //public Single CalculateTEP_3(int i, Single SQLw_Data, ObservableCollection<RuntimeModel> _RtModel)
        //{
        //    try
        //    {
        //        Single Q_4 = _RtModel[i].Value * 60;
        //        Single Data3 = Q_4 / 600;
        //        SQLw_Data = SQLw_Data + Data3;
        //    }
        //    catch (Exception exception)
        //    {
        //        String logText = DateTime.Now.ToString() + "|fail|MainViewModel - CalculateTEP_3|" + exception.Message;
        //        logFile.WriteLog(logText);                
        //    }
        //    return SQLw_Data;
        //}

        //public Single CalculateTEP_4(int i, Single SQLw_Data, ObservableCollection<RuntimeModel> _RtModel)
        //{
        //    try
        //    {
        //        Single Q_3 = _RtModel[i].Value;
        //        Single Data4 = Q_3 / 600;
        //        SQLw_Data = SQLw_Data + Data4;
        //    }
        //    catch (Exception exception)
        //    {
        //        String logText = DateTime.Now.ToString() + "|fail|MainViewModel - CalculateTEP_4|" + exception.Message;
        //        logFile.WriteLog(logText);
        //    }
        //    return SQLw_Data;
        //}





    }
}
