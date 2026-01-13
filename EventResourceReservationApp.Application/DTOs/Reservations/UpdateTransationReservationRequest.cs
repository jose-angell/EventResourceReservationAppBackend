using EventResourceReservationApp.Application.Common;
using EventResourceReservationApp.Application.Repositories;
using EventResourceReservationApp.Domain.Enums;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventResourceReservationApp.Application.DTOs.Reservations
{
    public class UpdateTransationReservationRequest
    {
        public Guid Id { get; set; }
        public Guid TrasnsationId { get; set; }
        public ReservationStatus StatusId { get; set; }
    }
}
