using System.Threading.Tasks;
using NLemos.EventHub;

namespace NLemos.Api.Gateway.Services
{
    public class GatewayService : IGatewayService
    {
        private readonly IEventHubHandler _eventHub;

        public GatewayService(IEventHubHandler eventHub)
        {
            _eventHub = eventHub;
        }

        public Task SendMessage(string body)
        {
            _eventHub.SendRawEvent(Config.GatewayQueue, body);
            return Task.CompletedTask;
        }
    }
}
