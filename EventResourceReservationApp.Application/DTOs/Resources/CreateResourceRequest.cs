using EventResourceReservationApp.Domain;
using EventResourceReservationApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventResourceReservationApp.Application.DTOs.Resources
{
    public class CreateResourceRequest
    {
        [Required(ErrorMessage = "El ID de la categoría es obligatorio.")]
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "El ID del estado es obligatorio.")]
        public int StatusId { get; set; }
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [Range(1, 200, ErrorMessage = "El nombre debe tener entre 1 y 200 caracteres.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "La descripción es obligatoria.")]
        [Range(1, 2000, ErrorMessage = "La descripción debe tener entre 1 y 2000 caracteres.")]
        public string Description { get; set; }
        [Required(ErrorMessage = "La cantidad es obligatoria.")]
        public int Quantity { get; set; }
        [Required(ErrorMessage = "El precio es obligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor que cero.")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "El tipo de autorización es obligatorio.")]
        public ResourceAuthorizationType AuthorizationType { get; set; }
        [Required(ErrorMessage = "El ID de la ubicación es obligatorio.")]
        public int LocationId { get; set; }
        [Required(ErrorMessage = "El ID del usuario creador es obligatorio.")]
        public Guid CreatedByUserId { get; set; }
    }
}
