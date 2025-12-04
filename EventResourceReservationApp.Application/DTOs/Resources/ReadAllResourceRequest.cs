using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventResourceReservationApp.Application.DTOs.Resources
{
    public class ReadAllResourceRequest
    {
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int? CategoryId { get; set; }
        public int? StatusId { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public Guid? CreatedByUserIdFilter { get; set; }
        public string? OrderBy { get; set; }
    }
}
