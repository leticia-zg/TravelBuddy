using System;

namespace TravelBuddy.Domain.Entities
{
    public class Destination
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal AveragePrice { get; set; }
    }
}
