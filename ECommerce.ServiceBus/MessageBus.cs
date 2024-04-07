using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.ServiceBus
{
    public class MessageBus : IMessageBus
    {
        private string _connectionString = string.Empty;

        public void SetConnetionString(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task PublishMessage(object message, string topic_queue_Name)
        {
            if (!String.IsNullOrEmpty(_connectionString))
            {
                await using var client = new ServiceBusClient(_connectionString);

                ServiceBusSender sender = client.CreateSender(topic_queue_Name);

                var jsonMessage = JsonConvert.SerializeObject(message);
                ServiceBusMessage finalMessage = new ServiceBusMessage(Encoding.UTF8.GetBytes(jsonMessage))
                {
                    CorrelationId = Guid.NewGuid().ToString(),
                };

                await sender.SendMessageAsync(finalMessage);
                await client.DisposeAsync();
            }
        }
    }
}
