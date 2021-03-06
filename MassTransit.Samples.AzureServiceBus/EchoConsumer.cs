using System;
using System.Threading.Tasks;

namespace MassTransit.Samples.AzureServiceBus
{
    public class EchoConsumer : IConsumer<EchoDataObject>
    {
        public EchoConsumer()
        {
            Console.WriteLine("Consumer Instantiated");
        }

        public Task Consume(ConsumeContext<EchoDataObject> context)
        {
            Console.WriteLine($"Message Received as: {context.Message.Sent}");
            Console.WriteLine();

            return Task.CompletedTask;
        }
    }
}
