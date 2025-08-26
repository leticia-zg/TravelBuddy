using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Globalization;
using System.Linq;
using TravelBuddy.Shared.Contracts.Destinations;
using TravelBuddy.Infrastructure.Integration.Options;
using System.Text.Json.Serialization;


namespace TravelBuddy.Infrastructure.Integration.Clients
{
    public sealed class OpenMeteoClient : IOpenMeteoClient
    {
        private readonly HttpClient _http;
        private readonly OpenMeteoOptions _opts;


        public OpenMeteoClient(HttpClient http, Microsoft.Extensions.Options.IOptions<OpenMeteoOptions> options)
        {
            _http = http;
            _opts = options.Value;
        }

        public async Task<DestinationDto?> GetDestinationWeatherAsync(string city, CancellationToken ct = default)
        {
            // 1) Buscar coordenadas da cidade (count maior e sem country)
            var geoUrl = $"https://geocoding-api.open-meteo.com/v1/search?name={Uri.EscapeDataString(city)}&count=5";
            var geoResponse = await _http.GetFromJsonAsync<GeoCodingResponse>(geoUrl, ct);

            if (geoResponse?.Results is null || geoResponse.Results.Length == 0)
                return null;

            // Filtrar apenas cidades brasileiras
            var place = geoResponse.Results.FirstOrDefault(r => r.Country != null && r.Country.Length > 0);

            if (place == null)
                return null;

            var latitude = place.Latitude.ToString(CultureInfo.InvariantCulture);
            var longitude = place.Longitude.ToString(CultureInfo.InvariantCulture);


            // 2) Buscar clima pela latitude/longitude
            var weatherUrl = $"{_opts.BaseUrl}/v1/forecast?latitude={latitude}&longitude={longitude}&current_weather=true";
            var weather = await _http.GetFromJsonAsync<WeatherResponse>(weatherUrl, ct);

            if (weather?.CurrentWeather is null)
                return null;

            // 3) Mapear para DestinationDto
            return new DestinationDto(
                Guid.NewGuid(),
                place.Name,
                place.Country,
                $"Temperatura atual: {weather.CurrentWeather.Temperature}°C, Vento: {weather.CurrentWeather.Windspeed} km/h",
                0m
            );
        }

        // Tipos auxiliares internos
        private sealed class GeoCodingResponse
        {
            public GeoResult[]? Results { get; set; }
        }

        private sealed class GeoResult
        {
            public string Name { get; set; } = string.Empty;
            public string Country { get; set; } = string.Empty;
            public double Latitude { get; set; }
            public double Longitude { get; set; }
        }

        private sealed class WeatherResponse
        {
            public double Latitude { get; set; }
            public double Longitude { get; set; }
            public double Generationtime_ms { get; set; }
            public int Utc_offset_seconds { get; set; }
            public string Timezone { get; set; }
            public string Timezone_abbreviation { get; set; }
            public double Elevation { get; set; }

            [JsonPropertyName("current_weather")]
            public CurrentWeather CurrentWeather { get; set; }
        }

        private sealed class CurrentWeather
        {
            public string Time { get; set; }
            public int Interval { get; set; }
            public double Temperature { get; set; }
            public double Windspeed { get; set; }
            public double Winddirection { get; set; }
            public int Is_day { get; set; }
            public int Weathercode { get; set; }
        }
    }
}
