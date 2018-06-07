namespace Alisa.Utils
{    
    using System;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;    
    using Model;
    using NLog;
    using Excel = Microsoft.Office.Interop.Excel;

    /// <summary>Сохранение ТЭП в Excel.</summary>
    public class TEPToExcel
    {
        private Logger logger = LogManager.GetCurrentClassLogger();
        private string filePath = $@"{Directory.GetCurrentDirectory()}\TEP\TEP_{DateTime.Now.ToString("yyyy.MM.dd")}.xlsx";

        private Excel.Application excelApp;
        private Excel.Worksheet workSheetExcel;

        /// <summary>Сохранить.</summary>
        /// <param name="histTEP">Отчет ТЭП.</param>
        public void SaveData(ObservableCollection<HistTEP> histTEP)
        {
            try
            {
                if (File.Exists(this.filePath))
                {
                    File.Delete(this.filePath);
                }

                this.excelApp = new Excel.Application();
                this.excelApp.Visible = true;
                this.excelApp.Workbooks.Add();
                this.workSheetExcel = (Excel.Worksheet)this.excelApp.ActiveSheet;

                this.workSheetExcel.Cells[1, 1] = "Дата/Время";
                this.workSheetExcel.Cells[1, 2] = "K1 - Fгаза[м3], 1-FI501";
                this.workSheetExcel.Cells[1, 3] = "K2 - Fгаза[м3], 2-FI501";
                this.workSheetExcel.Cells[1, 4] = "K3 - Fгаза[м3], 3-FI501";
                this.workSheetExcel.Cells[1, 5] = "K3 - Fводы[нм3], 3-FI502";
                this.workSheetExcel.Cells[1, 6] = "Fпара от котл[т/ч], FI502";
                this.workSheetExcel.Cells[1, 7] = "Етепла от котл [Гкал]";
                this.workSheetExcel.Cells[1, 8] = "Fпара на уст [т/ч], FI507";
                this.workSheetExcel.Cells[1, 9] = "Етепла на уст. [Гкал]";
                this.workSheetExcel.Cells[1, 10] = "Fводы на подп [нм3], FI503";
                this.workSheetExcel.Cells[1, 11] = "Fводы прямой сет[нм3], FI504";
                this.workSheetExcel.Cells[1, 12] = "Fгаза на вх. кот[нм3], FI505";
                this.workSheetExcel.Cells[1, 13] = "Етепла 3-го котла [Гкал]";
                this.workSheetExcel.Cells[1, 14] = "Кол-во газа (УВП)[тыс. нм3]";

                (this.workSheetExcel.Cells[1, 1] as Excel.Range).EntireColumn.ColumnWidth = 15;
                (this.workSheetExcel.Cells[1, 2] as Excel.Range).EntireColumn.ColumnWidth = 14;
                (this.workSheetExcel.Cells[1, 3] as Excel.Range).EntireColumn.ColumnWidth = 14;
                (this.workSheetExcel.Cells[1, 4] as Excel.Range).EntireColumn.ColumnWidth = 14;
                (this.workSheetExcel.Cells[1, 5] as Excel.Range).EntireColumn.ColumnWidth = 15;
                (this.workSheetExcel.Cells[1, 6] as Excel.Range).EntireColumn.ColumnWidth = 15;
                (this.workSheetExcel.Cells[1, 7] as Excel.Range).EntireColumn.ColumnWidth = 15;
                (this.workSheetExcel.Cells[1, 8] as Excel.Range).EntireColumn.ColumnWidth = 15;
                (this.workSheetExcel.Cells[1, 9] as Excel.Range).EntireColumn.ColumnWidth = 15;
                (this.workSheetExcel.Cells[1, 10] as Excel.Range).EntireColumn.ColumnWidth = 15;
                (this.workSheetExcel.Cells[1, 11] as Excel.Range).EntireColumn.ColumnWidth = 15;
                (this.workSheetExcel.Cells[1, 12] as Excel.Range).EntireColumn.ColumnWidth = 15;
                (this.workSheetExcel.Cells[1, 13] as Excel.Range).EntireColumn.ColumnWidth = 15;
                (this.workSheetExcel.Cells[1, 14] as Excel.Range).EntireColumn.ColumnWidth = 15;

                (this.workSheetExcel.Cells[1, 2] as Excel.Range).WrapText = true;
                (this.workSheetExcel.Cells[1, 3] as Excel.Range).WrapText = true;
                (this.workSheetExcel.Cells[1, 4] as Excel.Range).WrapText = true;
                (this.workSheetExcel.Cells[1, 5] as Excel.Range).WrapText = true;
                (this.workSheetExcel.Cells[1, 6] as Excel.Range).WrapText = true;
                (this.workSheetExcel.Cells[1, 7] as Excel.Range).WrapText = true;
                (this.workSheetExcel.Cells[1, 8] as Excel.Range).WrapText = true;
                (this.workSheetExcel.Cells[1, 9] as Excel.Range).WrapText = true;
                (this.workSheetExcel.Cells[1, 10] as Excel.Range).WrapText = true;
                (this.workSheetExcel.Cells[1, 11] as Excel.Range).WrapText = true;
                (this.workSheetExcel.Cells[1, 12] as Excel.Range).WrapText = true;
                (this.workSheetExcel.Cells[1, 13] as Excel.Range).WrapText = true;
                (this.workSheetExcel.Cells[1, 14] as Excel.Range).WrapText = true;

                var k = 1;
                while (k < 15)
                {
                    (this.workSheetExcel.Cells[1, k] as Excel.Range).Interior.ColorIndex = 45;
                    k++;
                }

                (this.workSheetExcel.Cells[1, 1] as Excel.Range).EntireRow.RowHeight = 33;

                (this.workSheetExcel.Cells[1, 1] as Excel.Range).EntireColumn.VerticalAlignment = Excel.XlVAlign.xlVAlignTop;

                var i = 0;

                object firstCol = 0;

                foreach (var record in histTEP)
                {
                    if (i == histTEP.Count() - 1)
                    {
                        firstCol = "Итого:";

                        var j = 1;
                        while (j < 15)
                        {
                            (this.workSheetExcel.Cells[i + 2, j] as Excel.Range).Interior.ColorIndex = 15;
                            j++;
                        }
                    }
                    else
                    {
                        firstCol = histTEP[i].DateTimeTEP;
                    }

                    this.workSheetExcel.Cells[i + 2, 1] = firstCol;
                    this.workSheetExcel.Cells[i + 2, 2] = histTEP[i].SQLw_Data1;

                    this.workSheetExcel.Cells[i + 2, 3] = histTEP[i].SQLw_Data2;
                    this.workSheetExcel.Cells[i + 2, 4] = histTEP[i].SQLw_Data3;
                    this.workSheetExcel.Cells[i + 2, 5] = histTEP[i].SQLw_Data4;
                    this.workSheetExcel.Cells[i + 2, 6] = histTEP[i].SQLw_Data5;
                    this.workSheetExcel.Cells[i + 2, 7] = histTEP[i].SQLw_Data6;
                    this.workSheetExcel.Cells[i + 2, 8] = histTEP[i].SQLw_Data7;
                    this.workSheetExcel.Cells[i + 2, 9] = histTEP[i].SQLw_Data8;
                    this.workSheetExcel.Cells[i + 2, 10] = histTEP[i].SQLw_Data9;
                    this.workSheetExcel.Cells[i + 2, 11] = histTEP[i].SQLw_Data10;
                    this.workSheetExcel.Cells[i + 2, 12] = histTEP[i].SQLw_Data11;
                    this.workSheetExcel.Cells[i + 2, 13] = histTEP[i].SQLw_Data12;
                    this.workSheetExcel.Cells[i + 2, 14] = histTEP[i].SQLw_Data13;
                    
                    i++;
                }

                this.workSheetExcel.SaveAs(this.filePath);
                this.excelApp.Quit();
                GC.Collect();

                this.logger.Info($"Отчет сохранен в Excel {this.filePath}");
            }
            catch (Exception ex)
            {
                this.logger.Error(ex.Message);

                this.workSheetExcel.SaveAs(this.filePath);
                this.excelApp.Quit();
                GC.Collect();
            }
        }
    }
}