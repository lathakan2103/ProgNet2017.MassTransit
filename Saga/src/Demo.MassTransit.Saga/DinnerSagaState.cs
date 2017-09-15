using System;
using Automatonymous;

namespace Demo.MassTransit.Saga
{
    public class DinnerSagaState : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }
        public string CurrentState { get; set; }
        public string CustomerName { get; set; }
        public string Food { get; set; }
        public string Coffee { get; set; }
        public int OrderTotal { get; set; }
        public bool IsPaid { get; set; }
        public int OrderStatus { get; set; }
    }
}