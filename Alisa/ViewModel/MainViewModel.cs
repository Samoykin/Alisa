using Alisa.Model;
using Alisa.Utils;
using MVVM_test.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace Alisa.ViewModel
{
    class MainViewModel
    {
        #region Fields
        //конфигурация
        //String path = @"Config.xml";
        String path = @"Settings.xml";
        //XMLConfig xmlConf = new XMLConfig();
        //XMLFields xmlFields = new XMLFields(); //модель конфигурации

        RootElement settingsModel = new RootElement();

        //Теги
        ReadTextFile rf = new ReadTextFile(); //работа со списками тегов
        String tagPath = @"TagList.txt";
        String tags;
        //БД
        List<Single> tagValue = new List<Single>(); //теги в MSSQL Runtime
        RuntimeDB rDB;
        //Таймер
        DispatcherTimer t1 = new DispatcherTimer();
        DispatcherTimer t11 = new DispatcherTimer();
        DispatcherTimer t2 = new DispatcherTimer();
        DispatcherTimer t3 = new DispatcherTimer();
        DispatcherTimer tConnStatus = new DispatcherTimer();
        DispatcherTimer tSlaveWrite = new DispatcherTimer();
        DispatcherTimer tAccessReport = new DispatcherTimer();

        Boolean reportFlag = false;

        String DataBaseName = "DBTEP.sqlite"; //БД SQLite

        private ObservableCollection<RuntimeModel> _RtModel; //коллекция значений тегов из файла
        
        String tagPathCoeff = @"Coeff.txt"; //путь к файлу коэфициентов
        LogFile logFile = new LogFile(); //логи

        #endregion
        

        #region Properties

        public TEPModel TEP { get; set; }
        public Filters Filters { get; set; }
        public RuntimeModel RtModel { get; set; }
        public LiveTEP liveTEP { get; set; }
        public LiveTEP TEPtoBase { get; set; }
        public HistTEP htep { get; set; }
        public ObservableCollection<HistTEP> histTEP { get; set; }
        public CoeffModel cModel { get; set; }
        public ObservableCollection<CoeffModel> _coeffModel { get; set; }

        public Misc Misc { get; set; }

        #endregion     

        public MainViewModel()
        {
            String logText = DateTime.Now.ToString() + "|event| |Запуск приложения Alisa";
            logFile.WriteLog(logText);

            ClickCommand = new Command(arg => ClickMethod());
            ClickCommand2 = new Command(arg => ClickMethod2());
            ClickApplyFilter = new Command(arg => ApplyFilter());
            ClickCommand4 = new Command(arg => ClickMethod4());
            ClickCommand5 = new Command(arg => ClickMethod5());
            ClickCommand6 = new Command(arg => ClickMethod6()); //закрыть окно

            TEP = new TEPModel { };
            Misc = new Misc { };

            //Вычитывание параметров из XML
            //Инициализация модели настроек
            SettingsXML<RootElement> settingsXML = new SettingsXML<RootElement>(path);
            settingsModel.MSSQL = new MSSQL();
            settingsModel.SQLite = new SQLite();
            settingsModel.Mail = new Mail();
            settingsModel.Mail.To = new List<string>();
            settingsModel.Reserv = new Reserv();

            if (!File.Exists(path))
            {
                settingsModel = SetDefaultValue(settingsModel); //значения по умолчанию
                settingsXML.WriteXml(settingsModel);
            }
            else
            {
                settingsModel = settingsXML.ReadXml(settingsModel);
            }

            







            //xmlFields = xmlConf.ReadXmlConf(path);

            rDB = new RuntimeDB(settingsModel.MSSQL);

            if (settingsModel.Reserv.Master)
                Misc.Master = "Master";
            else
                Misc.Master = "Slave";

            //вычитывание списка тегов из файла
            tags = rf.readFile(tagPath);

            //Коэффициенты  
            _coeffModel = new ObservableCollection<CoeffModel> { };
            _coeffModel = rf.readCoeff(tagPathCoeff);            

            //таймер вычитывания значений из БД
            t1.Interval = new TimeSpan(0, 0, 2);
            t1.Tick += new EventHandler(timer_Tick);
            t1.Start();

            //расчет ТЭП
            t11.Interval = new TimeSpan(0, 0, 6);
            t11.Tick += new EventHandler(timer_Tick_Calc);
            t11.Start();

            //таймер на вызов метода записи 2-х часовок
            t2.Interval = new TimeSpan(0, 1, 0);
            t2.Tick += new EventHandler(timer_Tick2);
            t2.Start();

            //таймер на отправку писем
            t3.Interval = new TimeSpan(0, 1, 0);
            t3.Tick += new EventHandler(timer_Tick3);
            t3.Start();

            //таймер проверки связи с БД MSSQL
            tConnStatus.Interval = new TimeSpan(0, 0, 1);
            tConnStatus.Tick += new EventHandler(timerConnStatus);
            tConnStatus.Start();

            //таймер на доступ к записи отчета
            tAccessReport.Interval = new TimeSpan(0, 2, 0);
            tAccessReport.Tick += new EventHandler(timerAccessReport);
            tAccessReport.Start();

            liveTEP = new LiveTEP { };
            TEPtoBase = new LiveTEP { };
            Filters = new Filters {  };
            
            Filters.StartDate = DateTime.Now.Subtract(new TimeSpan(1, 0, 0, 0));
            Filters.EndDate = DateTime.Now;

            histTEP = new ObservableCollection<HistTEP> { };

            Filters.onCount += Filter;
            Filters.Day = true;
            ApplyFilter();
        }

        #region Commands
        public ICommand ClickCommand { get; set; }
        public ICommand ClickCommand2 { get; set; }
        public ICommand ClickApplyFilter { get; set; }
        public ICommand ClickCommand4 { get; set; }
        public ICommand ClickCommand5 { get; set; }
        public ICommand ClickCommand6 { get; set; }


        #endregion

        #region Methods

        void timer_Tick(object sender, EventArgs e)
        {            
            ReadData();            
        }
        
        private async void ReadData()
        {
            try
            {                
                _RtModel = new ObservableCollection<RuntimeModel>();


                _RtModel = await Task<ObservableCollection<RuntimeModel>>.Factory.StartNew(() =>
                {
                    return rDB.DataReadTest(tags, _RtModel);
                });
                               
            }
            catch (Exception e)
            {
                String logText = DateTime.Now.ToString() + "|fail|MainViewModel - ReadData|" + e.Message;
                logFile.WriteLog(logText);
            }
        }

        void timer_Tick_Calc(object sender, EventArgs e)
        {
            CalcData();
        }


        private void CalcData()
        {
            try
            {
                CalculateTEP clcTEP = new CalculateTEP();

                liveTEP = clcTEP.Calculate(liveTEP, _RtModel, _coeffModel);

                Int32 indx = IndexCalc("OK_UVP_Q", _RtModel);

                liveTEP.OK_UVP_Q_old = _RtModel[indx].Value;

            }
            catch (Exception e)
            {
                String logText = DateTime.Now.ToString() + "|fail|MainViewModel - CalcData|" + e.Message;
                logFile.WriteLog(logText);
            }
        }

        private Int32 IndexCalc(String name, ObservableCollection<RuntimeModel> _RtModel)
        {
            Int32 indx;
            indx = _RtModel.IndexOf(_RtModel.Where(X => X.TagName == name).FirstOrDefault());
            return indx;
        }


        private Int32 IndexCalc(String name)
        {
            Int32 indx;
            indx = _RtModel.IndexOf(_RtModel.Where(X => X.TagName == name).FirstOrDefault());
            return indx;
        }


        void timer_Tick2(object sender, EventArgs e)
        {
            Decimal hour = DateTime.Now.Hour;
            Decimal minute = DateTime.Now.Minute;

            if (minute == 0)
            {
                if (hour == Math.Floor(hour / 2) * 2 + 1 && reportFlag==true)
                {
                    SQLiteDB sqliteDB = new SQLiteDB(settingsModel.SQLite);
                    if (!File.Exists(DataBaseName))
                    {
                        sqliteDB.CreateBase();
                    }

                    sqliteDB.TEPCreateTable();
                    sqliteDB.TEPWrite(liveTEP);

                    TEPtoBase = liveTEP;

                    if (Misc.Master == "Master" && Misc.MSSQLStatus == "")
                    {
                        Boolean lastReport = rDB.DataReadLastReport(hour);
                        if (lastReport == false)
                            rDB.DataWrite(TEPtoBase);
                    }
                    else if (Misc.Master == "Slave")
                    {
                        //таймер проверки записи отчета Masterom
                        tSlaveWrite.Interval = new TimeSpan(0, 0, 2);
                        tSlaveWrite.Tick += new EventHandler(timerSlaveWrite);
                        tSlaveWrite.Start();
                    }

                    liveTEP.SQLw_Data1 = 0;
                    liveTEP.SQLw_Data2 = 0;
                    liveTEP.SQLw_Data3 = 0;
                    liveTEP.SQLw_Data4 = 0;
                    liveTEP.SQLw_Data5 = 0;
                    liveTEP.SQLw_Data6 = 0;
                    liveTEP.SQLw_Data7 = 0;
                    liveTEP.SQLw_Data8 = 0;
                    liveTEP.SQLw_Data9 = 0;
                    liveTEP.SQLw_Data10 = 0;
                    liveTEP.SQLw_Data11 = 0;
                    liveTEP.SQLw_Data12 = 0;
                    liveTEP.SQLw_Data13 = 0;
                }
            }

        }

        void timerSlaveWrite(object sender, EventArgs e)
        {
            Decimal hour = DateTime.Now.Hour;
            if (Misc.MSSQLStatus == "")
            {
                Boolean lastReport = rDB.DataReadLastReport(hour);
                if (lastReport == false)
                {
                    rDB.DataWrite(TEPtoBase);
                }
                tSlaveWrite.Stop();
            }
        }

        void timerAccessReport(object sender, EventArgs e)
        {
            reportFlag = true;
            tAccessReport.Stop();
        }

        void timer_Tick3(object sender, EventArgs e)
        {
            Decimal hour = DateTime.Now.Hour;
            Decimal minute = DateTime.Now.Minute;

            if (minute == 0)
            {
                if (hour == 6)
                {
                    //выбираем данные за сутки
                    Filters.Day = true;
                    ApplyFilter();
                    //сохраняем отчет в csv
                    TEPToExcel tepToExcel = new TEPToExcel();
                    tepToExcel.saveData(histTEP);

                    //отправка письма
                    String date = DateTime.Now.ToString("yyyy.MM.dd");
                    String att = Directory.GetCurrentDirectory() + @"\TEP\TEP_" + date + ".xlsx";
                    String subject = "Отчет ТЭП " + date;
                    String body = "<h3>Суточный отчет ТЭП Котельной, площадка СБНПУ " + date + "</h3><br>" +
                        "---------------------------------------<br>" +
                        "Элком+, Алиса<br>" +
                        "тел./факс (3822) 522-511<br>";
                    SendMail(subject, body, att, false);

                    //отправка письма в Элком с логами
                    String log = Directory.GetCurrentDirectory() + @"\log.txt";
                    String log_temp = Directory.GetCurrentDirectory() + @"\log_temp.txt";

                    File.Delete(log_temp);
                    File.Copy(log, log_temp);

                    att = Directory.GetCurrentDirectory() + @"\log_temp.txt";
                    subject = "Логи " + date;
                    body = "<h3>Логи " + date + "</h3><br>" +
                        "---------------------------------------<br>" +
                        "Элком+, Алиса<br>";
                    SendMail(subject, body, att, true);

                }
            }

        }

        void timerConnStatus(object sender, EventArgs e)
        {
            ConnStatus();
        }

        private async void ConnStatus()
        {
            Boolean status = false;
            try
            {
                status = await Task<Boolean>.Factory.StartNew(() =>
                {
                    return rDB.CheckMSSQLConn();
                });
            }
            catch(Exception)
            {
                //String logText = DateTime.Now.ToString() + "|fail|MainViewModel - ConnStatus|" + ex.Message;
                //logFile.WriteLog(logText);
            }

            if (!status)
                Misc.MSSQLStatus = "| MSSQL - нет связи";
            else
                Misc.MSSQLStatus = "";
        }

        public void Filter()
        {
            DateTime dt = Filters.StartDate;
            try
            {
                if (Filters.Day)
                {
                    Filters.StartDate = new DateTime(dt.Year, dt.Month, dt.Day, 5, 00, 00);
                    Filters.EndDate = Filters.StartDate.Subtract(new TimeSpan(-1, 0, 0, 0));
                }
                if (Filters.FirstShift)
                {
                    Filters.StartDate = new DateTime(dt.Year, dt.Month, dt.Day, 9, 00, 00);
                    Filters.EndDate = Filters.StartDate.Subtract(new TimeSpan(0, -12, 0, 0));
                }
                if (Filters.SecondShift)
                {
                    Filters.StartDate = new DateTime(dt.Year, dt.Month, dt.Day, 21, 00, 00);
                    Filters.EndDate = Filters.StartDate.Subtract(new TimeSpan(0, -12, 0, 0));
                }
                if (Filters.Month)
                {
                    Filters.StartDate = new DateTime(dt.Year, dt.Month, dt.Day, 5, 00, 00);
                    Filters.EndDate = Filters.StartDate.Subtract(new TimeSpan(-30, 0, 0, 0));
                }
            }
            catch (Exception e) 
            {
                String logText = DateTime.Now.ToString() + "|fail|MainViewModel - Filter|" + e.Message;
                logFile.WriteLog(logText);
            }


        }


        //------------------------------------------
        //Кнопки
        //------------------------------------------
        
        //Применение выбранного фильтра на "Главной"
        private void ApplyFilter()
        {
            Filter();

            ObservableCollection<HistTEP>  histTEP2 = new ObservableCollection<HistTEP> { };
            SQLiteDB sqliteDB = new SQLiteDB(settingsModel.SQLite);

            try
            {
                histTEP2 = sqliteDB.TEPRead(Filters.StartDate, Filters.EndDate);

                histTEP.Clear();
                htep = new HistTEP { };
                foreach (HistTEP ht in histTEP2)
                {
                    htep.DateTimeTEP = DateTime.Now;
                    htep.SQLw_Data1 += ht.SQLw_Data1;
                    htep.SQLw_Data2 += ht.SQLw_Data2;
                    htep.SQLw_Data3 += ht.SQLw_Data3;
                    htep.SQLw_Data4 += ht.SQLw_Data4;
                    htep.SQLw_Data5 += ht.SQLw_Data5;
                    htep.SQLw_Data6 += ht.SQLw_Data6;
                    htep.SQLw_Data7 += ht.SQLw_Data7;
                    htep.SQLw_Data8 += ht.SQLw_Data8;
                    htep.SQLw_Data9 += ht.SQLw_Data9;
                    htep.SQLw_Data10 += ht.SQLw_Data10;
                    htep.SQLw_Data11 += ht.SQLw_Data11;
                    htep.SQLw_Data12 += ht.SQLw_Data12;
                    htep.SQLw_Data13 += ht.SQLw_Data13;
                    histTEP.Add(ht);
                }
                histTEP.Add(htep);
            }
            catch (Exception e)
            {
                String logText = DateTime.Now.ToString() + "|fail|MainViewModel - ClickMethod3|" + e.Message;
                logFile.WriteLog(logText);
            }
                          
        }

        //читает с БД
        private void ClickMethod()
        {
            //ReadData();
            RuntimeDB rtDB = new RuntimeDB(settingsModel.MSSQL);
            //rtDB.DataWrite(liveTEP, xmlFields);
            //rtDB.CheckMSSQLConn();

            Decimal hour = 7;
            if (Misc.Master == "Master" && Misc.MSSQLStatus == "")
            {
                Boolean lastReport = rDB.DataReadLastReport(hour);
                if (lastReport == false)
                    rDB.DataWrite(liveTEP);
            }

        }

        //создает БД, таблицу и делает запись
        private void ClickMethod2()
        {
            //String DataBaseName = "DBTEP.sqlite";
            //SQLiteDB sqliteDB = new SQLiteDB(xmlFields);
            //if (!File.Exists(DataBaseName))
            //{
            //    sqliteDB.CreateBase();
            //}

            //sqliteDB.TEPCreateTable();
            //sqliteDB.TEPWrite(liveTEP);

            


        }

        public RootElement SetDefaultValue(RootElement set)
        {
            set.MSSQL.Server = "192.168.8.224";
            set.MSSQL.DBName = "Runtime";
            set.MSSQL.Login = "sa";
            set.MSSQL.Pass = "sa";

            set.SQLite.DBName = "DBTEP.sqlite";
            set.SQLite.Pass = "";

            set.Mail.SmtpServer = "sdsdsds";
            set.Mail.Port = 25;
            set.Mail.Login = "SamoykinAA";
            set.Mail.Pass = "Ghjcnj_Gfhjkm3";
            set.Mail.From = "SamoykinAA@elcomplus.ru";
            set.Mail.To.Add("SamoykinAA@elcomplus.ru");
            set.Mail.ServiceTo = "";

            set.Reserv.Master = true;

            return set;
        }

        //запись в лог и отсылка сообщения с логами
        private void ClickMethod4()
        {
            //String log = Directory.GetCurrentDirectory() + @"\log.txt";
            //String log_temp = Directory.GetCurrentDirectory() + @"\log_temp.txt";

            //File.Delete(log_temp);

            //File.Copy(log, log_temp);
            //SendMail("Тема3", "Привет3", log_temp);


            //выбираем данные за сутки
            Filters.Day = true;
            ApplyFilter();
            //сохраняем отчет в csv
            TEPToExcel tepToExcel = new TEPToExcel();
            tepToExcel.saveData(histTEP);

            //отправка письма
            String date = DateTime.Now.ToString("yyyy.MM.dd");
            String att = Directory.GetCurrentDirectory() + @"\TEP\TEP_" + date + ".xlsx";
            String subject = "Отчет ТЭП " + date;
            String body = "<h2>Отчет ТЭП " + date + "</h2><br>" +
                "---------------------------------------<br>" +
                "Элком+, Алиса<br>" +
                "тел./факс (3822) 522-511<br>";
            SendMail(subject, body, att, false);



            //отправка письма в Элком с логами
            String log = Directory.GetCurrentDirectory() + @"\log.txt";
            String log_temp = Directory.GetCurrentDirectory() + @"\log_temp.txt";

            File.Delete(log_temp);
            File.Copy(log, log_temp);

            att = Directory.GetCurrentDirectory() + @"\log_temp.txt";
            subject = "Логи " + date;
            body = "<h2>Логи " + date + "</h2><br>" +
                "---------------------------------------<br>" +
                "Элком+, Алиса<br>";
            SendMail(subject, body, att, true);





        }

      

        private async void SendMail(String subject, String body, String att, Boolean service)
        {
            TEPMail tepMail = new TEPMail();
            try
            {
                await Task.Factory.StartNew(() =>
                {
                    tepMail.SendMail(settingsModel.Mail, subject, body, att, service);
                });
            }
            catch (Exception e)
            {
                String logText = DateTime.Now.ToString() + "|fail|MainViewModel - SendMail|" + e.Message;
                logFile.WriteLog(logText);
            }
        }


        private void ClickMethod5()
        {
            //выбираем данные за сутки
            Filters.Day = true;
            ApplyFilter();

            // TEPToCSV tepToCSV = new TEPToCSV();
            TEPToExcel tepToExcel = new TEPToExcel();
            tepToExcel.saveData(histTEP);
        }

        private void ClickMethod6()
        {
            
        }




        #endregion

        

    }


  


}
