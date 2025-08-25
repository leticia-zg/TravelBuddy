namespace TravelBuddy.Infrastructure.Integration.Options
{
    public sealed class OpenMeteoOptions
    {
        public const string SectionName = "OpenMeteo";
        public string BaseUrl { get; set; } = "https://api.open-meteo.com";
    }
}
