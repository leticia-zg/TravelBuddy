using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TravelBuddy.Application.Reservations;
using TravelBuddy.Shared.Contracts.Reservations;

namespace TravelBuddy.Reservations.Api.Controllers
{
    [ApiController]
    [Route("api/reservations")]
    public sealed class ReservationsController : ControllerBase
    {
        private readonly ReservationService _service;

        public ReservationsController(ReservationService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ReservationDto>>> GetAll(CancellationToken ct)
            => Ok(await _service.GetAllAsync(ct));

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ReservationDto>> GetById(Guid id, CancellationToken ct)
        {
            var res = await _service.GetByIdAsync(id, ct);
            return res is null ? NotFound() : Ok(res);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateReservationRequest request, CancellationToken ct)
        {
            var id = await _service.CreateAsync(request, ct);
            return CreatedAtAction(nameof(GetById), new { id }, id);
        }
    }
}
