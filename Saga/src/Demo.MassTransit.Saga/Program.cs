using System;
using System.Threading.Tasks;
using Automatonymous;
using MassTransit;
using MassTransit.RavenDbIntegration;
using MassTransit.Util;
using Raven.Client;
using Raven.Client.Document;
using Serilog;

namespace Demo.MassTransit.Saga
{
    internal static class Program
    {
        internal static void Main()
        {
            ConfigureLogging();
            
            var store = GetStore();
            var repository = new RavenDbSagaRepository<DinnerSagaState>(store);
            var machine = new DinnerSaga();
            
            var bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.UseSerilog();
                var host = cfg.Host(new Uri("rabbitmq://localhost/prognet"), h =>
                {
                    h.Username("prognet");
                    h.Password("skillsmatter");
                });
                cfg.ReceiveEndpoint(host, "cafe", ep =>
                {
                    ep.StateMachineSaga(machine, repository);
                });
            });
            
            bus.Start();

            Console.ReadLine();
            
            // TaskUtil.Await(() => Work(bus));
            bus.Stop();
        }

//        private static async Task Work(IBusControl bus)
//        {
//            
//        }

        private static IDocumentStore GetStore()
        {
            var store = new DocumentStore
            {
                ConnectionStringName = "ravenDb"
            };
            store.Initialize();
            return store;
        }

        private static void ConfigureLogging() =>
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.LiterateConsole()
                .CreateLogger();
    }
}