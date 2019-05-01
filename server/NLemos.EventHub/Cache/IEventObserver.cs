using System.Threading.Tasks;

namespace NLemos.EventHub.Cache
{
    internal interface IEventObserver
    {
        Task Invoke(object body);

        object Deserialize(string json);
    }
}
