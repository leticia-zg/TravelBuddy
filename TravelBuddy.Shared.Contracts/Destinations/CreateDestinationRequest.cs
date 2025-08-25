namespace TravelBuddy.Shared.Contracts.Destinations
{
    public sealed record CreateDestinationRequest(string Name, string Country, string Description, decimal AveragePrice);
}
