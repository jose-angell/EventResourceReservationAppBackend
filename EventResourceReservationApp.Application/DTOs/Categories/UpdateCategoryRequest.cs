using System.ComponentModel.DataAnnotations;

namespace EventResourceReservationApp.Application.DTOs.Categories
{
    public class UpdateCategoryRequest
    {
        [Required(ErrorMessage = "El ID de la categoría es obligatorio.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre de la categoría es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres.")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "La descripción no puede exceder los 500 caracteres.")]
        public string Description { get; set; }
    }
}
