using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NLemos.EventHub.Cache;
using NLemos.EventHub.Configuration;

namespace NLemos.EventHub
{
    public class EventHubHandler : IEventHubHandler, IDisposable
    {
        private readonly EventHubConnectionBuilder _connectionBuilder;
        private readonly Dictionary<string, HandlerCache> _cache = new Dictionary<string, HandlerCache>();

        public EventHubHandler(EventHubConnectionBuilder connectionBuilder)
        {
            _connectionBuilder = connectionBuilder;
        }

        private HandlerCache GetCacheInstance(string queue)
        {
            if (string.IsNullOrWhiteSpace(queue))
            {
                throw new ArgumentException("Queue name cannot be empty");
            }

            if (_cache.ContainsKey(queue))
            {
                return _cache[queue];
            }
            else
            {
                var serviceConnection = _connectionBuilder.GetServiceConnection(queue);
                if (serviceConnection == null)
                {
                    throw new ArgumentException($"No service connection found for queue {queue}");
                }

                var cache = new HandlerCache(queue, serviceConnection);
                _cache[queue] = cache;
                return cache;
            }
        }

        public void OnEventReceived<TEventType>(string queue, Func<TEventType, Task> onEventReceived)
        {
            var cache = GetCacheInstance(queue);
            cache.OpenConnection();
            cache.AddEventObserver(onEventReceived);
        }

        public void SendEvent(string queue, object body)
        {
            var cache = GetCacheInstance(queue);
            cache.OpenConnection();
            cache.Publish(body);
        }

        public void SendRawEvent(string queue, string body)
        {
            var cache = GetCacheInstance(queue);
            cache.OpenConnection();
            cache.PublishRaw(body);
        }

        public void Dispose()
        {
            for (var i = _cache.Count - 1; i >= 0; i--)
            {
                var key = _cache.Keys.ElementAt(i);
                _cache[key].Dispose();
                _cache.Remove(key);
            }
        }
    }
}
