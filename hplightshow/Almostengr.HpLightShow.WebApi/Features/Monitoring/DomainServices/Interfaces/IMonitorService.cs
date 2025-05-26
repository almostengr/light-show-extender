using Almostengr.Common.DomainServices.Interfaces;
using Almostengr.HpLightShow.WebApi.Features.Monitoring.DomainServices.Resources;

namespace Almostengr.HpLightShow.WebApi.Features.Monitoring.DomainServices.Interfaces;

internal interface IMonitorService : ICommandService<MonitorResource>;
