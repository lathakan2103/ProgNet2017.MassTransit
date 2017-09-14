using Automatonymous;

namespace Demo.MassTransit.Saga
{
    public class DinnerSaga : MassTransitStateMachine<DinnerSagaState>
    {
        public DinnerSaga()
        {
            InstanceState(x => x.CurrentState);
        }
    }
}