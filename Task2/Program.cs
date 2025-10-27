using Microsoft.Extensions.DependencyInjection;

namespace Task2
{
    internal class Program
    {
        static void Main(string[] args)
        {
          

            ServiceCollection services = new ServiceCollection();


            Console.WriteLine("Выберите способ отправки сообщения (1 - email, 2 - sms:");
            int number = int.Parse(Console.ReadLine());
            switch (number)
            {
                case 1:  services.AddSingleton<INotificationSender, EmailSender>();
                    break;
                case 2: services.AddSingleton<INotificationSender, SmsSender>();
                    break;
                default:
                    {
                        Console.WriteLine("Нужно ввести 1 или 2");
                        return;
                    }

            }

        services.AddSingleton<ILogger,FileLogger>();
           
//            
            services.AddSingleton<NotificationService>();
            ServiceProvider provider= services.BuildServiceProvider();

            var service = provider.GetRequiredService<NotificationService>();

           
            service.SendNotification("Ваш заказ готов", "user@example.com");
        }
    }
}