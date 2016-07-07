using Alisa.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
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
                Int32 indx, indx2, indx3, indx4, indx5, indx6, indx7;

                //-1---------------------------------------------------------------------------
                //К1 - Fгаза [м3] 1-FI501
                indx = IndexCalc("K4_Qg", _RtModel);
                Double Q_5 = _RtModel[indx].Value * 60;
                Double Data1 = Q_5 / 600;
                liveTEP.SQLw_Data1 = liveTEP.SQLw_Data1 + Data1;                

                //-2---------------------------------------------------------------------------
                //К2 - Fгаза [м3] 2-FI501
                indx = IndexCalc("K5_Qg", _RtModel);
                Double Q_6 = _RtModel[indx].Value * 60;
                Double Data2 = Q_6 / 600;
                liveTEP.SQLw_Data2 = liveTEP.SQLw_Data2 + Data2;

                //-3---------------------------------------------------------------------------
                //К3 - Fгаза [м3] 3-FI501
                indx = IndexCalc("K1_V10040", _RtModel);
                Double Q_4 = _RtModel[indx].Value * 60;
                Double Data3 = Q_4 / 600;
                liveTEP.SQLw_Data3 = liveTEP.SQLw_Data3 + Data3;

                //-4---------------------------------------------------------------------------
                //К3 - Fводы [нм3] 3-FI502
                indx = IndexCalc("K1_Fsv", _RtModel);
                Double Q_3 = _RtModel[indx].Value;
                Double Data4 = Q_3 / 600;
                liveTEP.SQLw_Data4 = liveTEP.SQLw_Data4 + Data4;

                //-5---------------------------------------------------------------------------
                //Fпара от котлов [т/ч] FI502
                indx = IndexCalc("OK_AI1004", _RtModel);
                indx2 = IndexCalc2("Pbar", _coeffModel);
                Double FI502_P = _RtModel[indx].Value + _coeffModel[indx2].Value;

                //{ Плотность насыщенного водяного пара в кг/м3 }
                Double FI502_Ro = 1.6585 * Math.Pow(10, -7) * Math.Pow(FI502_P, 5) +
                    (-1.2976 * Math.Pow(10, -5) * Math.Pow(FI502_P, 4)) +
                    (4.1981 * Math.Pow(10, -4) * Math.Pow(FI502_P, 3)) +
                    (-6.9431 * Math.Pow(10, -3) * Math.Pow(FI502_P, 2)) +
                    (0.5382 * FI502_P) + 0.0567;

                //{ Коэф. расширения }
                indx = IndexCalc2("FI502_d", _coeffModel);
                indx2 = IndexCalc2("FI502_Dt", _coeffModel);
                Double FI502_Betta = _coeffModel[indx].Value / _coeffModel[indx2].Value;

                indx = IndexCalc("OK_AI1102", _RtModel);
                indx2 = IndexCalc2("FI502_Ksi", _coeffModel);
                Double FI502_Eps = 1 - (0.41 + 0.35 * Math.Pow(FI502_Betta, 4)) * _RtModel[indx].Value * 100 / (10000 * FI502_P * _coeffModel[indx2].Value);

                //{ Массовый расход т/ч (основная формула) }
                indx = IndexCalc2("FI502_Alfa", _coeffModel);
                indx2 = IndexCalc2("FI502_d20", _coeffModel);
                indx3 = IndexCalc2("FI502_K0", _coeffModel);
                indx4 = IndexCalc2("FI502_Kp", _coeffModel);
                indx5 = IndexCalc2("FI502_Ksh", _coeffModel);
                indx6 = IndexCalc("OK_AI1102", _RtModel);

                Double FI502_Qm = 0.012522 * _coeffModel[indx].Value * FI502_Eps * Math.Pow(_coeffModel[indx2].Value, 2) * Math.Pow(_coeffModel[indx3].Value, 2) *
                    _coeffModel[indx4].Value * _coeffModel[indx5].Value * Math.Sqrt(_RtModel[indx6].Value * 100 * FI502_Ro) / 1000;

                Double Data5 = FI502_Qm / 600;
                //Double test = Convert.ToDouble(liveTEP.SQLw_Data5) + Data5;
                liveTEP.SQLw_Data5 = liveTEP.SQLw_Data5 + Data5;

                //-6---------------------------------------------------------------------------
                //Eтепла от котлов [Гкал]
                //{ Удельная энтальпия сухого насыщенного пара в кДж/кг }
                Double FI502_h = (-1.8395 * Math.Pow(10, -3) * Math.Pow(FI502_P, 4)) + (0.1079 * Math.Pow(FI502_P, 3)) + (-2.4192 * Math.Pow(FI502_P, 2)) + (26.7095 * FI502_P) + 2661.85;
                
                //{ Тепловая энергия в Гкал/ч }
                Double FI502_Etta = 0.23885 * Math.Pow(10, -6) * FI502_Qm * 1000 * FI502_h;
                
                Double Data6 = FI502_Etta / 600;
                liveTEP.SQLw_Data6 = liveTEP.SQLw_Data6 + Data6;

                //-7---------------------------------------------------------------------------
                //Fпара на установку [т/ч] FI507
                indx = IndexCalc("OK_AI1601", _RtModel);
                indx2 = IndexCalc2("Pbar", _coeffModel);
                Double FI507_P = _RtModel[indx].Value + _coeffModel[indx2].Value;

                //{ Плотность насыщенного водяного пара в кг/м3 }
                Double FI507_Ro = 1.6585 * Math.Pow(10, -7) * Math.Pow(FI507_P, 5) +
                    (-1.2976 * Math.Pow(10, -5) * Math.Pow(FI507_P, 4)) +
                    (4.1981 * Math.Pow(10, -4) * Math.Pow(FI507_P, 3)) +
                    (-6.9431 * Math.Pow(10, -3) * Math.Pow(FI507_P, 2)) +
                    (0.5382 * FI507_P) + 0.0567;

                //{ Коэф. расширения }
                indx = IndexCalc2("FI507_d", _coeffModel);
                indx2 = IndexCalc2("FI507_Dt", _coeffModel);
                Double FI507_Betta = _coeffModel[indx].Value / _coeffModel[indx2].Value;

                indx = IndexCalc("OK_AI1602", _RtModel);
                indx2 = IndexCalc2("FI507_Ksi", _coeffModel);
                Double FI507_Eps = 1 - (0.41 + 0.35 * Math.Pow(FI507_Betta, 4)) * _RtModel[indx].Value * 100 / (10000 * FI507_P * _coeffModel[indx2].Value);

                //{ Массовый расход т/ч (основная формула) }
                indx = IndexCalc2("FI507_Alfa", _coeffModel);
                indx2 = IndexCalc2("FI507_d20", _coeffModel);
                indx3 = IndexCalc2("FI507_K0", _coeffModel);
                indx4 = IndexCalc2("FI507_Kp", _coeffModel);
                indx5 = IndexCalc2("FI507_Ksh", _coeffModel);
                indx6 = IndexCalc("OK_AI1602", _RtModel);

                Double FI507_Qm = 0.012522 * _coeffModel[indx].Value * FI507_Eps * Math.Pow(_coeffModel[indx2].Value, 2) * Math.Pow(_coeffModel[indx3].Value, 2) *
                    _coeffModel[indx4].Value * _coeffModel[indx5].Value * Math.Sqrt(_RtModel[indx6].Value * 100 * FI507_Ro) / 1000;

                Double Data7 = FI507_Qm / 600;
                liveTEP.SQLw_Data7 = liveTEP.SQLw_Data7 + Data7;

                //-8---------------------------------------------------------------------------
                //Eтепла на установку [Гкал]
                //{ Удельная энтальпия сухого насыщенного пара в кДж/кг }
                Double FI507_h = (-1.8395 * Math.Pow(10, -3) * Math.Pow(FI507_P, 4)) + (0.1079 * Math.Pow(FI507_P, 3)) + (-2.4192 * Math.Pow(FI507_P, 2)) + (26.7095 * FI507_P) + 2661.85;

                //{ Тепловая энергия в Гкал/ч }
                Double FI507_Etta = 0.23885 * Math.Pow(10, -6) * FI507_Qm * 1000 * FI507_h;

                Double Data8 = FI507_Etta / 600;
                liveTEP.SQLw_Data8 = liveTEP.SQLw_Data8 + Data8;

                //-9---------------------------------------------------------------------------
                //Fводы на подпитку [нм3] FI503
                //{ Плотность воды кг/м3 }
                indx = IndexCalc("OK_AI1603", _RtModel);
                
                Double FI503_Ro = Math.Pow(10, 9) / (998792.53 + 95.33246 * _RtModel[indx].Value * 98.0665 + 3.4743522 * Math.Pow(_RtModel[indx].Value * 98.0665, 2));

                //{ Массовый расход т/ч (основная формула) }
                indx = IndexCalc2("FI503_Alfa", _coeffModel);
                indx2 = IndexCalc2("FI503_d20", _coeffModel);
                indx3 = IndexCalc2("FI503_K0", _coeffModel);
                indx4 = IndexCalc2("FI503_Kp", _coeffModel);
                indx5 = IndexCalc2("FI503_Ksh", _coeffModel);
                indx6 = IndexCalc("OK_AI1103", _RtModel);

                Double FI503_Qm = 0.012522 * _coeffModel[indx].Value * Math.Pow(_coeffModel[indx2].Value, 2) * Math.Pow(_coeffModel[indx3].Value, 2) *
                    _coeffModel[indx4].Value * _coeffModel[indx5].Value * Math.Sqrt(_RtModel[indx6].Value * 100 * FI503_Ro) / 1000;

                Double Data9 = FI503_Qm / 600;
                liveTEP.SQLw_Data9 = liveTEP.SQLw_Data9 + Data9;

                //-10--------------------------------------------------------------------------
                //Fводы прямой сетевой [нм3] FI504
                //{ Плотность воды кг/м3 }
                indx = IndexCalc("OK_AI1001", _RtModel);

                Double FI504_Ro = Math.Pow(10, 9) / (998792.53 + 95.33246 * _RtModel[indx].Value * 98.0665 + 3.4743522 * Math.Pow(_RtModel[indx].Value * 98.0665, 2));

                //{ Массовый расход т/ч (основная формула) }
                indx = IndexCalc2("FI504_Alfa", _coeffModel);
                indx2 = IndexCalc2("FI504_d20", _coeffModel);
                indx3 = IndexCalc2("FI504_K0", _coeffModel);
                indx4 = IndexCalc2("FI504_Kp", _coeffModel);
                indx5 = IndexCalc2("FI504_Ksh", _coeffModel);
                indx6 = IndexCalc("OK_AI1104", _RtModel);

                Double FI504_Qm = 0.012522 * _coeffModel[indx].Value * Math.Pow(_coeffModel[indx2].Value, 2) * Math.Pow(_coeffModel[indx3].Value, 2) *
                    _coeffModel[indx4].Value * _coeffModel[indx5].Value * Math.Sqrt(_RtModel[indx6].Value * 100 * FI504_Ro) / 1000;

                Double Data10 = FI504_Qm / 600;
                liveTEP.SQLw_Data10 = liveTEP.SQLw_Data10 + Data10;

                //-11--------------------------------------------------------------------------
                //Fгаза на вх. в котельн. [нм3] FI505
                indx = IndexCalc("OK_AI1106", _RtModel);
                indx2 = IndexCalc2("Pbar", _coeffModel);
                Double FI505_P = _RtModel[indx].Value / 100 + _coeffModel[indx2].Value;

                //{ Плотность газа кг/м3 }
                indx = IndexCalc2("FI505_Roc", _coeffModel);
                indx2 = IndexCalc("OK_AI1105", _RtModel);
                indx3 = IndexCalc2("FI505_K", _coeffModel);
                Double FI505_Ro = 283.73 * _coeffModel[indx].Value * FI505_P / ((_RtModel[indx2].Value + 273.15) * _coeffModel[indx3].Value);

                //{ Коэф. расширения топливного газа }
                indx = IndexCalc2("FI505_d", _coeffModel);
                indx2 = IndexCalc2("FI505_Dt", _coeffModel);
                Double FI505_Betta = _coeffModel[indx].Value / _coeffModel[indx2].Value;

                indx = IndexCalc("OK_AI1107", _RtModel);
                indx2 = IndexCalc2("FI505_Ksi", _coeffModel);
                Double FI505_Eps = 1 - (0.41 + 0.35 * Math.Pow(FI505_Betta, 4)) * _RtModel[indx].Value * 100 / (10000 * FI505_P * _coeffModel[indx2].Value);

                //{ Объемный расход нм3/ч }
                indx = IndexCalc2("FI505_Alfa", _coeffModel);
                indx2 = IndexCalc2("FI505_d20", _coeffModel);
                indx3 = IndexCalc2("FI505_K0", _coeffModel);
                indx4 = IndexCalc2("FI505_Kp", _coeffModel);
                indx5 = IndexCalc2("FI505_Ksh", _coeffModel);
                indx6 = IndexCalc("OK_AI1107", _RtModel);
                indx7 = IndexCalc2("FI505_Roc", _coeffModel);

                Double FI505_Qc = 0.012522 * _coeffModel[indx].Value * FI505_Eps * Math.Pow(_coeffModel[indx2].Value, 2) * Math.Pow(_coeffModel[indx3].Value, 2) *
                    _coeffModel[indx4].Value * _coeffModel[indx5].Value * Math.Sqrt(_RtModel[indx6].Value * 100 * FI505_Ro) / _coeffModel[indx7].Value;

                Double Data11 = FI505_Qc / 600;
                liveTEP.SQLw_Data11 = liveTEP.SQLw_Data11 + Data11;

                //-12--------------------------------------------------------------------------
                //Eтепла 3-го котла
                indx = IndexCalc("K1_Fsv", _RtModel);
                indx2 = IndexCalc("K1_Tsv_out", _RtModel);
                indx3 = IndexCalc("K1_Tsv_in", _RtModel);
                Double Et_K3 = _RtModel[indx].Value * (_RtModel[indx2].Value - _RtModel[indx3].Value) / 1000;
                
                Double Data12 = Et_K3 / 600;
                liveTEP.SQLw_Data12 = liveTEP.SQLw_Data12 + Data12;

                //-13--------------------------------------------------------------------------
                //Количество газа УВП [тыс. нм3]
                indx = IndexCalc("OK_UVP_Q", _RtModel);

                Double OK_UVP_Q_old = 0;
                if (_RtModel[indx].Value < OK_UVP_Q_old)
                    OK_UVP_Q_old = _RtModel[indx].Value;

                Double DeltaQ2 = _RtModel[indx].Value - OK_UVP_Q_old;
                
                Double Data13 = DeltaQ2;
                liveTEP.SQLw_Data13 = liveTEP.SQLw_Data13 + Data13;



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


       

    }
}
