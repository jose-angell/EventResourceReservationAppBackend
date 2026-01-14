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
    public class UpdateResourceRequest
    {
        [Required(ErrorMessage = "El ID del recurso es obligatorio.")]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "El ID de la categoría es obligatorio.")]
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "El ID del estado es obligatorio.")]
        public ResourceStatus StatusId { get; set; }
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "La descripción es obligatoria.")]
        public string Description { get; set; }
        [Required(ErrorMessage = "La cantidad es obligatoria.")]
        public int Quantity { get; set; }
        [Required(ErrorMessage = "El precio es obligatorio.")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "El tipo de autorización es obligatorio.")]
        public ResourceAuthorizationType AuthorizationType { get; set; }
        [Required(ErrorMessage = "El ID de la ubicación es obligatorio.")]
        public int LocationId { get; set; }
       
    }
}
