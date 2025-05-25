using Almostengr.Common.DomainServices.Interfaces;
using Almostengr.HpLightShow.Core.Countdowns.DataTransferObjects;

namespace Almostengr.HpLightShow.Core.Countdowns.Service;

public interface ICountdownService : ICommandService<HolidayCountdownResource>;
