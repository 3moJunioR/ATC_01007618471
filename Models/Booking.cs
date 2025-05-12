using System.ComponentModel.DataAnnotations;

namespace EventBookingAPI.Models
{
    public class Booking
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }
        public User User { get; set; }

        [Required]
        public int EventId { get; set; }
        public Event Event { get; set; }

        [Required]
        public int TicketCount { get; set; } = 1;

        public DateTime BookingDate { get; set; } = DateTime.UtcNow;

        public string Status { get; set; } = "Confirmed"; // Confirmed, Cancelled
    }
} 