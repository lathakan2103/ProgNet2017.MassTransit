using System;
using System.Threading.Tasks;
using Demo.MassTransit.Messages;
using MassTransit;
using MassTransit.Util;

namespace Demo.MassTransit.Saga.Customer
{
    internal class Program
    {
        private static Guid _orderId;

        public static void Main()
        {
            var bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                var host = cfg.Host(new Uri("rabbitmq://10.211.55.10/prognet"), h =>
                {
                    h.Username("prognet");
                    h.Password("skillsmatter");
                });
                cfg.ReceiveEndpoint(host, "cafe.customer", ep =>
                {
                    ep.Handler<BounceCustomer>(async c =>
                    {
                        if (c.Message.OrderId == _orderId)
                            await Console.Out.WriteLineAsync("You have not paid! Go away.");
                    });
                });
            });
            bus.Start();
            TaskUtil.Await(() => Work(bus));
            bus.Stop();
        }

        private static async Task Work(IBusControl bus)
        {
            while (true)
            {
                Console.Write("What's your name? ");
                var name = Console.ReadLine();
                Console.Write("What would you like to eat? ");
                var food = Console.ReadLine();
                Console.Write("What would you like to drink? ");
                var drink = Console.ReadLine();

                _orderId = Guid.NewGuid();
                await bus.Publish(new OrderPlaced
                {
                    OrderId = _orderId,
                    CustomerName = name,
                    Coffee = drink,
                    Food = food
                });
                Console.WriteLine("Please wait now. Enter 'stop' if you give up.");
                var response = Console.ReadLine();

                if (response.ToLower().Equals("stop"))
                    await bus.Publish(new GaveUpWaiting
                    {
                        OrderId = _orderId
                    });
            }
        }
    }
}