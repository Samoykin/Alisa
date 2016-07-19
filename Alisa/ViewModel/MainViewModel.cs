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


            liveTEP = new LiveTEP { };
            tdd = new Test1 {  };

            tdd.connect = _coeffModel[0].Value.ToString();
            tdd.startDate = DateTime.Now.Subtract(new TimeSpan(1, 0, 0, 0));
            tdd.endDate = DateTime.Now;

            histTEP = new ObservableCollection<HistTEP> { };



            //Handler_I Handler1 = new Handler_I();
            tdd.onCount += Message;
            tdd.day = true;
            ClickMethod3();
            //histTEP = new ObservableCollection<HistTEP>();
            //histTEP.Add(new HistTEP { dateTime = Convert.ToDateTime("08.07.2016 14:56:29"), SQLw_Data1 = 23, SQLw_Data2 = 23, SQLw_Data3 = 23, SQLw_Data4 = 23, SQLw_Data5 = 23, SQLw_Data6 = 23, SQLw_Data7 = 23, SQLw_Data8 = 23, SQLw_Data9 = 23, SQLw_Data10 = 23, SQLw_Data11 = 23, SQLw_Data12 = 23, SQLw_Data13 = 23 });
            //histTEP.Add(new HistTEP { dateTime = Convert.ToDateTime("08.07.2016 14:56:29"), SQLw_Data1 = 223, SQLw_Data2 = 283, SQLw_Data3 = 23, SQLw_Data4 = 283, SQLw_Data5 = 23, SQLw_Data6 = 23, SQLw_Data7 = 23, SQLw_Data8 = 23, SQLw_Data9 = 23, SQLw_Data10 = 23, SQLw_Data11 = 23, SQLw_Data12 = 23, SQLw_Data13 = 23 });
            

            //gridTEP.ItemsSource = histTEP;

            

            //histTEP1 = new ObservableCollection<HistTEP>()
            //{
            //    new HistTEP {Value1 = "Honda", Value2= 30000},
            //    new HistTEP {Value1 = "Ford", Value2= 15000},
            //    new HistTEP {Value1 = "Lada", Value2= 5000}
            //};

            //ItemsSource="{Binding Source=g_list}"
            
        }

        #region Commands

        /// <summary>
        /// Get or set ClickCommand.
        /// </summary>
        public ICommand ClickCommand { get; set; }
        public ICommand ClickCommand2 { get; set; }
        public ICommand ClickCommand3 { get; set; }

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
                MessageBox.Show(exception.Message, "Ошибка");
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
                if (hour == Math.Floor(hour / 2) * 2 + 1)
                {
                    
                    SQLiteDB sqliteDB = new SQLiteDB();
                    if (!File.Exists(DataBaseName))
                    {
                        sqliteDB.CreateBase();
                    }

                    sqliteDB.TEPCreateTable();
                    sqliteDB.TEPWrite(liveTEP);
                }
            }

        }

        public void Message()
        {
            DateTime dt = tdd.startDate;

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


            //MessageBox.Show("Точно, уже 71!");

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
            SQLiteDB sqliteDB = new SQLiteDB();
            if (!File.Exists(DataBaseName))
            {
                sqliteDB.CreateBase();
            }

            sqliteDB.TEPCreateTable();
            sqliteDB.TEPWrite(liveTEP);
        }

        private void ClickMethod3()
        {
            Message();

            ObservableCollection<HistTEP>  histTEP2 = new ObservableCollection<HistTEP> { };
            SQLiteDB sqliteDB = new SQLiteDB();
            
            //sqliteDB.TEPCreateTable();
            histTEP2 = sqliteDB.TEPRead(tdd.startDate, tdd.endDate);
            //DateTime.Now.Subtract(new TimeSpan(10, 0, 0, 0))
            
            histTEP.Clear();
            foreach (HistTEP ht in histTEP2)
                histTEP.Add(ht);            
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


    class Handler_I
    {
        public void Message()
        {
            //tdd.startDate = DateTime.Now.Subtract(new TimeSpan(1, 0, 0, 0));
            //tdd.endDate = DateTime.Now;
            //MessageBox.Show("Точно, уже 71!");

        }
    }


}
