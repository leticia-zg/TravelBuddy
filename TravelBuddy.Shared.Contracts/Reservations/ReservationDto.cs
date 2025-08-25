using System;

namespace TravelBuddy.Shared.Contracts.Reservations
{
    public sealed record ReservationDto(Guid Id, Guid DestinationId, string CustomerName, DateTime TravelDate, int NumberOfPeople, decimal TotalPrice);
}
