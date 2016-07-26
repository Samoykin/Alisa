using Alisa.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace Alisa.Utils
{
    class ReadTextFile
    {
        private LogFile logFile = new LogFile();

        //Вычитывание списка из файла 
        public String readFile(String path)
        {
            String text = "";

            try
            {
                using (StreamReader sr = new StreamReader(path, System.Text.Encoding.Default))
                {
                    String s = "";
                    while ((s = sr.ReadLine()) != null)
                    {
                        text = text + "'" + s + "',";
                    }
                    text = text.Trim(new char[] { ',' });
                }

                String logText = DateTime.Now.ToString() + "|event|ReadTextFile - readFile|Считан список из файла " + path;
                logFile.WriteLog(logText);
            }
            catch (Exception exception)
            {
                String logText = DateTime.Now.ToString() + "|fail|ReadTextFile - readFile|" + exception.Message;
                logFile.WriteLog(logText);
                
            }
            return text;
        }

        private ObservableCollection<CoeffModel> _coeffModel = new ObservableCollection<CoeffModel>();
        //Вычитывание коэффициентов из файла
        public ObservableCollection<CoeffModel> readCoeff(String path)
        {
            try
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
                String logText = DateTime.Now.ToString() + "|event|ReadTextFile - readCoeff|Считаны коэффициенты из файла " + path;
                logFile.WriteLog(logText);
            }
            catch (Exception exception)
            {
                String logText = DateTime.Now.ToString() + "|fail|ReadTextFile - readCoeff|" + exception.Message;
                logFile.WriteLog(logText);
            }

            return _coeffModel;
        }




    }
}
