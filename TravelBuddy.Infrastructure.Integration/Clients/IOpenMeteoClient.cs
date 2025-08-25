using System.Threading.Tasks;
using System.Threading;
using TravelBuddy.Shared.Contracts.Destinations;

namespace TravelBuddy.Infrastructure.Integration.Clients
{
    public interface IOpenMeteoClient
    {
        Task<DestinationDto?> GetDestinationWeatherAsync(string city, CancellationToken ct = default);
    }
}
