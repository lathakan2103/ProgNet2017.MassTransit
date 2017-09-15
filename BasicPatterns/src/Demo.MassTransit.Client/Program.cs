using System;
using System.Threading.Tasks;
using Demo.MassTransit.Messages;
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
            //Log.Information("Now we are going to send stuff");
            var endpoint = await _bus.GetSendEndpoint(
                new Uri("rabbitmq://10.211.55.10/prognet/test"));
            
            await endpoint.Send<HelloWorld>(new {Text = "Hi there"});
            
            Console.WriteLine("Sent stuff");
        }
        
        private static IBusControl ConfigureBus() =>
            Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host(new Uri("rabbitmq://10.211.55.10/prognet"), h =>
                {
                    h.Username("prognet");
                    h.Password("skillsmatter");
                });
            });
    }
}