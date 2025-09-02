using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventResourceReservationApp.Application.DTOs.ReservationCarItems
{
    public class UpdateReservationCarItemRequest
    {
        [Required(ErrorMessage = "El Id del item del carrito es requerido.")]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "La cantidad es requerida.")]
        public int Quantity { get; set; }
    }
}
