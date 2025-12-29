using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventResourceReservationApp.Application.DTOs.ReservationDetails
{
    public class CreateReservationDetailRequest
    {
        [Required(ErrorMessage = "El Id de la reserva es requerido.")]
        public Guid ReservationId { get; set; }
        [Required(ErrorMessage = "El Id del recurso es requerido.")]
        public Guid ResourceId { get; set; }
        [Required(ErrorMessage = "La cantida es requerido.")]
        public int Quantity { get; set; }
        [Required(ErrorMessage = "El Precio unitario es requerido.")]
        public decimal UnitPrice { get; set; }
    }
}
