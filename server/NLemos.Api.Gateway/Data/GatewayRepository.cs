namespace NLemos.Api.Gateway.Data
{
    public class GatewayRepository : IGatewayRepository
    {
        private readonly GatewayContext _context;

        public GatewayRepository(GatewayContext context)
        {
            _context = context;
        }
    }
}
