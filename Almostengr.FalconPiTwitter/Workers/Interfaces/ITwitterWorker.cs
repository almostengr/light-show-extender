using System.Threading.Tasks;

namespace Almostengr.FalconPiTwitter.Workers
{
    public interface ITwitterWorker : IBaseWorker
    { 
        Task LikeMentionedTweets();
    }
}