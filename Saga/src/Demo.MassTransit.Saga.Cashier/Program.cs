using System;
using System.Threading.Tasks;
using Demo.MassTransit.Messages;
using MassTransit;
using MassTransit.Util;

namespace Demo.MassTransit.Saga.Cashier
{
    internal class Program
    {
        internal static void Main()
        {
            var bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                var host = cfg.Host(new Uri("rabbitmq://10.211.55.10/prognet"), h =>
                {
                    h.Username("prognet");
                    h.Password("skillsmatter");
                });
                cfg.ReceiveEndpoint(host, "cafe.cashier", ep =>
                {
                    ep.Consumer<PaymentProcessor>();
                });
            });
            bus.Start();
            TaskUtil.Await(() => Work(bus));
            bus.Stop();
        }

        private static async Task Work(IBusControl bus)
        {
            Console.WriteLine("Type \"quit\" to stop");
            while (Console.ReadLine().ToLower() != "quit"){}
        }
    }

    internal class PaymentProcessor : IConsumer<TakePayment>
    {
        public async Task Consume(ConsumeContext<TakePayment> context)
        {
            Console.WriteLine($"Take payment of {context.Message.Amount} from {context.Message.CustomerName}");
            Console.Write("Write y or n: ");
            var ok = Console.ReadLine().ToLower().StartsWith("y");
            Console.WriteLine($"\r\nPayment result: {ok}");
            
            if (ok)
                await context.Publish(new PaymentDone {OrderId = context.Message.OrderId});
            else
                await context.Publish(new PaymentFailed {OrderId = context.Message.OrderId});
        }
    }
}