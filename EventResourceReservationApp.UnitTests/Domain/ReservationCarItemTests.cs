using EventResourceReservationApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventResourceReservationApp.UnitTests.Domain
{
    public class ReservationCarItemTests
    {
        [Fact]
        public void Constructor_WithValidParameters_InitializesCorrectly()
        {
            // Arrange
            var resourceId = Guid.NewGuid();
            var clientId = Guid.NewGuid();
            var quantity = 5;

            // Act
            var item = new ReservationCarItem(clientId, resourceId, quantity);

            // Assert
            Assert.Equal(resourceId, item.ResourceId);
            Assert.Equal(clientId, item.ClientId);
            Assert.Equal(quantity, item.Quantity);
            Assert.NotEqual(default(DateTime), item.AddedAt);
        }
        [Fact]
        public void Constructor_WithEmptyResourceId_ThrowsArgumentException()
        {
            // Arrange
            var resourceId = Guid.Empty;
            var clientId = Guid.NewGuid();
            var quantity = 5;

            // Act && Assert
            Assert.Throws<ArgumentException>(() => new ReservationCarItem(clientId, resourceId, quantity));
        }
        [Fact]
        public void Constructor_WithEmptyClientId_ThrowsArgumentException()
        {
            // Arrange
            var resourceId = Guid.NewGuid();
            var clientId = Guid.Empty;
            var quantity = 5;
            // Act && Assert
            Assert.Throws<ArgumentException>(() => new ReservationCarItem(clientId, resourceId, quantity));
        }
        [Fact]
        public void Constructor_WithNonPositiveQuantity_ThrowsArgumentException()
        {
            // Arrange
            var resourceId = Guid.NewGuid();
            var clientId = Guid.NewGuid();
            var quantity = 0;
            // Act && Assert
            Assert.Throws<ArgumentException>(() => new ReservationCarItem(clientId, resourceId, quantity));
        }
        [Fact]
        public void UpdateQuantity_WithValidQuantity_UpdatesCorrectly()
        {
            // Arrange
            var resourceId = Guid.NewGuid();
            var clientId = Guid.NewGuid();
            var initialQuantity = 5;
            var newQuantity = 10;
            var item = new ReservationCarItem(clientId, resourceId, initialQuantity);
            // Act
            item.UpdateQuantity(newQuantity);
            // Assert
            Assert.Equal(newQuantity, item.Quantity);
        }
    }
}
