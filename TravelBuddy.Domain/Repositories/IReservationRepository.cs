using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System;
using TravelBuddy.Domain.Entities;

namespace TravelBuddy.Domain.Repositories
{
    public interface IReservationRepository
    {
        Task<IReadOnlyList<Reservation>> GetAllAsync(CancellationToken ct = default);
        Task<Reservation?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task AddAsync(Reservation reservation, CancellationToken ct = default);
        Task UpdateAsync(Reservation reservation, CancellationToken ct = default);
        Task DeleteAsync(Guid id, CancellationToken ct = default);
    }
}
