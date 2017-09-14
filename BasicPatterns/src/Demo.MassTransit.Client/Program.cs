using System;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.Util;

namespace Demo.MassTransit.Client
{
    internal class Program
    {
        private static IBusControl _bus;

        public static void Main()
        {
            _bus = ConfigureBus();
            TaskUtil.Await(() => Work());
        }

        private static async Task Work()
        {
            
        }
        
        private static IBusControl ConfigureBus() =>
            Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host(new Uri("rabbitmq://localhost/prognet"), h =>
                {
                    h.Username("prognet");
                    h.Password("skillsmatter");
                });
            });
    }
}