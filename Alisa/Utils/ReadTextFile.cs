namespace Alisa.Utils
{    
    using System;
    using System.Collections.ObjectModel;
    using System.IO;

    using Model;

    /// <summary>Чтение данных из файла.</summary>
    public class ReadTextFile
    {
        private LogFile logFile = new LogFile();
        private ObservableCollection<CoeffModel> coeffModels = new ObservableCollection<CoeffModel>();

        /// <summary>Прочитать файл.</summary>
        /// <param name="path">Путь к файлу.</param>
        /// <returns>Теги в виде строки.</returns>
        public string ReadFile(string path)
        {
            var text = string.Empty;

            try
            {
                using (var sr = new StreamReader(path, System.Text.Encoding.Default))
                {
                    string s = string.Empty;
                    while ((s = sr.ReadLine()) != null)
                    {
                        text = text + "'" + s + "',";
                    }

                    text = text.Trim(new char[] { ',' });
                }

                var logText = DateTime.Now.ToString() + "|event|ReadTextFile - readFile|Считан список из файла " + path;
                this.logFile.WriteLog(logText);
            }
            catch (Exception ex)
            {
                var logText = DateTime.Now.ToString() + "|fail|ReadTextFile - readFile|" + ex.Message;
                this.logFile.WriteLog(logText);                
            }

            return text;
        }

        /// <summary>Прочитать коэффициенты.</summary>
        /// <param name="path">Путь к файлу.</param>
        /// <returns>Коэффициенты.</returns>
        public ObservableCollection<CoeffModel> ReadCoeff(string path)
        {
            try
            {
                using (var sr = new StreamReader(path, System.Text.Encoding.Default))
                {
                    string s = string.Empty;

                    while ((s = sr.ReadLine()) != null)
                    {
                        var coeffModel = new CoeffModel();
                        var substrings = s.Split('|');

                        coeffModel.TagName = substrings[0];
                        coeffModel.Value = Convert.ToSingle(substrings[1]);
                        coeffModel.Comment = substrings[2];

                        this.coeffModels.Add(coeffModel);
                    }
                }

                var logText = DateTime.Now.ToString() + "|event|ReadTextFile - readCoeff|Считаны коэффициенты из файла " + path;
                this.logFile.WriteLog(logText);
            }
            catch (Exception ex)
            {
                var logText = DateTime.Now.ToString() + "|fail|ReadTextFile - readCoeff|" + ex.Message;
                this.logFile.WriteLog(logText);
            }

            return this.coeffModels;
        }        
    }
}