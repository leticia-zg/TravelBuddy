using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System;
using TravelBuddy.Domain.Entities;

namespace TravelBuddy.Domain.Repositories
{
    public interface IDestinationRepository
    {
        Task<IReadOnlyList<Destination>> GetAllAsync(CancellationToken ct = default);
        Task<Destination?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task AddAsync(Destination destination, CancellationToken ct = default);
        Task UpdateAsync(Destination destination, CancellationToken ct = default);
        Task DeleteAsync(Guid id, CancellationToken ct = default);
    }
}
