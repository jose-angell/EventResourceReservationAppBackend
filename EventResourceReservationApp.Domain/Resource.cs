using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventResourceReservationApp.Domain
{
    public class Resource
    {
        public Guid Id { get; set; }
        public Guid CategoryId { get; set; }
        public Category? Category { get; set; }
        public int StatusId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public int AuthorizationType { get; set; }
        public Guid LocationId { get; set; }
        public Location? Location { get; set; }
        public Guid CreatedByUserId { get; set; }
        public ApplicationUser? CreatedByUser { get; set; }
        public DateTime CreatedAt { get; set; }

        public Resource()
        {
            Name = string.Empty;
            Description = string.Empty;
        }
        public Resource(Guid categoryId, string name, string description, int quantity, decimal price,
            int authorizationType, Guid locationId, Guid createdByUserId)
        {
            if (categoryId == Guid.Empty)
            {
                throw new ArgumentException("CategoryId cannot be empty.", nameof(categoryId));
            }
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name cannot be null or empty.", nameof(name));
            }
            if (string.IsNullOrWhiteSpace(description))
            {
                throw new ArgumentException("Description cannot be null or empty.", nameof(description));
            }
            if (quantity < 0)
            {
                throw new ArgumentException("AvailableQuantity cannot be negative.", nameof(quantity));
            }
            if (price < 0)
            {
                throw new ArgumentException("UnitPrice cannot be negative.", nameof(price));
            }
            if (authorizationType < 0)
            {
                throw new ArgumentException("AuthorizationType cannot be negative.", nameof(authorizationType));
            }
            if (locationId == Guid.Empty)
            {
                throw new ArgumentException("LocationId cannot be empty.", nameof(locationId));
            }
            if (createdByUserId == Guid.Empty)
            {
                throw new ArgumentException("userId cannot be empty.", nameof(createdByUserId));
            }

            CategoryId = categoryId;
            StatusId = 1;
            Name = name;
            Description = description;
            Quantity = quantity;
            Price = price;
            AuthorizationType = authorizationType;
            LocationId = locationId;
            CreatedByUserId = createdByUserId;
            CreatedAt = DateTime.UtcNow;
        }
        public void Update(Guid categoryId, int statud, string name, string description, int quantity, decimal price,
            int authorizationType, Guid locationId)
        {
            if (categoryId == Guid.Empty)
            {
                throw new ArgumentException("CategoryId cannot be empty.", nameof(categoryId));
            }
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name cannot be null or empty.", nameof(name));
            }
            if (string.IsNullOrWhiteSpace(description))
            {
                throw new ArgumentException("Description cannot be null or empty.", nameof(description));
            }
            if (quantity < 0)
            {
                throw new ArgumentException("AvailableQuantity cannot be negative.", nameof(quantity));
            }
            if (price < 0)
            {
                throw new ArgumentException("UnitPrice cannot be negative.", nameof(price));
            }
            if (authorizationType < 0)
            {
                throw new ArgumentException("AuthorizationType cannot be negative.", nameof(authorizationType));
            }
            if (locationId == Guid.Empty)
            {
                throw new ArgumentException("LocationId cannot be empty.", nameof(locationId));
            }
            if (statud < 0)
            {
                throw new ArgumentException("StatusId cannot be negative.", nameof(statud));
            }
            CategoryId = categoryId;
            StatusId = statud;
            Name = name;
            Description = description;
            Quantity = quantity;
            Price = price;
            AuthorizationType = authorizationType;
            LocationId = locationId;
        }
    }
}
