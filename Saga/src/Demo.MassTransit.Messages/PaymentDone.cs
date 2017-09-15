using System;

namespace Demo.MassTransit.Messages
{
    public class PaymentDone
    {
        public Guid OrderId { get; set; }
    }
}