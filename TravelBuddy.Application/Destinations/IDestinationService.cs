using TravelBuddy.Shared.Contracts.Destinations;
using System.Threading.Tasks;
using System.Threading;
using System;
using System.Collections.Generic;

namespace TravelBuddy.Application.Destinations
{
    public interface IDestinationService
    {
        Task<IReadOnlyList<DestinationDto>> GetAllAsync(CancellationToken ct = default);
        Task<DestinationDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<Guid> CreateAsync(CreateDestinationRequest request, CancellationToken ct = default);

        /// <summary>
        /// Consulta clima na API externa, salva destino no banco e retorna o DTO.
        /// </summary>
        Task<DestinationDto?> GetAndSaveDestinationAsync(string city, CancellationToken ct = default);
    }
}
