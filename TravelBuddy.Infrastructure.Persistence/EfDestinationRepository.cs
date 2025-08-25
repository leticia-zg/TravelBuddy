using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using TravelBuddy.Domain.Entities;
using TravelBuddy.Domain.Repositories;

namespace TravelBuddy.Infrastructure.Persistence
{
    public sealed class EfDestinationRepository : IDestinationRepository
    {
        private readonly TravelBuddyDbContext _db;

        public EfDestinationRepository(TravelBuddyDbContext db)
        {
            _db = db;
        }

        public async Task AddAsync(Destination entity, CancellationToken ct = default)
        {
            await _db.Destinations.AddAsync(entity, ct);
            await _db.SaveChangesAsync(ct);
        }

        public async Task<IReadOnlyList<Destination>> GetAllAsync(CancellationToken ct = default)
        {
            return await _db.Destinations
                .AsNoTracking()
                .OrderBy(d => d.Name)
                .ToListAsync(ct);
        }

        public async Task<Destination?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await _db.Destinations
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.Id == id, ct);
        }

        public async Task UpdateAsync(Destination entity, CancellationToken ct = default)
        {
            _db.Destinations.Update(entity);
            await _db.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(Guid id, CancellationToken ct = default)
        {
            var entity = await _db.Destinations.FindAsync(new object[] { id }, ct);
            if (entity is null) return;
            _db.Destinations.Remove(entity);
            await _db.SaveChangesAsync(ct);
        }
    }
}
