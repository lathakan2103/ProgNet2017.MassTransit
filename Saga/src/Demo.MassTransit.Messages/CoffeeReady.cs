using System;

namespace Demo.MassTransit.Messages
{
    public class CoffeeReady
    {
        public Guid OrderId { get; set; }
    }
}