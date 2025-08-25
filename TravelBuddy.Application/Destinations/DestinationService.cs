using TravelBuddy.Domain.Entities;
using TravelBuddy.Domain.Repositories;
using TravelBuddy.Shared.Contracts.Destinations;
using System.Threading.Tasks;
using System.Threading;
using System;
using System.Linq;
using System.Collections.Generic;

namespace TravelBuddy.Application.Destinations
{
    public sealed class DestinationService : IDestinationService
    {
        private readonly IDestinationRepository _repo;

        public DestinationService(IDestinationRepository repo)
        {
            _repo = repo;
        }

        public async Task<IReadOnlyList<DestinationDto>> GetAllAsync(CancellationToken ct = default)
        {
            var destinations = await _repo.GetAllAsync(); // remova ct se o repo não aceitar
            return destinations
                .Select(d => new DestinationDto(d.Id, d.Name, d.Country, d.Description, d.AveragePrice))
                .ToList();
        }

        public async Task<DestinationDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            var d = await _repo.GetByIdAsync(id); // remova ct se o repo não aceitar
            return d is null ? null : new DestinationDto(d.Id, d.Name, d.Country, d.Description, d.AveragePrice);
        }

        public async Task<Guid> CreateAsync(CreateDestinationRequest request, CancellationToken ct = default)
        {
            var entity = new Destination
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Country = request.Country,
                Description = request.Description,
                AveragePrice = request.AveragePrice
            };

            await _repo.AddAsync(entity); // remova ct se o repo não aceitar
            return entity.Id;
        }
    }
}
