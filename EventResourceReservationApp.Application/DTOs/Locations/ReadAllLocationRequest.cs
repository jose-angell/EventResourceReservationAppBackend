using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventResourceReservationApp.Application.DTOs.Locations
{
    public class ReadAllLocationRequest
    {
        public string? City { get; set; }
        public int? ZipCode { get; set; }
        public Guid? CreatedByUserIdFilter { get; set; }
        public string? OrderBy { get; set; }
    }
}
