namespace NLemos.EventHub.Configuration
{
    public class EventHubConnection
    {
        public string Hostname { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public EventHubConnectionType ConnectionType { get; set; }
    }
}
