using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Demo.MassTransit.Messages;
using MassTransit;
using MassTransit.Context;
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
            await _bus.StartAsync();

            var requestClient = new MessageRequestClient<HelloWorld, HelloResponse>(
                _bus,
                new Uri("rabbitmq://10.211.55.10/prognet/test"),
                TimeSpan.FromMinutes(10));

            await Task.WhenAll(Enumerable.Range(0, 1000).Select(
                async x =>
                {
                    var response = await requestClient.Request(
                        new HelloWorld {Text = "Hi there"},
                        CancellationToken.None);

                    Console.WriteLine(response.AmendedText);
                }).ToArray());
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