using System;

namespace Demo.MassTransit.Messages
{
    public class BounceCustomer
    {
        public Guid OrderId { get; set; }
        public string CustomerName { get; set; }
    }
}