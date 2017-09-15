using System;

namespace Demo.MassTransit.Messages
{
    public class PaymentFailed
    {
        public Guid OrderId { get; set; }
    }
}