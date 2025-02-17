using Almostengr.HpLightShow.Core.FalconPiPlayer.Domain;
using Almostengr.HpLightShow.Core.FalconPiPlayer.DomainServices.Interfaces;
using Almostengr.HpLightShow.Core.FalconPiPlayer.Shared;

namespace Almostengr.HpLightShow.Core.FalconPiPlayer.Infrastructure;

internal sealed class FppSequenceRepository : IFppSequenceRepository
{
    private readonly FppAppSettings _appSettings;

    public FppSequenceRepository(FppAppSettings appSettings)
    {
        _appSettings = appSettings;
    }

    public IList<SequenceRule> GetSequenceRules(int currentYear)
    {
        List<DayOfWeek> threeDayWeekend = [DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday, DayOfWeek.Monday];
        List<DayOfWeek> twoDayWeekend = [DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday];

        List<SequenceRule> rules =
        [
            // dynamic holidays
            new SequenceRule(new DateOnly(currentYear, 1, 15), new DateOnly(currentYear, 1, 21), LightingSequence.Green, threeDayWeekend), // mlk day
            new SequenceRule(new DateOnly(currentYear, 2, 15), new DateOnly(currentYear, 2, 21), LightingSequence.RedBlue, threeDayWeekend), // presidents day
            new SequenceRule(_appSettings.FatTuesdayDate, _appSettings.FatTuesdayDate, LightingSequence.Yellow, [DayOfWeek.Tuesday]), // fat tuesday, conditional on moon phases
            new SequenceRule(_appSettings.EasterStartDate, _appSettings.EasterEndDate, LightingSequence.PinkCyanYellow, twoDayWeekend), // easter, conditional on moon phases
            new SequenceRule(new DateOnly(currentYear, 5, 8), new DateOnly(currentYear, 5, 14), LightingSequence.RedPink,  twoDayWeekend), // mothers day
            new SequenceRule(new DateOnly(currentYear, 5, 28), new DateOnly(currentYear, 5, 31), LightingSequence.RedBlue, threeDayWeekend), // memorial day
            new SequenceRule(new DateOnly(currentYear, 6, 15), new DateOnly(currentYear, 6, 21), LightingSequence.Blue, twoDayWeekend), // fathers day
            new SequenceRule(new DateOnly(currentYear, 6, 15), new DateOnly(currentYear, 6, 21), LightingSequence.RedBlue, [DayOfWeek.Monday]), // juneteenth
            new SequenceRule(new DateOnly(currentYear, 8, 28), new DateOnly(currentYear, 9, 7), LightingSequence.RedBlue, threeDayWeekend), // labor day
            new SequenceRule(new DateOnly(currentYear, 11, 22), new DateOnly(currentYear, 11, 28), LightingSequence.Yellow, [DayOfWeek.Thursday]), // thanksgiving
            new SequenceRule(new DateOnly(currentYear, 11, 22), new DateOnly(currentYear, 11, 28), LightingSequence.ChristmasShow), 

            // static holidays
            new SequenceRule(new DateOnly(currentYear, 1, 1), new DateOnly(currentYear, 1, 1), LightingSequence.RedBlue), // new years day
            new SequenceRule(new DateOnly(currentYear, 2, 14), new DateOnly(currentYear, 2, 14), LightingSequence.RedPink), // valentines day 
            new SequenceRule(new DateOnly(currentYear, 3, 17), new DateOnly(currentYear, 3, 17), LightingSequence.Green), // st patricks day
            new SequenceRule(new DateOnly(currentYear, 4, 22), new DateOnly(currentYear, 4, 22), LightingSequence.BlueGreen), // earth day
            new SequenceRule(new DateOnly(currentYear, 5, 5), new DateOnly(currentYear, 5, 5), LightingSequence.RedGreen), // cinco de mayo
            new SequenceRule(new DateOnly(currentYear, 6, 14), new DateOnly(currentYear, 6, 14), LightingSequence.RedBlue), // flag day
            new SequenceRule(new DateOnly(currentYear, 6, 27), new DateOnly(currentYear, 7, 4), LightingSequence.IndependenceDayShow), // independence day light show
            new SequenceRule(new DateOnly(currentYear, 11, 11), new DateOnly(currentYear, 11, 11), LightingSequence.RedBlue), // veterans day
            new SequenceRule(new DateOnly(currentYear, 12, 1), new DateOnly(currentYear, 12, 31), LightingSequence.ChristmasShow), // christmas light show

            // months
            new SequenceRule(new DateOnly(currentYear, 2, 1), new DateOnly(currentYear, 2, 29), LightingSequence.RedGreen), // black history month
            new SequenceRule(new DateOnly(currentYear, 4, 1), new DateOnly(currentYear, 2, 30), LightingSequence.Blue),
            new SequenceRule(new DateOnly(currentYear, 5, 1), new DateOnly(currentYear, 5, 31), LightingSequence.Green),
            new SequenceRule(new DateOnly(currentYear, 10, 1), new DateOnly(currentYear, 10, 31), LightingSequence.Pink), // breast cancer awareness
        ];

        return rules;
    }
}
