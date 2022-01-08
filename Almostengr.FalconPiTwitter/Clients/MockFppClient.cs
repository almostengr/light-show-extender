using System;
using System.Threading.Tasks;
using Almostengr.FalconPiTwitter.DataTransferObjects;

namespace Almostengr.FalconPiTwitter.Clients
{
    public class MockFppClient : IFppClient
    {
        private readonly Random _random = new Random();

        public async Task<FalconMediaMetaDto> GetCurrentSongMetaDataAsync(string current_Song)
        {
            return await Task.Run(() => new FalconMediaMetaDto());
        }

        public async Task<FalconFppdStatusDto> GetFppdStatusAsync(string address)
        {
            return await Task.Run(() => new FalconFppdStatusDto());
        }

        public async Task<FalconFppdMultiSyncSystemsDto> GetMultiSyncStatusAsync(string address)
        {
            return await Task.Run(() => new FalconFppdMultiSyncSystemsDto());
        }
    }
}