using System.Threading.Tasks;

namespace Almostengr.FalconPiTwitter.Workers
{
    public interface ITwitterMentionsWorker : IBaseWorker
    { 
        Task ProcessMentionedTweets();
    }
}