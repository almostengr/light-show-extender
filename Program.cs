using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Almostengr.FalconPiMonitor
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // PiMonitor piMonitor = new PiMonitor();
            // while (true)
            // {
            //     piMonitor.PerformChecks();
            // }

            FalconApi falconApi = new FalconApi();
            var songName = await falconApi.GetCurrentSong();

            Console.WriteLine(songName);
        }
    }
}
