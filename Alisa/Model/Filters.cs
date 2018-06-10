namespace Alisa.Model
{
    using System;
    using System.ComponentModel;

    /// <summary>Фильтры.</summary>
    public sealed class Filters : INotifyPropertyChanged
    {
        #region Fields

        private DateTime startDate;
        private DateTime endDate;
        private bool day;
        private bool firstShift;
        private bool secondShift;
        private bool month;

        #endregion

        /// <summary>Контейнер делегата.</summary>
        public delegate void MethodContainer();

        /// <summary>Событие.</summary>
        public event MethodContainer OnCount;

        /// <summary>Событие изменения свойства.</summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #region Properties

        /// <summary>Начальная дата.</summary>
        public DateTime StartDate
        {
            get
            {
                return this.startDate;
            }

            set
            {
                if (this.startDate != value)
                {
                    this.startDate = value;
                    this.OnPropertyChanged("startDate");
                }
            }
        }

        /// <summary>Конечная дата.</summary>
        public DateTime EndDate
        {
            get
            {
                return this.endDate;
            }

            set
            {
                if (this.endDate != value)
                {
                    this.endDate = value;
                    this.OnPropertyChanged("endDate");
                }
            }
        }

        /// <summary>День.</summary>
        public bool Day
        {
            get
            {
                return this.day;
            }

            set
            {
                if (this.day != value)
                {
                    this.day = value;
                    this.OnPropertyChanged("day");
                    this.OnCount();
                }
            }
        }

        /// <summary>Первая смена.</summary>
        public bool FirstShift
        {
            get
            {
                return this.firstShift;
            }

            set
            {
                if (this.firstShift != value)
                {
                    this.firstShift = value;
                    this.OnPropertyChanged("firstShift");
                    this.OnCount();
                }
            }
        }

        /// <summary>Вторая смена.</summary>
        public bool SecondShift
        {
            get
            {
                return this.secondShift;
            }

            set
            {
                if (this.secondShift != value)
                {
                    this.secondShift = value;
                    this.OnPropertyChanged("secondShift");
                    this.OnCount();
                }
            }
        }

        /// <summary>Месяц.</summary>
        public bool Month
        {
            get
            {
                return this.month;
            }

            set
            {
                if (this.month != value)
                {
                    this.month = value;
                    this.OnPropertyChanged("month");
                    this.OnCount();
                }
            }
        }

        #endregion

        #region Implement INotyfyPropertyChanged members
        
        /// <summary>Изменения свойства.</summary>
        /// <param name="propertyName">Имя свойства.</param>
        private void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}