using System;
using System.Threading.Tasks;
using Demo.MassTransit.Messages;
using MassTransit;

namespace Demo.MassTransit
{
    public class HelloWorldConsumer : IConsumer<HelloWorld>
    {
        private string _whatToAdd;
        private static Random Random = new Random();

        public HelloWorldConsumer(string whatToAdd) =>
            _whatToAdd = whatToAdd;
        
        public async Task Consume(ConsumeContext<HelloWorld> context)
        {
            var wantToFail = Random.NextDouble() >= 0.5;
            if (wantToFail)
                throw new Exception("I am just sick of it");
            
            await Console.Out.WriteLineAsync(context.Message.Text);
            await context.RespondAsync(new HelloResponse
            {
                AmendedText = _whatToAdd + context.Message.Text
            });
        }
    }
}