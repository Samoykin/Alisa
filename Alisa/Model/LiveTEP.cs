namespace Alisa.Model
{
    using System;
    using System.ComponentModel;

    /// <summary>Текущие значения ТЭП.</summary>
    public class LiveTEP : INotifyPropertyChanged
    {     
        #region Fields

        private double sqlData1;
        private double sqlData2;
        private double sqlData3;
        private double sqlData4;
        private double sqlData5;
        private double sqlData6;
        private double sqlData7;
        private double sqlData8;
        private double sqlData9;
        private double sqlData10;
        private double sqlData11;
        private double sqlData12;
        private double sqlData13;
        private double okuvpQ;

        #endregion

        /// <summary>Событие изменения свойства.</summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #region Properties

        /// <summary>Текущее значение Data1.</summary>
        public double SQLw_Data1
        {
            get
            {
                return this.sqlData1;
            }

            set
            {
                if (this.sqlData1 != value)
                {
                    this.sqlData1 = value;
                    this.OnPropertyChanged("SQLw_Data1");
                }
            }
        }

        /// <summary>Текущее значение Data2.</summary>
        public double SQLw_Data2
        {
            get
            {
                return this.sqlData2;
            }

            set
            {
                if (this.sqlData2 != value)
                {
                    this.sqlData2 = value;
                    this.OnPropertyChanged("SQLw_Data2");
                }
            }
        }

        /// <summary>Текущее значение Data3.</summary>
        public double SQLw_Data3
        {
            get
            {
                return this.sqlData3;
            }

            set
            {
                if (this.sqlData3 != value)
                {
                    this.sqlData3 = value;
                    this.OnPropertyChanged("SQLw_Data3");
                }
            }
        }

        /// <summary>Текущее значение Data4.</summary>
        public double SQLw_Data4
        {
            get
            {
                return this.sqlData4;
            }

            set
            {
                if (this.sqlData4 != value)
                {
                    this.sqlData4 = value;
                    this.OnPropertyChanged("SQLw_Data4");
                }
            }
        }

        /// <summary>Текущее значение Data5.</summary>
        public double SQLw_Data5
        {
            get
            {
                return this.sqlData5;
            }

            set
            {
                if (this.sqlData5 != value)
                {
                    this.sqlData5 = value;
                    this.OnPropertyChanged("SQLw_Data5");
                }
            }
        }

        /// <summary>Текущее значение Data6.</summary>
        public double SQLw_Data6
        {
            get
            {
                return this.sqlData6;
            }

            set
            {
                if (this.sqlData6 != value)
                {
                    this.sqlData6 = value;
                    this.OnPropertyChanged("SQLw_Data6");
                }
            }
        }

        /// <summary>Текущее значение Data7.</summary>
        public double SQLw_Data7
        {
            get
            {
                return this.sqlData7;
            }

            set
            {
                if (this.sqlData7 != value)
                {
                    this.sqlData7 = value;
                    this.OnPropertyChanged("SQLw_Data7");
                }
            }
        }

        /// <summary>Текущее значение Data8.</summary>
        public double SQLw_Data8
        {
            get
            {
                return this.sqlData8;
            }

            set
            {
                if (this.sqlData8 != value)
                {
                    this.sqlData8 = value;
                    this.OnPropertyChanged("SQLw_Data8");
                }
            }
        }

        /// <summary>Текущее значение Data9.</summary>
        public double SQLw_Data9
        {
            get
            {
                return this.sqlData9;
            }

            set
            {
                if (this.sqlData9 != value)
                {
                    this.sqlData9 = value;
                    this.OnPropertyChanged("SQLw_Data9");
                }
            }
        }

        /// <summary>Текущее значение Data10.</summary>
        public double SQLw_Data10
        {
            get
            {
                return this.sqlData10;
            }

            set
            {
                if (this.sqlData10 != value)
                {
                    this.sqlData10 = value;
                    this.OnPropertyChanged("SQLw_Data10");
                }
            }
        }

        /// <summary>Текущее значение Data11.</summary>
        public double SQLw_Data11
        {
            get
            {
                return this.sqlData11;
            }

            set
            {
                if (this.sqlData11 != value)
                {
                    this.sqlData11 = value;
                    this.OnPropertyChanged("SQLw_Data11");
                }
            }
        }

        /// <summary>Текущее значение Data12.</summary>
        public double SQLw_Data12
        {
            get
            {
                return this.sqlData12;
            }

            set
            {
                if (this.sqlData12 != value)
                {
                    this.sqlData12 = value;
                    this.OnPropertyChanged("SQLw_Data12");
                }
            }
        }

        /// <summary>Текущее значение Data13.</summary>
        public double SQLw_Data13
        {
            get
            {
                return this.sqlData13;
            }

            set
            {
                if (this.sqlData13 != value)
                {
                    this.sqlData13 = value;
                    this.OnPropertyChanged("SQLw_Data13");
                }
            }
        }

        /// <summary>Текущее значение UVP.</summary>
        public double OK_UVP_Q_old
        {
            get
            {
                return this.okuvpQ;
            }

            set
            {
                if (this.okuvpQ != value)
                {
                    this.okuvpQ = value;
                    this.OnPropertyChanged("OK_UVP_Q_old");
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