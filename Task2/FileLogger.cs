using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task2
{
    internal class FileLogger : ILogger
    {
        public void WriteLine(string recipient)
        {
            File.WriteAllText("log.txt", $"Отправлено уведомление для {recipient}");
        }
    }
}
