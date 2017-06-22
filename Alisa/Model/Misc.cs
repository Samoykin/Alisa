using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alisa.Model
{
    class Misc : INotifyPropertyChanged
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
        //Поля
        private String _master;
        private String _mssqlStatus;

        //Свойства
        public String Master
        {
            get { return _master; }
            set
            {
                if (_master != value)
                {
                    _master = value;
                    OnPropertyChanged("Master");
                }
            }
        }

        public String MSSQLStatus
        {
            get { return _mssqlStatus; }
            set
            {
                if (_mssqlStatus != value)
                {
                    _mssqlStatus = value;
                    OnPropertyChanged("MSSQLStatus");
                }
            }
        }

    }
}
