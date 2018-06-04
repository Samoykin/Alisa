namespace Alisa.Model
{
    using System.ComponentModel;

    /// <summary>Состояния.</summary>
    public class Misc : INotifyPropertyChanged
    {
        #region Fields

        private string master;
        private string mssqlStatus;

        #endregion

        /// <summary>Событие изменения свойства.</summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #region Properties

        /// <summary>Статус.</summary>
        public string Master
        {
            get
            {
                return this.master;
            }

            set
            {
                if (this.master != value)
                {
                    this.master = value;
                    this.OnPropertyChanged("Master");
                }
            }
        }

        /// <summary>Статус связи.</summary>
        public string MSSQLStatus
        {
            get
            {
                return this.mssqlStatus;
            }

            set
            {
                if (this.mssqlStatus != value)
                {
                    this.mssqlStatus = value;
                    this.OnPropertyChanged("MSSQLStatus");
                }
            }
        }

        #endregion

        #region Implement INotyfyPropertyChanged members

        /// <summary>Изменения свойства.</summary>
        /// <param name="propertyName">Имя свойства.</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}