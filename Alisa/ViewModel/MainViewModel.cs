using Alisa.Model;
using Alisa.Utils;
using MVVM_test.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        

        #endregion

        

        #region Properties

        /// <summary>
        /// Get or set params.
        /// </summary>
        public TEPModel TEP { get; set; }
        public Test1 tdd { get; set; }

        

        #endregion     

        

        public MainViewModel()
        {
            ClickCommand = new Command(arg => ClickMethod());
            ClickCommand2 = new Command(arg => ClickMethod2());

            TEP = new TEPModel { };

            //Вычитывание параметров из XML
            dbc = xmlConf.ReadXmlConf(path);
            //вычитывание списка тегов из файла
            tags = rf.readFile(tagPath);

            t1.Interval = new TimeSpan(0, 0, 6);
            t1.Tick += new EventHandler(timer_Tick);
            t1.Start();



            tdd = new Test1 {  };


            
        }

        #region Commands

        /// <summary>
        /// Get or set ClickCommand.
        /// </summary>
        public ICommand ClickCommand { get; set; }
        public ICommand ClickCommand2 { get; set; }

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
                //вычитывание значений тегов из БД
                tagValue = await Task<List<Single>>.Factory.StartNew(() =>
                {
                    return rDB.DataRead(tags, dbc);
                });

                //Присвоение значений из БД к модели
                TEP.K4_Qg = tagValue[0].ToString();
                TEP.K5_Qg = tagValue[1].ToString();
                TEP.K1_V10040 = tagValue[2].ToString();
                //TEP.K1_Fsv = tagValue[3].ToString();
                //TEP.OK_AI1102 = tagValue[4].ToString();
                //TEP.OK_AI1105 = tagValue[5].ToString();

                Single rt1 = Convert.ToSingle(TEP.K4_Qg) / 10;
                tdd.connect = (Convert.ToSingle(tdd.connect) + rt1).ToString();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Ошибка");
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
            //ttt = "qwq";

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
