using System;
using System.Threading.Tasks;
using MassTransit.Azure.ServiceBus.Core;

namespace MassTransit.Samples.AzureServiceBus
{
    class Program
    {
        /* To get this Host value:
         * 1. Navigate in a browser to portal.azure.com.
         * 2. Navigate to the Azure Service Bus instance you have created for this example.
         * 3. Under the 'Settings' submenu on the left of the screen, click on 'Shared Access Policies', which will have a small golden key icon next to it.
         * 4. On the table being displayed in the middle of the screen, click on the policy item 'RootManageSharedAccessKey'.
         * 5. Copy the 'Primary Connection String' shown on the left part of the screen and paste the value for public const string Host.
         */
        public const string Host = "INSERT-HOST-NAME-HERE";
        
        //The Topic and TopicSubscription will be created auto-magically by MassTransit if they don't exist.
        //It's worth noting you can provide no names and MassTransit will create these using it's own naming conventions.
        public const string TopicName = "echo-example-topic";
        public const string TopicSubscriptionEndpoint = "echo-example-subscription";

        static async Task Main(string[] args)
        {
            var bus = Bus.Factory.CreateUsingAzureServiceBus(ConfigureBus);
            await bus.StartAsync();

            Console.WriteLine("What message would you like to send?");
            var messageToSend = Console.ReadLine();

            //Publish Message
            Console.WriteLine("Sending Message");
            await bus.Publish(new EchoDataObject { Sent = messageToSend });
            Console.WriteLine("Message Sent");

            //Wait for response
            Console.WriteLine("Waiting on Response");
            Console.ReadLine();

            await bus.StopAsync();
        }

        /// <summary>
        /// Configures the Azure Service Bus host instance with the topic and sets up a subscription consumer to retrieve a message from the topic.
        /// </summary>
        /// <param name="configurator">The MassTransit IServiceBusBusFactoryConfigurator configurator</param>
        private static void ConfigureBus(IServiceBusBusFactoryConfigurator configurator)
        {
            configurator.Host(Host);
            configurator.Message<EchoDataObject>(c => c.SetEntityName(TopicName));
            configurator.SubscriptionEndpoint<EchoDataObject>(TopicSubscriptionEndpoint, e => e.Consumer<EchoConsumer>());

            Console.WriteLine("Bus Configured.");
        }
    }
}
