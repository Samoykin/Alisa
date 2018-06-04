namespace Alisa.Model
{
    using System.ComponentModel;

    /// <summary>Модель параметров.</summary>
    public class TEPModel : INotifyPropertyChanged
    {
        #region Fields

        private string boiler4Qg;
        private string boiler5Qg;
        private string boiler1V10040;
        private string boiler1Fsv;
        private string commonBoilerAI1102;
        private string commonBoilerAI1105;

        #endregion

        /// <summary>Событие изменения свойства.</summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #region Properties

        /// <summary>Значение K4_Qg.</summary>
        public string K4_Qg
        {
            get
            {
                return this.boiler4Qg;
            }

            set
            {
                if (this.boiler4Qg != value)
                {
                    this.boiler4Qg = value;
                    this.OnPropertyChanged("K4_Qg");
                }
            }
        }

        /// <summary>Значение K5_Qg.</summary>
        public string K5_Qg
        {
            get
            {
                return this.boiler5Qg;
            }

            set
            {
                if (this.boiler5Qg != value)
                {
                    this.boiler5Qg = value;
                    this.OnPropertyChanged("K5_Qg");
                }
            }
        }

        /// <summary>Значение K1_V10040.</summary>
        public string K1_V10040
        {
            get
            {
                return this.boiler1V10040;
            }

            set
            {
                if (this.boiler1V10040 != value)
                {
                    this.boiler1V10040 = value;
                    this.OnPropertyChanged("K1_V10040");
                }
            }
        }

        /// <summary>Значение K1_Fsv.</summary>
        public string K1_Fsv
        {
            get
            {
                return this.boiler1Fsv;
            }

            set
            {
                if (this.boiler1Fsv != value)
                {
                    this.boiler1Fsv = value;
                    this.OnPropertyChanged("K1_Fsv");
                }
            }
        }

        /// <summary>Значение OK_AI1102.</summary>
        public string OK_AI1102
        {
            get
            {
                return this.commonBoilerAI1102;
            }

            set
            {
                if (this.commonBoilerAI1102 != value)
                {
                    this.commonBoilerAI1102 = value;
                    this.OnPropertyChanged("OK_AI1102");
                }
            }
        }

        /// <summary>Значение OK_AI1105.</summary>
        public string OK_AI1105
        {
            get
            {
                return this.commonBoilerAI1105;
            }

            set
            {
                if (this.commonBoilerAI1105 != value)
                {
                    this.commonBoilerAI1105 = value;
                    this.OnPropertyChanged("OK_AI1105");
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