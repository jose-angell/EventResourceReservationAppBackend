using EventResourceReservationApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace EventResourceReservationApp.UnitTests.Domain
{
    public class CategoryTests
    {
        [Fact]
        public void Constructor_WithValidParameters_InitializesCorrectly()
        {
            //Arrange
            var name = "Test Category";
            var description = "This is a test category.";
            var userId = Guid.NewGuid();
            //Act
            var category = new Category(name, description, userId);
            //Assert
            Assert.Equal(name, category.Name);
            Assert.Equal(description, category.Description);
            Assert.Equal(userId, category.CreatedByUserId);
            Assert.NotEqual(default(DateTime), category.CreatedAt);
        }
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void ConstructorWithNullOrEmptyName_ThrowsArgumentException(string invalidName)
        {
            //Arrage
            var description = "Some description.";
            var userId = Guid.NewGuid();
            // Act & Assert
            Assert.Throws<ArgumentException>(() => new Category(invalidName, description, userId));
        }
        [Fact]
        public void Constructor_WithNameLongerThan100Characters_ThrowsArgumentException()
        {
            //Arrage
            var longName = new string('a', 101);
            var description = "Some description.";
            var userId = Guid.NewGuid();
            //Act & Assert
            Assert.Throws<ArgumentException>(() => new Category(longName, description, userId));
        }
        [Fact]
        public void Constructor_WithEmptyUserId_ThrowsArgumentException()
        {
            //Arrange
            var name = "Valid Name";
            var description = "Some description.";
            var emptyUserId = Guid.Empty;
            //Act & Assert
            Assert.Throws<ArgumentException>(() => new Category(name, description, emptyUserId));
        }


        [Fact]
        public void Update_WithValidParameters_UpdatesCorrectly()
        {
            //Arrange
            var category = new Category("Initial Name", "Initial Description", Guid.NewGuid());
            var newName = "New Name";
            var newDescription = "New Description";
            //Act
            category.Update(newName, newDescription);
            //Assert
            Assert.Equal(newName, category.Name);
            Assert.Equal(newDescription, category.Description);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Update_WithNullOrEmptyName_ThrowsArgumentException(string invalidName)
        {
            //Arrange
            var category = new Category("Valid Name", "Description", Guid.NewGuid());
            var description = "Some description.";
            //Act & Assert
            Assert.Throws<ArgumentException>(() => category.Update(invalidName, description));
        }
        [Fact]
        public void Update_WithNameLongerThan100Characters_ThrowsArgumentException()
        {
            // Arrange
            var category = new Category("Valid Name", "Description", Guid.NewGuid());
            var longName = new string('a', 101);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => category.Update(longName, "Description"));
        }
    }
}
