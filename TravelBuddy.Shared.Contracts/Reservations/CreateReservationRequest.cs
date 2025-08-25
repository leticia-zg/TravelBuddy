using System;

namespace TravelBuddy.Shared.Contracts.Reservations
{
    public sealed record CreateReservationRequest(Guid DestinationId, string CustomerName, DateTime TravelDate, int NumberOfPeople, decimal TotalPrice);
}
