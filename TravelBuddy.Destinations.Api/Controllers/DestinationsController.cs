using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Threading;
using TravelBuddy.Shared.Contracts.Destinations;
using TravelBuddy.Application.Destinations; 

namespace TravelBuddy.Destinations.Api.Controllers
{
    [ApiController]
    [Route("api/destinations")]
    public sealed class DestinationsController : ControllerBase
    {
        private readonly IDestinationService _service;

        public DestinationsController(IDestinationService service)
        {
            _service = service;
        }

        /// <summary>
        /// Busca clima de uma cidade e salva destino no banco.
        /// </summary>
        [HttpGet("{city}")]
        public async Task<IActionResult> GetByCity(string city, CancellationToken ct)
        {
            var destination = await _service.GetAndSaveDestinationAsync(city, ct);

            if (destination is null)
                return NotFound(new { message = "Cidade não encontrada ou sem dados de clima" });

            return Ok(destination);
        }

        /// <summary>
        /// Lista todos os destinos já salvos no banco.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            var destinations = await _service.GetAllAsync(ct);
            return Ok(destinations);
        }

        /// <summary>
        /// Busca destino salvo pelo Id.
        /// </summary>
        [HttpGet("id/{destinationId:guid}")]
        public async Task<IActionResult> GetById(Guid destinationId, CancellationToken ct)
        {
            var destination = await _service.GetByIdAsync(destinationId, ct);

            if (destination is null)
                return NotFound(new { message = "Destino não encontrado" });

            return Ok(destination);
        }
    }
}
