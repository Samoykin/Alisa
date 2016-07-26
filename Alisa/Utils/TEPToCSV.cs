using Alisa.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Alisa.Utils
{
    class TEPToCSV
    {
        private LogFile logFile = new LogFile();
        private String filePath = Directory.GetCurrentDirectory() + @"\TEP\TEP_" + DateTime.Now.ToString("yyyy.MM.dd") + ".csv";

        public void saveData(ObservableCollection<HistTEP> histTEP)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            try
            {
                StreamWriter sw = new StreamWriter(filePath, false, Encoding.Unicode);

                sw.WriteLine("Дата/Время; K1 - Fгаза[м3], 1-FI501; K2 - Fгаза[м3], 2-FI501;" +
                    "K3 - Fгаза[м3], 3-FI501; K3 - Fводы[нм3],3-FI502; Fпара от котл[т/ч], FI502;" +
                    "Етепла от котл [Гкал]; Fпара на уст [т/ч], FI507; Етепла на уст. [Гкал];" +
                    "Fводы на подп [нм3], FI503; Fводы прямой сет[нм3], FI504; Fгаза на вх. кот[нм3], FI505;" +
                    "Етепла 3-го котла [Гкал]; Кол-во газа (УВП)[тыс. нм3];");
                Int32 i = 0;

                foreach (HistTEP record in histTEP)
                {
                    sw.WriteLine(histTEP[i].DateTimeTEP + ";" + histTEP[i].SQLw_Data1 + ";" + histTEP[i].SQLw_Data2 + ";" + histTEP[i].SQLw_Data3 + ";" + histTEP[i].SQLw_Data4 + ";" +
                        histTEP[i].SQLw_Data5 + ";" + histTEP[i].SQLw_Data6 + ";" + histTEP[i].SQLw_Data7 + ";" + histTEP[i].SQLw_Data8 + ";" + histTEP[i].SQLw_Data9 + ";" + 
                        histTEP[i].SQLw_Data10 + ";" + histTEP[i].SQLw_Data11 + ";" + histTEP[i].SQLw_Data12 + ";" + histTEP[i].SQLw_Data13);

                    i++;
                }

                sw.Close();

                String logText = DateTime.Now.ToString() + "|event|TEPToCSV - saveData|Отчет сохранен в csv " + filePath;
                logFile.WriteLog(logText);

            }
            catch (Exception exception)
            {
                String logText = DateTime.Now.ToString() + "|fail|TEPToCSV - saveData|" + exception.Message;
                logFile.WriteLog(logText);

            }
        }
    }
}
