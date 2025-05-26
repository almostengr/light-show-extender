using Almostengr.Common.DomainServices.Interfaces;
using Almostengr.HpLightShow.Core.Countdowns.DomainServices.Resources;

namespace Almostengr.HpLightShow.WebApi.Countdowns.DomainServices.Interfaces;

public interface ICountdownService : ICommandService<HolidayCountdownResource>;
