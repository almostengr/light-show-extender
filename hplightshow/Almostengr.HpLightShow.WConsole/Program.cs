using Almostengr.Common.DomainServices.Results;
using Almostengr.HpLightShow.Core.FalconPiPlayer.DomainServices;
using Almostengr.HpLightShow.Core.FalconPiPlayer.DomainServices.Infrastructure;
using Almostengr.HpLightShow.Core.FalconPiPlayer.DomainServices.Interfaces;
using Almostengr.HpLightShow.Core.FalconPiPlayer.Infrastructure;
using Almostengr.HpLightShow.Core.FalconPiPlayer.Shared;

namespace Almostengr.HpLightShow.WConsole;

class Program
{
    static async Task Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("No arguments provided.");
            return;
        }

        for (int i = 0; i < args.Length; i++)
        {
            switch (args[i])
            {
                case "--landscape":
                    HttpClient httpClient = new();
                    FppAppSettings fppAppSettings = new(); // load configuration in the future 
                    IFppClient fppClient = new FppClient(httpClient, fppAppSettings);

                    IFppSequenceRepository fppSequenceRepository = new FppSequenceRepository(fppAppSettings);
                    IFppStartSequenceService fppStartSequenceService = new FppStartSequenceService(fppAppSettings, fppSequenceRepository, fppClient);

                    DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);
                    SequenceSelectorResource resource = new(currentDate);

                    Result<SequenceSelectorResource> result = await fppStartSequenceService.ExecuteAsync(resource);
                    if (result.Failed)
                    {
                        Console.WriteLine("Unable to start landscape lighting.");
                        return;
                    }

                    Console.WriteLine("Landscape lighting has been started.");
                    return;

                default:
                    break;
            }
        }
    }
}
