using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventResourceReservationApp.Application.DTOs.Reservation
{
    public class UpdateStatusReservationRequest
    {
        [Required(ErrorMessage = "EL Id de la reserva es requerido")]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "EL Id de administrador es requerido")]
        public Guid AdminId { get; set; }
        [Required(ErrorMessage = "EL estatus es requerido")]
        public int StatusId { get; set; }
        public string? AdminComment { get; set; } = string.Empty;
    }
}
