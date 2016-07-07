using System;
using System.ComponentModel;

namespace Alisa.Model
{
    class LiveTEP : INotifyPropertyChanged
    {
        #region Implement INotyfyPropertyChanged members

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region Fields

        private Double _SQLw_Data1;
        private Double _SQLw_Data2;
        private Double _SQLw_Data3;
        private Double _SQLw_Data4;
        private Double _SQLw_Data5;
        private Double _SQLw_Data6;
        private Double _SQLw_Data7;
        private Double _SQLw_Data8;
        private Double _SQLw_Data9;
        private Double _SQLw_Data10;
        private Double _SQLw_Data11;
        private Double _SQLw_Data12;
        private Double _SQLw_Data13;

        #endregion

        #region Properties

        #region SQLw_Data1
        public Double SQLw_Data1
        {
            get { return _SQLw_Data1; }
            set
            {
                if (_SQLw_Data1 != value)
                {
                    _SQLw_Data1 = value;
                    OnPropertyChanged("SQLw_Data1");
                }
            }
        }
        #endregion

        #region SQLw_Data2
        public Double SQLw_Data2
        {
            get { return _SQLw_Data2; }
            set
            {
                if (_SQLw_Data2 != value)
                {
                    _SQLw_Data2 = value;
                    OnPropertyChanged("SQLw_Data2");
                }
            }
        }
        #endregion

        #region SQLw_Data3
        public Double SQLw_Data3
        {
            get { return _SQLw_Data3; }
            set
            {
                if (_SQLw_Data3 != value)
                {
                    _SQLw_Data3 = value;
                    OnPropertyChanged("SQLw_Data3");
                }
            }
        }
        #endregion

        #region SQLw_Data4
        public Double SQLw_Data4
        {
            get { return _SQLw_Data4; }
            set
            {
                if (_SQLw_Data4 != value)
                {
                    _SQLw_Data4 = value;
                    OnPropertyChanged("SQLw_Data4");
                }
            }
        }
        #endregion

        #region SQLw_Data5
        public Double SQLw_Data5
        {
            get { return _SQLw_Data5; }
            set
            {
                if (_SQLw_Data5 != value)
                {
                    _SQLw_Data5 = value;
                    OnPropertyChanged("SQLw_Data5");
                }
            }
        }
        #endregion

        #region SQLw_Data6
        public Double SQLw_Data6
        {
            get { return _SQLw_Data6; }
            set
            {
                if (_SQLw_Data6 != value)
                {
                    _SQLw_Data6 = value;
                    OnPropertyChanged("SQLw_Data6");
                }
            }
        }
        #endregion

        #region SQLw_Data7
        public Double SQLw_Data7
        {
            get { return _SQLw_Data7; }
            set
            {
                if (_SQLw_Data7 != value)
                {
                    _SQLw_Data7 = value;
                    OnPropertyChanged("SQLw_Data7");
                }
            }
        }
        #endregion

        #region SQLw_Data8
        public Double SQLw_Data8
        {
            get { return _SQLw_Data8; }
            set
            {
                if (_SQLw_Data8 != value)
                {
                    _SQLw_Data8 = value;
                    OnPropertyChanged("SQLw_Data8");
                }
            }
        }
        #endregion

        #region SQLw_Data9
        public Double SQLw_Data9
        {
            get { return _SQLw_Data9; }
            set
            {
                if (_SQLw_Data9 != value)
                {
                    _SQLw_Data9 = value;
                    OnPropertyChanged("SQLw_Data9");
                }
            }
        }
        #endregion

        #region SQLw_Data10
        public Double SQLw_Data10
        {
            get { return _SQLw_Data10; }
            set
            {
                if (_SQLw_Data10 != value)
                {
                    _SQLw_Data10 = value;
                    OnPropertyChanged("SQLw_Data10");
                }
            }
        }
        #endregion

        #region SQLw_Data11
        public Double SQLw_Data11
        {
            get { return _SQLw_Data11; }
            set
            {
                if (_SQLw_Data11 != value)
                {
                    _SQLw_Data11 = value;
                    OnPropertyChanged("SQLw_Data11");
                }
            }
        }
        #endregion

        #region SQLw_Data12
        public Double SQLw_Data12
        {
            get { return _SQLw_Data12; }
            set
            {
                if (_SQLw_Data12 != value)
                {
                    _SQLw_Data12 = value;
                    OnPropertyChanged("SQLw_Data12");
                }
            }
        }
        #endregion

        #region SQLw_Data13
        public Double SQLw_Data13
        {
            get { return _SQLw_Data13; }
            set
            {
                if (_SQLw_Data13 != value)
                {
                    _SQLw_Data13 = value;
                    OnPropertyChanged("SQLw_Data13");
                }
            }
        }
        #endregion

        #endregion





    }
}
