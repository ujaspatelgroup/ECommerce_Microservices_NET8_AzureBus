using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.ServiceBus
{
    public interface IMessageBus
    {
        public Task PublishMessage(object message, string topic_queue_Name);

        public void SetConnetionString(string connectionString);
    }
}
