using System;
using System.Threading.Tasks;
using Demo.MassTransit.Messages;
using GreenPipes;
using MassTransit;
using MassTransit.Util;
using Serilog;
using Topshelf;

namespace Demo.MassTransit
{
    public class Service : ServiceControl
    {
        private readonly IBusControl _bus;

        public Service()
        {
            _bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.UseSerilog();
//                cfg.Host(new Uri("rabbitmq://localhost"), h =>
//                {
//                    h.Username("guest");
//                    h.Password("guest");
//                });
                var host = cfg.Host(new Uri("rabbitmq://10.211.55.10/prognet"), h =>
                {
                    h.Username("prognet");
                    h.Password("skillsmatter");
                });
                cfg.ReceiveEndpoint(host, "test", ep =>
                {
                    ep.Consumer(() => new HelloWorldConsumer("Hahaha"),
                        cm => cm.Message<HelloWorld>(k => 
                            k.UseDelayedRedelivery(r => r.Immediate(1))));
                    ep.UseRetry(r =>
                    {
                        r.Handle<Exception>();
                        r.Immediate(5);
                    });
                });
            });
            TaskUtil.Await(() => DoWork());
        }

        private async Task DoWork()
        {
        }

        public bool Start(HostControl hostControl)
        {
            _bus.Start();
            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            _bus.Stop();
            return true;
        }
        
    }
}