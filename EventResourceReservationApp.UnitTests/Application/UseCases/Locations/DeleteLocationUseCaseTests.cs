using Castle.Core.Logging;
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
    public class DeleteLocationUseCaseTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<ILocationRepository> _mockLocationRepository;
        private readonly DeleteLocationUseCase _useCase;
        private readonly Mock<ILogger<DeleteLocationUseCase>> _mockLogger;

        public DeleteLocationUseCaseTests()
        {
            // Inicializamos los mocks
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockLocationRepository = new Mock<ILocationRepository>();
            _mockLogger = new Mock<ILogger<DeleteLocationUseCase>>();
            // Configuramos el UnitOfWork para que devuelva el repositorio de ubicaciones
            _mockUnitOfWork.Setup(u => u.Locations).Returns(_mockLocationRepository.Object);
            // Creamos la instancia del caso de uso
            _useCase = new DeleteLocationUseCase(_mockUnitOfWork.Object, _mockLogger.Object);
        }
        [Fact]
        public async Task ExecuteAsync_WithValidRequest_ReturnsSuccess()
        {
            // Arrange
            int locationId = 1;
            var existingLocation = new Location("Test Country", "Test City", 12345, "Test Street", "Test Neighborhood", "123", "A", Guid.NewGuid());
            _mockLocationRepository.Setup(r => r.GetByIdAsync(locationId))
                .ReturnsAsync(existingLocation);
            _mockLocationRepository.Setup(r => r.RemoveAsync(existingLocation))
                .Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(u => u.SaveAsync()).Returns(Task.CompletedTask);

            // Act
            var result = await _useCase.ExecuteAsync(locationId);
            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("Ubicación eliminada exitosamente.", result.Message);

            _mockLocationRepository.Verify(r => r.RemoveAsync(existingLocation), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
            _mockLogger.Verify(x => x.Log(
                It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
                Times.Never);
        }
        [Fact]
        public async Task ExecuteAsync_WithNonExistentLocation_ReturnsNotFound()
        {
            int locationId = 1;
            _mockLocationRepository.Setup(r => r.GetByIdAsync(locationId))
                .ReturnsAsync((Location)null);
            // Act
            var result = await _useCase.ExecuteAsync(locationId);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("NotFound", result.ErrorCode);
            Assert.Equal("La operación de actualización falló porque la ubicación no existe.", result.Message);
            _mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Never);
            _mockLogger.Verify(
            x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => o.ToString().Contains($"Fallo al eliminar: No se encontró una ubicación con el ID '{locationId}'.")),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
            Times.Once);
        }
        [Fact]
        public async Task ExecuteAsync_WithPersistenceError_ReturnsFailure()
        {
            int locationId = 1;
            var existingLocation = new Location("Test Country", "Test City", 12345, "Test Street", "Test Neighborhood", "123", "A", Guid.NewGuid());
            _mockLocationRepository.Setup(r => r.GetByIdAsync(locationId))
                .ReturnsAsync(existingLocation);
            _mockLocationRepository.Setup(r => r.RemoveAsync(existingLocation)).ThrowsAsync(new PersistenceException("Error al guardar en la base de datos."));

            // Act
            var result = await _useCase.ExecuteAsync(locationId);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("PersistenceError", result.ErrorCode);
            Assert.Equal("La operación de eliminación falló debido a un problema de almacenamiento de datos.", result.Message);
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => o.ToString().Contains("error de persistencia")),
                    It.IsAny<PersistenceException>(),
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
                Times.Once);
        }
    }
}
