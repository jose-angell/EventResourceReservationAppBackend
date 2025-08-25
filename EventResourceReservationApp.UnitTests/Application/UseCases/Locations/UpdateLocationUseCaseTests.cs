using EventResourceReservationApp.Application.Common;
using EventResourceReservationApp.Application.DTOs.Locations;
using EventResourceReservationApp.Application.Repositories;
using EventResourceReservationApp.Application.UseCases.Locations;
using EventResourceReservationApp.Domain;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventResourceReservationApp.UnitTests.Application.UseCases.Locations
{
    public class UpdateLocationUseCaseTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<ILocationRepository> _mockLocationRepository;
        private readonly UpdateLocationUseCase _useCase;
        private readonly Mock<ILogger<UpdateLocationUseCase>> _mockLogger;

        public UpdateLocationUseCaseTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockLocationRepository = new Mock<ILocationRepository>();
            _mockLogger = new Mock<ILogger<UpdateLocationUseCase>>();

            _mockUnitOfWork.Setup(u => u.Locations).Returns(_mockLocationRepository.Object);
            _useCase = new UpdateLocationUseCase(_mockUnitOfWork.Object, _mockLogger.Object);
        }
        [Fact]
        public async Task ExecuteAsync_WithValidRequest_ReturnsSuccess()
        {
            // Arrange
            var request = new UpdateLocationRequest
            {
                Id = 1,
                Country = "Updated Country",
                City = "Updated City",
                ZipCode = 54321,
                Street = "Updated Street",
                Neighborhood = "Updated Neighborhood",
                ExteriorNumber = "456",
                InteriorNumber = "B"
            };
            var existingLocation = new Location("Old Country", "Old City", 12345, "Old Street", "Old Neighborhood", "123", "A", Guid.NewGuid());
            _mockLocationRepository.Setup(r => r.GetByIdAsync(request.Id))
                .ReturnsAsync(existingLocation);
            _mockLocationRepository.Setup(r => r.UpdateAsync(It.IsAny<Location>()))
                .Returns(Task.CompletedTask);

            _mockUnitOfWork.Setup(u => u.SaveAsync()).Returns(Task.CompletedTask);

            // Actt
            var result = await _useCase.ExecuteAsync(request);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("Ubicación actualizada exitosamente.", result.Message);
            _mockLocationRepository.Verify(r => r.GetByIdAsync(request.Id), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
        }
        [Fact]
        public async Task ExecuteAsync_WithNonExistentLocation_ReturnsNotFound()
        {
            var request = new UpdateLocationRequest
            {
                Id = 1,
                Country = "Updated Country",
                City = "Updated City",
                ZipCode = 54321,
                Street = "Updated Street",
                Neighborhood = "Updated Neighborhood",
                ExteriorNumber = "456",
                InteriorNumber = "B"
            };
            _mockLocationRepository.Setup(r => r.GetByIdAsync(request.Id))
                .ReturnsAsync((Location)null);

            // Act
            var result = await _useCase.ExecuteAsync(request);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("La operación de actualización falló porque la ubicación no existe.", result.Message);

            _mockLocationRepository.Verify(r => r.GetByIdAsync(request.Id), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Never);
        }
        [Fact]
        public async Task ExecuteAsync_WithInvalidRequest_ReturnsFailure()
        {
            // Arrange
            var request = new UpdateLocationRequest
            {
                Id = 1,
                Country = "", // Campo obligatorio vacío
                City = "Updated City",
                ZipCode = 54321,
                Street = "Updated Street",
                Neighborhood = "Updated Neighborhood",
                ExteriorNumber = "456",
                InteriorNumber = "B"
            };
            var existingLocation = new Location("Old Country", "Old City", 12345, "Old Street", "Old Neighborhood", "123", "A", Guid.NewGuid());
            _mockLocationRepository.Setup(r => r.GetByIdAsync(request.Id))
                .ReturnsAsync(existingLocation);
            // Act
            var result = await _useCase.ExecuteAsync(request);
            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("InvalidInput", result.ErrorCode);
            Assert.Equal("Country cannot be null or empty. (Parameter 'country')", result.Message);
            _mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Never);

        }
        [Fact]
        public async Task ExecuteAsync_WithPersistenceError_ReturnsFailure()
        {
            //Arrange
            var request = new UpdateLocationRequest
            {
                Id = 1,
                Country = "Updated Country",
                City = "Updated City",
                ZipCode = 54321,
                Street = "Updated Street",
                Neighborhood = "Updated Neighborhood",
                ExteriorNumber = "456",
                InteriorNumber = "B"
            };
            var existingLocation = new Location("Old Country", "Old City", 12345, "Old Street", "Old Neighborhood", "123", "A", Guid.NewGuid());
            _mockLocationRepository.Setup(r => r.GetByIdAsync(request.Id))
                .ReturnsAsync(existingLocation);
            _mockLocationRepository.Setup(r => r.UpdateAsync(It.IsAny<Location>()))
            .ThrowsAsync(new PersistenceException("Error al guardar en la base de datos."));

            //Act
            var result = await _useCase.ExecuteAsync(request);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("PersistenceError", result.ErrorCode);
            Assert.Equal("La operación de actualización falló debido a un error de persistencia.", result.Message);
            _mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Never);

        }
    }
}
