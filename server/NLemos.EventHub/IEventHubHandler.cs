using System;
using System.Threading.Tasks;

namespace NLemos.EventHub
{
    public interface IEventHubHandler : IDisposable
    {
        void OnEventReceived<TEventType>(string queue, Func<TEventType, Task> onEventReceived);

        void SendEvent(string queue, object body);

        void SendRawEvent(string queue, string body);
    }
}
