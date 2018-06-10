namespace Alisa.Utils
{    
    using System;
    using System.Collections.ObjectModel;
    using System.IO;    
    using Model;
    using NLog;

    /// <summary>Чтение данных из файла.</summary>
    public class ReadTextFile
    {
        private Logger logger = LogManager.GetCurrentClassLogger();
        private ObservableCollection<CoeffModel> coeffModels = new ObservableCollection<CoeffModel>();

        /// <summary>Прочитать файл.</summary>
        /// <param name="path">Путь к файлу.</param>
        /// <returns>Теги в виде строки.</returns>
        public string ReadFile(string path)
        {
            var text = string.Empty;

                using (var sr = new StreamReader(path, System.Text.Encoding.Default))
                {
                    string s;
                    while ((s = sr.ReadLine()) != null)
                    {
                        text += $"'{s}',";
                    }

                    text = text.Trim(new char[] { ',' });
                }

                this.logger.Info($"Считан список из файла {path}");

            return text;
        }

        /// <summary>Прочитать коэффициенты.</summary>
        /// <param name="path">Путь к файлу.</param>
        /// <returns>Коэффициенты.</returns>
        public ObservableCollection<CoeffModel> ReadCoeff(string path)
        {
                using (var sr = new StreamReader(path, System.Text.Encoding.Default))
                {
                    string s;

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

                this.logger.Info($"Считаны коэффициенты из файла {path}");

            return this.coeffModels;
        }        
    }
}