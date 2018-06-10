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
    using static Model.SettingsShell;

    /// <summary>Главная ViewModel.</summary>
    public class MainViewModel
    {
        #region Fields
        private const string SettingsPath = "Settings.xml";
        private const string TagPath = "TagList.txt";
        private const string DataBaseName = "DBTEP.sqlite"; // БД SQLite
        private const string CoeffPath = "Coeff.txt"; // Путь к файлу коэфициентов

        private Logger logger = LogManager.GetCurrentClassLogger();
        private RootElement settings = new RootElement(); // Конфигурация
        private ReadTextFile rf = new ReadTextFile(); // Работа со списками тегов
        private string tags; // Теги
        private RuntimeDb rDB; // БД

        // Таймеры
        private DispatcherTimer readTEPTimer = new DispatcherTimer();
        private DispatcherTimer calculateTEPTimer = new DispatcherTimer();
        private DispatcherTimer write2HourTEPTimer = new DispatcherTimer();
        private DispatcherTimer sendMailTimer = new DispatcherTimer();
        private DispatcherTimer checkConnectionTimer = new DispatcherTimer();
        private DispatcherTimer slaveWriteTimer = new DispatcherTimer();
        private DispatcherTimer accessReportTimer = new DispatcherTimer();

        private bool reportFlag = false;        
        private ObservableCollection<RuntimeModel> runtimeModels; // Коллекция значений тегов из файла        

        #endregion

        /// <summary>Initializes a new instance of the <see cref="MainViewModel" /> class.</summary>
        public MainViewModel()
        {
            this.logger.Info("Запуск приложения Alisa");

            try
            {
                this.FilteredTEPCmd = new Command(arg => this.ApplyFilter());

                // Сервисные
                this.WriteTEPCmd = new Command(arg => this.WriteTEP());
                this.DBCreateCmd = new Command(arg => this.DBCreate());                
                this.LogMailCmd = new Command(arg => this.LogMail());
                this.FilterAndSaveCmd = new Command(arg => this.FilterAndSave());

                this.TEP = new TEPModel { };
                this.Misc = new Misc { };

                // Вычитывание параметров из XML
                // Инициализация модели настроек
                var settingsXml = new SettingsXml<RootElement>(SettingsPath);
                this.settings.MSSQL = new MSSQL();
                this.settings.SQLite = new SQLite();
                this.settings.Mail = new Mail
                {
                    To = new List<string>()
                };

                this.settings.Reserv = new Reserv();

                if (File.Exists(SettingsPath))
                {
                    this.settings = settingsXml.ReadXml(this.settings);
                }
                else
                {
                    this.settings = this.SetDefaultValue(this.settings); // Значения по умолчанию
                    settingsXml.WriteXml(this.settings);
                }

                this.rDB = new RuntimeDb(this.settings.MSSQL);

                Misc.Master = this.settings.Reserv.Master ? "Master" : "Slave";

                // Вычитывание списка тегов из файла
                this.tags = this.rf.ReadFile(TagPath);

                // Коэффициенты  
                this.CoeffModels = new ObservableCollection<CoeffModel> { };
                this.CoeffModels = this.rf.ReadCoeff(CoeffPath);

                // Таймер вычитывания значений из БД
                this.readTEPTimer.Interval = new TimeSpan(0, 0, 2);
                this.readTEPTimer.Tick += this.ReadTEP;
                this.readTEPTimer.Start();

                // Расчет ТЭП
                this.calculateTEPTimer.Interval = new TimeSpan(0, 0, 6);
                this.calculateTEPTimer.Tick += this.CalculateTEP;
                this.calculateTEPTimer.Start();

                // Таймер на вызов метода записи 2-х часовок
                this.write2HourTEPTimer.Interval = new TimeSpan(0, 1, 0);
                this.write2HourTEPTimer.Tick += new EventHandler(this.Write2HourTEP);
                this.write2HourTEPTimer.Start();

                // Таймер на отправку писем
                this.sendMailTimer.Interval = new TimeSpan(0, 1, 0);
                this.sendMailTimer.Tick += this.SendMail;
                this.sendMailTimer.Start();

                // Таймер проверки связи с БД MSSQL
                this.checkConnectionTimer.Interval = new TimeSpan(0, 0, 10);
                this.checkConnectionTimer.Tick += this.CheckConnection;
                this.checkConnectionTimer.Start();

                // Таймер на доступ к записи отчета
                this.accessReportTimer.Interval = new TimeSpan(0, 2, 0);
                this.accessReportTimer.Tick += this.AccessReport;
                this.accessReportTimer.Start();

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

        /// <summary>Команда - применить фильтр.</summary>
        public ICommand FilteredTEPCmd { get; set; }

        /// <summary>Команда 1.</summary>
        public ICommand WriteTEPCmd { get; set; }

        /// <summary>Команда 2.</summary>
        public ICommand DBCreateCmd { get; set; }

        /// <summary>Команда 4.</summary>
        public ICommand LogMailCmd { get; set; }

        /// <summary>Команда 5.</summary>
        public ICommand FilterAndSaveCmd { get; set; }

        #endregion

        #region Methods

        #region TimerMetods

        private void ReadTEP(object sender, EventArgs e)
        {
            this.ReadData();
        }

        private void CalculateTEP(object sender, EventArgs e)
        {
            this.CalcData();
        }

        private void Write2HourTEP(object sender, EventArgs e)
        {
            decimal hour = DateTime.Now.Hour;
            decimal minute = DateTime.Now.Minute;

            if (minute != 0)
            {
                return;
            }

            if (hour == (Math.Floor(hour / 2) * 2) + 1 && this.reportFlag == true)
            {
                var sqliteDb = new SqLiteDb(this.settings.SQLite);
                if (!File.Exists(DataBaseName))
                {
                    sqliteDb.CreateBase();
                }

                sqliteDb.TEPCreateTable();
                sqliteDb.TEPWrite(this.LiveTEP);

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
                    // Таймер проверки записи отчета Masterom
                    this.slaveWriteTimer.Interval = new TimeSpan(0, 0, 2);
                    this.slaveWriteTimer.Tick += this.SlaveWrite;
                    this.slaveWriteTimer.Start();
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

        private void SendMail(object sender, EventArgs e)
        {
            decimal hour = DateTime.Now.Hour;
            decimal minute = DateTime.Now.Minute;

            if (minute != 0 && hour != 6)
            {
                return;
            }

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
            var body = "<h3>Суточный отчет ТЭП Котельной, площадка СБНПУ " + date + "</h3><br>" +
                       "---------------------------------------<br>" +
                       "Элком+, Алиса<br>" +
                       "тел./факс (3822) 522-511<br>";
            this.SendMail(subject, body, att, false);

            // Отправка письма в Элком с логами
            var log = Directory.GetCurrentDirectory() + @"\log.txt";
            var logTemp = Directory.GetCurrentDirectory() + @"\log_temp.txt";

            File.Delete(logTemp);
            File.Copy(log, logTemp);

            att = Directory.GetCurrentDirectory() + @"\log_temp.txt";
            subject = "Логи " + date;
            body = "<h3>Логи " + date + "</h3><br>" +
                   "---------------------------------------<br>" +
                   "Элком+, Алиса<br>";
            this.SendMail(subject, body, att, true);
        }

        private void CheckConnection(object sender, EventArgs e)
        {
            this.ConnStatus();
        }

        private void SlaveWrite(object sender, EventArgs e)
        {
            decimal hour = DateTime.Now.Hour;

            if (Misc.MSSQLStatus != string.Empty)
            {
                return;
            }

            if (this.rDB.DataReadLastReport(hour) == false)
            {
                this.rDB.DataWrite(this.TEPtoBase);
            }

            this.slaveWriteTimer.Stop();
        }

        // Доступ к отчету
        private void AccessReport(object sender, EventArgs e)
        {
            this.reportFlag = true;
            this.accessReportTimer.Stop();
        }

        #endregion

        #region Buttons
              
        // Применение выбранного фильтра на "Главной"
        private void ApplyFilter()
        {
            this.Filter();

            var sqliteDb = new SqLiteDb(this.settings.SQLite);

                var histTEP = sqliteDb.TEPRead(Filters.StartDate, Filters.EndDate);

                this.HistTEP.Clear();
                this.Htep = new HistTEP { };

                foreach (var ht in histTEP)
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

        // Запись в БД
        private void WriteTEP()
        {
            const decimal Hour = 7;

            if (Misc.Master == "Master" && Misc.MSSQLStatus == string.Empty)
            {
                var lastReport = this.rDB.DataReadLastReport(Hour);
                if (!lastReport)
                {
                    this.rDB.DataWrite(this.LiveTEP);
                }
            }
        }

        // Создание БД, таблицы и сделать запись
        private void DBCreate()
        {
            var sqliteDb = new SqLiteDb(this.settings.SQLite);

            if (!File.Exists(DataBaseName))
            {
                sqliteDb.CreateBase();
            }

            sqliteDb.TEPCreateTable();
            sqliteDb.TEPWrite(LiveTEP);
        }

        // Запись в лог и отсылка сообщения с логами
        private void LogMail()
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
            var logTemp = Directory.GetCurrentDirectory() + @"\log_temp.txt";

            File.Delete(logTemp);
            File.Copy(log, logTemp);

            att = Directory.GetCurrentDirectory() + @"\log_temp.txt";
            subject = "Логи " + date;
            body = "<h2>Логи " + date + "</h2><br>" +
                "---------------------------------------<br>" +
                "Элком+, Алиса<br>";
            this.SendMail(subject, body, att, true);
        }

        // Отфильтровать и сохранить
        private void FilterAndSave()
        {
            // Выбираем данные за сутки
            Filters.Day = true;
            this.ApplyFilter();

            var tepToExcel = new TEPToExcel();
            tepToExcel.SaveData(this.HistTEP);
        }

        #endregion

        private async void ReadData()
        {
            this.runtimeModels = new ObservableCollection<RuntimeModel>();
            this.runtimeModels = await Task<ObservableCollection<RuntimeModel>>.Factory.StartNew(() => this.rDB.DataReadTest(this.tags));
        }

        private void CalcData()
        {
            var clcTEP = new CalculateTEP();

            this.LiveTEP = clcTEP.Calculate(this.LiveTEP, this.runtimeModels, this.CoeffModels);

            var indx = this.IndexCalc("OK_UVP_Q", this.runtimeModels);

            this.LiveTEP.OK_UVP_Q_old = this.runtimeModels[indx].Value;
        }

        private int IndexCalc(string name, IList<RuntimeModel> runtimeModelList)
        {
            return runtimeModelList.IndexOf(this.runtimeModels.FirstOrDefault(x => x.TagName == name));
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
            var status = await Task<bool>.Factory.StartNew(() => this.rDB.CheckMSSQLConn());

            Misc.MSSQLStatus = !status ? "| MSSQL - нет связи" : string.Empty;
        }

        private async void SendMail(string subject, string body, string att, bool service)
        {
            var tepMail = new TEPMail();

            await Task.Factory.StartNew(() =>
            {
                tepMail.SendMail(settings.Mail, subject, body, att, service);
            });
        }

        private RootElement SetDefaultValue(RootElement set)
        {
            set.MSSQL.Server = "192.168.8.224";
            set.MSSQL.DBName = "Runtime";
            set.MSSQL.Login = "sa";
            set.MSSQL.Pass = "sa";

            set.SQLite.DBName = "DBTEP.sqlite";
            set.SQLite.Pass = string.Empty;

            set.Mail.SmtpServer = "Server";
            set.Mail.Port = 25;
            set.Mail.Login = "Login";
            set.Mail.Pass = "Password";
            set.Mail.From = "LoginFrom@mail.ru";
            set.Mail.To.Add("LoginTo@mail.ru");
            set.Mail.ServiceTo = string.Empty;

            set.Reserv.Master = true;

            return set;
        }

        #endregion        
    }    
}