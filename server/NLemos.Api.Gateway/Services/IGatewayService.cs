using System.Threading.Tasks;

namespace NLemos.Api.Gateway.Services
{
    public interface IGatewayService
    {
        Task SendMessage(string body);
    }
}
