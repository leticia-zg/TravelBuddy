using TravelBuddy.Domain.Entities;
using TravelBuddy.Domain.Repositories;
using TravelBuddy.Shared.Contracts.Reservations;
using System.Threading.Tasks;
using System.Threading;
using System;
using System.Linq; // necessário para Select
using System.Collections.Generic;

namespace TravelBuddy.Application.Reservations
{
    public sealed class ReservationService : IReservationService
    {
        private readonly IReservationRepository _repo;

        public ReservationService(IReservationRepository repo)
        {
            _repo = repo;
        }

        public async Task<IReadOnlyList<ReservationDto>> GetAllAsync(CancellationToken ct = default)
        {
            var reservations = await _repo.GetAllAsync(); // remova ct se o repo não aceitar
            return reservations
                .Select(r => new ReservationDto(r.Id, r.DestinationId, r.CustomerName, r.TravelDate, r.NumberOfPeople, r.TotalPrice))
                .ToList();
        }

        public async Task<ReservationDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            var r = await _repo.GetByIdAsync(id); // remova ct se o repo não aceitar
            return r is null ? null : new ReservationDto(r.Id, r.DestinationId, r.CustomerName, r.TravelDate, r.NumberOfPeople, r.TotalPrice);
        }

        public async Task<Guid> CreateAsync(CreateReservationRequest request, CancellationToken ct = default)
        {
            var entity = new Reservation
            {
                Id = Guid.NewGuid(),
                DestinationId = request.DestinationId,
                CustomerName = request.CustomerName,
                TravelDate = request.TravelDate,
                NumberOfPeople = request.NumberOfPeople,
                TotalPrice = request.TotalPrice
            };

            await _repo.AddAsync(entity); // remova ct se o repo não aceitar
            return entity.Id;
        }
    }
}
