using System;

namespace Demo.MassTransit.Messages
{
    public class TakePayment
    {
        public Guid OrderId { get; set; }
        public string CustomerName { get; set; }
        public int Amount { get; set; }
    }
}