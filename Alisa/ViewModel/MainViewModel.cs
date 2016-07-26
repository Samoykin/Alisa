using Alisa.Model;
using Alisa.Utils;
using MVVM_test.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
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
        String path = @"Config.xml";
        XMLConfig xmlConf = new XMLConfig();
        XMLFields xmlFields = new XMLFields();
        //Теги
        ReadTextFile rf = new ReadTextFile();
        String tagPath = @"TagList.txt";
        String tags;
        //БД
        List<Single> tagValue = new List<float>();
        RuntimeDB rDB = new RuntimeDB();
        //Таймер
        DispatcherTimer t1 = new DispatcherTimer();
        DispatcherTimer t2 = new DispatcherTimer();
        DispatcherTimer t3 = new DispatcherTimer();

        String DataBaseName = "DBTEP.sqlite";

        private ObservableCollection<RuntimeModel> _RtModel;
        private ObservableCollection<CoeffModel> _coeffModel = new ObservableCollection<CoeffModel>();
        
        String tagPathCoeff = @"Coeff.txt";
        LogFile logFile = new LogFile();

        #endregion

        

        #region Properties

        /// <summary>
        /// Get or set params.
        /// </summary>
        public TEPModel TEP { get; set; }
        public Test1 tdd { get; set; }
        public RuntimeModel RtModel { get; set; }
        public LiveTEP liveTEP { get; set; }

        #endregion     

        public HistTEP htep { get; set; }
        public ObservableCollection<HistTEP> histTEP { get; set; }

        public MainViewModel()
        {
            String logText = DateTime.Now.ToString() + "|event| |Запуск приложения Alisa";
            logFile.WriteLog(logText);

            ClickCommand = new Command(arg => ClickMethod());
            ClickCommand2 = new Command(arg => ClickMethod2());
            ClickCommand3 = new Command(arg => ClickMethod3());
            ClickCommand4 = new Command(arg => ClickMethod4());
            ClickCommand5 = new Command(arg => ClickMethod5());
            ClickCommand6 = new Command(arg => ClickMethod6()); //закрыть окно

            TEP = new TEPModel { };

            //Вычитывание параметров из XML
            xmlFields = xmlConf.ReadXmlConf(path);
            //вычитывание списка тегов из файла
            tags = rf.readFile(tagPath);

            //Коэффициенты            
            _coeffModel = rf.readCoeff(tagPathCoeff);

            //таймер вычитывания значений из БД и расчета ТЭП
            t1.Interval = new TimeSpan(0, 0, 6);
            t1.Tick += new EventHandler(timer_Tick);
            t1.Start();

            //таймер на вызов метода записи 2-х часовок
            t2.Interval = new TimeSpan(0, 1, 0);
            t2.Tick += new EventHandler(timer_Tick2);
            t2.Start();

            //таймер на отправку писем
            t3.Interval = new TimeSpan(0, 1, 0);
            t3.Tick += new EventHandler(timer_Tick3);
            t3.Start();


            liveTEP = new LiveTEP { };
            tdd = new Test1 {  };

            tdd.connect = _coeffModel[0].Value.ToString();
            tdd.startDate = DateTime.Now.Subtract(new TimeSpan(1, 0, 0, 0));
            tdd.endDate = DateTime.Now;

            histTEP = new ObservableCollection<HistTEP> { };


            tdd.onCount += Filter;
            tdd.day = true;
            ClickMethod3();

        }

        #region Commands

        /// <summary>
        /// Get or set ClickCommand.
        /// </summary>
        public ICommand ClickCommand { get; set; }
        public ICommand ClickCommand2 { get; set; }
        public ICommand ClickCommand3 { get; set; }
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
                    return rDB.DataRead(tags, xmlFields);
                });

                CalculateTEP clcTEP = new CalculateTEP();

                liveTEP = clcTEP.Calculate(liveTEP, _RtModel, _coeffModel);

                               
            }
            catch (Exception exception)
            {
                String logText = DateTime.Now.ToString() + "|fail|MainViewModel - ReadData|" + exception.Message;
                logFile.WriteLog(logText);
            }
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
                if (hour == Math.Floor(hour / 2) * 2 )
                {

                    SQLiteDB sqliteDB = new SQLiteDB(xmlFields);
                    if (!File.Exists(DataBaseName))
                    {
                        sqliteDB.CreateBase();
                    }

                    sqliteDB.TEPCreateTable();
                    sqliteDB.TEPWrite(liveTEP);

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

        void timer_Tick3(object sender, EventArgs e)
        {
            Decimal hour = DateTime.Now.Hour;
            Decimal minute = DateTime.Now.Minute;


            if (minute == 0)
            {
                if (hour == 3)
                {
                    //выбираем данные за сутки
                    tdd.day = true;
                    ClickMethod3();
                    //сохраняем отчет в csv
                    TEPToCSV tepToCSV = new TEPToCSV();
                    tepToCSV.saveData(histTEP);

                    //отправка письма
                    String date = DateTime.Now.ToString("yyyy.MM.dd");
                    String att = Directory.GetCurrentDirectory() + @"\TEP\TEP_" + date + ".csv";
                    String subject = "Отчет ТЭП " + date;
                    String body = "<h2>Отчет ТЭП " + date + "</h2><br>" +
                        "---------------------------------------<br>" +
                        "Элком+, Алиса<br>" +
                        "тел./факс (3822) 522-511<br>";
                    SendMail(subject, body, att);

                    //отправка письма в Элком с логами
                    att = Directory.GetCurrentDirectory() + @"\log.txt";
                    subject = "Логи " + date;
                    body = "<h2>Логи " + date + "</h2><br>" +
                        "---------------------------------------<br>" +
                        "Элком+, Алиса<br>";
                    SendMail(subject, body, att);

                }
            }

        }

        public void Filter()
        {
            DateTime dt = tdd.startDate;
            try
            {
                if (tdd.day)
                {
                    tdd.startDate = new DateTime(dt.Year, dt.Month, dt.Day, 3, 00, 00);
                    tdd.endDate = tdd.startDate.Subtract(new TimeSpan(-1, 0, 0, 0));
                }
                if (tdd.firstShift)
                {
                    tdd.startDate = new DateTime(dt.Year, dt.Month, dt.Day, 9, 00, 00);
                    tdd.endDate = tdd.startDate.Subtract(new TimeSpan(0, -12, 0, 0));
                }
                if (tdd.secondShift)
                {
                    tdd.startDate = new DateTime(dt.Year, dt.Month, dt.Day, 21, 00, 00);
                    tdd.endDate = tdd.startDate.Subtract(new TimeSpan(0, -12, 0, 0));
                }
                if (tdd.month)
                {
                    tdd.startDate = new DateTime(dt.Year, dt.Month, dt.Day, 3, 00, 00);
                    tdd.endDate = tdd.startDate.Subtract(new TimeSpan(-30, 0, 0, 0));
                }
            }
            catch (Exception exception) 
            {
                String logText = DateTime.Now.ToString() + "|fail|MainViewModel - Filter|" + exception.Message;
                logFile.WriteLog(logText);
            }


        }



        /// <summary>
        /// Click method.
        /// </summary>
        private void ClickMethod()
        {
            ReadData();

        }

        private void ClickMethod2()
        {
            String DataBaseName = "DBTEP.sqlite";
            SQLiteDB sqliteDB = new SQLiteDB(xmlFields);
            if (!File.Exists(DataBaseName))
            {
                sqliteDB.CreateBase();
            }

            sqliteDB.TEPCreateTable();
            sqliteDB.TEPWrite(liveTEP);
        }

        private void ClickMethod3()
        {
            Filter();

            ObservableCollection<HistTEP>  histTEP2 = new ObservableCollection<HistTEP> { };
            SQLiteDB sqliteDB = new SQLiteDB(xmlFields);

            try
            {
                //sqliteDB.TEPCreateTable();
                histTEP2 = sqliteDB.TEPRead(tdd.startDate, tdd.endDate);
                //DateTime.Now.Subtract(new TimeSpan(10, 0, 0, 0))

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
            catch (Exception exception)
            {
                String logText = DateTime.Now.ToString() + "|fail|MainViewModel - ClickMethod3|" + exception.Message;
                logFile.WriteLog(logText);
            }
                          
        }

        private void ClickMethod4()
        {
            String log = Directory.GetCurrentDirectory() + @"\log.txt";
            String log_temp = Directory.GetCurrentDirectory() + @"\log_temp.txt";

            File.Delete(log_temp);

            File.Copy(log, log_temp);
            SendMail("Тема", "Привет", log_temp);
        }

        private async void SendMail(String subject, String body, String att)
        {
            TEPMail tepMail = new TEPMail();
            try
            {
                await Task.Factory.StartNew(() =>
                {
                    tepMail.SendMail(xmlFields, subject, body, att);
                });
            }
            catch (Exception exception)
            {
                String logText = DateTime.Now.ToString() + "|fail|MainViewModel - SendMail|" + exception.Message;
                logFile.WriteLog(logText);
            }
        }


        private void ClickMethod5()
        {
            //выбираем данные за сутки
            tdd.day = true;
            ClickMethod3();

            TEPToCSV tepToCSV = new TEPToCSV();
            tepToCSV.saveData(histTEP);
        }

        private void ClickMethod6()
        {
            
        }

        #endregion

        

    }


    class Test1 : INotifyPropertyChanged
    {
        public delegate void MethodContainer();
        public event MethodContainer onCount;


        #region Implement INotyfyPropertyChanged members

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        private string _connect;
        private DateTime _startDate;
        private DateTime _endDate;
        private Boolean _day;
        private Boolean _firstShift;
        private Boolean _secondShift;
        private Boolean _month;

        public string connect
        {
            get { return _connect; }
            set
            {
                if (_connect != value)
                {
                    _connect = value;
                    OnPropertyChanged("connect");
                }
            }
        }              

        public DateTime startDate
        {
            get { return _startDate; }
            set
            {
                if (_startDate != value)
                {
                    _startDate = value;
                    OnPropertyChanged("startDate");
                    //onCount();
                }
            }
        }

        public DateTime endDate
        {
            get { return _endDate; }
            set
            {
                if (_endDate != value)
                {
                    _endDate = value;
                    OnPropertyChanged("endDate");
                }
            }
        }

        public Boolean day
        {
            get { return _day; }
            set
            {
                if (_day != value)
                {
                    _day = value;
                    OnPropertyChanged("day");
                    onCount();
                }
            }
        }

        public Boolean firstShift
        {
            get { return _firstShift; }
            set
            {
                if (_firstShift != value)
                {
                    _firstShift = value;
                    OnPropertyChanged("firstShift");
                    onCount();
                }
            }
        }

        public Boolean secondShift
        {
            get { return _secondShift; }
            set
            {
                if (_secondShift != value)
                {
                    _secondShift = value;
                    OnPropertyChanged("secondShift");
                    onCount();
                }
            }
        }

        public Boolean month
        {
            get { return _month; }
            set
            {
                if (_month != value)
                {
                    _month = value;
                    OnPropertyChanged("month");
                    onCount();
                }
            }
        }



    }




}
