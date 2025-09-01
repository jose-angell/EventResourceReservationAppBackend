using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventResourceReservationApp.Domain
{
    public class ReservationCarItem
    {
        public Guid Id { get; set; }
        public Guid ClientId { get; set; }
        public Guid ResourceId { get; set; }
        public int Quantity { get; set; }
        public DateTime AddedAt { get; set; }


        public ReservationCarItem()
        {
            Id = Guid.NewGuid();
            Quantity = 1;
            AddedAt = DateTime.UtcNow;
        }
        public ReservationCarItem(Guid clientId, Guid resourceId, int quantity)
        {
            if (clientId == Guid.Empty)
            {
                throw new ArgumentException("ClientId cannot be empty.", nameof(clientId));
            }
            if (resourceId == Guid.Empty)
            {
                throw new ArgumentException("ClientId cannot be empty.", nameof(resourceId));
            }
            if (quantity <= 0)
            {
                throw new ArgumentException("Quantity must be greater than zero.", nameof(quantity));
            }

            Id = Guid.NewGuid();
            ClientId = clientId;
            ResourceId = resourceId;
            Quantity = quantity;
            AddedAt = DateTime.UtcNow;
        }
        public void UpdateQuantity(int quantity)
        {
            if (quantity <= 0)
            {
                throw new ArgumentException("Quantity must be greater than zero.", nameof(quantity));
            }
            Quantity = quantity;
        }
    }
}
