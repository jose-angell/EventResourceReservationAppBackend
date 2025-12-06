using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventResourceReservationApp.Application.DTOs.Resources
{
    public class ReadByIdAndDateRangeResourceRequest
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "La fecha de inicio es obligatoria.")]
        public DateTime? StartTime { get; set; }
        [Required(ErrorMessage = "La fecha de fin es obligatoria.")]
        public DateTime? EndTime { get; set; }
    }
}
