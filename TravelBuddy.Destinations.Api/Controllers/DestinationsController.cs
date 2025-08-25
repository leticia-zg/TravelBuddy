using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Threading;
using TravelBuddy.Infrastructure.Integration.Clients;
using TravelBuddy.Shared.Contracts.Destinations;

namespace TravelBuddy.Destinations.Api.Controllers
{
    [ApiController]
    [Route("api/destinations")]
    public sealed class DestinationsController : ControllerBase
    {
        private readonly IOpenMeteoClient _client;

        public DestinationsController(IOpenMeteoClient client)
        {
            _client = client;
        }

        [HttpGet("{city}")]
        public async Task<IActionResult> GetByCity(string city, CancellationToken ct)
        {
            var destination = await _client.GetDestinationWeatherAsync(city, ct);

            if (destination is null)
                return NotFound(new { message = "Cidade não encontrada ou sem dados de clima" });

            return Ok(destination);
        }
    }
}
