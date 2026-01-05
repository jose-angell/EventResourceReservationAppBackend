using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventResourceReservationApp.Application.DTOs.Reservations
{
    public class ReservationResponse
    {
        public Guid Id { get; set; }
        public Guid ResourceId { get; set; }
        public string ResourceName { get; set; } = string.Empty;
        public Guid ClientId { get; set; }
        public string ClientName { get; set; } = string.Empty;
        public string ClientPhoneNumber { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Quantity { get; set; }
        public decimal TotalAmount { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
