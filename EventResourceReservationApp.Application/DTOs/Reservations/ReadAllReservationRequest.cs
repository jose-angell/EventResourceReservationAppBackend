using EventResourceReservationApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventResourceReservationApp.Application.DTOs.Reservations
{
    public class ReadAllReservationRequest
    {
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public ReservationStatus? StatusId { get; set; }
        public int? LocationId { get; set; }
        public Guid? CreatedByUserIdFilter { get; set; }
        public string? OrderBy { get; set; }
    }
}
