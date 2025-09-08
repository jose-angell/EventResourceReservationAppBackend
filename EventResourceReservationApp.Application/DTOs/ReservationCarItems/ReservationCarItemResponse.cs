using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventResourceReservationApp.Application.DTOs.ReservationCarItems
{
    public class ReservationCarItemResponse
    {
        public Guid Id { get; set; }
        public Guid ClientId { get; set; }
        public Guid ResourceId { get; set; }
        public int Quantity { get; set; }
        public DateTime AddedAt { get; set; }
    }
}
