using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventResourceReservationApp.Application.DTOs.ReservationDetails
{
    public class ReservationDetailsResponse
    {
        public Guid Id { get; set; }
        public Guid ReservationId { get; set; }
        public Guid ResourceId { get; set; }
        public string? ResourceName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime Created { get; set; }
    }
}
