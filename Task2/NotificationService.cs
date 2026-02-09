using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task2
{
    public class NotificationService
    {
        private readonly INotificationSender _sender;
        private readonly ILogger _logger;

        public NotificationService(INotificationSender emailSender, ILogger logger)
        {
            _sender = emailSender; // Нарушение: жесткая привязка
            _logger = logger;
        }

        public void SendNotification(string message, string recipient)
        {
            // Логика подготовки уведомления
            string formattedMessage = $"Уведомление: {message}";

            // Отправка email
            _sender.Send(recipient, formattedMessage);

        }

        
    }
}
