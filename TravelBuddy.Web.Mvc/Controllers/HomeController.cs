using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using TravelBuddy.Shared.Contracts.Reservations;
using TravelBuddy.Shared.Contracts.Destinations;

namespace Web.TravelBuddy.Mvc.Controllers
{
    public sealed class HomeController(IHttpClientFactory httpFactory) : Controller
    {
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var reservationsApi = httpFactory.CreateClient("reservations");
            var reservations = await reservationsApi
                .GetFromJsonAsync<List<ReservationDto>>("/api/reservations", ct)
                ?? new List<ReservationDto>();

            return View(reservations);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateReservationRequest dto, CancellationToken ct)
        {
            var reservationsApi = httpFactory.CreateClient("reservations");
            var response = await reservationsApi.PostAsJsonAsync("/api/reservations", dto, ct);

            TempData["msg"] = response.IsSuccessStatusCode ? "Reserva criada!" : "Erro ao criar";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Destination(string city, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(city))
                return RedirectToAction(nameof(Index));

            var destinationsApi = httpFactory.CreateClient("destinations");
            var destination = await destinationsApi
                .GetFromJsonAsync<DestinationDto>($"/api/destinations/{Uri.EscapeDataString(city)}", ct);

            return View(destination);
        }
    }
}
