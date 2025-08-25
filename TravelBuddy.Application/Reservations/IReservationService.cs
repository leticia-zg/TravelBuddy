using TravelBuddy.Shared.Contracts.Reservations;
using System.Threading.Tasks;
using System.Threading;
using System;

namespace TravelBuddy.Application.Reservations
{
    public interface IReservationService
    {
        Task<IReadOnlyList<ReservationDto>> GetAllAsync(CancellationToken ct = default);
        Task<ReservationDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<Guid> CreateAsync(CreateReservationRequest request, CancellationToken ct = default);
    }
}
