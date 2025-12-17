using EventResourceReservationApp.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventResourceReservationApp.Application.DTOs.Reservation
{
    public class CreateReservationRequest
    {
        [Required(ErrorMessage = "La fecha de inicio es requerida")]
        public DateTime StartTime { get; set; }
        [Required(ErrorMessage = "La fecha fin es requerida")]
        public DateTime EndTime { get; set; }
        [Required(ErrorMessage = "El monto total de la reservación es requerida")]
        public decimal TotalAmount { get; set; }
        public string? ClientComment { get; set; }
        [Required(ErrorMessage = "El número de teléfono del cliente es requerido")]
        public string ClientPhoneNumber { get; set; }
        [Required(ErrorMessage = "La ubicación es requerida")]
        public int LocationId { get; set; }
        [Required(ErrorMessage = "El Id del cliente es requerido")]
        public Guid ClientId { get; set; }
    }
}
