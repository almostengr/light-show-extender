using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Tweetinvi;
using Tweetinvi.Client;
using Tweetinvi.Core.Client.Validators;
using Tweetinvi.Core.DTO;
using Tweetinvi.Core.Models;
using Tweetinvi.Iterators;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

namespace Almostengr.FalconPiTwitter.Clients
{
    public class MockTweetsClient : ITweetsClient
    {
        private readonly ILogger<MockTweetsClient> _logger;

        public MockTweetsClient(ILogger<MockTweetsClient> logger)
        {
            _logger = logger;
        }

        public ITweetsClientParametersValidator ParametersValidator => throw new System.NotImplementedException();

        public Task DestroyRetweetAsync(long retweetId)
        {
            throw new System.NotImplementedException();
        }

        public Task DestroyRetweetAsync(ITweetIdentifier retweet)
        {
            throw new System.NotImplementedException();
        }

        public Task DestroyRetweetAsync(IDestroyRetweetParameters parameters)
        {
            throw new System.NotImplementedException();
        }

        public Task DestroyTweetAsync(long tweetId)
        {
            throw new System.NotImplementedException();
        }

        public Task DestroyTweetAsync(ITweetIdentifier tweet)
        {
            throw new System.NotImplementedException();
        }

        public Task DestroyTweetAsync(ITweet tweet)
        {
            throw new System.NotImplementedException();
        }

        public Task DestroyTweetAsync(ITweetDTO tweet)
        {
            throw new System.NotImplementedException();
        }

        public Task DestroyTweetAsync(IDestroyTweetParameters parameters)
        {
            throw new System.NotImplementedException();
        }

        public Task FavoriteTweetAsync(long tweetId)
        {
            throw new System.NotImplementedException();
        }

        public Task FavoriteTweetAsync(ITweetIdentifier tweet)
        {
            throw new System.NotImplementedException();
        }

        public Task FavoriteTweetAsync(ITweet tweet)
        {
            throw new System.NotImplementedException();
        }

        public Task FavoriteTweetAsync(ITweetDTO tweet)
        {
            throw new System.NotImplementedException();
        }

        public Task FavoriteTweetAsync(IFavoriteTweetParameters parameters)
        {
            throw new System.NotImplementedException();
        }

        public Task<IOEmbedTweet> GetOEmbedTweetAsync(ITweetIdentifier tweet)
        {
            throw new System.NotImplementedException();
        }

        public Task<IOEmbedTweet> GetOEmbedTweetAsync(long tweetId)
        {
            throw new System.NotImplementedException();
        }

        public Task<IOEmbedTweet> GetOEmbedTweetAsync(IGetOEmbedTweetParameters parameters)
        {
            throw new System.NotImplementedException();
        }

        public Task<long[]> GetRetweeterIdsAsync(long tweetId)
        {
            throw new System.NotImplementedException();
        }

        public Task<long[]> GetRetweeterIdsAsync(ITweetIdentifier tweet)
        {
            throw new System.NotImplementedException();
        }

        public Task<long[]> GetRetweeterIdsAsync(IGetRetweeterIdsParameters parameters)
        {
            throw new System.NotImplementedException();
        }

        public ITwitterIterator<long> GetRetweeterIdsIterator(long tweetId)
        {
            throw new System.NotImplementedException();
        }

        public ITwitterIterator<long> GetRetweeterIdsIterator(ITweetIdentifier tweet)
        {
            throw new System.NotImplementedException();
        }

        public ITwitterIterator<long> GetRetweeterIdsIterator(IGetRetweeterIdsParameters parameters)
        {
            throw new System.NotImplementedException();
        }

        public Task<ITweet[]> GetRetweetsAsync(long tweetId)
        {
            throw new System.NotImplementedException();
        }

        public Task<ITweet[]> GetRetweetsAsync(ITweetIdentifier tweet)
        {
            throw new System.NotImplementedException();
        }

        public Task<ITweet[]> GetRetweetsAsync(IGetRetweetsParameters parameters)
        {
            throw new System.NotImplementedException();
        }

        public Task<ITweet> GetTweetAsync(long tweetId)
        {
            throw new System.NotImplementedException();
        }

        public Task<ITweet> GetTweetAsync(IGetTweetParameters parameters)
        {
            throw new System.NotImplementedException();
        }

        public Task<ITweet[]> GetTweetsAsync(long[] tweetIds)
        {
            throw new System.NotImplementedException();
        }

        public Task<ITweet[]> GetTweetsAsync(ITweetIdentifier[] tweets)
        {
            throw new System.NotImplementedException();
        }

        public Task<ITweet[]> GetTweetsAsync(IGetTweetsParameters parameters)
        {
            throw new System.NotImplementedException();
        }

        public Task<ITweet[]> GetUserFavoriteTweetsAsync(long userId)
        {
            throw new System.NotImplementedException();
        }

        public Task<ITweet[]> GetUserFavoriteTweetsAsync(string username)
        {
            throw new System.NotImplementedException();
        }

        public Task<ITweet[]> GetUserFavoriteTweetsAsync(IUserIdentifier user)
        {
            throw new System.NotImplementedException();
        }

        public Task<ITweet[]> GetUserFavoriteTweetsAsync(IGetUserFavoriteTweetsParameters parameters)
        {
            throw new System.NotImplementedException();
        }

        public ITwitterIterator<ITweet, long?> GetUserFavoriteTweetsIterator(long userId)
        {
            throw new System.NotImplementedException();
        }

        public ITwitterIterator<ITweet, long?> GetUserFavoriteTweetsIterator(string username)
        {
            throw new System.NotImplementedException();
        }

        public ITwitterIterator<ITweet, long?> GetUserFavoriteTweetsIterator(IUserIdentifier user)
        {
            throw new System.NotImplementedException();
        }

        public ITwitterIterator<ITweet, long?> GetUserFavoriteTweetsIterator(IGetUserFavoriteTweetsParameters parameters)
        {
            throw new System.NotImplementedException();
        }

        public Task<ITweet> PublishRetweetAsync(long tweetId)
        {
            throw new System.NotImplementedException();
        }

        public Task<ITweet> PublishRetweetAsync(ITweetIdentifier tweet)
        {
            throw new System.NotImplementedException();
        }

        public Task<ITweet> PublishRetweetAsync(IPublishRetweetParameters parameters)
        {
            throw new System.NotImplementedException();
        }

        public async Task<ITweet> PublishTweetAsync(string text)
        {
            _logger.LogDebug(text);

            return await Task.Run(() => new Tweet(
                new TweetDTO
                {
                    Id = 1,
                    CreatedAt = DateTime.Now,
                    Text = text,
                },
                TweetMode.Extended,
                new TwitterClient(string.Empty, string.Empty)
            ));
        }

        public Task<ITweet> PublishTweetAsync(IPublishTweetParameters parameters)
        {
            throw new System.NotImplementedException();
        }

        public Task UnfavoriteTweetAsync(long tweetId)
        {
            throw new System.NotImplementedException();
        }

        public Task UnfavoriteTweetAsync(ITweetIdentifier tweet)
        {
            throw new System.NotImplementedException();
        }

        public Task UnfavoriteTweetAsync(ITweet tweet)
        {
            throw new System.NotImplementedException();
        }

        public Task UnfavoriteTweetAsync(ITweetDTO tweet)
        {
            throw new System.NotImplementedException();
        }

        public Task UnfavoriteTweetAsync(IUnfavoriteTweetParameters parameters)
        {
            throw new System.NotImplementedException();
        }
    }
}