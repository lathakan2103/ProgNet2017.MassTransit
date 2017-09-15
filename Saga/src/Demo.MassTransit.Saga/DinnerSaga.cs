using System;
using Automatonymous;
using Demo.MassTransit.Messages;
using RabbitMQ.Client;

namespace Demo.MassTransit.Saga
{
    public class DinnerSaga : MassTransitStateMachine<DinnerSagaState>
    {
        private static Random Random = new Random();
        
        public DinnerSaga()
        {
            InstanceState(x => x.CurrentState);
            
            Initially(
                When(OrderHasBeenPlaced)
                    .Then(c =>
                    {
                        c.Instance.CustomerName = c.Data.CustomerName;
                        c.Instance.Food = c.Data.Food;
                        c.Instance.Coffee = c.Data.Coffee;
                        c.Instance.OrderTotal = Random.Next(1, 100);
                        c.Instance.IsPaid = false;
                    })
                    .ThenAsync(c =>
                        Console.Out.WriteLineAsync($"Order for {c.Instance.CustomerName} received"))
                    .Publish(c => 
                        new TakePayment
                        {
                            OrderId = c.Instance.CorrelationId,
                            CustomerName = c.Instance.CustomerName,
                            Amount = c.Instance.OrderTotal
                        })
                    .TransitionTo(WaitingForPayment)
                );

            During(WaitingForPayment,
                When(PaymentDoneOk)
                    .Then(c => c.Instance.IsPaid = true)
                    .ThenAsync(c => 
                        Console.Out.WriteLineAsync($"Payment ok for {c.Instance.CustomerName}")),
                When(PaymentNotDone)
                    .Then(c => c.Instance.IsPaid = false)
                    .ThenAsync(c => 
                        Console.Out.WriteLineAsync($"Payment failed for {c.Instance.CustomerName}"))
                    .Publish(c => new BounceCustomer
                    {
                        OrderId = c.Instance.CorrelationId,
                        CustomerName = c.Instance.CustomerName
                    })
                    .Finalize()
                );
            
            
            Event(() => OrderHasBeenPlaced, 
                x => x.CorrelateById(c => c.Message.OrderId).SelectId(c => c.Message.OrderId));
            
            Event(() => PaymentDoneOk,
                x => x.CorrelateById(c => c.Message.OrderId));
            Event(() => PaymentNotDone,
                x => x.CorrelateById(c => c.Message.OrderId));
            Event(() => FoodIsReady,
                x => x.CorrelateById(c => c.Message.OrderId));
            Event(() => CoffeeIsReady,
                x => x.CorrelateById(c => c.Message.OrderId));
            
            CompositeEvent(() => OrderReady, x => x.OrderStatus, FoodIsReady, CoffeeIsReady);
        }
        
        public State WaitingForPayment { get; private set; }
        public State WaitingForFoodAndCoffee { get; private set; }
        
        public Event<OrderPlaced> OrderHasBeenPlaced { get; private set; }
        public Event<PaymentDone> PaymentDoneOk { get; private set; }
        public Event<PaymentFailed> PaymentNotDone { get; private set; }
        public Event<FoodReady> FoodIsReady { get; private set; }
        public Event<CoffeeReady> CoffeeIsReady { get; private set; }
        public Event OrderReady { get; private set; }
    }
}