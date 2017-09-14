using Automatonymous;

namespace Demo.MassTransit.Saga
{
    public class OrderSaga : MassTransitStateMachine<OrderSagaState>
    {
        public OrderSaga()
        {
            InstanceState(x => x.CurrentState);
        }
    }
}