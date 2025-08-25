using EventResourceReservationApp.Application.Common;
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
    public class ReadByIdLocationUseCaseTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<ILocationRepository> _mockLocationRepository;
        private readonly ReadByIdLocationUseCase _useCase;
        private readonly Mock<ILogger<ReadByIdLocationUseCase>> _mockLogger;
        public ReadByIdLocationUseCaseTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockLocationRepository = new Mock<ILocationRepository>();
            _mockLogger = new Mock<ILogger<ReadByIdLocationUseCase>>();
            _mockUnitOfWork.Setup(u => u.Locations).Returns(_mockLocationRepository.Object);
            _useCase = new ReadByIdLocationUseCase(_mockUnitOfWork.Object, _mockLogger.Object);
        }
        [Fact]
        public async Task ExecuteAsync_WithValidRequest_ReturnsLocation()
        {
            // Arrange
            int locationId = 1;
            var existingLocation = new Location("Test Country", "Test City", 12345, "Test Street", "Test Neighborhood", "123", "A", Guid.NewGuid());
            _mockLocationRepository.Setup(r => r.GetByIdAsync(locationId))
                .ReturnsAsync(existingLocation);
            // Act
            var result = await _useCase.ExecuteAsync(locationId);
            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(existingLocation.Country, result.Data.Country);
            _mockLocationRepository.Verify(r => r.GetByIdAsync(locationId), Times.Once);
        }
        [Fact]
        public async Task ExecuteAsync_WithNonExistentLocation_ReturnsNotFound()
        {
            // Arrange
            int locationId = 1;
            _mockLocationRepository.Setup(r => r.GetByIdAsync(locationId))
                .ReturnsAsync((Location)null);
            // Act
            var result = await _useCase.ExecuteAsync(locationId);
            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("NotFound", result.ErrorCode);
            Assert.Equal($"No se encontró una ubicación con el ID '{locationId}'.", result.Message);
            _mockLocationRepository.Verify(r => r.GetByIdAsync(locationId), Times.Once);
        }
        [Fact]
        public async Task ExecuteAsync_WhenPersistenceErrorOccurs_ReturnsPersistenceError()
        {
            // Arrange
            int locationId = 1;
            _mockLocationRepository.Setup(r => r.GetByIdAsync(locationId))
                .ThrowsAsync(new PersistenceException("Database error"));
            // Act
            var result = await _useCase.ExecuteAsync(locationId);
            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("PersistenceError", result.ErrorCode);
            Assert.Equal("La operación de lectura falló debido a un problema de almacenamiento de datos.", result.Message);
        }
    }
}
