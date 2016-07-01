using Alisa.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alisa.ViewModel
{
    class MainViewModel
    {
        #region Properties

        /// <summary>
        /// Get or set people.
        /// </summary>
        public TEPModel TEP { get; set; }

        #endregion


        public MainViewModel()
        {
           
            TEP = new TEPModel
            {
                K4_Qg = "12.3",
                K5_Qg = "17"
            };

        }

    }
}
