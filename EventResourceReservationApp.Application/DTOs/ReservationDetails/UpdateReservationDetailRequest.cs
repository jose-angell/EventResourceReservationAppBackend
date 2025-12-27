using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventResourceReservationApp.Application.DTOs.ReservationDetails
{
    public class UpdateReservationDetailRequest
    {
        public Guid Id { get; set; }
        public int Quantity { get; set; }
    }
}
