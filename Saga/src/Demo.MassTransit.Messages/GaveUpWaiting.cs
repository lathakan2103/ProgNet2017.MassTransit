using System;

namespace Demo.MassTransit.Messages
{
    public class GaveUpWaiting
    {
        public Guid OrderId { get; set; }
    }
}