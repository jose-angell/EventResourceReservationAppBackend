using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventResourceReservationApp.Domain
{
    public class ReservationDetail
    {
        public Guid Id { get; set; }
        public Guid ReservationId { get; set; }
        public Reservation? Reservation { get; set; }
        public Guid ResourceId { get; set; }
        public Resource? Resource { get; set; } 
        public int Quantity { get; set; }   
        public decimal UnitPrice { get; set; }

        public DateTime Created { get; set; }
        public ReservationDetail()
        {
        }
        public ReservationDetail(Guid reservationId, Guid resourceId, int quantity, decimal unitPrice)
        {
            if (reservationId == Guid.Empty)
            {
                throw new ArgumentException("ReservationId cannot be empty.", nameof(reservationId));
            }
            if (resourceId == Guid.Empty)
            {
                throw new ArgumentException("ResourceId cannot be empty.", nameof(resourceId));
            }
            if (quantity <= 0)
            {
                throw new ArgumentException("Quantity must be a positive value.", nameof(quantity));
            }
            if (unitPrice < 0)
            {
                throw new ArgumentException("UnitPrice cannot be negative.", nameof(unitPrice));
            }
            ReservationId = reservationId;
            ResourceId = resourceId;
            Quantity = quantity;
            UnitPrice = unitPrice;
        }
        public void UpdateQuantity(int quantity)
        {
            if (quantity <= 0)
            {
                throw new ArgumentException("Quantity must be a positive value.", nameof(quantity));
            }
            Quantity = quantity;
        }

    }
}
