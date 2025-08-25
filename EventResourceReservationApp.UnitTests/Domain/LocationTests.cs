using EventResourceReservationApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventResourceReservationApp.UnitTests.Domain
{
    public class LocationTests
    {
        [Fact]
        public void Constructor_WithValidParameters_InitializesCorrectly()
        {
            //Arrange
            var Country = "Test Country";
            var City = "Test City";
            var ZipCode = 12345;
            var street = "Test Street";
            var Neighborhood = "Test Neighborhood";
            var ExteriorNumber = "123";
            var InteriorNumber = "456";
            var CreatedByUserId = Guid.NewGuid();

            //Act
            var location = new Location(Country, City, ZipCode, street, Neighborhood, ExteriorNumber, InteriorNumber, CreatedByUserId);

            //Assert
            Assert.Equal(Country, location.Country);
            Assert.Equal(City, location.City);
            Assert.Equal(ZipCode, location.ZipCode);
            Assert.Equal(street, location.Street);
            Assert.Equal(Neighborhood, location.Neighborhood);
            Assert.Equal(ExteriorNumber, location.ExteriorNumber);
            Assert.Equal(InteriorNumber, location.InteriorNumber);
            Assert.Equal(CreatedByUserId, location.CreatedByUserId);
            Assert.NotEqual(default(DateTime), location.CreatedAt);
        }
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Constructor_WithNullOrEmptyCountry_ThrowsArgumentException(string invalidCountry)
        {
            //Arrage
            var City = "Test City";
            var ZipCode = 12345;
            var street = "Test Street";
            var Neighborhood = "Test Neighborhood";
            var ExteriorNumber = "123";
            var InteriorNumber = "456";
            var CreatedByUserId = Guid.NewGuid();
            // Act & Assert
            Assert.Throws<ArgumentException>(() => new Location(invalidCountry, City, ZipCode, street, Neighborhood, ExteriorNumber, InteriorNumber, CreatedByUserId));
        }
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Constructor_WithNullOrEmptyCity_ThrowsArgumentException(string invalidCity)
        {
            //Arrage
            var Country = "Test Country";
            var ZipCode = 12345;
            var street = "Test Street";
            var Neighborhood = "Test Neighborhood";
            var ExteriorNumber = "123";
            var InteriorNumber = "456";
            var CreatedByUserId = Guid.NewGuid();
            // Act & Assert
            Assert.Throws<ArgumentException>(() => new Location(Country, invalidCity, ZipCode, street, Neighborhood, ExteriorNumber, InteriorNumber, CreatedByUserId));
        }
        [Fact]
        public void Constructor_WithCeroZipCode_ThrowsArgumentException()
        {
            //Arrange
            var Country = "Test Country";
            var City = "Test City";
            var invalidZipCode = 0;
            var street = "Test Street";
            var Neighborhood = "Test Neighborhood";
            var ExteriorNumber = "123";
            var InteriorNumber = "456";
            var CreatedByUserId = Guid.NewGuid();
            // Act & Assert
            Assert.Throws<ArgumentException>(() => new Location(Country, City, invalidZipCode, street, Neighborhood, ExteriorNumber, InteriorNumber, CreatedByUserId));
        }
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Constructor_WithNullOrEmptystreet_ThrowsArgumentException(string invalidStreet)
        {
            //Arrange
            var Country = "Test Country";
            var City = "Test City";
            var ZipCode = 12345;
            var Neighborhood = "Test Neighborhood";
            var ExteriorNumber = "123";
            var InteriorNumber = "456";
            // Act & Assert
            Assert.Throws<ArgumentException>(() => new Location(Country, City, ZipCode, invalidStreet, Neighborhood, ExteriorNumber, InteriorNumber, Guid.NewGuid()));
        }
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Constructor_WithNullOrEmptyExteriorNumber_ThrowsArgumentException(string invalidExteriorNumber)
        {
            //Arrange
            var Country = "Test Country";
            var City = "Test City";
            var ZipCode = 12345;
            var street = "Test Street";
            var Neighborhood = "Test Neighborhood";
            var InteriorNumber = "456";
            // Act & Assert
            Assert.Throws<ArgumentException>(() => new Location(Country, City, ZipCode, street, Neighborhood, invalidExteriorNumber, InteriorNumber, Guid.NewGuid()));
        }
        [Fact]
        public void Constructor_WithEmptyUserId_ThrowsArgumentException()
        {
            //Arrange
            var Country = "Test Country";
            var City = "Test City";
            var ZipCode = 12345;
            var street = "Test Street";
            var Neighborhood = "Test Neighborhood";
            var ExteriorNumber = "123";
            var InteriorNumber = "456";
            var emptyUserId = Guid.Empty;
            //Act & Assert
            Assert.Throws<ArgumentException>(() => new Location(Country, City, ZipCode, street, Neighborhood, ExteriorNumber, InteriorNumber, emptyUserId));
        }
        [Fact]
        public void Update_WithValidParameters_UpdatesCorrectly()
        {
            //Arrange
            var location = new Location("Initial Country", "Initial City",12345, "Initial Street", "Initial Neighborhood", "123", "456", Guid.NewGuid());
            var newCountry = "New Country";
            var newCity = "New City";
            var newZipCode = 54321;
            var newStreet = "New Street";
            var newNeighborhood = "New Neighborhood";
            var newExteriorNumber = "789";
            var newInteriorNumber = "012";

            // Act
            location.Update(newCountry, newCity, newZipCode, newStreet, newNeighborhood, newExteriorNumber, newInteriorNumber);

            // Assert
            Assert.Equal(newCountry, location.Country);
            Assert.Equal(newCity, location.City);
            Assert.Equal(newZipCode, location.ZipCode);
            Assert.Equal(newStreet, location.Street);
            Assert.Equal(newNeighborhood, location.Neighborhood);
            Assert.Equal(newExteriorNumber, location.ExteriorNumber);
            Assert.Equal(newInteriorNumber, location.InteriorNumber);
        }
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Update_WithNullOrEmptyCountry_ThrowsArgumentException(string invalidCountry)
        {
            //Arrange
            var location = new Location("Initial Country", "Initial City", 12345, "Initial Street", "Initial Neighborhood", "123", "456", Guid.NewGuid());
            var newCity = "New City";
            var newZipCode = 54321;
            var newStreet = "New Street";
            var newNeighborhood = "New Neighborhood";
            var newExteriorNumber = "789";
            var newInteriorNumber = "012";

            // Act & Assert
            Assert.Throws<ArgumentException>(() => location.Update(invalidCountry, newCity, newZipCode, newStreet, newNeighborhood, newExteriorNumber, newInteriorNumber));
        }
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Update_WithNullOrEmptyCity_ThrowsArgumentException(string invalidCity)
        {
            //Arrange
            var location = new Location("Initial Country", "Initial City", 12345, "Initial Street", "Initial Neighborhood", "123", "456", Guid.NewGuid());
            var newCountry = "New Country";
            var newZipCode = 54321;
            var newStreet = "New Street";
            var newNeighborhood = "New Neighborhood";
            var newExteriorNumber = "789";
            var newInteriorNumber = "012";

            // Act & Assert
            Assert.Throws<ArgumentException>(() => location.Update(newCountry, invalidCity, newZipCode, newStreet, newNeighborhood, newExteriorNumber, newInteriorNumber));
        }
        [Fact]
        public void Update_WithCeroZipCode_ThrowsArgumentException()
        {
            //Arrange
            var location = new Location("Initial Country", "Initial City", 12345, "Initial Street", "Initial Neighborhood", "123", "456", Guid.NewGuid());
            var newCountry = "New Country";
            var newCity = "New City";
            var invalidZipCode = 0;
            var newStreet = "New Street";
            var newNeighborhood = "New Neighborhood";
            var newExteriorNumber = "789";
            var newInteriorNumber = "012";

            // Act & Assert
            Assert.Throws<ArgumentException>(() => location.Update(newCountry, newCity, invalidZipCode, newStreet, newNeighborhood, newExteriorNumber, newInteriorNumber));
        }
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Update_WithNullOrEmptyStreet_ThrowsArgumentException(string invalidStreet)
        {
            //Arrange
            var location = new Location("Initial Country", "Initial City", 12345, "Initial Street", "Initial Neighborhood", "123", "456", Guid.NewGuid());
            var newCountry = "New Country";
            var newCity = "New City";
            var newZipCode = 54321;
            var newNeighborhood = "New Neighborhood";
            var newExteriorNumber = "789";
            var newInteriorNumber = "012";

            // Act & Assert
            Assert.Throws<ArgumentException>(() => location.Update(newCountry, newCity, newZipCode, invalidStreet, newNeighborhood, newExteriorNumber, newInteriorNumber));
        }
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Update_WithNullOrEmptyExteriorNumber_ThrowsArgumentException(string invalidExteriorNumber)
        {
            //Arrange
            var location = new Location("Initial Country", "Initial City", 12345, "Initial Street", "Initial Neighborhood", "123", "456", Guid.NewGuid());
            var newCountry = "New Country";
            var newStreet = "New Street";
            var newCity = "New City";
            var newZipCode = 54321;
            var newNeighborhood = "New Neighborhood";
            var newInteriorNumber = "012";

            // Act & Assert
            Assert.Throws<ArgumentException>(() => location.Update(newCountry, newCity, newZipCode, newStreet, newNeighborhood, invalidExteriorNumber, newInteriorNumber));
        }
    }
}
