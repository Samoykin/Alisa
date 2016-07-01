using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alisa.Model
{
    class TEPModel : INotifyPropertyChanged
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

        #region Fields

        private string _K4_Qg;
        private string _K5_Qg;

        #endregion

        #region Properties

        /// <summary>
        /// Get or set K4_Qg.
        /// </summary>
        public string K4_Qg
        {
            get { return _K4_Qg; }
            set
            {
                if (_K4_Qg != value)
                {
                    _K4_Qg = value;
                    OnPropertyChanged("K4_Qg");
                }
            }
        }


        /// <summary>
        /// Get or set K5_Qg.
        /// </summary>
        public string K5_Qg
        {
            get { return _K5_Qg; }
            set
            {
                if (_K5_Qg != value)
                {
                    _K5_Qg = value;
                    OnPropertyChanged("K5_Qg");
                }
            }
        }

        #endregion
    }
}
