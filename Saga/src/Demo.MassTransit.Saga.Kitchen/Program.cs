using System;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.Util;

namespace Demo.MassTransit.Saga.Kitchen
{
    internal class Program
    {
        public static void Main()
        {
            var bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                var host = cfg.Host(new Uri("rabbitmq://localhost/prognet"), h =>
                {
                    h.Username("prognet");
                    h.Password("skillsmatter");
                });
                cfg.ReceiveEndpoint(host, "cafe.counter", ep =>
                {

                });
            });
            bus.Start();
            TaskUtil.Await(() => Work(bus));
            bus.Stop();
        }

        private static async Task Work(IBusControl bus)
        {
        }
    }
}