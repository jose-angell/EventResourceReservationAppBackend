using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventResourceReservationApp.Application.DTOs.ReservationCarItems
{
    public class CreateReservationCarItemRequest
    {
        [Required(ErrorMessage = "El Id del cliente es requerido.")]
        public Guid ClientId { get; set; }

        [Required(ErrorMessage = "El Id del recurso es requerido.")]
        public Guid ResourceId { get; set; }

        [Required(ErrorMessage = "La cantidad es requerida.")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor que cero.")]
        public int Quantity { get; set; }
    }
}
