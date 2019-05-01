using System;
using System.Collections.Generic;

namespace NLemos.EventHub.Configuration
{
    public class EventHubConnectionBuilder
    {
        private EventHubConnection _connection = null;
        private Dictionary<string, EventHubConnection> _connections = new Dictionary<string, EventHubConnection>();

        public EventHubConnectionBuilder AddConnection(string hostname, string username, string password, EventHubConnectionType connectionType)
        {
            _connection = new EventHubConnection
            {
                Hostname = hostname,
                Username = username,
                Password = password,
                ConnectionType = connectionType
            };

            return this;
        }

        public EventHubConnectionBuilder AddQueue(string queue)
        {
            if (_connection == null)
            {
                throw new NullReferenceException("No connection set");
            }

            if (string.IsNullOrWhiteSpace(queue))
            {
                throw new ArgumentException("Queue name cannot be empty");
            }

            _connections.Add(queue, _connection);
            return this;
        }

        public EventHubConnection GetServiceConnection(string queue)
        {
            if (_connections.ContainsKey(queue))
            {
                return _connections[queue];
            }
            else
            {
                return null;
            }
        }
    }
}
