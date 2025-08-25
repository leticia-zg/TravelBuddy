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
    public sealed class EfReservationRepository : IReservationRepository
    {
        private readonly TravelBuddyDbContext _db;

        public EfReservationRepository(TravelBuddyDbContext db)
        {
            _db = db;
        }

        public async Task AddAsync(Reservation entity, CancellationToken ct = default)
        {
            await _db.Reservations.AddAsync(entity, ct);
            await _db.SaveChangesAsync(ct);
        }

        public async Task<IReadOnlyList<Reservation>> GetAllAsync(CancellationToken ct = default)
        {
            return await _db.Reservations
                .AsNoTracking()
                .OrderBy(r => r.TravelDate)
                .ToListAsync(ct);
        }

        public async Task<Reservation?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await _db.Reservations
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == id, ct);
        }

        public async Task UpdateAsync(Reservation entity, CancellationToken ct = default)
        {
            _db.Reservations.Update(entity);
            await _db.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(Guid id, CancellationToken ct = default)
        {
            var entity = await _db.Reservations.FindAsync(new object[] { id }, ct);
            if (entity is null) return;
            _db.Reservations.Remove(entity);
            await _db.SaveChangesAsync(ct);
        }
    }
}
