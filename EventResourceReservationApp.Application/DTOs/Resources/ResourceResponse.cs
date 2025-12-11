using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventResourceReservationApp.Application.DTOs.Resources
{
    public class ResourceResponse
    {
        public Guid Id { get; set; }
        public int StatusId { get; set; }
        public string StatusDescription { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; } // Total quantity available
        public int QuantityInUse { get; set; } // Quantity currently reserved or in use
        public int AvailableQuantity => Quantity - QuantityInUse;
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int LocationId { get; set; }
        public string LocationDescription { get; set; }
        public int AuthorizationType { get; set; }
        public DateTime Created { get; set; }

    }
}
