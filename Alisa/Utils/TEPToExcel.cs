using Alisa.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

namespace Alisa.Utils
{
    class TEPToExcel
    {
        private LogFile logFile = new LogFile();
        private String filePath = Directory.GetCurrentDirectory() + @"\TEP\TEP_" + DateTime.Now.ToString("yyyy.MM.dd") + ".xlsx";

        private Excel.Application ExcelApp;
        private Excel.Workbook WorkBookExcel;
        private Excel.Worksheet WorkSheetExcel;
        private Excel.Range RangeExcel;


        public void saveData(ObservableCollection<HistTEP> histTEP)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            try
            {
                ExcelApp = new Excel.Application();
                ExcelApp.Visible = true;
                ExcelApp.Workbooks.Add();
                WorkSheetExcel = (Excel.Worksheet)ExcelApp.ActiveSheet;

                WorkSheetExcel.Cells[1, 1] = "Дата/Время";
                WorkSheetExcel.Cells[1, 2] = "K1 - Fгаза[м3], 1-FI501";
                WorkSheetExcel.Cells[1, 3] = "K2 - Fгаза[м3], 2-FI501";
                WorkSheetExcel.Cells[1, 4] = "K3 - Fгаза[м3], 3-FI501";
                WorkSheetExcel.Cells[1, 5] = "K3 - Fводы[нм3],3-FI502";
                WorkSheetExcel.Cells[1, 6] = "Fпара от котл[т/ч], FI502";
                WorkSheetExcel.Cells[1, 7] = "Етепла от котл [Гкал]";
                WorkSheetExcel.Cells[1, 8] = "Fпара на уст [т/ч], FI507";
                WorkSheetExcel.Cells[1, 9] = "Етепла на уст. [Гкал]";
                WorkSheetExcel.Cells[1, 10] = "Fводы на подп [нм3], FI503";
                WorkSheetExcel.Cells[1, 11] = "Fводы прямой сет[нм3], FI504";
                WorkSheetExcel.Cells[1, 12] = "Fгаза на вх. кот[нм3], FI505";
                WorkSheetExcel.Cells[1, 13] = "Етепла 3-го котла [Гкал]";
                WorkSheetExcel.Cells[1, 14] = "Кол-во газа (УВП)[тыс. нм3]";

                int i = 0;

                foreach (HistTEP record in histTEP)
                {
                    RangeExcel = (Excel.Range)WorkSheetExcel.Cells[i + 2, 1];
                    RangeExcel.Value = histTEP[i].DateTimeTEP;
                    RangeExcel = (Excel.Range)WorkSheetExcel.Cells[i + 2, 2];
                    RangeExcel.Value = histTEP[i].SQLw_Data1;
                    RangeExcel = (Excel.Range)WorkSheetExcel.Cells[i + 2, 3];
                    RangeExcel.Value = histTEP[i].SQLw_Data2;
                    RangeExcel = (Excel.Range)WorkSheetExcel.Cells[i + 2, 4];
                    RangeExcel.Value = histTEP[i].SQLw_Data3;
                    RangeExcel = (Excel.Range)WorkSheetExcel.Cells[i + 2, 5];
                    RangeExcel.Value = histTEP[i].SQLw_Data4;
                    RangeExcel = (Excel.Range)WorkSheetExcel.Cells[i + 2, 6];
                    RangeExcel.Value = histTEP[i].SQLw_Data5;
                    RangeExcel = (Excel.Range)WorkSheetExcel.Cells[i + 2, 7];
                    RangeExcel.Value = histTEP[i].SQLw_Data6;
                    RangeExcel = (Excel.Range)WorkSheetExcel.Cells[i + 2, 8];
                    RangeExcel.Value = histTEP[i].SQLw_Data7;
                    RangeExcel = (Excel.Range)WorkSheetExcel.Cells[i + 2, 9];
                    RangeExcel.Value = histTEP[i].SQLw_Data8;
                    RangeExcel = (Excel.Range)WorkSheetExcel.Cells[i + 2, 10];
                    RangeExcel.Value = histTEP[i].SQLw_Data9;
                    RangeExcel = (Excel.Range)WorkSheetExcel.Cells[i + 2, 11];
                    RangeExcel.Value = histTEP[i].SQLw_Data10;
                    RangeExcel = (Excel.Range)WorkSheetExcel.Cells[i + 2, 12];
                    RangeExcel.Value = histTEP[i].SQLw_Data11;
                    RangeExcel = (Excel.Range)WorkSheetExcel.Cells[i + 2, 13];
                    RangeExcel.Value = histTEP[i].SQLw_Data12;
                    RangeExcel = (Excel.Range)WorkSheetExcel.Cells[i + 2, 14];
                    RangeExcel.Value = histTEP[i].SQLw_Data13;
                                       
                    i++;
                }

                WorkSheetExcel.SaveAs(filePath);
                ExcelApp.Quit();
                GC.Collect();



                String logText = DateTime.Now.ToString() + "|event|TEPToExcel - saveData|Отчет сохранен в Excel " + filePath;
                logFile.WriteLog(logText);

            }
            catch (Exception exception)
            {
                String logText = DateTime.Now.ToString() + "|fail|TEPToExcel - saveData|" + exception.Message;
                logFile.WriteLog(logText);

            }
        }

    }
}
