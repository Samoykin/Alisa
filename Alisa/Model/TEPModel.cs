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
        private string _K1_V10040;
        private string _K1_Fsv;
        private string _OK_AI1102;
        private string _OK_AI1105;

        #endregion

        #region Properties

        #region K4_Qg
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
        #endregion

        #region K5_Qg
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

        #region K1_V10040
            public string K1_V10040
            {
                get { return _K1_V10040; }
                set
                {
                    if (_K1_V10040 != value)
                    {
                        _K1_V10040 = value;
                        OnPropertyChanged("K1_V10040");
                    }
                }
            }
        #endregion

        #region K1_Fsv
            public string K1_Fsv
            {
                get { return _K1_Fsv; }
                set
                {
                    if (_K1_Fsv != value)
                    {
                        _K1_Fsv = value;
                        OnPropertyChanged("K1_Fsv");
                    }
                }
            }
            #endregion

        #region OK_AI1102
            public string OK_AI1102
            {
                get { return _OK_AI1102; }
                set
                {
                    if (_OK_AI1102 != value)
                    {
                        _OK_AI1102 = value;
                        OnPropertyChanged("OK_AI1102");
                    }
                }
            }
            #endregion

        #region OK_AI1105
            public string OK_AI1105
            {
                get { return _OK_AI1105; }
                set
                {
                    if (_OK_AI1105 != value)
                    {
                        _OK_AI1105 = value;
                        OnPropertyChanged("OK_AI1105");
                    }
                }
            }
            #endregion



        #endregion
    }
}
