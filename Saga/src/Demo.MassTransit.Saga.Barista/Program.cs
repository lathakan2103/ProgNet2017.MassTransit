using System;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.Util;

namespace Demo.MassTransit.Saga.Barista
{
    internal class Program
    {
        internal static void Main()
        {
            var bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.UseSerilog();
                var host = cfg.Host(new Uri("rabbitmq://localhost/prognet"), h =>
                {
                    h.Username("prognet");
                    h.Password("skillsmatter");
                });
                cfg.ReceiveEndpoint(host, "cafe.barista", ep =>
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