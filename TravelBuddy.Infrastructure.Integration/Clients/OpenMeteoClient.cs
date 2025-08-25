using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Threading;
using TravelBuddy.Shared.Contracts.Destinations;
using TravelBuddy.Infrastructure.Integration.Options;

namespace TravelBuddy.Infrastructure.Integration.Clients
{
    public sealed class OpenMeteoClient : IOpenMeteoClient
    {
        private readonly HttpClient _http;
        private readonly OpenMeteoOptions _opts;

        public OpenMeteoClient(HttpClient http, IOptions<OpenMeteoOptions> options)
        {
            _http = http;
            _opts = options.Value;
        }

        public async Task<DestinationDto?> GetDestinationWeatherAsync(string city, CancellationToken ct = default)
        {
            var url = $"{_opts.BaseUrl}/weather/{Uri.EscapeDataString(city)}";
            var result = await _http.GetFromJsonAsync<DestinationDto>(url, ct);
            return result;
        }
    }
}
