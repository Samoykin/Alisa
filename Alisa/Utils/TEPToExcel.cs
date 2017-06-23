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
        private Excel.Worksheet WorkSheetExcel;
        //private Excel.Range RangeExcel;


        public void saveData(ObservableCollection<HistTEP> histTEP)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                ExcelApp = new Excel.Application();
                ExcelApp.Visible = true;
                ExcelApp.Workbooks.Add();
                WorkSheetExcel = (Excel.Worksheet)ExcelApp.ActiveSheet;

                WorkSheetExcel.Cells[1, 1] = "Дата/Время";
                WorkSheetExcel.Cells[1, 2] = "K1 - Fгаза[м3], 1-FI501";
                WorkSheetExcel.Cells[1, 3] = "K2 - Fгаза[м3], 2-FI501";
                WorkSheetExcel.Cells[1, 4] = "K3 - Fгаза[м3], 3-FI501";
                WorkSheetExcel.Cells[1, 5] = "K3 - Fводы[нм3], 3-FI502";
                WorkSheetExcel.Cells[1, 6] = "Fпара от котл[т/ч], FI502";
                WorkSheetExcel.Cells[1, 7] = "Етепла от котл [Гкал]";
                WorkSheetExcel.Cells[1, 8] = "Fпара на уст [т/ч], FI507";
                WorkSheetExcel.Cells[1, 9] = "Етепла на уст. [Гкал]";
                WorkSheetExcel.Cells[1, 10] = "Fводы на подп [нм3], FI503";
                WorkSheetExcel.Cells[1, 11] = "Fводы прямой сет[нм3], FI504";
                WorkSheetExcel.Cells[1, 12] = "Fгаза на вх. кот[нм3], FI505";
                WorkSheetExcel.Cells[1, 13] = "Етепла 3-го котла [Гкал]";
                WorkSheetExcel.Cells[1, 14] = "Кол-во газа (УВП)[тыс. нм3]";

                (WorkSheetExcel.Cells[1, 1] as Excel.Range).EntireColumn.ColumnWidth = 15;
                (WorkSheetExcel.Cells[1, 2] as Excel.Range).EntireColumn.ColumnWidth = 14;
                (WorkSheetExcel.Cells[1, 3] as Excel.Range).EntireColumn.ColumnWidth = 14;
                (WorkSheetExcel.Cells[1, 4] as Excel.Range).EntireColumn.ColumnWidth = 14;
                (WorkSheetExcel.Cells[1, 5] as Excel.Range).EntireColumn.ColumnWidth = 15;
                (WorkSheetExcel.Cells[1, 6] as Excel.Range).EntireColumn.ColumnWidth = 15;
                (WorkSheetExcel.Cells[1, 7] as Excel.Range).EntireColumn.ColumnWidth = 15;
                (WorkSheetExcel.Cells[1, 8] as Excel.Range).EntireColumn.ColumnWidth = 15;
                (WorkSheetExcel.Cells[1, 9] as Excel.Range).EntireColumn.ColumnWidth = 15;
                (WorkSheetExcel.Cells[1, 10] as Excel.Range).EntireColumn.ColumnWidth = 15;
                (WorkSheetExcel.Cells[1, 11] as Excel.Range).EntireColumn.ColumnWidth = 15;
                (WorkSheetExcel.Cells[1, 12] as Excel.Range).EntireColumn.ColumnWidth = 15;
                (WorkSheetExcel.Cells[1, 13] as Excel.Range).EntireColumn.ColumnWidth = 15;
                (WorkSheetExcel.Cells[1, 14] as Excel.Range).EntireColumn.ColumnWidth = 15;

                (WorkSheetExcel.Cells[1, 2] as Excel.Range).WrapText = true;
                (WorkSheetExcel.Cells[1, 3] as Excel.Range).WrapText = true;
                (WorkSheetExcel.Cells[1, 4] as Excel.Range).WrapText = true;
                (WorkSheetExcel.Cells[1, 5] as Excel.Range).WrapText = true;
                (WorkSheetExcel.Cells[1, 6] as Excel.Range).WrapText = true;
                (WorkSheetExcel.Cells[1, 7] as Excel.Range).WrapText = true;
                (WorkSheetExcel.Cells[1, 8] as Excel.Range).WrapText = true;
                (WorkSheetExcel.Cells[1, 9] as Excel.Range).WrapText = true;
                (WorkSheetExcel.Cells[1, 10] as Excel.Range).WrapText = true;
                (WorkSheetExcel.Cells[1, 11] as Excel.Range).WrapText = true;
                (WorkSheetExcel.Cells[1, 12] as Excel.Range).WrapText = true;
                (WorkSheetExcel.Cells[1, 13] as Excel.Range).WrapText = true;
                (WorkSheetExcel.Cells[1, 14] as Excel.Range).WrapText = true;

                int k = 1;
                while (k < 15)
                {
                    (WorkSheetExcel.Cells[1, k] as Excel.Range).Interior.ColorIndex = 45;
                    k++;
                }

                (WorkSheetExcel.Cells[1, 1] as Excel.Range).EntireRow.RowHeight = 33;

                (WorkSheetExcel.Cells[1, 1] as Excel.Range).EntireColumn.VerticalAlignment = Excel.XlVAlign.xlVAlignTop;

                int i = 0;

                Int32 TEPCount = histTEP.Count();
                Object tt = 0;

                foreach (HistTEP record in histTEP)
                {
                    if (i == TEPCount-1)
                    {
                        tt = "Итого:";

                        int j = 1;
                        while (j < 15)
                        {
                            (WorkSheetExcel.Cells[i + 2, j] as Excel.Range).Interior.ColorIndex = 15;
                            j++;
                        }
                    }
                    else
                    {
                        tt = histTEP[i].DateTimeTEP;
                    }
                    WorkSheetExcel.Cells[i + 2, 1] = tt;
                    WorkSheetExcel.Cells[i + 2, 2] = histTEP[i].SQLw_Data1;

                    WorkSheetExcel.Cells[i + 2, 3] = histTEP[i].SQLw_Data2;
                    WorkSheetExcel.Cells[i + 2, 4] = histTEP[i].SQLw_Data3;
                    WorkSheetExcel.Cells[i + 2, 5] = histTEP[i].SQLw_Data4;
                    WorkSheetExcel.Cells[i + 2, 6] = histTEP[i].SQLw_Data5;
                    WorkSheetExcel.Cells[i + 2, 7] = histTEP[i].SQLw_Data6;
                    WorkSheetExcel.Cells[i + 2, 8] = histTEP[i].SQLw_Data7;
                    WorkSheetExcel.Cells[i + 2, 9] = histTEP[i].SQLw_Data8;
                    WorkSheetExcel.Cells[i + 2, 10] = histTEP[i].SQLw_Data9;
                    WorkSheetExcel.Cells[i + 2, 11] = histTEP[i].SQLw_Data10;
                    WorkSheetExcel.Cells[i + 2, 12] = histTEP[i].SQLw_Data11;
                    WorkSheetExcel.Cells[i + 2, 13] = histTEP[i].SQLw_Data12;
                    WorkSheetExcel.Cells[i + 2, 14] = histTEP[i].SQLw_Data13;


                    i++;
                }

                WorkSheetExcel.SaveAs(filePath);
                ExcelApp.Quit();
                GC.Collect();



                String logText = DateTime.Now.ToString() + "|event|TEPToExcel - saveData|Отчет сохранен в Excel " + filePath;
                logFile.WriteLog(logText);

            }
            catch (Exception e)
            {
                String logText = DateTime.Now.ToString() + "|fail|TEPToExcel - saveData|" + e.Message;
                logFile.WriteLog(logText);

                WorkSheetExcel.SaveAs(filePath);
                ExcelApp.Quit();
                GC.Collect();

            }
        }

    }
}
