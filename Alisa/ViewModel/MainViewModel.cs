using Alisa.Model;
using Alisa.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alisa.ViewModel
{
    class MainViewModel
    {
        #region Fields
        String path = @"Config.xml";
        String tagPath = @"TagList.txt";
        String tags;
        //String tags = "'OK_AI1601','K5_Qg','OK_AI1102','OK_AI1105'";
        //float[] val2 = { 0, 0, 0, 0 };
        List<Single> tagValue = new List<float>();
        RuntimeDB rDB = new RuntimeDB();
        XMLConfig xmlConf = new XMLConfig();
        DBConnect dbc = new DBConnect();
        ReadTextFile rf = new ReadTextFile();
        

        #endregion

        #region Properties

        /// <summary>
        /// Get or set params.
        /// </summary>
        public TEPModel TEP { get; set; }
        public String ttt { get; set; }

        #endregion

        

        

        public MainViewModel()
        {
            //Вычитывание параметров из XML
            dbc = xmlConf.ReadXmlConf(path);

            //Вычитывание списка тегов из файла
            tags = rf.readFile(tagPath);

            tagValue = rDB.DataRead(tags, dbc);

            TEP = new TEPModel
            {
                K4_Qg = tagValue[0].ToString(),
                K5_Qg = tagValue[1].ToString()
            };

            ttt = "qwq";

            
        }


        

    }
}
