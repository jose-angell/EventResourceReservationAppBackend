using Castle.Core.Logging;
using EventResourceReservationApp.Application.Repositories;
using EventResourceReservationApp.Domain;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventResourceReservationApp.UnitTests.Domain
{
    public class ReviewTests
    {
        [Fact]
        public void Constructor_WithValidParameters_InitializesCorrectly()
        {
            // Arrange
            var resourceId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var rating = 4;
            var comment = "Great resource!";

            // Act
            var review = new Review(resourceId, userId, rating, comment);

            // Assert
            Assert.Equal(resourceId, review.ResourceId);
            Assert.Equal(userId, review.UserId);
            Assert.Equal(rating, review.Rating);
            Assert.Equal(comment, review.Comment);
        }
        [Fact]
        public void Constructor_WithInvalidRating_ThrowsArgumentException()
        {
            // Arrange
            var resourceId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var invalidRating = 6; // Invalid rating
            var comment = "Great resource!";
            // Act & Assert
            Assert.Throws<ArgumentException>(() => new Review(resourceId, userId, invalidRating, comment));
        }
        [Fact]
        public void Constructor_WithEmptyResource_ThrowsArgumentException()
        {
            // Arrange
            var resourceId = Guid.Empty;
            var userId = Guid.NewGuid();
            var rating = 4;
            var comment = "Great resource!";
            // Act & Assert
            Assert.Throws<ArgumentException>(() => new Review(resourceId, userId, rating, comment));
        }
        [Fact]
        public void Constructor_WithEmptyUser_ThrowsArgumentException()
        {
            // Arrange
            var resourceId = Guid.NewGuid();
            var userId = Guid.Empty;
            var rating = 4;
            var comment = "Great resource!";
            // Act & Assert
            Assert.Throws<ArgumentException>(() => new Review(resourceId, userId, rating, comment));
        }
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Constructor_WithInvalidComment_ThrowsArgumentException(string invalidComment)
        {
            // Arrange
            var resourceId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var rating = 4;
            // Act & Assert
            Assert.Throws<ArgumentException>(() => new Review(resourceId, userId, rating, invalidComment));
        }
        [Fact]
        public void Update_WithValidParameters_UpdatesCorrectly()
        {
            // Arrange
            var review = new Review(Guid.NewGuid(), Guid.NewGuid(), 3, "Good resource.");
            var newRating = 5;
            var newComment = "Excellent resource!";
            // Act
            review.Update(newRating, newComment);
            // Assert
            Assert.Equal(newRating, review.Rating);
            Assert.Equal(newComment, review.Comment);
        }
        [Fact]
        public void Update_WithInvalidRating_ThrowsArgumentException()
        {
            // Arrange
            var review = new Review(Guid.NewGuid(), Guid.NewGuid(), 3, "Good resource.");
            var invalidRating = 0;
            var newComment = "Excellent resource!";
            // Act & Assert
            Assert.Throws<ArgumentException>(() => review.Update(invalidRating, newComment));
        }
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Update_WithInvalidComment_ThrowsArgumentException(string invalidComment)
        {
            // Arrange
            var review = new Review(Guid.NewGuid(), Guid.NewGuid(), 3, "Good resource.");
            var newRating = 5;
            // Act & Assert
            Assert.Throws<ArgumentException>(() => review.Update(newRating, invalidComment));
        }   
    }
}
