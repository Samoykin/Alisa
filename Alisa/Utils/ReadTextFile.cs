using System;
using System.Collections.Generic;
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
    }
}
