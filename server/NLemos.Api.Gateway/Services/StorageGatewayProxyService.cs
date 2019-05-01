using System.Threading.Tasks;
using NLemos.Api.Gateway.Data;

namespace NLemos.Api.Gateway.Services
{
    public class StorageGatewayProxyService : IGatewayService
    {
        private readonly IGatewayRepository _repository;
        private readonly IGatewayService _instance;

        public StorageGatewayProxyService(IGatewayRepository repository, GatewayService instance)
        {
            _repository = repository;
            _instance = instance;
        }

        public Task SendMessage(string body)
        {
            return _instance.SendMessage(body);
        }
    }
}
