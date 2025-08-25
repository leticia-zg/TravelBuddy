using TravelBuddy.Shared.Contracts.Destinations;
using System.Threading.Tasks;
using System.Threading;
using System;

namespace TravelBuddy.Application.Destinations
{
    public interface IDestinationService
    {
        Task<IReadOnlyList<DestinationDto>> GetAllAsync(CancellationToken ct = default);
        Task<DestinationDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<Guid> CreateAsync(CreateDestinationRequest request, CancellationToken ct = default);
    }
}
