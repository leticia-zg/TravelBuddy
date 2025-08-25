using System;

namespace TravelBuddy.Shared.Contracts.Destinations
{
    public sealed record DestinationDto(Guid Id, string Name, string Country, string Description, decimal AveragePrice);
}
