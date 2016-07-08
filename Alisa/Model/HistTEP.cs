using System;
using System.ComponentModel;

namespace Alisa.Model
{
    class HistTEP
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

        private Double _SQLw_Data1;
        private Double _SQLw_Data2;

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

        //public DateTime dateTime { get; set; }
        //public Double SQLw_Data1 { get; set; }
        //public Double SQLw_Data2 { get; set; }
        //public Double SQLw_Data3 { get; set; }
        //public Double SQLw_Data4 { get; set; }
        //public Double SQLw_Data5 { get; set; }
        //public Double SQLw_Data6 { get; set; }
        //public Double SQLw_Data7 { get; set; }
        //public Double SQLw_Data8 { get; set; }
        //public Double SQLw_Data9 { get; set; }
        //public Double SQLw_Data10 { get; set; }
        //public Double SQLw_Data11 { get; set; }
        //public Double SQLw_Data12 { get; set; }
        //public Double SQLw_Data13 { get; set; }
    }
}
