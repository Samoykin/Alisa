namespace Alisa.Utils
{
    using System;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Text;    
    using Model;
    using NLog;

    /// <summary>Сохранение ТЭП в csv.</summary>
    public class TEPToCSV
    {
        private Logger logger = LogManager.GetCurrentClassLogger();
        private string filePath = Directory.GetCurrentDirectory() + @"\TEP\TEP_" + DateTime.Now.ToString("yyyy.MM.dd") + ".csv";

        /// <summary>Сохранить.</summary>
        /// <param name="histTEP">Отчет ТЭП.</param>
        public void SaveData(ObservableCollection<HistTEP> histTEP)
        {
            if (File.Exists(this.filePath))
            {
                File.Delete(this.filePath);
            }

                var sw = new StreamWriter(this.filePath, false, Encoding.Unicode);

                sw.WriteLine("Дата/Время; K1 - Fгаза[м3], 1-FI501; K2 - Fгаза[м3], 2-FI501;" +
                    "K3 - Fгаза[м3], 3-FI501; K3 - Fводы[нм3],3-FI502; Fпара от котл[т/ч], FI502;" +
                    "Етепла от котл [Гкал]; Fпара на уст [т/ч], FI507; Етепла на уст. [Гкал];" +
                    "Fводы на подп [нм3], FI503; Fводы прямой сет[нм3], FI504; Fгаза на вх. кот[нм3], FI505;" +
                    "Етепла 3-го котла [Гкал]; Кол-во газа (УВП)[тыс. нм3];");
                var i = 0;

                foreach (var record in histTEP)
                {
                    sw.WriteLine(histTEP[i].DateTimeTEP + ";" + histTEP[i].SQLw_Data1 + ";" + histTEP[i].SQLw_Data2 + ";" + histTEP[i].SQLw_Data3 + ";" + histTEP[i].SQLw_Data4 + ";" +
                        histTEP[i].SQLw_Data5 + ";" + histTEP[i].SQLw_Data6 + ";" + histTEP[i].SQLw_Data7 + ";" + histTEP[i].SQLw_Data8 + ";" + histTEP[i].SQLw_Data9 + ";" + 
                        histTEP[i].SQLw_Data10 + ";" + histTEP[i].SQLw_Data11 + ";" + histTEP[i].SQLw_Data12 + ";" + histTEP[i].SQLw_Data13);

                    i++;
                }

                sw.Close();

                this.logger.Info("Отчет сохранен в csv " + this.filePath);
        }
    }
}