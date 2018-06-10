namespace Alisa.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;    
    using Model;

    /// <summary>Расчет ТЭП.</summary>
    public class CalculateTEP
    {
        /// <summary>Расчет.</summary>
        /// <param name="liveTEP">Текущие значения.</param>
        /// <param name="runtimeModel">Модель параметров.</param>
        /// <param name="coeffModel">Модель Коэффициентов.</param>
        /// <returns>Расчитанные значения.</returns>
        public LiveTEP Calculate(LiveTEP liveTEP, ObservableCollection<RuntimeModel> runtimeModel, ObservableCollection<CoeffModel> coeffModel)
        {
                int indx, indx2, indx3, indx4, indx5, indx6, indx7;

                // -1---------------------------------------------------------------------------
                // К1 - Fгаза [м3] 1-FI501
                liveTEP.SQLw_Data1 += runtimeModel[this.IndexCalc("K4_Qg", runtimeModel)].Value * 0.1;                

                // -2---------------------------------------------------------------------------
                // К2 - Fгаза [м3] 2-FI501
                liveTEP.SQLw_Data2 += runtimeModel[this.IndexCalc("K5_Qg", runtimeModel)].Value * 0.1;

                // -3---------------------------------------------------------------------------
                // К3 - Fгаза [м3] 3-FI501
                liveTEP.SQLw_Data3 += runtimeModel[this.IndexCalc("K1_V10040", runtimeModel)].Value * 0.1;

                // -4---------------------------------------------------------------------------
                // К3 - Fводы [нм3] 3-FI502
                liveTEP.SQLw_Data4 += runtimeModel[this.IndexCalc("K1_Fsv", runtimeModel)].Value / 600;

                // -5---------------------------------------------------------------------------
                // Fпара от котлов [т/ч] FI502
                double paramFI502_P = runtimeModel[this.IndexCalc("OK_AI1004", runtimeModel)].Value + coeffModel[this.IndexCalc2("Pbar", coeffModel)].Value;

                // { Плотность насыщенного водяного пара в кг/м3 }
                var paramFI502_Ro = (1.6585 * Math.Pow(10, -7) * Math.Pow(paramFI502_P, 5)) +
                    (-1.2976 * Math.Pow(10, -5) * Math.Pow(paramFI502_P, 4)) +
                    (4.1981 * Math.Pow(10, -4) * Math.Pow(paramFI502_P, 3)) +
                    (-6.9431 * Math.Pow(10, -3) * Math.Pow(paramFI502_P, 2)) +
                    (0.5382 * paramFI502_P) + 0.0567;

                // { Коэф. расширения }
                double paramFI502_Betta = coeffModel[this.IndexCalc2("FI502_d", coeffModel)].Value / coeffModel[this.IndexCalc2("FI502_Dt", coeffModel)].Value;

                indx = this.IndexCalc("OK_AI1102", runtimeModel);
                indx2 = this.IndexCalc2("FI502_Ksi", coeffModel);
                var paramFI502_Eps = 1 - ((0.41 + (0.35 * Math.Pow(paramFI502_Betta, 4))) * runtimeModel[indx].Value * 100 / (10000 * paramFI502_P * coeffModel[indx2].Value));

                // { Массовый расход т/ч (основная формула) }
                indx = this.IndexCalc2("FI502_Alfa", coeffModel);
                indx2 = this.IndexCalc2("FI502_d20", coeffModel);
                indx3 = this.IndexCalc2("FI502_K0", coeffModel);
                indx4 = this.IndexCalc2("FI502_Kp", coeffModel);
                indx5 = this.IndexCalc2("FI502_Ksh", coeffModel);
                indx6 = this.IndexCalc("OK_AI1102", runtimeModel);

                var paramFI502_Qm = 0.012522 * coeffModel[indx].Value * paramFI502_Eps * Math.Pow(coeffModel[indx2].Value, 2) * Math.Pow(coeffModel[indx3].Value, 2) *
                    coeffModel[indx4].Value * coeffModel[indx5].Value * Math.Sqrt(runtimeModel[indx6].Value * 100 * paramFI502_Ro) / 1000;

                liveTEP.SQLw_Data5 += paramFI502_Qm / 600;

                // -6---------------------------------------------------------------------------
                // Eтепла от котлов [Гкал]
                // { Удельная энтальпия сухого насыщенного пара в кДж/кг }
                var paramFI502_h = (-1.8395 * Math.Pow(10, -3) * Math.Pow(paramFI502_P, 4)) + (0.1079 * Math.Pow(paramFI502_P, 3)) + (-2.4192 * Math.Pow(paramFI502_P, 2)) + (26.7095 * paramFI502_P) + 2661.85;

                // { Тепловая энергия в Гкал/ч }
                var paramFI502_Etta = 0.23885 * Math.Pow(10, -6) * paramFI502_Qm * 1000 * paramFI502_h;

                var data6 = paramFI502_Etta / 600;
                liveTEP.SQLw_Data6 = liveTEP.SQLw_Data6 + data6;

                // -7---------------------------------------------------------------------------
                // Fпара на установку [т/ч] FI507
                indx = this.IndexCalc("OK_AI1601", runtimeModel);
                indx2 = this.IndexCalc2("Pbar", coeffModel);
                double paramFI507_P = runtimeModel[indx].Value + coeffModel[indx2].Value;

                // { Плотность насыщенного водяного пара в кг/м3 }
                var paramFI507_Ro = (1.6585 * Math.Pow(10, -7) * Math.Pow(paramFI507_P, 5)) +
                    (-1.2976 * Math.Pow(10, -5) * Math.Pow(paramFI507_P, 4)) +
                    (4.1981 * Math.Pow(10, -4) * Math.Pow(paramFI507_P, 3)) +
                    (-6.9431 * Math.Pow(10, -3) * Math.Pow(paramFI507_P, 2)) +
                    (0.5382 * paramFI507_P) + 0.0567;

                // { Коэф. расширения }
                indx = this.IndexCalc2("FI507_d", coeffModel);
                indx2 = this.IndexCalc2("FI507_Dt", coeffModel);
                double paramFI507_Betta = coeffModel[indx].Value / coeffModel[indx2].Value;

                indx = this.IndexCalc("OK_AI1602", runtimeModel);
                indx2 = this.IndexCalc2("FI507_Ksi", coeffModel);
                var paramFI507_Eps = 1 - ((0.41 + (0.35 * Math.Pow(paramFI507_Betta, 4))) * runtimeModel[indx].Value * 100 / (10000 * paramFI507_P * coeffModel[indx2].Value));

                // { Массовый расход т/ч (основная формула) }
                indx = this.IndexCalc2("FI507_Alfa", coeffModel);
                indx2 = this.IndexCalc2("FI507_d20", coeffModel);
                indx3 = this.IndexCalc2("FI507_K0", coeffModel);
                indx4 = this.IndexCalc2("FI507_Kp", coeffModel);
                indx5 = this.IndexCalc2("FI507_Ksh", coeffModel);
                indx6 = this.IndexCalc("OK_AI1602", runtimeModel);

                var paramFI507_Qm = 0.012522 * coeffModel[indx].Value * paramFI507_Eps * Math.Pow(coeffModel[indx2].Value, 2) * Math.Pow(coeffModel[indx3].Value, 2) *
                    coeffModel[indx4].Value * coeffModel[indx5].Value * Math.Sqrt(runtimeModel[indx6].Value * 100 * paramFI507_Ro) / 1000;

                liveTEP.SQLw_Data7 += paramFI507_Qm / 600;

                // -8---------------------------------------------------------------------------
                // Eтепла на установку [Гкал]
                // { Удельная энтальпия сухого насыщенного пара в кДж/кг }
                var paramFI507_h = (-1.8395 * Math.Pow(10, -3) * Math.Pow(paramFI507_P, 4)) + (0.1079 * Math.Pow(paramFI507_P, 3)) + (-2.4192 * Math.Pow(paramFI507_P, 2)) + (26.7095 * paramFI507_P) + 2661.85;

                // { Тепловая энергия в Гкал/ч }
                var paramFI507_Etta = 0.23885 * Math.Pow(10, -6) * paramFI507_Qm * 1000 * paramFI507_h;

                liveTEP.SQLw_Data8 += paramFI507_Etta / 600;

                // -9---------------------------------------------------------------------------
                // Fводы на подпитку [нм3] FI503
                // { Плотность воды кг/м3 }
                indx = this.IndexCalc("OK_AI1603", runtimeModel);

                var paramFI503_Ro = Math.Pow(10, 9) / (998792.53 + (95.33246 * runtimeModel[indx].Value * 98.0665) + (3.4743522 * Math.Pow(runtimeModel[indx].Value * 98.0665, 2)));

                // { Массовый расход т/ч (основная формула) }
                indx = this.IndexCalc2("FI503_Alfa", coeffModel);
                indx2 = this.IndexCalc2("FI503_d20", coeffModel);
                indx3 = this.IndexCalc2("FI503_K0", coeffModel);
                indx4 = this.IndexCalc2("FI503_Kp", coeffModel);
                indx5 = this.IndexCalc2("FI503_Ksh", coeffModel);
                indx6 = this.IndexCalc("OK_AI1103", runtimeModel);

                var paramFI503_Qm = 0.012522 * coeffModel[indx].Value * Math.Pow(coeffModel[indx2].Value, 2) * Math.Pow(coeffModel[indx3].Value, 2) *
                    coeffModel[indx4].Value * coeffModel[indx5].Value * Math.Sqrt(runtimeModel[indx6].Value * 100 * paramFI503_Ro) / 1000;

                liveTEP.SQLw_Data9 += paramFI503_Qm / 600;

                // -10--------------------------------------------------------------------------
                // Fводы прямой сетевой [нм3] FI504
                // { Плотность воды кг/м3 }
                indx = this.IndexCalc("OK_AI1001", runtimeModel);

                var paramFI504_Ro = Math.Pow(10, 9) / (998792.53 + (95.33246 * runtimeModel[indx].Value * 98.0665) + (3.4743522 * Math.Pow(runtimeModel[indx].Value * 98.0665, 2)));

                // { Массовый расход т/ч (основная формула) }
                indx = this.IndexCalc2("FI504_Alfa", coeffModel);
                indx2 = this.IndexCalc2("FI504_d20", coeffModel);
                indx3 = this.IndexCalc2("FI504_K0", coeffModel);
                indx4 = this.IndexCalc2("FI504_Kp", coeffModel);
                indx5 = this.IndexCalc2("FI504_Ksh", coeffModel);
                indx6 = this.IndexCalc("OK_AI1104", runtimeModel);

                var paramFI504_Qm = 0.012522 * coeffModel[indx].Value * Math.Pow(coeffModel[indx2].Value, 2) * Math.Pow(coeffModel[indx3].Value, 2) *
                    coeffModel[indx4].Value * coeffModel[indx5].Value * Math.Sqrt(runtimeModel[indx6].Value * 100 * paramFI504_Ro) / 1000;

                liveTEP.SQLw_Data10 += paramFI504_Qm / 600;

                // -11--------------------------------------------------------------------------
                // Fгаза на вх. в котельн. [нм3] FI505
                indx = this.IndexCalc("OK_AI1106", runtimeModel);
                indx2 = this.IndexCalc2("Pbar", coeffModel);
                double paramFI505_P = (runtimeModel[indx].Value / 100) + coeffModel[indx2].Value;

                // { Плотность газа кг/м3 }
                indx = this.IndexCalc2("FI505_Roc", coeffModel);
                indx2 = this.IndexCalc("OK_AI1105", runtimeModel);
                indx3 = this.IndexCalc2("FI505_K", coeffModel);
                var paramFI505_Ro = 283.73 * coeffModel[indx].Value * paramFI505_P / ((runtimeModel[indx2].Value + 273.15) * coeffModel[indx3].Value);

                // { Коэф. расширения топливного газа }
                indx = this.IndexCalc2("FI505_d", coeffModel);
                indx2 = this.IndexCalc2("FI505_Dt", coeffModel);
                double paramFI505_Betta = coeffModel[indx].Value / coeffModel[indx2].Value;

                indx = this.IndexCalc("OK_AI1107", runtimeModel);
                indx2 = this.IndexCalc2("FI505_Ksi", coeffModel);
                var paramFI505_Eps = 1 - ((0.41 + (0.35 * Math.Pow(paramFI505_Betta, 4))) * runtimeModel[indx].Value * 100 / (10000 * paramFI505_P * coeffModel[indx2].Value));

                // { Объемный расход нм3/ч }
                indx = this.IndexCalc2("FI505_Alfa", coeffModel);
                indx2 = this.IndexCalc2("FI505_d20", coeffModel);
                indx3 = this.IndexCalc2("FI505_K0", coeffModel);
                indx4 = this.IndexCalc2("FI505_Kp", coeffModel);
                indx5 = this.IndexCalc2("FI505_Ksh", coeffModel);
                indx6 = this.IndexCalc("OK_AI1107", runtimeModel);
                indx7 = this.IndexCalc2("FI505_Roc", coeffModel);

                var paramFI505_Qc = 0.012522 * coeffModel[indx].Value * paramFI505_Eps * Math.Pow(coeffModel[indx2].Value, 2) * Math.Pow(coeffModel[indx3].Value, 2) *
                    coeffModel[indx4].Value * coeffModel[indx5].Value * Math.Sqrt(runtimeModel[indx6].Value * 100 * paramFI505_Ro) / coeffModel[indx7].Value;

                liveTEP.SQLw_Data11 += paramFI505_Qc / 600;

                // -12--------------------------------------------------------------------------
                // Eтепла 3-го котла
                indx = this.IndexCalc("K1_Fsv", runtimeModel);
                indx2 = this.IndexCalc("K1_Tsv_out", runtimeModel);
                indx3 = this.IndexCalc("K1_Tsv_in", runtimeModel);
                double paramEt_K3 = runtimeModel[indx].Value * (runtimeModel[indx2].Value - runtimeModel[indx3].Value) / 1000;

                liveTEP.SQLw_Data12 += paramEt_K3 / 600;

                // -13--------------------------------------------------------------------------
                // Количество газа УВП [тыс. нм3]
                indx = this.IndexCalc("OK_UVP_Q", runtimeModel);

                if (liveTEP.OK_UVP_Q_old == 0)
                {
                    liveTEP.OK_UVP_Q_old = runtimeModel[indx].Value;
                }

                if (runtimeModel[indx].Value < liveTEP.OK_UVP_Q_old)
                {
                    liveTEP.OK_UVP_Q_old = runtimeModel[indx].Value;
                }

                liveTEP.SQLw_Data13 += runtimeModel[indx].Value - liveTEP.OK_UVP_Q_old;                

            return liveTEP;
        }

        private int IndexCalc(string name, IList<RuntimeModel> runtimeModel)
        {
            return runtimeModel.IndexOf(runtimeModel.FirstOrDefault(x => x.TagName == name));
        }

        private int IndexCalc2(string name, IList<CoeffModel> coeffModel)
        {
            return coeffModel.IndexOf(coeffModel.FirstOrDefault(x => x.TagName == name));
        }        
    }
}