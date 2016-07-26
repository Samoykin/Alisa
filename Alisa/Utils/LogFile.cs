using System;
using System.IO;
using System.Threading;

namespace Alisa.Utils
{
    class LogFile
    {
        String path = @"log.txt";

        public void WriteLog(String str)
        {
                using (StreamWriter sw = new StreamWriter(path, true, System.Text.Encoding.Default))
                {
                    sw.WriteLine(str);
                }

        }
    }
}
