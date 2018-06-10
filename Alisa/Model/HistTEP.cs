namespace Alisa.Model
{
    using System;
    using System.ComponentModel;

    /// <summary>Сохраненные значения ТЭП.</summary>
    public sealed class HistTEP : INotifyPropertyChanged
    {
        #region Fields

        private DateTime dateTimeTEPValue;
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

        #endregion

        /// <summary>Событие изменения свойства.</summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #region Properties

        /// <summary>Дата.</summary>
        public DateTime DateTimeTEP
        {
            get
            {
                return this.dateTimeTEPValue;
            }

            set
            {
                if (this.dateTimeTEPValue != value)
                {
                    this.dateTimeTEPValue = value;
                    this.OnPropertyChanged("DateTimeTEP");
                }
            }
        }

        /// <summary>Сохраненное значение Data1.</summary>
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

        /// <summary>Сохраненное значение Data2.</summary>
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

        /// <summary>Сохраненное значение Data3.</summary>
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

        /// <summary>Сохраненное значение Data4.</summary>
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

        /// <summary>Сохраненное значение Data5.</summary>
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

        /// <summary>Сохраненное значение Data6.</summary>
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

        /// <summary>Сохраненное значение Data7.</summary>
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

        /// <summary>Сохраненное значение Data8.</summary>
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

        /// <summary>Сохраненное значение Data9.</summary>
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

        /// <summary>Сохраненное значение Data10.</summary>
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

        /// <summary>Сохраненное значение Data11.</summary>
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

        /// <summary>Сохраненное значение Data12.</summary>
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

        /// <summary>Сохраненное значение Data13.</summary>
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