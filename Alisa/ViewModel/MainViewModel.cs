namespace Alisa.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using System.Windows.Threading;    
    using Model;
    using NLog;
    using Utils;
    using static Model.Shell;    

    /// <summary>Главная ViewModel.</summary>
    public class MainViewModel
    {
        #region Fields
        private const string SettingsPath = "Settings.xml";
        private const string TagPath = "TagList.txt";
        private const string DataBaseName = "DBTEP.sqlite"; // БД SQLite
        private const string CoeffPath = "Coeff.txt"; // путь к файлу коэфициентов

        private Logger logger = LogManager.GetCurrentClassLogger();

        // конфигурация        
        private RootElement settingsModel = new RootElement();

        // Теги
        private ReadTextFile rf = new ReadTextFile(); // работа со списками тегов        
        private string tags;

        // БД
        private List<float> tagValue = new List<float>(); // теги в MSSQL Runtime
        private RuntimeDB rDB;

        // Таймер
        private DispatcherTimer timer1 = new DispatcherTimer();
        private DispatcherTimer timer11 = new DispatcherTimer();
        private DispatcherTimer timer2 = new DispatcherTimer();
        private DispatcherTimer timer3 = new DispatcherTimer();
        private DispatcherTimer timerConnStatus = new DispatcherTimer();
        private DispatcherTimer timerSlaveWrite = new DispatcherTimer();
        private DispatcherTimer timerAccessReport = new DispatcherTimer();

        private bool reportFlag = false;        
        private ObservableCollection<RuntimeModel> runtimeModels; // коллекция значений тегов из файла        

        #endregion

        /// <summary>Initializes a new instance of the <see cref="MainViewModel" /> class.</summary>
        public MainViewModel()
        {
            this.logger.Info("Запуск приложения Alisa");

            try
            {
                this.ClickCommand = new Command(arg => this.ClickMethod());
                this.ClickCommand2 = new Command(arg => this.ClickMethod2());
                this.ClickApplyFilter = new Command(arg => this.ApplyFilter());
                this.ClickCommand4 = new Command(arg => this.ClickMethod4());
                this.ClickCommand5 = new Command(arg => this.ClickMethod5());

                this.TEP = new TEPModel { };
                this.Misc = new Misc { };

                // Вычитывание параметров из XML
                // Инициализация модели настроек
                var settingsXML = new SettingsXML<RootElement>(SettingsPath);
                this.settingsModel.MSSQL = new MSSQL();
                this.settingsModel.SQLite = new SQLite();
                this.settingsModel.Mail = new Mail();
                this.settingsModel.Mail.To = new List<string>();
                this.settingsModel.Reserv = new Reserv();

                if (!File.Exists(SettingsPath))
                {
                    this.settingsModel = this.SetDefaultValue(this.settingsModel); // значения по умолчанию
                    settingsXML.WriteXml(this.settingsModel);
                }
                else
                {
                    this.settingsModel = settingsXML.ReadXml(this.settingsModel);
                }

                this.rDB = new RuntimeDB(this.settingsModel.MSSQL);

                if (this.settingsModel.Reserv.Master)
                {
                    Misc.Master = "Master";
                }
                else
                {
                    Misc.Master = "Slave";
                }

                // вычитывание списка тегов из файла
                this.tags = this.rf.ReadFile(TagPath);

                // Коэффициенты  
                this.CoeffModels = new ObservableCollection<CoeffModel> { };
                this.CoeffModels = this.rf.ReadCoeff(CoeffPath);

                // таймер вычитывания значений из БД
                this.timer1.Interval = new TimeSpan(0, 0, 2);
                this.timer1.Tick += new EventHandler(this.Timer_Tick);
                this.timer1.Start();

                // расчет ТЭП
                this.timer11.Interval = new TimeSpan(0, 0, 6);
                this.timer11.Tick += new EventHandler(this.Timer_Tick_Calc);
                this.timer11.Start();

                // таймер на вызов метода записи 2-х часовок
                this.timer2.Interval = new TimeSpan(0, 1, 0);
                this.timer2.Tick += new EventHandler(this.Timer_Tick2);
                this.timer2.Start();

                // таймер на отправку писем
                this.timer3.Interval = new TimeSpan(0, 1, 0);
                this.timer3.Tick += new EventHandler(this.Timer_Tick3);
                this.timer3.Start();

                // таймер проверки связи с БД MSSQL
                this.timerConnStatus.Interval = new TimeSpan(0, 0, 1);
                this.timerConnStatus.Tick += new EventHandler(this.TimerConnStatus);
                this.timerConnStatus.Start();

                // таймер на доступ к записи отчета
                this.timerAccessReport.Interval = new TimeSpan(0, 2, 0);
                this.timerAccessReport.Tick += new EventHandler(this.TimerAccessReport);
                this.timerAccessReport.Start();

                this.LiveTEP = new LiveTEP { };
                this.TEPtoBase = new LiveTEP { };
                this.Filters = new Filters { };

                this.Filters.StartDate = DateTime.Now.Subtract(new TimeSpan(1, 0, 0, 0));
                this.Filters.EndDate = DateTime.Now;

                this.HistTEP = new ObservableCollection<HistTEP> { };

                this.Filters.OnCount += this.Filter;
                this.Filters.Day = true;
                this.ApplyFilter();
            }
            catch (Exception ex)
            {
                this.logger.Error(ex.Message);
            }
        }

        #region Properties

        /// <summary>Модель ТЭП.</summary>
        public TEPModel TEP { get; set; }

        /// <summary>Фильтры.</summary>
        public Filters Filters { get; set; }

        /// <summary>Модель текущих значений.</summary>
        public RuntimeModel RuntimeModel { get; set; }

        /// <summary>Текущие значения.</summary>
        public LiveTEP LiveTEP { get; set; }

        /// <summary>Отчеты ТЭП.</summary>
        public LiveTEP TEPtoBase { get; set; }

        /// <summary>Исторические отчеты ТЭП.</summary>
        public HistTEP Htep { get; set; }

        /// <summary>Коллекция исторических отчетов ТЭП.</summary>
        public ObservableCollection<HistTEP> HistTEP { get; set; }

        /// <summary>Модель коэффициентов.</summary>
        public CoeffModel Cmodel { get; set; }

        /// <summary>Коллекция коэффициентов.</summary>
        public ObservableCollection<CoeffModel> CoeffModels { get; set; }

        /// <summary>Доп настройки.</summary>
        public Misc Misc { get; set; }

        #endregion     

        #region Commands

        /// <summary>Команда 1.</summary>
        public ICommand ClickCommand { get; set; }

        /// <summary>Команда 2.</summary>
        public ICommand ClickCommand2 { get; set; }

        /// <summary>Команда - применить фильтр.</summary>
        public ICommand ClickApplyFilter { get; set; }

        /// <summary>Команда 4.</summary>
        public ICommand ClickCommand4 { get; set; }

        /// <summary>Команда 5.</summary>
        public ICommand ClickCommand5 { get; set; }

        /// <summary>Команда 6.</summary>
        public ICommand ClickCommand6 { get; set; }
        
        #endregion

        #region Methods

        private void Timer_Tick(object sender, EventArgs e)
        {
            this.ReadData();            
        }
        
        private async void ReadData()
        {
                this.runtimeModels = new ObservableCollection<RuntimeModel>();

                this.runtimeModels = await Task<ObservableCollection<RuntimeModel>>.Factory.StartNew(() =>
                {
                    return rDB.DataReadTest(tags, runtimeModels);
                });                               
        }

        private void Timer_Tick_Calc(object sender, EventArgs e)
        {
            this.CalcData();
        }

        private void CalcData()
        {
                var clcTEP = new CalculateTEP();

                this.LiveTEP = clcTEP.Calculate(this.LiveTEP, this.runtimeModels, this.CoeffModels);

                var indx = this.IndexCalc("OK_UVP_Q", this.runtimeModels);

                this.LiveTEP.OK_UVP_Q_old = this.runtimeModels[indx].Value;
        }

        private int IndexCalc(string name, ObservableCollection<RuntimeModel> runtimeModels)
        {
            return runtimeModels.IndexOf(runtimeModels.Where(X => X.TagName == name).FirstOrDefault());
        }

        private int IndexCalc(string name)
        {
            return this.runtimeModels.IndexOf(this.runtimeModels.Where(X => X.TagName == name).FirstOrDefault());
        }

        private void Timer_Tick2(object sender, EventArgs e)
        {
            decimal hour = DateTime.Now.Hour;
            decimal minute = DateTime.Now.Minute;

            if (minute == 0)
            {
                if (hour == (Math.Floor(hour / 2) * 2) + 1 && this.reportFlag == true)
                {
                    var sqliteDB = new SQLiteDB(this.settingsModel.SQLite);
                    if (!File.Exists(DataBaseName))
                    {
                        sqliteDB.CreateBase();
                    }

                    sqliteDB.TEPCreateTable();
                    sqliteDB.TEPWrite(this.LiveTEP);

                    this.TEPtoBase = this.LiveTEP;

                    if (Misc.Master == "Master" && Misc.MSSQLStatus == string.Empty)
                    {
                        if (this.rDB.DataReadLastReport(hour) == false)
                        {
                            this.rDB.DataWrite(this.TEPtoBase);
                        }
                    }
                    else if (Misc.Master == "Slave")
                    {
                        // таймер проверки записи отчета Masterom
                        this.timerSlaveWrite.Interval = new TimeSpan(0, 0, 2);
                        this.timerSlaveWrite.Tick += new EventHandler(this.TimerSlaveWrite);
                        this.timerSlaveWrite.Start();
                    }

                    this.LiveTEP.SQLw_Data1 = 0;
                    this.LiveTEP.SQLw_Data2 = 0;
                    this.LiveTEP.SQLw_Data3 = 0;
                    this.LiveTEP.SQLw_Data4 = 0;
                    this.LiveTEP.SQLw_Data5 = 0;
                    this.LiveTEP.SQLw_Data6 = 0;
                    this.LiveTEP.SQLw_Data7 = 0;
                    this.LiveTEP.SQLw_Data8 = 0;
                    this.LiveTEP.SQLw_Data9 = 0;
                    this.LiveTEP.SQLw_Data10 = 0;
                    this.LiveTEP.SQLw_Data11 = 0;
                    this.LiveTEP.SQLw_Data12 = 0;
                    this.LiveTEP.SQLw_Data13 = 0;
                }
            }
        }

        private void TimerSlaveWrite(object sender, EventArgs e)
        {
            decimal hour = DateTime.Now.Hour;

            if (Misc.MSSQLStatus == string.Empty)
            {
                if (this.rDB.DataReadLastReport(hour) == false)
                {
                    this.rDB.DataWrite(this.TEPtoBase);
                }

                this.timerSlaveWrite.Stop();
            }
        }

        private void TimerAccessReport(object sender, EventArgs e)
        {
            this.reportFlag = true;
            this.timerAccessReport.Stop();
        }

        private void Timer_Tick3(object sender, EventArgs e)
        {
            decimal hour = DateTime.Now.Hour;
            decimal minute = DateTime.Now.Minute;

            if (minute == 0)
            {
                if (hour == 6)
                {
                    // выбираем данные за сутки
                    Filters.Day = true;
                    this.ApplyFilter();

                    // сохраняем отчет в csv
                    var tepToExcel = new TEPToExcel();
                    tepToExcel.SaveData(this.HistTEP);

                    // отправка письма
                    var date = DateTime.Now.ToString("yyyy.MM.dd");
                    var att = Directory.GetCurrentDirectory() + @"\TEP\TEP_" + date + ".xlsx";
                    var subject = "Отчет ТЭП " + date;
                    var body = "<h3>Суточный отчет ТЭП Котельной, площадка СБНПУ " + date + "</h3><br>" +
                        "---------------------------------------<br>" +
                        "Элком+, Алиса<br>" +
                        "тел./факс (3822) 522-511<br>";
                    this.SendMail(subject, body, att, false);

                    // отправка письма в Элком с логами
                    var log = Directory.GetCurrentDirectory() + @"\log.txt";
                    var log_temp = Directory.GetCurrentDirectory() + @"\log_temp.txt";

                    File.Delete(log_temp);
                    File.Copy(log, log_temp);

                    att = Directory.GetCurrentDirectory() + @"\log_temp.txt";
                    subject = "Логи " + date;
                    body = "<h3>Логи " + date + "</h3><br>" +
                        "---------------------------------------<br>" +
                        "Элком+, Алиса<br>";
                    this.SendMail(subject, body, att, true);
                }
            }
        }

        private void TimerConnStatus(object sender, EventArgs e)
        {
            this.ConnStatus();
        }

        private void Filter()
        {
            var dt = Filters.StartDate;

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

        private async void ConnStatus()
        {
            var status = false;

            status = await Task<bool>.Factory.StartNew(() =>
            {
                return rDB.CheckMSSQLConn();
            });

            if (!status)
            {
                Misc.MSSQLStatus = "| MSSQL - нет связи";
            }
            else
            {
                Misc.MSSQLStatus = string.Empty;
            }
        }

        // ------------------------------------------
        // Кнопки
        // ------------------------------------------        
        // Применение выбранного фильтра на "Главной"
        private void ApplyFilter()
        {
            this.Filter();

            var histTEP2 = new ObservableCollection<HistTEP> { };
            var sqliteDB = new SQLiteDB(this.settingsModel.SQLite);

                histTEP2 = sqliteDB.TEPRead(Filters.StartDate, Filters.EndDate);

                this.HistTEP.Clear();
                this.Htep = new HistTEP { };

                foreach (var ht in histTEP2)
                {
                    this.Htep.DateTimeTEP = DateTime.Now;
                    this.Htep.SQLw_Data1 += ht.SQLw_Data1;
                    this.Htep.SQLw_Data2 += ht.SQLw_Data2;
                    this.Htep.SQLw_Data3 += ht.SQLw_Data3;
                    this.Htep.SQLw_Data4 += ht.SQLw_Data4;
                    this.Htep.SQLw_Data5 += ht.SQLw_Data5;
                    this.Htep.SQLw_Data6 += ht.SQLw_Data6;
                    this.Htep.SQLw_Data7 += ht.SQLw_Data7;
                    this.Htep.SQLw_Data8 += ht.SQLw_Data8;
                    this.Htep.SQLw_Data9 += ht.SQLw_Data9;
                    this.Htep.SQLw_Data10 += ht.SQLw_Data10;
                    this.Htep.SQLw_Data11 += ht.SQLw_Data11;
                    this.Htep.SQLw_Data12 += ht.SQLw_Data12;
                    this.Htep.SQLw_Data13 += ht.SQLw_Data13;
                    this.HistTEP.Add(ht);
                }

                this.HistTEP.Add(this.Htep);                        
        }

        // Читает с БД
        private void ClickMethod()
        {
            var runtimeDB = new RuntimeDB(this.settingsModel.MSSQL);
            decimal hour = 7;

            if (Misc.Master == "Master" && Misc.MSSQLStatus == string.Empty)
            {
                bool lastReport = this.rDB.DataReadLastReport(hour);
                if (lastReport == false)
                {
                    this.rDB.DataWrite(this.LiveTEP);
                }
            }
        }

        private RootElement SetDefaultValue(RootElement set)
        {
            set.MSSQL.Server = "192.168.8.224";
            set.MSSQL.DBName = "Runtime";
            set.MSSQL.Login = "sa";
            set.MSSQL.Pass = "sa";

            set.SQLite.DBName = "DBTEP.sqlite";
            set.SQLite.Pass = string.Empty;

            set.Mail.SmtpServer = "sdsdsds";
            set.Mail.Port = 25;
            set.Mail.Login = "SamoykinAA";
            set.Mail.Pass = "Ghjcnj_Gfhjkm3";
            set.Mail.From = "SamoykinAA@elcomplus.ru";
            set.Mail.To.Add("SamoykinAA@elcomplus.ru");
            set.Mail.ServiceTo = string.Empty;

            set.Reserv.Master = true;

            return set;
        }

        // Создает БД, таблицу и делает запись
        private void ClickMethod2()
        {
            // string DataBaseName = "DBTEP.sqlite";
            // SQLiteDB sqliteDB = new SQLiteDB(xmlFields);
            // if (!File.Exists(DataBaseName))
            // {
            //    sqliteDB.CreateBase();
            // }

            // sqliteDB.TEPCreateTable();
            // sqliteDB.TEPWrite(LiveTEP);
        }

        // Запись в лог и отсылка сообщения с логами
        private void ClickMethod4()
        {
            // Выбираем данные за сутки
            Filters.Day = true;
            this.ApplyFilter();

            // Сохраняем отчет в csv
            var tepToExcel = new TEPToExcel();
            tepToExcel.SaveData(this.HistTEP);

            // Отправка письма
            var date = DateTime.Now.ToString("yyyy.MM.dd");
            var att = Directory.GetCurrentDirectory() + @"\TEP\TEP_" + date + ".xlsx";
            var subject = "Отчет ТЭП " + date;
            var body = "<h2>Отчет ТЭП " + date + "</h2><br>" +
                "---------------------------------------<br>" +
                "Элком+, Алиса<br>" +
                "тел./факс (3822) 522-511<br>";
            this.SendMail(subject, body, att, false);

            // Отправка письма в Элком с логами
            var log = Directory.GetCurrentDirectory() + @"\log.txt";
            var log_temp = Directory.GetCurrentDirectory() + @"\log_temp.txt";

            File.Delete(log_temp);
            File.Copy(log, log_temp);

            att = Directory.GetCurrentDirectory() + @"\log_temp.txt";
            subject = "Логи " + date;
            body = "<h2>Логи " + date + "</h2><br>" +
                "---------------------------------------<br>" +
                "Элком+, Алиса<br>";
            this.SendMail(subject, body, att, true);
        }
        
        private async void SendMail(string subject, string body, string att, bool service)
        {
            var tepMail = new TEPMail();

            await Task.Factory.StartNew(() =>
            {
                tepMail.SendMail(settingsModel.Mail, subject, body, att, service);
            });
        }

        private void ClickMethod5()
        {
            // Выбираем данные за сутки
            Filters.Day = true;
            this.ApplyFilter();

            var tepToExcel = new TEPToExcel();
            tepToExcel.SaveData(this.HistTEP);
        }
        
        #endregion        
    }    
}