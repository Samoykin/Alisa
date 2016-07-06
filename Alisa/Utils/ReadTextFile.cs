using Alisa.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace Alisa.Utils
{
    class ReadTextFile
    {
        //Вычитывание списка из файла
        String text = "";

        public String readFile(String path)
        {
            using (StreamReader sr = new StreamReader(path, System.Text.Encoding.Default))
            {
                String s = "";
                while ((s = sr.ReadLine()) != null)
                {
                    text = text + "'" + s + "',";
                }
            }

            text = text.Trim(new char[] { ',' });

            return text;
        }

        private ObservableCollection<CoeffModel> _coeffModel = new ObservableCollection<CoeffModel>();

        public ObservableCollection<CoeffModel> readCoeff(String path)
        {   
            using (StreamReader sr = new StreamReader(path, System.Text.Encoding.Default))
            {
                
                String s = "";

                while ((s = sr.ReadLine()) != null)
                {
                    CoeffModel coeffModel = new CoeffModel();

                    String[] substrings = s.Split('|');

                    coeffModel.TagName = substrings[0];
                    coeffModel.Value = Convert.ToSingle(substrings[1]);
                    coeffModel.Comment = substrings[2];

                    _coeffModel.Add(coeffModel);
                }
                
            }

            return _coeffModel;
        }
    }
}
