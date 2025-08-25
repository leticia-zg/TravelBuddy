using System;

namespace TravelBuddy.Domain.Entities
{
    public class Reservation
    {
        public Guid Id { get; set; }
        public Guid DestinationId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public DateTime TravelDate { get; set; }
        public int NumberOfPeople { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
