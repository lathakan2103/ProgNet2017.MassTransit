using System;
using Automatonymous;

namespace Demo.MassTransit.Saga
{
    public class DinnerSagaState : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }
        public string CurrentState { get; set; }
    }
}