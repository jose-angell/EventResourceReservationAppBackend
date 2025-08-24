using EventResourceReservationApp.Application.Common;
using EventResourceReservationApp.Application.DTOs.Loctions;
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
    public class CreateLocationUseCaseTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<ILocationRepository> _mockLocationRepository;
        private readonly CreateLocationUseCase _useCase;
        private readonly Mock<ILogger<CreateLocationUseCase>> _mockLogger;
        public CreateLocationUseCaseTests()
        {
            // Inicializamos los mocks
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockLocationRepository = new Mock<ILocationRepository>();
            _mockLogger = new Mock<ILogger<CreateLocationUseCase>>();
            // Configuramos el UnitOfWork para que devuelva el repositorio de ubicaciones
            _mockUnitOfWork.Setup(u => u.Locations).Returns(_mockLocationRepository.Object);
            // Creamos la instancia del caso de uso
            _useCase = new CreateLocationUseCase(_mockUnitOfWork.Object, _mockLogger.Object);
        }
        [Fact]
        public async Task ExecuteAsync_WithValidRequest_RetunrsSuccess()
        {
            // Arrange
            var request = new CreateLocationRequest
            {
                Country = "Test Country",
                City = "Test City",
                ZipCode = 12345,
                Street = "Test Street",
                Neighborhood = "Test Neighborhood",
                ExteriorNumber = "123",
                InteriorNumber = "A",
                CreatedByUserId = Guid.NewGuid()
            };
            _mockLocationRepository.Setup(r => r.AddAsync(It.IsAny<Location>()))
                .Returns(Task.CompletedTask);

            _mockUnitOfWork.Setup(u => u.SaveAsync())
                .Returns(Task.CompletedTask);
            // Act
            var result = await _useCase.ExecuteAsync(request);
            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(request.Country, result.Data.Country);

            _mockLocationRepository.Verify(r => r.AddAsync(It.IsAny<Location>()), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
        }
        [Fact]
        public async Task ExecuteAsync_WithInvalidRequest_ReturnsFailure()
        {
            // Arrange
            var request = new CreateLocationRequest
            {
                Country = "", // Campo obligatorio vacío
                City = "Test City",
                ZipCode = 12345,
                Street = "Test Street",
                Neighborhood = "Test Neighborhood",
                ExteriorNumber = "123",
                InteriorNumber = "A",
                CreatedByUserId = Guid.NewGuid()
            };
            // Act
            var result = await _useCase.ExecuteAsync(request);
            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("InvalidInput", result.ErrorCode);
            Assert.Equal("Country cannot be null or empty. (Parameter 'country')", result.Message);
            _mockUnitOfWork.Verify(u => u.Locations.AddAsync(It.IsAny<Location>()), Times.Never);
            _mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Never);
        }
        [Fact]
        public async Task ExecuteAsync_WhenPersistenceFails_ReturnsFailure()
        {
            // Arrange
            var request = new CreateLocationRequest
            {
                Country = "Test Country",
                City = "Test City",
                ZipCode = 12345,
                Street = "Test Street",
                Neighborhood = "Test Neighborhood",
                ExteriorNumber = "123",
                InteriorNumber = "A",
                CreatedByUserId = Guid.NewGuid()
            };
            _mockLocationRepository.Setup(r => r.AddAsync(It.IsAny<Location>()))
                .ThrowsAsync(new PersistenceException("Database error"));
            // Act
            var result = await _useCase.ExecuteAsync(request);
            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("PersistenceError", result.ErrorCode);
            Assert.Equal("La operación de creación falló debido a un problema de almacenamiento de datos.", result.Message);
            _mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Never);
        }
    }
}
