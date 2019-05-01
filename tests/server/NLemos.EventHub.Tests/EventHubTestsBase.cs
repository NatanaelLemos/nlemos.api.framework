using System.Text;
using System.Threading;
using NLemos.EventHub.Configuration;
using RabbitMQ.Client;

namespace NLemos.EventHub.Tests
{
    public class EventHubTestsBase
    {
        protected EventHubConnectionBuilder _defaultConnection;

        public EventHubTestsBase(EventHubConnectionBuilder serviceConnectionBuilder)
        {
            _defaultConnection = serviceConnectionBuilder;
        }

        public string ReceiveEvent(string queue, string queueType = "In")
        {
            var serviceConnection = _defaultConnection.GetServiceConnection(queue);

            using (var connection = CreateConnection(serviceConnection.Hostname, serviceConnection.Username, serviceConnection.Password))
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue + queueType, false, false, false, null);
                var response = channel.BasicGet(queue + queueType, true);

                if (response == null)
                {
                    return string.Empty;
                }

                return Encoding.UTF8.GetString(response.Body);
            }
        }

        public void SendEvent(string queue, string eventBody, string queueType = "Out")
        {
            var serviceConnection = _defaultConnection.GetServiceConnection(queue);

            using (var connection = CreateConnection(serviceConnection.Hostname, serviceConnection.Username, serviceConnection.Password))
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue + queueType, false, false, false, null);
                channel.BasicPublish(string.Empty, queue + queueType, null, Encoding.UTF8.GetBytes(eventBody));
            }
        }

        public void CleanPipes(string queue)
        {
            while (!string.IsNullOrWhiteSpace(ReceiveEvent(queue, "")))
            {
                Thread.Sleep(10);
            }

            while (!string.IsNullOrWhiteSpace(ReceiveEvent(queue, "Out")))
            {
                Thread.Sleep(10);
            }

            while (!string.IsNullOrWhiteSpace(ReceiveEvent(queue)))
            {
                Thread.Sleep(10);
            }
        }

        private IConnection CreateConnection(string hostname, string username, string password)
        {
            var connectionFactory = new ConnectionFactory();
            connectionFactory.HostName = hostname;
            connectionFactory.UserName = username;
            connectionFactory.Password = password;
            return connectionFactory.CreateConnection();
        }
    }
}
