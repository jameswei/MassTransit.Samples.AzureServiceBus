using System;
using System.Threading.Tasks;

namespace MassTransit.Samples.AzureServiceBus
{
    // 实现 IConsumer 定义 consumer
    public class EchoConsumer : IConsumer<EchoDataObject>
    {
        public EchoConsumer()
        {
            Console.WriteLine("Consumer Instantiated");
        }

        // message 被包装成 ConsumerContext，相比直接从 ASB 获得的 message，它包含了 message headers
        public Task Consume(ConsumeContext<EchoDataObject> context)
        {
            Console.WriteLine($"Message Received as: {context.Message.Msg}");
            Console.WriteLine();

            return Task.CompletedTask;
        }
    }
}
