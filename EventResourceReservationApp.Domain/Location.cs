namespace EventResourceReservationApp.Domain
{
    public class Location
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
        public DateTime CreatedAt { get; set; }

        public Location()
        {
            Country = string.Empty;
            City = string.Empty;
            Street = string.Empty;
            Neighborhood = string.Empty;
            ExteriorNumber = string.Empty;
            InteriorNumber = string.Empty;
        }
        public Location(string country, string city, int zipCode, string street, string neighborhood, string exteriorNumber, string interiorNumber, Guid createdByUserId)
        {
            if (string.IsNullOrWhiteSpace(country))
            {
                throw new ArgumentException("Country cannot be null or empty.", nameof(country));
            }
            if (string.IsNullOrWhiteSpace(city))
            {
                throw new ArgumentException("City cannot be null or empty.", nameof(city));
            }
            if (zipCode <= 0)
            {
                throw new ArgumentException("ZipCode must be a positive integer.", nameof(zipCode));
            }
            if (string.IsNullOrWhiteSpace(street))
            {
                throw new ArgumentException("Street cannot be null or empty.", nameof(street));
            }
            if (string.IsNullOrWhiteSpace(exteriorNumber))
            {
                throw new ArgumentException("ExteriorNumber cannot be null or empty.", nameof(neighborhood));
            }
            if (createdByUserId == Guid.Empty)
            {
                throw new ArgumentException("CreatedByUserId cannot be empty.", nameof(createdByUserId));
            }
            Country = country;
            City = city;
            ZipCode = zipCode;
            Street = street;
            Neighborhood = neighborhood ?? string.Empty;
            ExteriorNumber = exteriorNumber;
            InteriorNumber = interiorNumber ?? string.Empty;
            CreatedByUserId = createdByUserId;
            CreatedAt = DateTime.UtcNow;
        }
        public void Update(string country, string city, int zipCode, string street, string neighborhood, string exteriorNumber, string interiorNumber)
        {
            if (string.IsNullOrWhiteSpace(country))
            {
                throw new ArgumentException("Country cannot be null or empty.", nameof(country));
            }
            if (string.IsNullOrWhiteSpace(city))
            {
                throw new ArgumentException("City cannot be null or empty.", nameof(city));
            }
            if (zipCode <= 0)
            {
                throw new ArgumentException("ZipCode must be a positive integer.", nameof(zipCode));
            }
            if (string.IsNullOrWhiteSpace(street))
            {
                throw new ArgumentException("Street cannot be null or empty.", nameof(street));
            }
            if (string.IsNullOrWhiteSpace(exteriorNumber))
            {
                throw new ArgumentException("ExteriorNumber cannot be null or empty.", nameof(neighborhood));
            }
            Country = country;
            City = city;
            ZipCode = zipCode;
            Street = street;
            Neighborhood = neighborhood ?? string.Empty;
            ExteriorNumber = exteriorNumber;
            InteriorNumber = interiorNumber ?? string.Empty;
        }
    }
}
