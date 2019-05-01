using System;
using Newtonsoft.Json;

namespace NLemos.Api.Gateway.Memento
{
    public class ContentTracker : IContentTracker
    {
        public string AddTracking(string body, string userId)
        {
            var deserializedBody = JsonConvert.DeserializeObject<dynamic>(body);
            deserializedBody.__EventHubTrackingId = Guid.NewGuid();
            deserializedBody.__UserId = userId;
            deserializedBody.__SentOn = DateTimeOffset.UtcNow;

            return JsonConvert.SerializeObject(deserializedBody);
        }
    }
}
