using System;
using System.Threading.Tasks;
using MassTransit.Azure.ServiceBus.Core;

namespace MassTransit.Samples.AzureServiceBus
{
    class Program
    {
        // 从 Azure Portal 上获得 ASB 的 connection string
        public const string Host = "INSERT-HOST-NAME-HERE";

        // topic 及 subscription name 都会由 MassTransit 自动创建
        public const string TopicName = "echo-example-topic";
        public const string TopicSubscriptionEndpoint = "echo-example-subscription";

        static async Task Main(string[] args)
        {
            // 使用 ASB 来初始化 MassTransit bus，接收一个配置 ASB 的 delegate
            var bus = Bus.Factory.CreateUsingAzureServiceBus(ConfigureBus);
            await bus.StartAsync();

            Console.WriteLine("What message would you like to send?");
            var msg = Console.ReadLine();

            //Publish Message
            Console.WriteLine("Sending Message");
            await bus.Publish(new EchoDataObject { Msg = msg });
            Console.WriteLine("Message Sent");

            //Wait for response
            Console.WriteLine("Waiting on Response");
            Console.ReadLine();

            await bus.StopAsync();
        }

        // 配置 MassTransit 的 transport provider，作为方法指针传给 MassTransit 的 bus factory
        private static void ConfigureBus(IServiceBusBusFactoryConfigurator configurator)
        {
            configurator.Host(Host);
            // 通过类型参数指定 message type，以及该 message 对应的 topic name
            configurator.Message<EchoDataObject>(c => c.SetEntityName(TopicName));
            // 指定该 message 对应的 subscription endpoint 以及 consumer
            configurator.SubscriptionEndpoint<EchoDataObject>(TopicSubscriptionEndpoint, e => e.Consumer<EchoConsumer>());
            Console.WriteLine("Bus Configured.");
        }
    }
}
