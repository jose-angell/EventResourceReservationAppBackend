using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventResourceReservationApp.Application.DTOs.Reviews
{
    public class ReadReviewRequest
    {
        public Guid? ResourceId { get; set; }
        public Guid? UserId { get; set; }
        public int? Rating { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? OrderBy { get; set; } 

    }
}
