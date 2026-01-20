using EventResourceReservationApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventResourceReservationApp.UnitTests.Domain
{
    public class ResourceTests
    {
        [Fact]
        public void Constructor_WithValidParameters_ShouldCreateResource()
        {
            // Arrange
            int categoryId = 1;
            string name = "Projector";
            string description = "HD Projector";
            int quantity = 5;
            decimal price = 150.00m;
            ResourceAuthorizationType authorizationType = ResourceAuthorizationType.Automatico;
            int locationId = 2;
            Guid createdByUserId = Guid.NewGuid();
            // Act
            var resource = new EventResourceReservationApp.Domain.Resource(
                categoryId, name, description, quantity, price,
                authorizationType, locationId, createdByUserId);
            // Assert
            Assert.Equal(categoryId, resource.CategoryId);
            Assert.Equal(name, resource.Name);
            Assert.Equal(description, resource.Description);
            Assert.Equal(quantity, resource.Quantity);
            Assert.Equal(price, resource.Price);
            Assert.Equal(authorizationType, resource.AuthorizationType);
            Assert.Equal(locationId, resource.LocationId);
            Assert.Equal(createdByUserId, resource.CreatedByUserId);
        }
        [Fact]
        public void Constructor_WithInvalidCategoryId_ShouldThrowArgumentException()
        {
            // Arrange
            int categoryId = 0; // Invalid
            string name = "Projector";
            string description = "HD Projector";
            int quantity = 5;
            decimal price = 150.00m;
            ResourceAuthorizationType authorizationType = ResourceAuthorizationType.Automatico;
            int locationId = 2;
            Guid createdByUserId = Guid.NewGuid();
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => new EventResourceReservationApp.Domain.Resource(
                categoryId, name, description, quantity, price,
                authorizationType, locationId, createdByUserId));
            Assert.Equal("CategoryId cannot be empty. (Parameter 'categoryId')", exception.Message);
        }
        [Fact]
        public void Constructor_WithNegativeQuantity_ShouldThrowArgumentException()
        {
            // Arrange
            int categoryId = 1;
            string name = "Projector";
            string description = "HD Projector";
            int quantity = -5; // Invalid
            decimal price = 150.00m;
            ResourceAuthorizationType authorizationType = ResourceAuthorizationType.Automatico;
            int locationId = 2;
            Guid createdByUserId = Guid.NewGuid();
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => new EventResourceReservationApp.Domain.Resource(
                categoryId, name, description, quantity, price,
                authorizationType, locationId, createdByUserId));
            Assert.Equal("AvailableQuantity cannot be negative. (Parameter 'quantity')", exception.Message);
        }
        [Fact]
        public void Constructor_WithEmptyCreatedByUserId_ShouldThrowArgumentException()
        {
            // Arrange
            int categoryId = 1;
            string name = "Projector";
            string description = "HD Projector";
            int quantity = 5;
            decimal price = 150.00m;
            ResourceAuthorizationType authorizationType = ResourceAuthorizationType.Automatico;
            int locationId = 2;
            Guid createdByUserId = Guid.Empty; // Invalid
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => new EventResourceReservationApp.Domain.Resource(
                categoryId, name, description, quantity, price,
                authorizationType, locationId, createdByUserId));
            Assert.Equal("userId cannot be empty. (Parameter 'createdByUserId')", exception.Message);
        }
        [Fact]
        public void Constructor_WithEmptyName_ShouldThrowArgumentException()
        {
            // Arrange
            int categoryId = 1;
            string name = "";
            string description = "HD Projector";
            int quantity = 5;
            decimal price = 150.00m;
            ResourceAuthorizationType authorizationType = ResourceAuthorizationType.Automatico;
            int locationId = 2;
            Guid createdByUserId = Guid.NewGuid(); // Invalid
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => new EventResourceReservationApp.Domain.Resource(
                categoryId, name, description, quantity, price,
                authorizationType, locationId, createdByUserId));
            Assert.Equal("Name cannot be null or empty. (Parameter 'name')", exception.Message);
        }
        [Fact]
        public void Constructor_WithEmptyDescription_ShouldThrowArgumentException()
        {
            // Arrange
            int categoryId = 1;
            string name = "Projector";
            string description = "";
            int quantity = 5;
            decimal price = 150.00m;
            ResourceAuthorizationType authorizationType = ResourceAuthorizationType.Automatico;
            int locationId = 2;
            Guid createdByUserId = Guid.NewGuid(); 
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => new EventResourceReservationApp.Domain.Resource(
                categoryId, name, description, quantity, price,
                authorizationType, locationId, createdByUserId));
            Assert.Equal("Description cannot be null or empty. (Parameter 'description')", exception.Message);
        }
        [Fact]
        public void Constructor_WithEmptyPrice_ShouldThrowArgumentException()
        {
            // Arrange
            int categoryId = 1;
            string name = "Projector";
            string description = "HD Projector";
            int quantity = 5; 
            decimal price = -1m;
            ResourceAuthorizationType authorizationType = ResourceAuthorizationType.Automatico;
            int locationId = 2;
            Guid createdByUserId = Guid.NewGuid();
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => new EventResourceReservationApp.Domain.Resource(
                categoryId, name, description, quantity, price,
                authorizationType, locationId, createdByUserId));
            Assert.Equal("Price cannot be negative. (Parameter 'price')", exception.Message);
        }
        [Fact]
        public void Constructor_WithEmptyAuthorizationType_ShouldThrowArgumentException()
        {
            // Arrange
            int categoryId = 1;
            string name = "Projector";
            string description = "HD Projector";
            int quantity = 5;
            decimal price = 150.00m;
            ResourceAuthorizationType authorizationType = (ResourceAuthorizationType)(-1);// Invalid
            int locationId = 2;
            Guid createdByUserId = Guid.NewGuid(); 
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => new EventResourceReservationApp.Domain.Resource(
                categoryId, name, description, quantity, price,
                authorizationType, locationId, createdByUserId));
            Assert.Equal("AuthorizationType cannot be negative. (Parameter 'authorizationType')", exception.Message);
        }
        [Fact]
        public void Constructor_WithInvalidLocationId_ShouldThrowArgumentException()
        {
            // Arrange
            int categoryId = 1; 
            string name = "Projector";
            string description = "HD Projector";
            int quantity = 5;
            decimal price = 150.00m;
            ResourceAuthorizationType authorizationType = ResourceAuthorizationType.Automatico;
            int locationId = 0;// Invalid
            Guid createdByUserId = Guid.NewGuid();
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => new EventResourceReservationApp.Domain.Resource(
                categoryId, name, description, quantity, price,
                authorizationType, locationId, createdByUserId));
            Assert.Equal("LocationId cannot be empty. (Parameter 'locationId')", exception.Message);
        }
        
    }
}
