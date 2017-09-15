using System;

namespace Demo.MassTransit.Messages
{
    public class OrderPlaced
    {
        public Guid OrderId { get; set; }
        public string CustomerName { get; set; }
        public string Food { get; set; }
        public string Coffee { get; set; }
    }
}