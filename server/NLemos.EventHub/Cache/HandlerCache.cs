using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NLemos.EventHub.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace NLemos.EventHub.Cache
{
    internal class HandlerCache : IDisposable
    {
        private readonly string _queue;
        private readonly EventHubConnection _serviceConnection;
        private readonly List<IEventObserver> _eventObservers = new List<IEventObserver>();

        private IConnection _connection;
        private IModel _channelIn;
        private IModel _channelOut;

        //Client In = Server Out
        private string inQueueName => _queue + (_serviceConnection.ConnectionType == EventHubConnectionType.Client ? "In" : "Out");

        //Client Out = Server In
        private string outQueueName => _queue + (_serviceConnection.ConnectionType == EventHubConnectionType.Client ? "Out" : "In");

        internal HandlerCache(string queue, EventHubConnection serviceConnection)
        {
            _queue = queue;
            _serviceConnection = serviceConnection;
        }

        internal void OpenConnection()
        {
            if (_connection != null && _connection.IsOpen)
            {
                return;
            }

            if (_connection != null)
            {
                DisposeConnection();
            }

            var connectionFactory = new ConnectionFactory
            {
                DispatchConsumersAsync = true,
                HostName = _serviceConnection.Hostname,
                UserName = _serviceConnection.Username,
                Password = _serviceConnection.Password
            };

            _connection = connectionFactory.CreateConnection();
            SetupConsumer();
        }

        private void SetupConsumer()
        {
            if (_channelIn != null && _channelIn.IsOpen)
            {
                return;
            }

            if (_channelIn != null)
            {
                _channelIn.Dispose();
                _channelIn = null;
            }

            //Leaves the client as out, enters here as in
            _channelIn = _connection.CreateModel();
            _channelIn.QueueDeclare(inQueueName, false, false, false, null);

            var consumer = new AsyncEventingBasicConsumer(_channelIn);
            consumer.Received += async (object sender, BasicDeliverEventArgs @event) =>
            {
                var json = Encoding.UTF8.GetString(@event.Body);
                await Task.WhenAll(
                    _eventObservers.Select(e =>
                        e.Invoke(e.Deserialize(json))
                    )
                );
            };

            _channelIn.BasicConsume(inQueueName, true, consumer);
        }

        internal void AddEventObserver<TEventType>(Func<TEventType, Task> onEventReceived)
        {
            _eventObservers.Add(EventObserver<TEventType>.Create(onEventReceived));
        }

        internal void Publish(object body)
        {
            var channelOut = GetChannelOut();
            var json = JsonConvert.SerializeObject(body);
            channelOut.BasicPublish(string.Empty, outQueueName, null, Encoding.UTF8.GetBytes(json));
        }

        internal void PublishRaw(string body)
        {
            var channelOut = GetChannelOut();
            channelOut.BasicPublish(string.Empty, outQueueName, null, Encoding.UTF8.GetBytes(body));
        }

        private IModel GetChannelOut()
        {
            if (_channelOut != null && _channelOut.IsOpen)
            {
                return _channelOut;
            }

            if (_channelOut != null)
            {
                _channelOut.Dispose();
                _channelOut = null;
            }

            _channelOut = _connection.CreateModel();
            _channelOut.QueueDeclare(outQueueName, false, false, false, null);
            return _channelOut;
        }

        private void DisposeConnection()
        {
            if (_channelIn != null)
            {
                _channelIn.Dispose();
                _channelIn = null;
            }

            if (_channelOut != null)
            {
                _channelOut.Dispose();
                _channelOut = null;
            }

            if (_connection != null)
            {
                _connection.Dispose();
                _connection = null;
            }
        }

        public void Dispose()
        {
            DisposeConnection();
        }
    }
}
