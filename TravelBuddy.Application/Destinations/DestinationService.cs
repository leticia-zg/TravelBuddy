using TravelBuddy.Domain.Entities;
using TravelBuddy.Domain.Repositories;
using TravelBuddy.Shared.Contracts.Destinations;
using TravelBuddy.Infrastructure.Integration.Clients; 
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
        private readonly IOpenMeteoClient _client;

        public DestinationService(IDestinationRepository repo, IOpenMeteoClient client)
        {
            _repo = repo;
            _client = client;
        }

        public async Task<IReadOnlyList<DestinationDto>> GetAllAsync(CancellationToken ct = default)
        {
            var destinations = await _repo.GetAllAsync(); 
            return destinations
                .Select(d => new DestinationDto(d.Id, d.Name, d.Country, d.Description, d.AveragePrice))
                .ToList();
        }

        public async Task<DestinationDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            var d = await _repo.GetByIdAsync(id);
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

            await _repo.AddAsync(entity); 
            return entity.Id;
        }

        /// <summary>
        /// Consulta clima na API externa, salva destino no banco e retorna o DTO.
        /// </summary>
        public async Task<DestinationDto?> GetAndSaveDestinationAsync(string city, CancellationToken ct = default)
        {
            var dto = await _client.GetDestinationWeatherAsync(city, ct);

            if (dto is null)
                return null;

            // Monta a entidade para persistência
            var entity = new Destination
            {
                Id = dto.Id,
                Name = dto.Name,
                Country = dto.Country,
                Description = dto.Description,
                AveragePrice = dto.AveragePrice
            };

            // Salvar no banco
            await _repo.AddAsync(entity);

            return dto;
        }
    }
}
