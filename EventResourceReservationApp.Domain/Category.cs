namespace EventResourceReservationApp.Domain
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid CreatedByUserId { get; set; }

        public Category()
        {
            Name = string.Empty;
            Description = string.Empty;
        }
        public Category(string name, string description, Guid createdByUserId)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name cannot be null or empty.", nameof(name));
            }
            if (name.Length > 100)
            {
                throw new ArgumentException("Name cannot exceed 100 characters.", nameof(name));
            }
            if (createdByUserId == Guid.Empty)
            {
                throw new ArgumentException("CreatedByUserId cannot be empty.", nameof(createdByUserId));
            }
            Name = name;
            Description = description ?? string.Empty;
            CreatedAt = DateTime.UtcNow;
            CreatedByUserId = createdByUserId;
        }
        public void Update(string name, string description)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name cannot be null or empty.", nameof(name));
            }
            if (name.Length > 100)
            {
                throw new ArgumentException("Name cannot exceed 100 characters.", nameof(name));
            }
            Name = name;
            Description = description ?? string.Empty;
        }
    }
}
