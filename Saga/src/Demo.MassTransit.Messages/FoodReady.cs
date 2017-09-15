using System;

namespace Demo.MassTransit.Messages
{
    public class FoodReady
    {
        public Guid OrderId { get; set; }
    }
}