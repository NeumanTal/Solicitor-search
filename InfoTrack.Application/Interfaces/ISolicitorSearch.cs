using InfoTrack.Domain.Entities;

namespace InfoTrack.Application.Interfaces;

public interface ISolicitorSearch
{
    Task<IEnumerable<Solicitor>> SearchByLocationsAsync(IEnumerable<string> locations, CancellationToken cancellationToken = default);
}