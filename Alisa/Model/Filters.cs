using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alisa.Model
{
    class Filters : INotifyPropertyChanged
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

        DateTime _startDate;
        DateTime _endDate;
        Boolean _day;
        Boolean _firstShift;
        Boolean _secondShift;
        Boolean _month;
        
        public DateTime StartDate
        {
            get { return _startDate; }
            set
            {
                if (_startDate != value)
                {
                    _startDate = value;
                    OnPropertyChanged("startDate");
                }
            }
        }

        public DateTime EndDate
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

        public Boolean Day
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

        public Boolean FirstShift
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

        public Boolean SecondShift
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

        public Boolean Month
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
