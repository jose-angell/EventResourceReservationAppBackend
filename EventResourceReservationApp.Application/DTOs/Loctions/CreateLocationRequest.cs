using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventResourceReservationApp.Application.DTOs.Loctions
{
    public class CreateLocationRequest
    {
        [Required(ErrorMessage = "El nombre del pais es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre del pais no puede exceder los 100 caracteres.")]
        public string Country { get; set; }

        [Required(ErrorMessage = "El nombre del estado es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre del estado no puede exceder los 100 caracteres.")]
        public string City { get; set; }

        [Required(ErrorMessage = "El codigo postal es obligatorio.")]
        public int ZipCode { get; set; }

        [Required(ErrorMessage = "El nombre de la calle es obligatorio.")]
        [StringLength(200, ErrorMessage = "El nombre de la calle no puede exceder los 200 caracteres.")]
        public string Street { get; set; }

        [StringLength(100, ErrorMessage = "El nombre de la colonia no puede exceder los 100 caracteres.")]
        public string Neighborhood { get; set; }

        [Required(ErrorMessage = "El número exterior es obligatorio.")]
        [StringLength(50, ErrorMessage = "El número exterior no puede exceder los 50 caracteres.")]
        public string ExteriorNumber { get; set; }

        [StringLength(50, ErrorMessage = "El número interior no puede exceder los 50 caracteres.")]
        public string InteriorNumber { get; set; }

        [Required(ErrorMessage = "El ID del usuario creador es obligatorio.")]
        public Guid CreatedByUserId { get; set; }
    }
}
