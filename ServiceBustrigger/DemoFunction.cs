using Azure.Identity;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace ServiceBustrigger
{
    public class DemoFunction
    {
        
        ServiceBusClient client;
        ServiceBusSender sender;
        ServiceBusReceiver receiver;

        [FunctionName("sendingfunction")]
        public async Task Run([ServiceBusTrigger("%" + Constant.SendingTopicName + "%" , "%" + Constant.SubscriptionName + "%", Connection = Constant.ServicebusConectionstring)]Message sendingMessage)  
        {

            var serviceBusConection = Environment.GetEnvironmentVariable(Constant.ServicebusConectionstring);
            var receivingTopicName = Environment.GetEnvironmentVariable(Constant.ReceivingTopicName);


            client = new ServiceBusClient(serviceBusConection);
            sender = client.CreateSender(receivingTopicName);
            receiver = client.CreateReceiver(receivingTopicName);

            
            ServiceBusMessage message = new ServiceBusMessage(sendingMessage.ToString());
            await sender.SendMessageAsync(message);
        
        }
    }
}
