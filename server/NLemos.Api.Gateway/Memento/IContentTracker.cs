namespace NLemos.Api.Gateway.Memento
{
    public interface IContentTracker
    {
        string AddTracking(string body, string userId);
    }
}
