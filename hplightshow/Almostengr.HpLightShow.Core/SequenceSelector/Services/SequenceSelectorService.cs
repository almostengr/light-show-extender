using Almostengr.HpLightShow.Core.FalconPiPlayer.Common;
using Almostengr.HpLightShow.Core.SequenceSelector.DataTransferObjects;
using Almostengr.HpLightShow.Core.SequenceSelector.Enums;
using Almostengr.HpLightShow.Core.SequenceSelector.Services;

namespace Almostengr.HpLightShow.Core.Common;

public sealed class SequenceSelectorService : BaseService, ISequenceSelectorService
{
    private readonly HpLightShowAppSettings _appSettings;

    public SequenceSelectorService(HttpClient httpClient, HpLightShowAppSettings appSettings) : base(httpClient)
    {
        _appSettings = appSettings;
    }

    public async Task ExecuteAsync(SequenceSelectorDto selectorDto)
    {
        if (!string.IsNullOrWhiteSpace(_appSettings.SequenceSelector.SequenceOverride))
        {
            await StartPlaylistAsync(_appSettings.SequenceSelector.SequenceOverride);
            return;
        }

        IList<SequenceRule> rules = GetSequenceRules(selectorDto.CurrentDate.Year);
        string selectedSequence = LightingSequence.Blue.Value;
        foreach (var rule in rules)
        {
            if (selectorDto.CurrentDate >= rule.StartDate &&
                selectorDto.CurrentDate <= rule.EndDate &&
                (rule.DaysOfWeek == null || rule.DaysOfWeek.Contains(selectorDto.CurrentDate.DayOfWeek)))
            {
                selectedSequence = rule.Sequence.Value;
                break;
            }
        }

        await StartPlaylistAsync(selectedSequence);
    }

    private IList<SequenceRule> GetSequenceRules(int currentYear)
    {
        List<DayOfWeek> threeDayWeekend = [DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday, DayOfWeek.Monday];
        List<DayOfWeek> twoDayWeekend = [DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday];

        List<SequenceRule> rules =
        [
            // dynamic holidays
            new SequenceRule(new DateOnly(currentYear, 1, 15), new DateOnly(currentYear, 1, 21), LightingSequence.Green, threeDayWeekend), // mlk day
            new SequenceRule(new DateOnly(currentYear, 2, 15), new DateOnly(currentYear, 2, 21), LightingSequence.RedBlue, threeDayWeekend), // presidents day
            new SequenceRule(_appSettings.SequenceSelector.FatTuesdayDate, _appSettings.SequenceSelector.FatTuesdayDate, LightingSequence.Yellow, [DayOfWeek.Tuesday]), // fat tuesday, conditional on moon phases
            new SequenceRule(_appSettings.SequenceSelector.EasterStartDate, _appSettings.SequenceSelector.EasterEndDate, LightingSequence.PinkCyanYellow, twoDayWeekend), // easter, conditional on moon phases
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
