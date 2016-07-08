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
        DBConnect dbc = new DBConnect();
        //Теги
        ReadTextFile rf = new ReadTextFile();
        String tagPath = @"TagList.txt";
        String tags;
        //БД
        List<Single> tagValue = new List<float>();
        RuntimeDB rDB = new RuntimeDB();
        //Таймер
        DispatcherTimer t1 = new DispatcherTimer();

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
            dbc = xmlConf.ReadXmlConf(path);
            //вычитывание списка тегов из файла
            tags = rf.readFile(tagPath);

            //Коэффициенты            
            _coeffModel = rf.readCoeff(tagPathCoeff);


            t1.Interval = new TimeSpan(0, 0, 6);
            t1.Tick += new EventHandler(timer_Tick);
            t1.Start();


            liveTEP = new LiveTEP { };
            tdd = new Test1 {  };

            tdd.connect = _coeffModel[0].Value.ToString();

            histTEP = new ObservableCollection<HistTEP>();
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
                    return rDB.DataRead(tags, dbc);
                });

                CalculateTEP clcTEP = new CalculateTEP();

                liveTEP = clcTEP.Calculate(liveTEP, _RtModel, _coeffModel);



                //Int32 indx = IndexCalc("K4_Qg");

                //liveTEP.SQLw_Data1 = clcTEP.CalculateTEP_1(indx, liveTEP.SQLw_Data1,_RtModel);

                //indx = IndexCalc("K5_Qg");
                //liveTEP.SQLw_Data2 = clcTEP.CalculateTEP_2(indx, liveTEP.SQLw_Data2, _RtModel);

                //indx = IndexCalc("K1_V10040");
                //liveTEP.SQLw_Data3 = clcTEP.CalculateTEP_3(indx, liveTEP.SQLw_Data3, _RtModel);

                //indx = IndexCalc("K1_Fsv");
                //liveTEP.SQLw_Data4 = clcTEP.CalculateTEP_4(indx, liveTEP.SQLw_Data4, _RtModel);

                

                //TEP.K4_Qg = _RtModel[indx].Value.ToString();

                
                //if (_RtModel[0].TagName == "K4_Qg")
                //{
                //    TEP.K4_Qg = _RtModel[0].Value.ToString();
                //}

                //Присвоение значений из БД к модели
                //TEP.K4_Qg = tagValue[0].ToString();
                //TEP.K5_Qg = tagValue[1].ToString();
                //TEP.K1_V10040 = tagValue[2].ToString();
                //TEP.K1_Fsv = tagValue[3].ToString();
                //TEP.OK_AI1102 = tagValue[4].ToString();
                //TEP.OK_AI1105 = tagValue[5].ToString();

                //Single rt1 = Convert.ToSingle(TEP.K4_Qg) / 10;
                //liveTEP.SQLw_Data1 = liveTEP.SQLw_Data1 + rt1;
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

            //sqliteDB.TEPCreateTable();
            sqliteDB.TEPWrite(liveTEP);
        }

        private void ClickMethod3()
        {
            histTEP = new ObservableCollection<HistTEP> { };
            SQLiteDB sqliteDB = new SQLiteDB();
            
            //sqliteDB.TEPCreateTable();
            histTEP= sqliteDB.TEPRead();
        }

        #endregion

        

    }


    class Test1 : INotifyPropertyChanged
    {
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
    }



}
