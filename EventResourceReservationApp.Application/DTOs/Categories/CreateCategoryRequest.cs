using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventResourceReservationApp.Application.DTOs.Categories
{
    public class CreateCategoryRequest
    {
        [Required(ErrorMessage = "El nombre de la categotría es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre de la categotría no puede exceder los 100 caracteres.")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "La descripción de la categotría no puede exceder los 500 caracteres.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "El ID del usuario creador es obligatorio.")]
        public Guid CreatedByUserId { get; set; }
    }
}
