using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task2
{
    public class SmsSender : INotificationSender
    {
        void INotificationSender.Send(string to, string message)
        {
            Console.WriteLine($"SMS для {to}: {message}");
        }
    }
}
