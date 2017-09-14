using System;
using Automatonymous;

namespace Demo.MassTransit.Saga
{
    public class OrderSagaState : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }
        public string CurrentState { get; set; }
    }
}