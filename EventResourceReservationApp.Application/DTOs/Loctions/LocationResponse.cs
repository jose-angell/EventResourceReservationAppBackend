using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventResourceReservationApp.Application.DTOs.Loctions
{
    public class LocationResponse
    {
        public int Id { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public int ZipCode { get; set; }
        public string Street { get; set; }
        public string Neighborhood { get; set; }
        public string ExteriorNumber { get; set; }
        public string InteriorNumber { get; set; }
        public Guid CreatedByUserId { get; set; }
        public DateTime CreateAt { get; set; }
    }
}
