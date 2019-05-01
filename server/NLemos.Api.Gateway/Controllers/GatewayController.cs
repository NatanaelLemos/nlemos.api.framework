using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NLemos.Api.Framework.Extensions.Controllers;
using NLemos.Api.Gateway.Memento;
using NLemos.Api.Gateway.Services;

namespace NLemos.Api.Gateway.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    [Authorize]
    public class GatewayController : ControllerBase
    {
        private readonly IGatewayService _service;
        private readonly IContentTracker _contentTracker;

        public GatewayController(IGatewayService service, IContentTracker contentTracker)
        {
            _service = service;
            _contentTracker = contentTracker;
        }

        [HttpPost]
        public async Task<IActionResult> Post()
        {
            using (var reader = new StreamReader(Request.Body))
            {
                var body = await reader.ReadToEndAsync();
                body = _contentTracker.AddTracking(body, this.GetUserEmail());

                await _service.SendMessage(body);

                return Ok();
            }
        }
    }
}
