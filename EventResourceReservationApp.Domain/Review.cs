using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventResourceReservationApp.Domain
{
    public class Review
    {
        public Guid Id { get; set; }
        public Guid ResourceId { get; set; }
        public Guid UserId { get; set; }
        public int Rating { get; set; } 
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }

        public Review()
        {
            Comment = string.Empty;
            CreatedAt = DateTime.UtcNow;
            Rating = 1;
        }
        public Review(Guid resourceId, Guid userId, int rating, string comment)
        {
            if(resourceId == Guid.Empty)
            {
                throw new ArgumentException("ResourceId cannot be empty.", nameof(resourceId));
            }
            if(userId == Guid.Empty)
            {
                throw new ArgumentException("UserId cannot be empty.", nameof(userId));
            }
            if (rating < 1 || rating > 5)
            {
                throw new ArgumentException("Rating must be between 1 and 5.", nameof(rating));
            }
            if (string.IsNullOrWhiteSpace(comment))
            {
                throw new ArgumentException("Comment cannot be null or empty.", nameof(comment));
            }
            Id = Guid.NewGuid();
            ResourceId = resourceId;
            UserId = userId;
            Rating = rating;
            Comment = comment;
            CreatedAt = DateTime.UtcNow;
        }
        public void Update(int rating, string comment)
        {
            if (rating < 1 || rating > 5)
            {
                throw new ArgumentException("Rating must be between 1 and 5.", nameof(rating));
            }
            if (string.IsNullOrWhiteSpace(comment))
            {
                throw new ArgumentException("Comment cannot be null or empty.", nameof(comment));
            }
            Rating = rating;
            Comment = comment;
        }
    }
}
