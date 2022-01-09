using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Almostengr.FalconPiTwitter.Constants;
using Almostengr.FalconPiTwitter.DataTransferObjects;

namespace Almostengr.FalconPiTwitter.Clients
{
    public class MockFppClient : IFppClient
    {
        private readonly Random _random = new Random();
        private const string FppVersion = "4.6.1";
        private List<FalconFppdMultiSyncSystemsDto> _multiSyncDtos = new List<FalconFppdMultiSyncSystemsDto>();
        private List<FalconFppdStatusDto> _fppdStatusDtos = new List<FalconFppdStatusDto>();
        private List<FalconMediaMetaDto> _mediaMetaDtos = new List<FalconMediaMetaDto>();

        public MockFppClient()
        {
            for (int i = 0; i < _random.Next(4, 10); i++)
            {
                FalconFppdMultiSyncSystemsDto newDto = new FalconFppdMultiSyncSystemsDto()
                {
                    RemoteSystems = new List<RemoteSystems>()
                    {
                        new RemoteSystems()
                        {
                            Address = $"testing{i}",
                            Version = FppVersion,
                            FppModeString = i == 0 ? FppMode.Master : FppMode.Remote,
                            Hostname = $"fpptest{i}"
                        }
                    }
                };

                _multiSyncDtos.Add(newDto);
            }

            for (int i = 0; i < _random.Next(4, 10); i++)
            {
                FalconFppdStatusDto newDto = new FalconFppdStatusDto()
                {
                    Current_PlayList = new FalconFppdStatusCurrentPlayList()
                    {
                        Playlist = $"testing{i}"
                    },
                };

                _fppdStatusDtos.Add(newDto);
            }

            for (int i = 0; i < _random.Next(4, 10); i++)
            {
                FalconMediaMetaDto newDto = new FalconMediaMetaDto()
                {
                    Format = new FalconMediaMetaFormat()
                    {
                        Tags = new FalconMediaMetaFormatTags()
                        {
                            Title = $"Title{i}",
                            Artist = $"Artist{i}",
                            Album = $"Album{i}"
                        }
                    }
                };

                _mediaMetaDtos.Add(newDto);
            }
        }

        public async Task<FalconMediaMetaDto> GetCurrentSongMetaDataAsync(string current_Song)
        {
            // return await Task.Run(() => new FalconMediaMetaDto());
            int ranNumber = _random.Next(0, _mediaMetaDtos.Count)-1;
            return await Task.Run(() => _mediaMetaDtos[ranNumber]);
        }

        public async Task<FalconFppdStatusDto> GetFppdStatusAsync(string address)
        {
            // return await Task.Run(() => new FalconFppdStatusDto()
            // {
            //     Current_PlayList = new FalconFppdStatusCurrentPlayList()
            //     {
            //         Playlist = "offline"
            //     },
            //     Current_Song = "Current_Song",
            //     Fppd = FppVersion,
            //     Mode_Name = FppMode.Master,
            //     Next_Playlist = new FalconFppdStatusNextPlaylist()
            //     {
            //         Start_Time = "Start_Time"
            //     },
            //     Seconds_Played = _random.Next(0, 100).ToString(),
            //     Seconds_Remaining = _random.Next(0, 100).ToString(),
            //     Sensors = new List<FalconFppdStatusSensor>()
            //     {
            //         new FalconFppdStatusSensor()
            //         {
            //             Label = "temperature",
            //             Value = _random.NextDouble(),
            //             ValueType = "30"
            //         }
            //     },
            //     Status_Name = "playing"
            // });
            int ranNumber = _random.Next(0, _fppdStatusDtos.Count)-1;
            return await Task.Run(() => _fppdStatusDtos[ranNumber]);
        }

        public async Task<FalconFppdMultiSyncSystemsDto> GetMultiSyncStatusAsync(string address)
        {
            int ranNumber = _random.Next(0, _multiSyncDtos.Count)-1;
            return await Task.Run(() => _multiSyncDtos[ranNumber]);
        }

    }
}