using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventResourceReservationApp.Application.DTOs.Reviews
{
    public class UpdateReviewRequest
    {
        [Required(ErrorMessage = "El ID de la reseña es obligatorio.")]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "La calificación es obligatoria.")]
        [Range(1, 5, ErrorMessage = "La calificación debe estar entre 1 y 5.")]
        public int Rating { get; set; }
        [Required(ErrorMessage = "El comentario es obligatorio.")]
        [StringLength(1000, ErrorMessage = "El comentario no puede exceder los 1000 caracteres.")]
        public string Comment { get; set; } = string.Empty;
    }
}
