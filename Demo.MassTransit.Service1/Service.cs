using System;
using MassTransit;
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
                cfg.Host(new Uri("rabbitmq://localhost"), h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });
            });
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