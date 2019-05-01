using System;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NLemos.EventHub.Cache
{
    internal class EventObserver<TEventType> : IEventObserver
    {
        private Func<TEventType, Task> _onEventReceived;

        private EventObserver()
        {
        }

        public object Deserialize(string json)
        {
            return JsonConvert.DeserializeObject<TEventType>(json);
        }

        public Task Invoke(object body)
        {
            return _onEventReceived.Invoke((TEventType)body);
        }

        internal static IEventObserver Create(Func<TEventType, Task> onEventReceived)
        {
            return new EventObserver<TEventType>
            {
                _onEventReceived = onEventReceived
            };
        }
    }
}
